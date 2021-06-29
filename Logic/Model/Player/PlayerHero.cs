using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.ComponentModel;
using System.ComponentModel.Design;
using System.Linq;
using System.Runtime.CompilerServices;
using System.Threading.Tasks;
using Logic.ActionManger;
using Logic.Annotations;
using Logic.GameLevel;
using Logic.Log;
using Logic.Model.Cards.EquipmentCards;
using Logic.Model.Enums;
using Logic.Model.Hero;
using Logic.Model.Interface;
using Logic.Model.RequestResponse;
using Logic.Model.RequestResponse.Request;
using Logic.Model.Skill;
using Logic.Util;
using Logic.Util.Extension;

namespace Logic.Model.Player
{
    /// <summary>
    /// 玩家英雄
    /// </summary>
    public class PlayerHero : IAbility, INotifyPropertyChanged
    {
        #region 属性
        public int Id { get; set; }

        /// <summary>
        /// 当前血量
        /// </summary>
        private int _CurrentLife;

        public int CurrentLife
        {
            get
            {
                return _CurrentLife;
            }
            private set
            {
                _CurrentLife = value;
                OnPropertyChanged();
            }
        }

        private int _ExtraXianShou;
        private int ExtraXianShou
        {
            get
            {
                return _ExtraXianShou;
            }
            set
            {
                _ExtraXianShou = value;
                OnPropertyChanged();
            }
        }


        /// <summary>
        /// 获取英雄的先手值
        /// </summary>
        /// <returns></returns>
        public int GetXianshou => Hero.Xianshou + ExtraXianShou;



        ///// <summary>
        ///// 最大生命
        ///// </summary>
        //public int MaxLife { get; private set; }

        /// <summary>
        /// 是否死亡
        /// </summary>
        private bool _IsDead;
        public bool IsDead
        {
            get
            {
                return _IsDead;
            }
            set
            {
                _IsDead = value;
                OnPropertyChanged();
            }
        }

        /// <summary>
        /// 是否是活跃状态
        /// </summary>
        private bool _IsActive;
        public bool IsActive
        {
            get
            {
                return _IsActive;
            }
            set
            {
                _IsActive = value;
                OnPropertyChanged();
            }
        }

        /// <summary>
        /// 基础攻击属性
        /// </summary>
        private AttackDynamicFactor _BaseAttackFactor;
        public AttackDynamicFactor BaseAttackFactor
        {
            get
            {
                return _BaseAttackFactor;
            }
            set
            {
                _BaseAttackFactor = value;
                OnPropertyChanged();
            }
        }

        private HeroBase _hero;
        /// <summary>
        /// 具体的英雄
        /// </summary>
        public HeroBase Hero
        {
            get => _hero;
            private set
            {
                _hero = value;
                OnPropertyChanged();
            }
        }

        /// <summary>
        /// 玩家英雄星级{1-5}
        /// </summary>
        public int Star { get; }

        public PlayerContext PlayerContext { get; private set; }

        /// <summary>
        /// 额外的主技能
        /// </summary>
        public ObservableCollection<SkillBase> ExtraMainSkillSet { get; }

        /// <summary>
        /// 额外的副技能
        /// </summary>
        public ObservableCollection<SkillBase> ExtraSubSkillSet { get; }

        #endregion

        public PlayerHero(int star, HeroBase hero, IEnumerable<SkillBase> extraMainSkillSet, IEnumerable<SkillBase> extraSubSkillSet)
        {
            Star = star <= 1 ? 1 : (star >= 5 ? 5 : star);
            Hero = hero;
            BaseAttackFactor = Hero.GetBaseAttackFactor();
            CurrentLife = BaseAttackFactor.MaxLife;
            //加载技能，初始化事件监听
            ExtraMainSkillSet = extraMainSkillSet == null ? new ObservableCollection<SkillBase>() : new ObservableCollection<SkillBase>(extraMainSkillSet); ;
            ExtraSubSkillSet = extraSubSkillSet == null ? new ObservableCollection<SkillBase>() : new ObservableCollection<SkillBase>(extraSubSkillSet);

            //初始化攻击因子
            InitAttackFactor(star, hero);
        }

        #region 公有方法

        public void SetupSkills()
        {
            var allMainSkills = GetAllMainSkills();
            var allSubSkills = GetAllSubSkills();

            LoadSkills(allMainSkills);
            LoadSkills(allSubSkills);
        }

        public PlayerHero AttachPlayerContext(PlayerContext playerContext)
        {
            PlayerContext = playerContext;
            return this;
        }

        /// <summary>
        /// 获取所有主技能
        /// </summary>
        /// <returns></returns>
        public ObservableCollection<SkillBase> GetAllMainSkills()
        {
            var skills = new List<SkillBase>();
            skills.AddRange(Hero.MainSkillSet);
            if (ExtraMainSkillSet != null)
            {
                return skills.Union(ExtraMainSkillSet, new GenericCompare<SkillBase>(x => x.Name)).ToObservableCollection();
            }
            return skills.ToObservableCollection();
        }

        /// <summary>
        /// 获取所有副技能
        /// </summary>
        /// <returns></returns>
        public ObservableCollection<SkillBase> GetAllSubSkills()
        {
            var skills = new List<SkillBase>();
            skills.AddRange(Hero.SubSkillSet);
            if (ExtraSubSkillSet != null)
            {
                return skills.Union(ExtraSubSkillSet, new GenericCompare<SkillBase>(x => x.Name)).ToObservableCollection();
            }

            return skills.ToObservableCollection();
        }

        /// <summary>
        /// 获取综合的攻击信息
        /// </summary>
        /// <returns></returns>
        public AttackDynamicFactor GetAttackFactor()
        {
            return BaseAttackFactor;
        }

        /// <summary>
        /// 当前PlayerHero掉血
        /// </summary>
        /// <param name="request"></param>
        /// <returns></returns>
        public async Task LoseLife(LoseLifeRequest request)
        {
            await PlayerContext.Player.TriggerEvent(EventTypeEnum.BeforeLoseLife, request.CardRequestContext,
                 request.CardResponseContext, request.SrcRoundContext);
            await PlayerContext.GameLevel.GlobalEventBus.TriggerEvent(EventTypeEnum.BeforeLoseLife, PlayerContext.GameLevel.HostPlayerHero, request.CardRequestContext, request.SrcRoundContext,
                request.CardResponseContext);

            request.CardRequestContext.AdditionalContext = PlayerContext.Player;

            await PlayerContext.Player.TriggerEvent(EventTypeEnum.LoseLife, request.CardRequestContext,
                request.CardResponseContext, request.SrcRoundContext);
            await PlayerContext.GameLevel.GlobalEventBus.TriggerEvent(EventTypeEnum.LoseLife, PlayerContext.GameLevel.HostPlayerHero, request.CardRequestContext, request.SrcRoundContext,
                request.CardResponseContext);

            var actDamage = 0;
            var mergedRequest = PlayerContext.Player.GetCombindCardRequestContext(request.CardRequestContext,
                  null, request.SrcRoundContext);
            switch (request.DamageType)
            {
                //无源掉血请求，则直接掉血
                case DamageTypeEnum.None:
                    {
                        actDamage = mergedRequest.AttackDynamicFactor?.Damage?.ShaDamage ?? 0;
                    };
                    break;
                //杀
                case DamageTypeEnum.Sha:
                //决斗
                case DamageTypeEnum.Sanbanfu:
                    actDamage = (mergedRequest.AttackDynamicFactor?.Damage?.ShaDamage ?? 0);
                    break;
                //烽火狼烟
                case DamageTypeEnum.Juedou:
                    actDamage = (mergedRequest.AttackDynamicFactor?.Damage?.JuedouDamage ?? 0);
                    break;
                //万箭齐发
                case DamageTypeEnum.Fenghuolangyan:
                    actDamage = (mergedRequest.AttackDynamicFactor?.Damage?.FenghuolangyanDamage ?? 0);
                    break;
                ////三板斧
                //else if (request.DamageType == DamageTypeEnum.Sanbanfu)
                //{
                //    //三板斧，判断杀的response到底是几个闪,如果没闪则伤害+1。
                //    var shaDamage = (mergedRequest.AttackDynamicFactor?.Damage?.ShaDamage ?? 0);
                //    if (request.CardResponseContext.Cards.Count == 0)
                //    {
                //        shaDamage++;
                //    }
                //    else if (request.CardResponseContext.Cards.Count == 1)
                //    {
                //    }
                //    actDamage = shaDamage;
                //}
                //攻心
                case DamageTypeEnum.Wanjianqifa:
                    actDamage = (mergedRequest.AttackDynamicFactor?.Damage?.WanjianqifaDamage ?? 0);
                    break;
                //手捧雷
                case DamageTypeEnum.Gongxin:
                    actDamage = mergedRequest.AttackDynamicFactor.Damage.GongxinDamage;
                    break;
                //未知攻击
                case DamageTypeEnum.Shoupenglei:
                    actDamage = mergedRequest.AttackDynamicFactor.Damage.ShoupengleiDamage;
                    break;
                default:
                    actDamage -= 1;
                    break;
            }

            CurrentLife -= actDamage;

            Console.WriteLine($"{PlayerContext.Player.PlayerName + PlayerContext.Player.PlayerId}的【{Hero.DisplayName}】被“{request.DamageType.GetDescription()}”掉血{actDamage}.");
            PlayerContext.GameLevel.LogManager.LogAction(
                                   new RichTextParagraph(
                                   new RichTextWrapper(PlayerContext.Player.ToString(), RichTextWrapper.GetColor(ColorEnum.Blue)),
                                   new RichTextWrapper("被“"),
                                   new RichTextWrapper(request.DamageType.GetDescription(), RichTextWrapper.GetColor(ColorEnum.Red)),
                                   new RichTextWrapper("”掉血"),
                                   new RichTextWrapper(actDamage.ToString(), RichTextWrapper.GetColor(ColorEnum.Red)),
                                   new RichTextWrapper("。")
                                ));
            await PlayerContext.Player.TriggerEvent(EventTypeEnum.AfterLoseLife, request.CardRequestContext,
                request.CardResponseContext, request.SrcRoundContext);
            await PlayerContext.GameLevel.GlobalEventBus.TriggerEvent(EventTypeEnum.AfterLoseLife, PlayerContext.GameLevel.HostPlayerHero, request.CardRequestContext, request.SrcRoundContext,
                request.CardResponseContext);
        }

        /// <summary>
        /// 回血
        /// </summary>
        /// <param name="request"></param>
        /// <returns>是否回血</returns>
        public async Task<bool> AddLife(AddLifeRequest request)
        {
            if (CurrentLife < BaseAttackFactor.MaxLife)
            {
                request.CardRequestContext.AdditionalContext = PlayerContext.Player;
                await PlayerContext.Player.TriggerEvent(EventTypeEnum.BeforeAddLife, request.CardRequestContext,
                    request.CardResponseContext, request.SrcRoundContext);
                await PlayerContext.GameLevel.GlobalEventBus.TriggerEvent(EventTypeEnum.BeforeAddLife, PlayerContext.GameLevel.HostPlayerHero, request.CardRequestContext, request.SrcRoundContext,
                    request.CardResponseContext);

                await PlayerContext.Player.TriggerEvent(EventTypeEnum.AddLife, request.CardRequestContext,
                    request.CardResponseContext, request.SrcRoundContext);
                await PlayerContext.GameLevel.GlobalEventBus.TriggerEvent(EventTypeEnum.AddLife, PlayerContext.GameLevel.HostPlayerHero, request.CardRequestContext, request.SrcRoundContext,
                    request.CardResponseContext);

                var roundContextAttackDynamicFactor = request.SrcRoundContext?.AttackDynamicFactor;
                var deltaLife = 0;
                //吸血
                if (request.RecoverType == RecoverTypeEnum.Xixue)
                {
                    deltaLife = request.CardRequestContext.AttackDynamicFactor.Recover.XixueLife + (roundContextAttackDynamicFactor?.Recover.XixueLife ?? 0);
                    AddLife(deltaLife, BaseAttackFactor.MaxLife);
                }
                //休养生息
                else if (request.RecoverType == RecoverTypeEnum.Xiuyangshengxi)
                {
                    deltaLife = request.CardRequestContext.AttackDynamicFactor.Recover.XiuyangshengxiLife + (roundContextAttackDynamicFactor?.Recover.XiuyangshengxiLife ?? 0);
                    AddLife(deltaLife, BaseAttackFactor.MaxLife);
                }
                //吃药
                else if (request.RecoverType == RecoverTypeEnum.Yao)
                {
                    deltaLife = request.CardRequestContext.AttackDynamicFactor.Recover.YaoLife + (roundContextAttackDynamicFactor?.Recover.YaoLife ?? 0);
                    AddLife(deltaLife, BaseAttackFactor.MaxLife);
                }

                //Console.WriteLine($"{PlayerContext.Player.PlayerName + PlayerContext.Player.PlayerId}的【{Hero.DisplayName}】被“{request.RecoverType}”回复{deltaLife}血.");
                await PlayerContext.Player.TriggerEvent(EventTypeEnum.AfterAddLife, request.CardRequestContext,
                    request.CardResponseContext, request.SrcRoundContext);
                await PlayerContext.GameLevel.GlobalEventBus.TriggerEvent(EventTypeEnum.AfterAddLife, PlayerContext.GameLevel.HostPlayerHero, request.CardRequestContext, request.SrcRoundContext,
                    request.CardResponseContext);

                return true;
            }
            return false;
        }
        #endregion

        #region 保护方法
        protected void AddLife(int deltaLife, int maxLife)
        {
            if (deltaLife + CurrentLife >= maxLife)
            {

                Console.WriteLine($"{PlayerContext.Player.PlayerName + PlayerContext.Player.PlayerId}的【{Hero.DisplayName}】回复{maxLife - CurrentLife}点血量.");
                PlayerContext.GameLevel.LogManager.LogAction(
                                     new RichTextParagraph(
                                     new RichTextWrapper(PlayerContext.Player.ToString(), RichTextWrapper.GetColor(ColorEnum.Blue)),
                                     new RichTextWrapper("回复"),
                                     new RichTextWrapper((maxLife - CurrentLife).ToString(), RichTextWrapper.GetColor(ColorEnum.Red)),
                                     new RichTextWrapper("点血量。")
                                  ));
                this.CurrentLife = maxLife;
            }
            else
            {
                Console.WriteLine($"{PlayerContext.Player.PlayerName + PlayerContext.Player.PlayerId}的【{Hero.DisplayName}】回复{deltaLife}点血量.");
                PlayerContext.GameLevel.LogManager.LogAction(
                                   new RichTextParagraph(
                                   new RichTextWrapper(PlayerContext.Player.ToString(), RichTextWrapper.GetColor(ColorEnum.Blue)),
                                   new RichTextWrapper("回复"),
                                   new RichTextWrapper(deltaLife.ToString(), RichTextWrapper.GetColor(ColorEnum.Red)),
                                   new RichTextWrapper("点血量。")
                                ));
                this.CurrentLife += deltaLife;
            }
        }

        protected void InitAttackFactor(int star, HeroBase hero)
        {
            BaseAttackFactor.MaxLife = (star - 1) + hero.MaxLife;
            CurrentLife = BaseAttackFactor.MaxLife;
        }

        protected void LoadSkills(IEnumerable<SkillBase> skillSet)
        {
            if (skillSet == null || !skillSet.Any())
            {
                return;
            }
            foreach (var skillBase in skillSet)
            {
                skillBase.LoadSkill(this);
            }
        }

        #endregion

        #region IAbility
        /// <summary>
        /// Player能否提供杀
        /// </summary>
        /// <returns></returns>
        public bool CanProvideSha()
        {
            throw new System.NotImplementedException();
        }

        public bool CanProvideFuidichouxin()
        {
            throw new NotImplementedException();
        }

        public bool CanProvideJiedaosharen()
        {
            throw new NotImplementedException();
        }

        public bool CanProvideShan()
        {
            throw new System.NotImplementedException();
        }

        public bool CanProvideYao()
        {
            throw new NotImplementedException();
        }

        public bool CanProviderWuxiekeji()
        {
            throw new NotImplementedException();
        }

        public bool CanProvideJuedou()
        {
            throw new System.NotImplementedException();
        }

        public bool CanProvideFenghuolangyan()
        {
            throw new System.NotImplementedException();
        }

        public bool CanProvideWanjianqifa()
        {
            throw new System.NotImplementedException();
        }

        public bool CanProvideTannangquwu()
        {
            throw new System.NotImplementedException();
        }

        public bool CanProvideWuzhongshengyou()
        {
            throw new NotImplementedException();
        }

        public bool CanProvideHudadiweilao()
        {
            throw new NotImplementedException();
        }

        public bool CanProvideXiuyangshengxi()
        {
            throw new NotImplementedException();
        }

        #endregion

        public event PropertyChangedEventHandler PropertyChanged;

        [NotifyPropertyChangedInvocator]
        protected virtual void OnPropertyChanged([CallerMemberName] string propertyName = null)
        {
            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propertyName));
        }
    }
}

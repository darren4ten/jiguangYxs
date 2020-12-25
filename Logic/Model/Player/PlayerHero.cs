using System;
using System.Collections.Generic;
using System.ComponentModel.Design;
using System.Linq;
using System.Threading.Tasks;
using Logic.ActionManger;
using Logic.GameLevel;
using Logic.Model.Cards.EquipmentCards;
using Logic.Model.Enums;
using Logic.Model.Hero;
using Logic.Model.Interface;
using Logic.Model.RequestResponse;
using Logic.Model.RequestResponse.Request;
using Logic.Model.Skill;
using Logic.Util;

namespace Logic.Model.Player
{
    /// <summary>
    /// 玩家英雄
    /// </summary>
    public class PlayerHero : IAbility
    {
        public int Id { get; set; }

        /// <summary>
        /// 当前血量
        /// </summary>
        public int CurrentLife { get; private set; }

        private int ExtraXianShou { get; set; }


        /// <summary>
        /// 获取英雄的先手值
        /// </summary>
        /// <returns></returns>
        public int GetXianshou()
        {
            return Hero.Xianshou + ExtraXianShou;
        }


        ///// <summary>
        ///// 最大生命
        ///// </summary>
        //public int MaxLife { get; private set; }

        /// <summary>
        /// 是否死亡
        /// </summary>
        public bool IsDead { get; set; }

        /// <summary>
        /// 基础攻击属性
        /// </summary>
        public AttackDynamicFactor BaseAttackFactor { get; set; }

        /// <summary>
        /// 具体的英雄
        /// </summary>
        public HeroBase Hero { get; }

        /// <summary>
        /// 玩家英雄星级{1-5}
        /// </summary>
        public int Star { get; }

        public PlayerContext PlayerContext { get; private set; }

        /// <summary>
        /// 额外的主技能
        /// </summary>
        public List<SkillBase> ExtraMainSkillSet { get; }

        /// <summary>
        /// 额外的副技能
        /// </summary>
        public List<SkillBase> ExtraSubSkillSet { get; }


        public PlayerHero(int star, HeroBase hero, List<SkillBase> extraMainSkillSet, List<SkillBase> extraSubSkillSet)
        {
            Star = star <= 1 ? 1 : (star >= 5 ? 5 : star);
            Hero = hero;
            BaseAttackFactor = Hero.GetBaseAttackFactor();
            CurrentLife = BaseAttackFactor.MaxLife;
            //加载技能，初始化事件监听
            ExtraMainSkillSet = extraMainSkillSet;
            ExtraSubSkillSet = extraSubSkillSet;

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
        public List<SkillBase> GetAllMainSkills()
        {
            var skills = new List<SkillBase>();
            skills.AddRange(Hero.MainSkillSet);
            if (ExtraMainSkillSet != null)
            {
                return skills.Union(ExtraMainSkillSet, new GenericCompare<SkillBase>(x => x.Name)).ToList();
            }
            return skills;
        }

        /// <summary>
        /// 获取所有副技能
        /// </summary>
        /// <returns></returns>
        public List<SkillBase> GetAllSubSkills()
        {
            var skills = new List<SkillBase>();
            skills.AddRange(Hero.SubSkillSet);
            if (ExtraSubSkillSet != null)
            {
                return skills.Union(ExtraSubSkillSet, new GenericCompare<SkillBase>(x => x.Name)).ToList();
            }

            return skills;
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
            await PlayerContext.GameLevel.GlobalEventBus.TriggerEvent(EventTypeEnum.BeforeLoseLife, null, request.CardRequestContext, request.SrcRoundContext,
                request.CardResponseContext);


            await PlayerContext.Player.TriggerEvent(EventTypeEnum.LoseLife, request.CardRequestContext,
                request.CardResponseContext, request.SrcRoundContext);
            await PlayerContext.GameLevel.GlobalEventBus.TriggerEvent(EventTypeEnum.LoseLife, null, request.CardRequestContext, request.SrcRoundContext,
                request.CardResponseContext);

            var actDamage = 0;
            //杀
            if (request.DamageType == DamageTypeEnum.Sha)
            {
                actDamage = (request.CardRequestContext.AttackDynamicFactor?.Damage?.ShaDamage ?? 0);
            }
            //决斗
            else if (request.DamageType == DamageTypeEnum.Juedou)
            {
                actDamage = (request.CardRequestContext.AttackDynamicFactor?.Damage?.JuedouDamage ?? 0);
            }
            //烽火狼烟
            else if (request.DamageType == DamageTypeEnum.Fenghuolangyan)
            {
                actDamage = (request.CardRequestContext.AttackDynamicFactor?.Damage?.FenghuolangyanDamage ?? 0);
            }
            //万箭齐发
            else if (request.DamageType == DamageTypeEnum.Wanjianqifa)
            {
                actDamage = (request.CardRequestContext.AttackDynamicFactor?.Damage?.WanjianqifaDamage ?? 0);
            }
            //三板斧
            else if (request.DamageType == DamageTypeEnum.Sanbanfu)
            {
                //三板斧，判断杀的response到底是几个闪,如果没闪则伤害+1。
                var shaDamage = (request.CardRequestContext.AttackDynamicFactor?.Damage?.ShaDamage ?? 0);
                if (request.CardResponseContext.Cards.Count == 0)
                {
                    shaDamage++;
                }
                actDamage = shaDamage;
            }
            //攻心
            else if (request.DamageType == DamageTypeEnum.Gongxin)
            {
                actDamage = request.CardRequestContext.AttackDynamicFactor.Damage.GongxinDamage;
            }
            //未知攻击
            else
            {
                actDamage -= 1;
            }

            CurrentLife -= actDamage;

            Console.WriteLine($"{PlayerContext.Player.PlayerName + PlayerContext.Player.PlayerId}的【{Hero.DisplayName}】被“{request.DamageType}”掉血{actDamage}.");
            await PlayerContext.Player.TriggerEvent(EventTypeEnum.AfterLoseLife, request.CardRequestContext,
                request.CardResponseContext, request.SrcRoundContext);
            await PlayerContext.GameLevel.GlobalEventBus.TriggerEvent(EventTypeEnum.AfterLoseLife, null, request.CardRequestContext, request.SrcRoundContext,
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
                await PlayerContext.Player.TriggerEvent(EventTypeEnum.BeforeAddLife, request.CardRequestContext,
                    request.CardResponseContext, request.SrcRoundContext);
                await PlayerContext.GameLevel.GlobalEventBus.TriggerEvent(EventTypeEnum.BeforeAddLife, null, request.CardRequestContext, request.SrcRoundContext,
                    request.CardResponseContext);

                await PlayerContext.Player.TriggerEvent(EventTypeEnum.AddLife, request.CardRequestContext,
                    request.CardResponseContext, request.SrcRoundContext);
                await PlayerContext.GameLevel.GlobalEventBus.TriggerEvent(EventTypeEnum.AddLife, null, request.CardRequestContext, request.SrcRoundContext,
                    request.CardResponseContext);

                var defaultRecover = AttackDynamicFactor.GetDefaultBaseAttackFactor();
                var roundContextAttackDynamicFactor = request.SrcRoundContext?.AttackDynamicFactor;
                var deltaLife = 0;
                //吸血
                if (request.RecoverType == RecoverTypeEnum.Xixue)
                {
                    deltaLife = request.CardRequestContext.AttackDynamicFactor.Recover.XixueLife + defaultRecover.Recover.XixueLife + (roundContextAttackDynamicFactor?.Recover.XixueLife ?? 0);
                    AddLife(deltaLife, BaseAttackFactor.MaxLife);
                }
                //休养生息
                else if (request.RecoverType == RecoverTypeEnum.Xiuyangshengxi)
                {
                    deltaLife = request.CardRequestContext.AttackDynamicFactor.Recover.XiuyangshengxiLife + defaultRecover.Recover.XiuyangshengxiLife + (roundContextAttackDynamicFactor?.Recover.XiuyangshengxiLife ?? 0);
                    AddLife(deltaLife, BaseAttackFactor.MaxLife);
                }
                //吃药
                else if (request.RecoverType == RecoverTypeEnum.Yao)
                {
                    deltaLife = request.CardRequestContext.AttackDynamicFactor.Recover.YaoLife + defaultRecover.Recover.YaoLife + (roundContextAttackDynamicFactor?.Recover.YaoLife ?? 0);
                    AddLife(deltaLife, BaseAttackFactor.MaxLife);
                }

                //Console.WriteLine($"{PlayerContext.Player.PlayerName + PlayerContext.Player.PlayerId}的【{Hero.DisplayName}】被“{request.RecoverType}”回复{deltaLife}血.");
                await PlayerContext.Player.TriggerEvent(EventTypeEnum.AfterAddLife, request.CardRequestContext,
                    request.CardResponseContext, request.SrcRoundContext);
                await PlayerContext.GameLevel.GlobalEventBus.TriggerEvent(EventTypeEnum.AfterAddLife, null, request.CardRequestContext, request.SrcRoundContext,
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
                this.CurrentLife = maxLife;
            }
            else
            {
                Console.WriteLine($"{PlayerContext.Player.PlayerName + PlayerContext.Player.PlayerId}的【{Hero.DisplayName}】回复{deltaLife}点血量.");
                this.CurrentLife += deltaLife;
            }
        }

        protected void InitAttackFactor(int star, HeroBase hero)
        {
            BaseAttackFactor.MaxLife = (star - 1) + hero.MaxLife;
            CurrentLife = BaseAttackFactor.MaxLife;
        }

        protected void LoadSkills(List<SkillBase> skillSet)
        {
            if (skillSet == null || !skillSet.Any())
            {
                return;
            }
            skillSet.ForEach(s =>
            {
                s.LoadSkill(this);
            });
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


        #endregion

    }
}

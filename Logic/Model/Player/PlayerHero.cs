using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Logic.ActionManger;
using Logic.GameLevel;
using Logic.Model.Cards.EquipmentCards;
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

        /// <summary>
        /// 用户行为管理器
        /// </summary>
        public IActionManager ActionManager { get; }


        public PlayerContext PlayerContext { get; private set; }

        /// <summary>
        /// 额外的主技能
        /// </summary>
        public List<SkillBase> ExtraMainSkillSet { get; }

        /// <summary>
        /// 额外的副技能
        /// </summary>
        public List<SkillBase> ExtraSubSkillSet { get; }


        public PlayerHero(int star, HeroBase hero, List<SkillBase> extraMainSkillSet, List<SkillBase> extraSubSkillSet, IActionManager actionManager)
        {
            ActionManager = actionManager;
            Star = star <= 1 ? 1 : (star >= 5 ? 5 : star);
            Hero = hero;
            BaseAttackFactor = Hero.GetBaseAttackFactor();
            //加载技能，初始化事件监听
            ExtraMainSkillSet = extraMainSkillSet;
            ExtraSubSkillSet = extraSubSkillSet;
            LoadSkills(ExtraMainSkillSet);
            LoadSkills(ExtraSubSkillSet);

            //初始化攻击因子
            InitAttackFactor(star, hero);
        }

        #region 公有方法

        public void SetPlayerContext(PlayerContext playerContext)
        {
            PlayerContext = playerContext;
        }

        /// <summary>
        /// 获取所有主技能
        /// </summary>
        /// <returns></returns>
        public List<SkillBase> GetAllMainSkills()
        {
            var skills = new List<SkillBase>();
            skills.AddRange(Hero.MainSkillSet);
            return skills.Union(ExtraMainSkillSet, new GenericCompare<SkillBase>(x => x.Name)).ToList();
        }

        /// <summary>
        /// 获取所有副技能
        /// </summary>
        /// <returns></returns>
        public List<SkillBase> GetAllSubSkills()
        {
            var skills = new List<SkillBase>();
            skills.AddRange(Hero.SubSkillSet);
            return skills.Union(ExtraSubSkillSet, new GenericCompare<SkillBase>(x => x.Name)).ToList();
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
        /// 掉血
        /// </summary>
        /// <param name="request"></param>
        /// <returns></returns>
        public async Task LoseLife(LoseLifeRequest request)
        {
            //todo:targetPlayer.triggerBeforeLoseLifeEvent
        }

        /// <summary>
        /// 回血
        /// </summary>
        /// <param name="request"></param>
        /// <returns></returns>
        public async Task AddLife(AddLifeRequest request)
        {
            //todo:targetPlayer.triggerBeforeLoseLifeEvent
            if (CurrentLife < BaseAttackFactor.MaxLife)
            {
                //todo: trigger add life events

            }
        }
        #endregion

        #region 保护方法

        protected void InitAttackFactor(int star, HeroBase hero)
        {
            BaseAttackFactor.MaxLife = (star - 1) + hero.MaxLife + BaseAttackFactor.MaxLife;
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

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
        public int CurrentLife { get; set; }

        /// <summary>
        /// 最大生命
        /// </summary>
        public int MaxLife { get; set; }

        /// <summary>
        /// 是否死亡
        /// </summary>
        public bool IsDead { get; set; }

        /// <summary>
        /// 攻击属性
        /// </summary>
        protected AttackDynamicFactor AttackFactor { get; set; }

        /// <summary>
        /// 具体的英雄
        /// </summary>
        public HeroBase Hero { get; set; }

        /// <summary>
        /// 玩家英雄星级{0-4}
        /// </summary>
        public int Star { get; set; }

        /// <summary>
        /// 用户行为管理器
        /// </summary>
        protected IActionManager _actionManager { get; set; }

        /// <summary>
        /// 额外的主技能
        /// </summary>
        public List<SkillBase> ExtraMainSkillSet { get; set; }

        /// <summary>
        /// 额外的副技能
        /// </summary>
        public List<SkillBase> ExtraSubSkillSet { get; set; }


        public PlayerHero(IActionManager actionManager)
        {
            _actionManager = actionManager;
            AttackFactor = Hero.GetBaseAttackFactor();
        }

        /// <summary>
        /// 获取当前用户行为管理器
        /// </summary>
        /// <returns></returns>
        public IActionManager GetActionManager()
        {
            return _actionManager;
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
            return AttackFactor;
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
            if (CurrentLife < MaxLife)
            {
                //todo: trigger add life events

            }
        }

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

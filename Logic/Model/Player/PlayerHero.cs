﻿using System.Collections.Generic;
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

            var defaultDamage = AttackDynamicFactor.GetDefaultBaseAttackFactor();
            var roundContextAttackDynamicFactor = request.SrcRoundContext?.AttackDynamicFactor;
            //杀
            if (request.DamageType == DamageTypeEnum.Sha)
            {
                this.CurrentLife -= request.CardRequestContext.AttackDynamicFactor.Damage.ShaDamage + defaultDamage.Damage.ShaDamage + (roundContextAttackDynamicFactor?.Damage.ShaDamage ?? 0);
            }
            //决斗
            else if (request.DamageType == DamageTypeEnum.Juedou)
            {
                this.CurrentLife -= request.CardRequestContext.AttackDynamicFactor.Damage.JuedouDamage + defaultDamage.Damage.JuedouDamage + (roundContextAttackDynamicFactor?.Damage.JuedouDamage ?? 0);
            }
            //烽火狼烟
            else if (request.DamageType == DamageTypeEnum.Fenghuolangyan)
            {
                this.CurrentLife -= request.CardRequestContext.AttackDynamicFactor.Damage.FenghuolangyanDamage + defaultDamage.Damage.FenghuolangyanDamage + (roundContextAttackDynamicFactor?.Damage.FenghuolangyanDamage ?? 0);
            }
            //万箭齐发
            else if (request.DamageType == DamageTypeEnum.Wanjianqifa)
            {
                this.CurrentLife -= request.CardRequestContext.AttackDynamicFactor.Damage.WanjianqifaDamage + defaultDamage.Damage.WanjianqifaDamage + (roundContextAttackDynamicFactor?.Damage.WanjianqifaDamage ?? 0);
            }
            //三板斧
            else if (request.DamageType == DamageTypeEnum.Sanbanfu)
            {
                //三板斧，判断杀的response到底是几个闪,如果没闪则伤害+1。
                var shaDamage = request.CardRequestContext.AttackDynamicFactor.Damage.ShaDamage +
                                defaultDamage.Damage.ShaDamage +
                                (roundContextAttackDynamicFactor?.Damage.ShaDamage ?? 0);
                if (request.CardResponseContext.Cards.Count == 0)
                {
                    shaDamage++;
                }
                this.CurrentLife -= shaDamage;
            }
            //攻心
            else if (request.DamageType == DamageTypeEnum.Gongxin)
            {
                this.CurrentLife -= request.CardRequestContext.AttackDynamicFactor.Damage.GongxinDamage + defaultDamage.Damage.GongxinDamage + (roundContextAttackDynamicFactor?.Damage.GongxinDamage ?? 0);
            }
            //未知攻击
            else
            {
                this.CurrentLife -= 1;
            }

            await PlayerContext.Player.TriggerEvent(EventTypeEnum.AfterLoseLife, request.CardRequestContext,
                request.CardResponseContext, request.SrcRoundContext);
            await PlayerContext.GameLevel.GlobalEventBus.TriggerEvent(EventTypeEnum.AfterLoseLife, null, request.CardRequestContext, request.SrcRoundContext,
                request.CardResponseContext);
        }

        /// <summary>
        /// 回血
        /// </summary>
        /// <param name="request"></param>
        /// <returns></returns>
        public async Task AddLife(AddLifeRequest request)
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
                //吸血
                if (request.RecoverType == RecoverTypeEnum.Xixue)
                {
                    var deltaLife = request.CardRequestContext.AttackDynamicFactor.Recover.XixueLife + defaultRecover.Recover.XixueLife + (roundContextAttackDynamicFactor?.Recover.XixueLife ?? 0);
                    AddLife(deltaLife, BaseAttackFactor.MaxLife);
                }
                //休养生息
                else if (request.RecoverType == RecoverTypeEnum.Xiuyangshengxi)
                {
                    var deltaLife = request.CardRequestContext.AttackDynamicFactor.Recover.XiuyangshengxiLife + defaultRecover.Recover.XiuyangshengxiLife + (roundContextAttackDynamicFactor?.Recover.XiuyangshengxiLife ?? 0);
                    AddLife(deltaLife, BaseAttackFactor.MaxLife);
                }
                //吃药
                else if (request.RecoverType == RecoverTypeEnum.Yao)
                {
                    var deltaLife = request.CardRequestContext.AttackDynamicFactor.Recover.YaoLife + defaultRecover.Recover.YaoLife + (roundContextAttackDynamicFactor?.Recover.YaoLife ?? 0);
                    AddLife(deltaLife, BaseAttackFactor.MaxLife);
                }

                await PlayerContext.Player.TriggerEvent(EventTypeEnum.AfterAddLife, request.CardRequestContext,
                    request.CardResponseContext, request.SrcRoundContext);
                await PlayerContext.GameLevel.GlobalEventBus.TriggerEvent(EventTypeEnum.AfterAddLife, null, request.CardRequestContext, request.SrcRoundContext,
                    request.CardResponseContext);
            }
        }
        #endregion

        #region 保护方法

        protected void AddLife(int deltaLife, int maxLife)
        {
            if (deltaLife + CurrentLife >= BaseAttackFactor.MaxLife)
            {
                this.CurrentLife = BaseAttackFactor.MaxLife;
            }
            else
            {
                this.CurrentLife += deltaLife;
            }
        }

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

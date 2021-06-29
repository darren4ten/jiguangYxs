using Logic.Model.Enums;
using Logic.Model.Interface;
using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;
using Logic.Model.Player;
using Logic.Model.Skill.Interface;

namespace Logic.Model.Skill
{
    public abstract class SkillBase : ISkill, IAbility
    {
        /// <summary>
        /// 技能是否已经加载
        /// </summary>
        protected bool IsLoaded { get; set; } = false;
        public string Name { get; set; }

        public string DisplayName { get; set; }
        public string Description { get; set; }

        protected PlayerHero PlayerHero { get; set; }

        public virtual Task LoadSkill(PlayerHero playerHero)
        {
            IsLoaded = true;
            PlayerHero = playerHero;
            return Task.FromResult("");
        }

        public virtual Task UnLoadSkill()
        {
            IsLoaded = false;
            return Task.FromResult("");
        }

        public virtual SkillTypeEnum SkillType()
        {
            throw new NotImplementedException();
        }

        /// <summary>
        /// 获取当前回合技能主动触发次数
        /// </summary>
        /// <returns></returns>
        public int GetZhudongTriggerTimes()
        {
            if (PlayerHero.PlayerContext.Player.RoundContext == null) return -1;
            var containsSkill = PlayerHero.PlayerContext.Player.RoundContext.SkillTriggerTimesDic?.ContainsKey(SkillType());
            if (containsSkill == null)
            {
                PlayerHero.PlayerContext.Player.RoundContext.SkillTriggerTimesDic =
                    new Dictionary<SkillTypeEnum, int>();
                return 0;
            }

            if (containsSkill == false) return 0;

            return PlayerHero.PlayerContext.Player.RoundContext.SkillTriggerTimesDic[SkillTypeEnum.SanBanfu];
        }

        #region IAbility
        public virtual bool CanProvideSha()
        {
            return false;
        }

        public bool CanProvideFuidichouxin()
        {
            return false;
        }

        public bool CanProvideJiedaosharen()
        {
            return false;
        }

        public virtual bool CanProvideShan()
        {
            return false;
        }

        public virtual bool CanProvideYao()
        {
            return false;
        }

        public virtual bool CanProviderWuxiekeji()
        {
            return false;
        }

        public virtual bool CanProvideJuedou()
        {
            return false;
        }

        public virtual bool CanProvideFenghuolangyan()
        {
            return false;
        }

        public virtual bool CanProvideWanjianqifa()
        {
            return false;
        }

        public virtual bool CanProvideTannangquwu()
        {
            return false;
        }

        public bool CanProvideWuzhongshengyou()
        {
            return false;
        }

        public bool CanProvideHudadiweilao()
        {
            return false;
        }

        public bool CanProvideXiuyangshengxi()
        {
            return false;
        }

        #endregion

    }
}

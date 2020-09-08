using Logic.Model.Enums;
using Logic.Model.Interface;
using System;
using System.Collections.Generic;
using System.Text;

namespace Logic.Model.Skill
{
    public abstract class SkillBase : ISkill
    {
        public string Name { get; set; }

        public string DisplayName { get; set; }
        public string Description { get; set; }
        public SkillTypeEnum SkillType { get; set; }

        protected SkillContext SkillContext { get; set; }

        public SkillBase(SkillContext SkillContext)
        {
            this.SkillContext = SkillContext;
        }

        public virtual bool IsSkillTriggerable()
        {
            return false;
        }

        public virtual bool IsSkillClickable()
        {
            return SkillType != SkillTypeEnum.Beidong;
        }

        ////public bool TriggerConditionFun() { }

        //public virtual void OnAddLifeCard()
        //{
        //    throw new NotImplementedException();
        //}

        //public virtual void OnAfterAddLife()
        //{
        //    throw new NotImplementedException();
        //}

        //public virtual void OnAfterBeJuedoued()
        //{
        //    throw new NotImplementedException();
        //}

        //public virtual void OnAfterDetermine()
        //{
        //    throw new NotImplementedException();
        //}

        //public virtual void OnAfterDoJuedou()
        //{
        //    throw new NotImplementedException();
        //}

        //public virtual void OnAfterDying()
        //{
        //    throw new NotImplementedException();
        //}

        //public virtual void OnAfterLoseLife()
        //{
        //    throw new NotImplementedException();
        //}

        //public virtual void OnAfterPickCard()
        //{
        //    throw new NotImplementedException();
        //}

        //public virtual void OnAfterPlayCard()
        //{
        //    throw new NotImplementedException();
        //}

        //public virtual void OnAfterPlayJinlang()
        //{
        //    throw new NotImplementedException();
        //}

        //public virtual void OnAfterPlaySha()
        //{
        //    throw new NotImplementedException();
        //}

        //public virtual void OnAfterPlayShan()
        //{
        //    throw new NotImplementedException();
        //}

        //public virtual void OnAfterThrowCard()
        //{
        //    throw new NotImplementedException();
        //}

        //public virtual void OnBeforeAddLife()
        //{
        //    throw new NotImplementedException();
        //}

        //public virtual void OnBeforeBeJuedoued()
        //{
        //    throw new NotImplementedException();
        //}

        //public virtual void OnBeforeDetermine()
        //{
        //    throw new NotImplementedException();
        //}

        //public virtual void OnBeforeDoJuedou()
        //{
        //    throw new NotImplementedException();
        //}

        //public virtual void OnBeforeDying()
        //{
        //    throw new NotImplementedException();
        //}

        //public virtual void OnBeforeLoseLife()
        //{
        //    throw new NotImplementedException();
        //}

        //public virtual void OnBeforePlayCard()
        //{
        //    throw new NotImplementedException();
        //}

        //public virtual void OnBeforePlayJinlang()
        //{
        //    throw new NotImplementedException();
        //}

        //public virtual void OnBeforePlaySha()
        //{
        //    throw new NotImplementedException();
        //}

        //public virtual void OnBeforePlayShan()
        //{
        //    throw new NotImplementedException();
        //}

        //public virtual void OnBeforeThrowCardCard()
        //{
        //    throw new NotImplementedException();
        //}

        //public virtual void OnBeJuedoued()
        //{
        //    throw new NotImplementedException();
        //}

        //public virtual void OnDetermine()
        //{
        //    throw new NotImplementedException();
        //}

        //public virtual void OnDoJuedou()
        //{
        //    throw new NotImplementedException();
        //}

        //public virtual void OnDying()
        //{
        //    throw new NotImplementedException();
        //}

        //public virtual void OnLoseLife()
        //{
        //    throw new NotImplementedException();
        //}

        //public virtual void OnPickingCard()
        //{
        //    throw new NotImplementedException();
        //}

        //public virtual void OnPickPlayCard()
        //{
        //    throw new NotImplementedException();
        //}

        //public virtual void OnPlayingCard()
        //{
        //    throw new NotImplementedException();
        //}

        //public virtual void OnPlayJinlang()
        //{
        //    throw new NotImplementedException();
        //}

        //public virtual void OnPlaySha()
        //{
        //    throw new NotImplementedException();
        //}

        //public virtual void OnPlayShan()
        //{
        //    throw new NotImplementedException();
        //}

        //public virtual void OnThrowingCard()
        //{
        //    throw new NotImplementedException();
        //}
    }
}

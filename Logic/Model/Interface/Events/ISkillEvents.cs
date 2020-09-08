using System;
using System.Collections.Generic;
using System.Text;

namespace Logic.Model.Interface.Events
{
    public interface ISkillPlayCard
    {
        void OnBeforePlayCard();
        void OnPlayingCard();
        void OnAfterPlayCard();
    }

    public interface ISkillPickCard
    {
        void OnPickCard();
        void OnPickingCard();
        void OnAfterPickCard();
    }

    public interface ISkillThrowCard
    {
        void OnBeforeThrowCard();
        void OnThrowingCard();
        void OnAfterThrowCard();
    }

    public interface ISkillAddLife
    {
        void OnBeforeAddLife();
        void OnAddLifeCard();
        void OnAfterAddLife();
    }
    public interface ISkillLoseLife
    {
        void OnBeforeLoseLife();
        void OnLoseLife();
        void OnAfterLoseLife();
    }
    public interface ISkillPlaySha
    {
        void OnBeforePlaySha();
        void OnPlaySha();
        void OnAfterPlaySha();
    }
    public interface ISkillPlayShan
    {
        void OnBeforePlayShan();
        void OnPlayShan();
        void OnAfterPlayShan();
    }
    public interface ISkillJinlang
    {
        void OnBeforePlayJinlang();
        void OnPlayJinlang();
        void OnAfterPlayJinlang();
    }
    public interface ISkillDetermine
    {
        void OnBeforeDetermine();
        void OnDetermine();
        void OnAfterDetermine();
    }
    public interface ISkillDoJuedou
    {
        void OnBeforeDoJuedou();
        void OnDoJuedou();
        void OnAfterDoJuedou();
    }
    public interface ISkillBeJuedoued
    {
        void OnBeforeBeJuedoued();
        void OnBeJuedoued();
        void OnAfterBeJuedoued();
    }
    public interface ISkillDying
    {
        void OnBeforeDying();
        void OnDying();
        void OnAfterDying();
    }
}

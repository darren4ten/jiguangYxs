using System;
using System.Collections.Generic;
using System.Text;

namespace Logic.Model.Enums
{
    /// <summary>
    /// 游戏中所有的事件枚举
    /// </summary>
    public enum GameEventsEnum
    {
        #region GameEvents

        BeforeGameStart,
        GameStart,
        AfterGameStart,
        BeforeGameEnd,
        GameEnd,
        AfterGameEnd,
        BeforePanding,
        Panding,
        AfterPanding,

        #endregion

        #region RoundEventEnum

        BeforeEnterMyRound,
        EnterMyRound,
        AfterEnterMyRound,
        PickCard,
        OnPlayCard,
        ThrowCard

        #endregion
    }

}

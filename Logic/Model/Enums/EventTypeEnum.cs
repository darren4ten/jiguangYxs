using System;
using System.Collections.Generic;
using System.Text;

namespace Logic.Model.Enums
{
    /// <summary>
    /// 所有的事件类型
    /// </summary>
    public enum EventTypeEnum
    {
        /// <summary>
        /// 出杀
        /// </summary>
        Sha,
        /// <summary>
        /// 杀之前
        /// </summary>
        BeforeSha,
        /// <summary>
        /// 出杀之后
        /// </summary>
        AfterSha,

        /// <summary>
        /// 杀成功
        /// </summary>
        ShaSuccess,
        /// <summary>
        /// 杀成功之前
        /// </summary>
        BeforeShaSuccess,
        /// <summary>
        /// 杀成功之后
        /// </summary>
        AfterShaSuccess,

        /// <summary>
        /// 杀失败
        /// </summary>
        ShaFailed,
        /// <summary>
        /// 在杀失败之前
        /// </summary>
        BeforeShaFailed,
        /// <summary>
        /// 在杀失败之后
        /// </summary>
        AfterShaFailed,

        /// <summary>
        /// 决斗成功之时
        /// </summary>
        JuedouSuccess,
        /// <summary>
        /// 决斗成功之前
        /// </summary>
        BeforeJuedouSuccess,
        /// <summary>
        /// 决斗成功之后
        /// </summary>
        AfterJuedouSuccess,

        /// <summary>
        /// 决斗失败之时
        /// </summary>
        JuedouFailed,
        /// <summary>
        /// 决斗失败之前
        /// </summary>
        BeforeJuedouFailed,
        /// <summary>
        /// 决斗失败之后
        /// </summary>
        AfterJuedouFailed,

        /// <summary>
        /// 决斗之时
        /// </summary>
        Juedou,
        /// <summary>
        /// 决斗之前
        /// </summary>
        BeforeJuedou,
        /// <summary>
        /// 决斗之后
        /// </summary>
        AfterJuedou,

        /// <summary>
        /// 被动出牌之前
        /// </summary>
        BeforeBeidongPlayCard,
        /// <summary>
        /// 被动出牌
        /// </summary>
        BeidongPlayCard,
        /// <summary>
        /// 被动出牌之后
        /// </summary>
        AfterBeidongPlayCard,

        /// <summary>
        /// 被杀
        /// </summary>
        BeShaed,
        /// <summary>
        /// 被杀前
        /// </summary>
        BeforeBeShaed,
        /// <summary>
        /// 被杀后
        /// </summary>
        AfterBeShaed,

        /// <summary>
        /// 出闪
        /// </summary>
        Shan,
        /// <summary>
        /// 出闪之前
        /// </summary>
        BeforeShan,
        /// <summary>
        /// 出闪之后
        /// </summary>
        AfterShan,

        /// <summary>
        /// 吃药之时
        /// </summary>
        EatYao,
        /// <summary>
        /// 吃药之前
        /// </summary>
        BeforeEatYao,
        /// <summary>
        /// 吃药之后
        /// </summary>
        AfterEatYao,


        /// <summary>
        /// 打出锦囊牌
        /// </summary>
        PlayJinnang,
        /// <summary>
        /// 打出锦囊之前
        /// </summary>
        BeforePlayJinnang,
        /// <summary>
        /// 打出锦囊之后
        /// </summary>
        AfterPlayJinnang,


        /// <summary>
        /// 在请求出杀之时
        /// </summary>
        BeRequestedSha,
        /// <summary>
        /// 在请求出杀之前
        /// </summary>
        BeforeRequestedSha,
        /// <summary>
        /// 在请求出杀之后
        /// </summary>
        AfterBeRequestedSha,

        /// <summary>
        /// 被请求出闪时
        /// </summary>
        BeRequestedShan,
        /// <summary>
        /// 被请求出闪之前
        /// </summary>
        BeforeBeRequestedShan,
        /// <summary>
        /// 被请求出闪之后
        /// </summary>
        AfterBeRequestedShan,

        /// <summary>
        /// 被请求出无懈可击时
        /// </summary>
        BeRequestedWuxiekeji,
        /// <summary>
        /// 被请求出无懈可击之前
        /// </summary>
        BeforeBeRequestedWuxiekeji,
        /// <summary>
        /// 被请求出无懈可击之后
        /// </summary>
        AfterBeRequestedWuxiekeji,

        /// <summary>
        /// 打出烽火狼烟之时
        /// </summary>
        PlayFenghuolangyan,
        /// <summary>
        /// 打出烽火狼烟之前
        /// </summary>
        BeforePlayFenghuolangyan,
        /// <summary>
        /// 打出烽火狼烟之后
        /// </summary>
        AfterPlayFenghuolangyan,

        /// <summary>
        /// 打出万箭齐发之时
        /// </summary>
        PlayWanjianqifa,
        /// <summary>
        /// 打出万箭齐发之前
        /// </summary>
        BeforePlayWanjianqifa,
        /// <summary>
        /// 打出万箭齐发之后
        /// </summary>
        AfterPlayWanjianqifa,

        /// <summary>
        /// 打出万箭齐发之时
        /// </summary>
        PlayWuxiekeji,
        /// <summary>
        /// 打出万箭齐发之前
        /// </summary>
        BeforePlayWuxiekeji,
        /// <summary>
        /// 打出万箭齐发之后
        /// </summary>
        AfterPlayWuxiekeji,

        /// <summary>
        /// 打出五谷丰登之时
        /// </summary>
        PlayWugufengdeng,
        /// <summary>
        /// 打出五谷丰登之前
        /// </summary>
        BeforePlayWugufengdeng,
        /// <summary>
        /// 打出五谷丰登之后
        /// </summary>
        AfterPlayWugufengdeng,

        /// <summary>
        /// 打出无中生有之时
        /// </summary>
        PlayWuzhongshengyou,
        /// <summary>
        /// 打出无中生有之前
        /// </summary>
        BeforePlayWuzhongshengyou,
        /// <summary>
        /// 打出无中生有之后
        /// </summary>
        AfterPlayWuzhongshengyou,

        /// <summary>
        /// 掉血之时
        /// </summary>
        LoseLife,
        /// <summary>
        /// 掉血之前
        /// </summary>
        BeforeLoseLife,
        /// <summary>
        /// 掉血之后
        /// </summary>
        AfterLoseLife,

        /// <summary>
        /// 回血之时
        /// </summary>
        AddLife,
        /// <summary>
        /// 回血之前
        /// </summary>
        BeforeAddLife,
        /// <summary>
        /// 回血之后
        /// </summary>
        AfterAddLife,

        /// <summary>
        /// 死亡之时
        /// </summary>
        Dying,
        /// <summary>
        /// 死亡之前
        /// </summary>
        BeforeDying,
        /// <summary>
        /// 死亡之后
        /// </summary>
        AfterDying,


        /// <summary>
        /// 判定之时
        /// </summary>
        Panding,
        /// <summary>
        /// 判定之前
        /// </summary>
        BeforePanding,
        /// <summary>
        /// 判定之后
        /// </summary>
        AfterPanding,

        /// <summary>
        /// 被检查可达之时
        /// </summary>
        BeSelectablityCheck,
        /// <summary>
        /// 被检查可达之前
        /// </summary>
        BeforeBeSelectablityCheck,
        /// <summary>
        /// 被检查可达之后
        /// </summary>
        AfterBeSelectablityCheck,

        /// <summary>
        /// 游戏开始之时
        /// </summary>
        GameStart,
        /// <summary>
        /// 游戏开始之前
        /// </summary>
        BeforeGameStart,
        /// <summary>
        /// 游戏开始之后
        /// </summary>
        AfterGameStart,

        /// <summary>
        /// 游戏结束之时
        /// </summary>
        GameEnd,
        /// <summary>
        /// 游戏结束之前
        /// </summary>
        BeforeGameEnd,
        /// <summary>
        /// 游戏结束之后
        /// </summary>
        AfterGameEnd,

        /// <summary>
        /// 进入我的回合之时
        /// </summary>
        EnterMyRound,
        /// <summary>
        /// 进入我的回合之前
        /// </summary>
        BeforeEnterMyRound,
        /// <summary>
        /// 进入我的回合之后
        /// </summary>
        AfterEnterMyRound,

        /// <summary>
        /// 摸牌之时
        /// </summary>
        PickCard,
        /// <summary>
        /// 摸牌之前
        /// </summary>
        BeforePickCard,
        /// <summary>
        /// 摸牌之后
        /// </summary>
        AfterPickCard,

        /// <summary>
        /// 丢失手牌之时
        /// </summary>
        LoseCardsInHand,
        /// <summary>
        /// 丢失手牌之前
        /// </summary>
        BeforeLoseCardsInHand,
        /// <summary>
        /// 丢失手牌之后
        /// </summary>
        AfterLoseCardsInHand,

        /// <summary>
        /// 弃牌之时
        /// </summary>
        ThrowCard,
        /// <summary>
        /// 弃牌之前
        /// </summary>
        BeforeThrowCard,
        /// <summary>
        /// 弃牌之后
        /// </summary>
        AfterThrowCard,

        /// <summary>
        /// 回合结束之时
        /// </summary>
        EndRound,
        /// <summary>
        /// 回合结束之前
        /// </summary>
        BeforeEndRound,
        /// <summary>
        /// 回合结束之后
        /// </summary>
        AfterEndRound,

        /// <summary>
        /// 装备武器之时
        /// </summary>
        Equip,
        /// <summary>
        /// 装备武器之前
        /// </summary>
        BeforeEquip,
        /// <summary>
        /// 装备武器之后
        /// </summary>
        AfterEquip,

        /// <summary>
        /// 卸载装备之时
        /// </summary>
        UnEquip,
        /// <summary>
        /// 卸载装备之前
        /// </summary>
        BeforeUnEquip,
        /// <summary>
        /// 卸载装备之后
        /// </summary>
        AfterUnEquip,

        /// <summary>
        /// 主动出牌之前
        /// </summary>
        BeforeZhudongPlayCard,
        /// <summary>
        /// 主动出牌之时
        /// </summary>
        ZhudongPlayCard,
        /// <summary>
        /// 主动出牌之后
        /// </summary>
        AfterZhudongPlayCard,
    }
}

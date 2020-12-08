using System;
using System.Collections.Generic;
using System.Text;
using Logic.Cards;
using Logic.Enums;
using Logic.Model.Enums;
using Logic.Model.Player;

namespace Logic.GameLevel
{
    /// <summary>
    /// 请求出牌的上下文
    /// </summary>
    public class CardRequestContext
    {
        /// <summary>
        /// 最大出牌数
        /// </summary>
        public int MaxCardCountToPlay { get; set; }

        /// <summary>
        /// 最小出牌数
        /// </summary>
        public int MinCardCountToPlay { get; set; }

        /// <summary>
        /// 需要出的牌。如果是任意牌，则对应的CardType=any,RequestCard=null
        /// </summary>
        public CardBase RequestCard { get; set; }

        /// <summary>
        /// 卡牌类型
        /// </summary>
        public CardTypeEnum CardType { get; set; }

        /// <summary>
        /// 花色要求
        /// </summary>
        public FlowerKindEnum FlowerKind { get; set; }

        /// <summary>
        /// 花色要求
        /// </summary>
        public AttackTypeEnum AttackType { get; set; }

        /// <summary>
        /// 攻击者
        /// </summary>
        public Player SrcPlayer { get; set; }

        /// <summary>
        /// 被攻击者
        /// </summary>
        public List<Player> TargetPlayers { get; set; }

        /// <summary>
        /// 用来攻击的来源牌
        /// </summary>
        public List<CardBase> SrcCards { get; set; }

        /// <summary>
        /// 攻击加成信息
        /// </summary>
        public AttackDynamicFactor AttackDynamicFactor { get; set; }

        /// <summary>
        /// 提示信息
        /// </summary>
        public string Message { get; set; }
    }
}

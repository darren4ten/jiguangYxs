using System;
using System.Collections.Generic;
using System.Text;

namespace WpfApp1.Model
{
    public class CardBase
    {
        public int Number { get; set; }

        public CardColorEnum CardColor { get; set; }

        public CardTypeEnum CardType { get; set; }
    }

    public enum CardColorEnum
    {
        Hongtao,
        Heitao,
        Fangkuai,
        Meihua
    }

    public enum CardTypeEnum
    {
        Sha,
        Shan,
        Juedou,
        Wuxiekeji
    }
}
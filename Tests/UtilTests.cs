using Logic.Cards;
using Logic.Util;
using NUnit.Framework;
using System;
using System.Text;
using System.Linq;

namespace Tests
{
    public class UtilTests
    {
        CardStackUtil util;
        [SetUp]
        public void Setup()
        {
            util = new CardStackUtil();
        }

        protected string GetCardText(CardBase card)
        {
            if (null == card) return "";
            string flower = "", displayName;
            displayName = (string.IsNullOrEmpty(card.DisplayNumberText) ? card.Number.ToString() : card.DisplayNumberText) + " " + card.DisplayName;
            if (card.FlowerKind == Logic.Enums.FlowerKindEnum.Fangkuai)
            {
                flower = "◇";
            }
            else if (card.FlowerKind == Logic.Enums.FlowerKindEnum.Heitao)
            {
                flower = "♠";
            }
            else if (card.FlowerKind == Logic.Enums.FlowerKindEnum.Hongtao)
            {
                flower = "❤";
            }
            else
            {
                flower = "♣";
            }
            return $"{flower} {displayName}- {card.Description}";
        }

        [Test]
        public void Test1()
        {
            var cards = util.GenerateNewCardStack();
            int total = 0;
            StringBuilder sb = new StringBuilder();
            foreach (var c in cards)
            {
                total++;
                sb.Append(GetCardText(c) + "\r\n");
            }
            sb.Append($"-------Total-{total}------");
            string cardsStr = sb.ToString();

            var dicResult = cards.GroupBy(c => c.Name).ToDictionary(k => k.Key, v => v.Count());

            Assert.AreEqual(total, 106);
            Assert.AreEqual(dicResult["Sha"], 30);
            Assert.AreEqual(dicResult["Shan"], 15);
            Assert.AreEqual(dicResult["Yao"], 8);
            Assert.AreEqual(dicResult["Shoupenglei"], 2);
            Assert.AreEqual(dicResult["Wuxiekeji"], 4);
            Assert.AreEqual(dicResult["Huadiweilao"], 3);
            Assert.AreEqual(dicResult["Jiedaosharen"], 2);
            Assert.AreEqual(dicResult["Wugufengdeng"], 2);
            Assert.AreEqual(dicResult["Wuzhongshengyou"], 4);
            Assert.AreEqual(dicResult["Xiuyangshengxi"], 1);
            Assert.AreEqual(dicResult["Fenghuolangyan"], 3);
            Assert.AreEqual(dicResult["Wanjianqifa"], 1);
            Assert.AreEqual(dicResult["Tannangquwu"], 5);
            Assert.AreEqual(dicResult["Fudichouxin"],6);
            Assert.AreEqual(dicResult["Hufu"], 2);
            Assert.AreEqual(dicResult["Yuruyi"], 2);
            Assert.AreEqual(dicResult["Fangyuma"], 3);
            Assert.AreEqual(dicResult["Jingongma"], 3);
            Assert.AreEqual(dicResult["Longlindao"], 1);
            Assert.AreEqual(dicResult["Panlonggun"], 1);
            Assert.AreEqual(dicResult["Yuchangjian"], 1);
            Assert.AreEqual(dicResult["Bolangchui"], 1);
            Assert.AreEqual(dicResult["Langyabang"], 1);
            Assert.AreEqual(dicResult["Bawanggong"], 1);
            Assert.AreEqual(dicResult["Luyeqiang"], 1);

        }
    }
}
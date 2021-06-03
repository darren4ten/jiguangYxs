using System.Collections.Generic;
using Logic.Cards;
using Logic.Enums;
using Logic.Model.Cards.BaseCards;
using Logic.Model.Cards.EquipmentCards;
using Logic.Model.Cards.EquipmentCards.Defense;
using Logic.Model.Cards.JinlangCards;
using Logic.Model.Enums;

namespace Logic.Util
{
    public class CardStackUtil
    {
        public IEnumerable<CardBase> GenerateNewCardStack()
        {
            #region cardsStack

            var cardsStack = new List<CardBase>
            {
                #region Tannangquwu

                new Tannangquwu
                {
                    CardId = 1,
                    FlowerKind = FlowerKindEnum.Hongtao,
                    Number = 3,
                    Color = CardColorEnum.Red
                },
                new Tannangquwu
                {
                    CardId = 2,
                    FlowerKind = FlowerKindEnum.Hongtao,
                    Number = 4,
                    Color = CardColorEnum.Red
                },
                new Tannangquwu
                {
                    CardId = 3,
                    FlowerKind = FlowerKindEnum.Meihua,
                    Number = 3,
                    Color = CardColorEnum.Black
                },
                new Tannangquwu
                {
                    CardId = 4,
                    FlowerKind = FlowerKindEnum.Meihua,
                    Number = 4,
                    Color = CardColorEnum.Black
                },
                new Tannangquwu
                {
                    CardId = 5,
                    FlowerKind = FlowerKindEnum.Meihua,
                    Number = 11,
                    DisplayNumberText = "J",
                    Color = CardColorEnum.Black
                },

                #endregion

                #region Shoupenglei

                new Shoupenglei
                {
                    CardId = 6,
                    FlowerKind = FlowerKindEnum.Fangkuai,
                    Number = 12,
                    DisplayNumberText = "Q",
                    Color = CardColorEnum.Red
                },
                new Shoupenglei
                {
                    CardId = 7,
                    FlowerKind = FlowerKindEnum.Meihua,
                    Number = 1,
                    DisplayNumberText = "A",
                    Color = CardColorEnum.Black
                },

                #endregion

                #region Fudichouxin

                new Fudichouxin
                {
                    CardId = 8,
                    FlowerKind = FlowerKindEnum.Heitao,
                    Number = 3,
                    DisplayNumberText = "3",
                    Color = CardColorEnum.Black
                },
                new Fudichouxin
                {
                    CardId = 9,
                    FlowerKind = FlowerKindEnum.Heitao,
                    Number = 4,
                    Color = CardColorEnum.Black
                },
                new Fudichouxin
                {
                    CardId = 10,
                    FlowerKind = FlowerKindEnum.Meihua,
                    Number = 3,
                    Color = CardColorEnum.Black
                },
                new Fudichouxin
                {
                    CardId = 11,
                    FlowerKind = FlowerKindEnum.Meihua,
                    Number = 4,
                    Color = CardColorEnum.Black
                },
                new Fudichouxin
                {
                    CardId = 12,
                    FlowerKind = FlowerKindEnum.Fangkuai,
                    Number = 12,
                    Color = CardColorEnum.Red
                },
                new Fudichouxin
                {
                    CardId = 13,
                    FlowerKind = FlowerKindEnum.Meihua,
                    Number = 12,
                    Color = CardColorEnum.Black
                },

                #endregion

                #region Huadiweilao

                new Huadiweilao
                {
                    CardId = 14,
                    FlowerKind = FlowerKindEnum.Meihua,
                    Number = 6,
                    Color = CardColorEnum.Black
                },
                new Huadiweilao
                {
                    CardId = 15,
                    FlowerKind = FlowerKindEnum.Heitao,
                    Number = 6,
                    Color = CardColorEnum.Black
                },
                new Huadiweilao
                {
                    CardId = 16,
                    FlowerKind = FlowerKindEnum.Fangkuai,
                    Number = 6,
                    Color = CardColorEnum.Black
                },

                #endregion

                #region Yao

                new Yao
                {
                    CardId = 17,
                    FlowerKind = FlowerKindEnum.Hongtao,
                    Number = 3,
                    Color = CardColorEnum.Red
                },
                new Yao
                {
                    CardId = 18,
                    FlowerKind = FlowerKindEnum.Hongtao,
                    Number = 4,
                    Color = CardColorEnum.Red
                },
                new Yao
                {
                    CardId = 19,
                    FlowerKind = FlowerKindEnum.Hongtao,
                    Number = 6,
                    Color = CardColorEnum.Red
                },
                new Yao
                {
                    CardId = 20,
                    FlowerKind = FlowerKindEnum.Hongtao,
                    Number = 7,
                    Color = CardColorEnum.Red
                },
                new Yao
                {
                    CardId = 21,
                    FlowerKind = FlowerKindEnum.Hongtao,
                    Number = 8,
                    Color = CardColorEnum.Red
                },
                new Yao
                {
                    CardId = 22,
                    FlowerKind = FlowerKindEnum.Hongtao,
                    Number = 9,
                    Color = CardColorEnum.Red
                },
                new Yao
                {
                    CardId = 23,
                    FlowerKind = FlowerKindEnum.Hongtao,
                    Number = 12,
                    DisplayNumberText = "Q",
                    Color = CardColorEnum.Red
                },
                new Yao
                {
                    CardId = 24,
                    FlowerKind = FlowerKindEnum.Hongtao,
                    Number = 12,
                    DisplayNumberText = "Q",
                    Color = CardColorEnum.Red
                },

                #endregion

                #region Fenghuolangyan

                new Fenghuolangyan
                {
                    CardId = 25,
                    FlowerKind = FlowerKindEnum.Meihua,
                    Number = 7,
                    Color = CardColorEnum.Black
                },
                new Fenghuolangyan
                {
                    CardId = 26,
                    FlowerKind = FlowerKindEnum.Heitao,
                    Number = 7,
                    Color = CardColorEnum.Black
                },
                new Fenghuolangyan
                {
                    CardId = 27,
                    FlowerKind = FlowerKindEnum.Meihua,
                    Number = 13,
                    DisplayNumberText = "K",
                    Color = CardColorEnum.Black
                },

                #endregion

                #region Wanjianqifa

                new Wanjianqifa
                {
                    CardId = 28,
                    FlowerKind = FlowerKindEnum.Fangkuai,
                    Number = 1,
                    DisplayNumberText = "A",
                    Color = CardColorEnum.Red
                },

                #endregion

                #region Wuxiekeji

                new Wuxiekeji
                {
                    CardId = 29,
                    FlowerKind = FlowerKindEnum.Hongtao,
                    Number = 12,
                    DisplayNumberText = "Q",
                    Color = CardColorEnum.Red
                },
                new Wuxiekeji
                {
                    CardId = 30,
                    FlowerKind = FlowerKindEnum.Meihua,
                    Number = 11,
                    DisplayNumberText = "J",
                    Color = CardColorEnum.Black
                },
                new Wuxiekeji
                {
                    CardId = 31,
                    FlowerKind = FlowerKindEnum.Heitao,
                    Number = 12,
                    DisplayNumberText = "Q",
                    Color = CardColorEnum.Black
                },
                new Wuxiekeji
                {
                    CardId = 32,
                    FlowerKind = FlowerKindEnum.Heitao,
                    Number = 13,
                    DisplayNumberText = "K",
                    Color = CardColorEnum.Black
                },

                #endregion

                #region Juedou

                new Juedou
                {
                    CardId = 33,
                    FlowerKind = FlowerKindEnum.Heitao,
                    Number = 1,
                    DisplayNumberText = "A",
                    Color = CardColorEnum.Black
                },
                new Juedou
                {
                    CardId = 34,
                    FlowerKind = FlowerKindEnum.Hongtao,
                    Number = 1,
                    DisplayNumberText = "A",
                    Color = CardColorEnum.Red
                },
                new Juedou
                {
                    CardId = 35,
                    FlowerKind = FlowerKindEnum.Meihua,
                    Number = 1,
                    DisplayNumberText = "A",
                    Color = CardColorEnum.Black
                },

                #endregion

                #region Wugufengdeng

                new Wugufengdeng
                {
                    CardId = 36,
                    FlowerKind = FlowerKindEnum.Fangkuai,
                    Number = 3,
                    Color = CardColorEnum.Red
                },
                new Wugufengdeng
                {
                    CardId = 37,
                    FlowerKind = FlowerKindEnum.Fangkuai,
                    Number = 4,
                    Color = CardColorEnum.Red
                },

                #endregion

                #region Xiuyangshengxi

                new Xiuyangshengxi
                {
                    CardId = 38,
                    FlowerKind = FlowerKindEnum.Fangkuai,
                    Number = 1,
                    DisplayNumberText = "A",
                    Color = CardColorEnum.Red
                },

                #endregion

                #region Wuzhongshengyou

                new Wuzhongshengyou
                {
                    CardId = 39,
                    FlowerKind = FlowerKindEnum.Hongtao,
                    Number = 7,
                    Color = CardColorEnum.Red
                },
                new Wuzhongshengyou
                {
                    CardId = 40,
                    FlowerKind = FlowerKindEnum.Hongtao,
                    Number = 8,
                    Color = CardColorEnum.Red
                },
                new Wuzhongshengyou
                {
                    CardId = 41,
                    FlowerKind = FlowerKindEnum.Hongtao,
                    Number = 9,
                    Color = CardColorEnum.Red
                },
                new Wuzhongshengyou
                {
                    CardId = 42,
                    FlowerKind = FlowerKindEnum.Hongtao,
                    Number = 11,
                    DisplayNumberText = "J",
                    Color = CardColorEnum.Red
                },

                #endregion

                #region Jiedaosharen

                new Jiedaosharen
                {
                    CardId = 43,
                    FlowerKind = FlowerKindEnum.Heitao,
                    Number = 12,
                    DisplayNumberText = "Q",
                    Color = CardColorEnum.Black
                },
                new Jiedaosharen
                {
                    CardId = 44,
                    FlowerKind = FlowerKindEnum.Heitao,
                    Number = 13,
                    DisplayNumberText = "K",
                    Color = CardColorEnum.Black
                },

                #endregion,

                #region Yuruyi

                new Yuruyi
                {
                    CardId = 45,
                    FlowerKind = FlowerKindEnum.Heitao,
                    Number = 2,
                    Color = CardColorEnum.Black
                },
                new Yuruyi
                {
                    CardId = 46,
                    FlowerKind = FlowerKindEnum.Meihua,
                    Number = 2,
                    Color = CardColorEnum.Black
                },

                #endregion

                #region Yuchangjian

                new Yuchangjian
                {
                    CardId = 47,
                    FlowerKind = FlowerKindEnum.Meihua,
                    Number = 6,
                    Color = CardColorEnum.Black
                },

                #endregion

                #region Longlindao

                new Longlindao
                {
                    CardId = 48,
                    FlowerKind = FlowerKindEnum.Meihua,
                    Number = 2,
                    Color = CardColorEnum.Black
                },

                #endregion

                #region Panlonggun

                new Panlonggun
                {
                    CardId = 49,
                    FlowerKind = FlowerKindEnum.Meihua,
                    Number = 5,
                    Color = CardColorEnum.Black
                },

                #endregion

                #region Luyeqiang

                new Luyeqiang
                {
                    CardId = 50,
                    FlowerKind = FlowerKindEnum.Meihua,
                    Number = 12,
                    DisplayNumberText = "Q",
                    Color = CardColorEnum.Black
                },

                #endregion

                #region Bolangchui

                new Bolangchui
                {
                    CardId = 51,
                    FlowerKind = FlowerKindEnum.Fangkuai,
                    Number = 5,
                    Color = CardColorEnum.Red
                },

                #endregion

                #region Hufu

                new Hufu
                {
                    CardId = 52,
                    FlowerKind = FlowerKindEnum.Heitao,
                    Number = 1,
                    DisplayNumberText = "A",
                    Color = CardColorEnum.Black
                },
                new Hufu
                {
                    CardId = 53,
                    FlowerKind = FlowerKindEnum.Hongtao,
                    Number = 1,
                    DisplayNumberText = "A",
                    Color = CardColorEnum.Red
                },

                #endregion

                #region Bawanggong

                new Bawanggong
                {
                    CardId = 54,
                    FlowerKind = FlowerKindEnum.Heitao,
                    Number = 5,
                    Color = CardColorEnum.Red
                },

                #endregion

                #region Langyabang

                new Langyabang
                {
                    CardId = 55,
                    FlowerKind = FlowerKindEnum.Hongtao,
                    Number = 12,
                    DisplayNumberText = "Q",
                    Color = CardColorEnum.Red
                },

                #endregion

                #region Jingongma

                new Jingongma
                {
                    CardId = 56,
                    FlowerKind = FlowerKindEnum.Hongtao,
                    Number = 5,
                    Color = CardColorEnum.Red
                },
                new Jingongma
                {
                    CardId = 57,
                    FlowerKind = FlowerKindEnum.Hongtao,
                    Number = 13,
                    DisplayNumberText = "K",
                    Color = CardColorEnum.Red
                },
                new Jingongma
                {
                    CardId = 58,
                    FlowerKind = FlowerKindEnum.Meihua,
                    Number = 13,
                    DisplayNumberText = "K",
                    Color = CardColorEnum.Black
                },

                #endregion

                #region Fangyuma

                new Fangyuma
                {
                    CardId = 59,
                    FlowerKind = FlowerKindEnum.Fangkuai,
                    Number = 5,
                    Color = CardColorEnum.Red
                },
                new Fangyuma
                {
                    CardId = 60,
                    FlowerKind = FlowerKindEnum.Meihua,
                    Number = 5,
                    Color = CardColorEnum.Black
                },
                new Fangyuma
                {
                    CardId = 61,
                    FlowerKind = FlowerKindEnum.Heitao,
                    Number = 5,
                    Color = CardColorEnum.Black
                },

                #endregion

                #region Sha

                new Sha
                {
                    CardId = 62,
                    FlowerKind = FlowerKindEnum.Fangkuai,
                    Number = 7,
                    Color = CardColorEnum.Red
                },
                new Sha
                {
                    CardId = 63,
                    FlowerKind = FlowerKindEnum.Fangkuai,
                    Number = 8,
                    Color = CardColorEnum.Red
                },
                new Sha
                {
                    CardId = 64,
                    FlowerKind = FlowerKindEnum.Fangkuai,
                    Number = 9,
                    Color = CardColorEnum.Red
                },
                new Sha
                {
                    CardId = 65,
                    FlowerKind = FlowerKindEnum.Fangkuai,
                    Number = 10,
                    Color = CardColorEnum.Red
                },
                new Sha
                {
                    CardId = 66,
                    FlowerKind = FlowerKindEnum.Fangkuai,
                    Number = 13,
                    DisplayNumberText = "K",
                    Color = CardColorEnum.Red
                },

                new Sha
                {
                    CardId = 67,
                    FlowerKind = FlowerKindEnum.Hongtao,
                    Number = 6,
                    Color = CardColorEnum.Red
                },
                new Sha
                {
                    CardId = 68,
                    FlowerKind = FlowerKindEnum.Hongtao,
                    Number = 10,
                    Color = CardColorEnum.Red
                },
                new Sha
                {
                    CardId = 69,
                    FlowerKind = FlowerKindEnum.Hongtao,
                    Number = 10,
                    Color = CardColorEnum.Red
                },
                new Sha
                {
                    CardId = 70,
                    FlowerKind = FlowerKindEnum.Hongtao,
                    Number = 11,
                    DisplayNumberText = "J",
                    Color = CardColorEnum.Red
                },

                new Sha
                {
                    CardId = 71,
                    FlowerKind = FlowerKindEnum.Meihua,
                    Number = 7,
                    Color = CardColorEnum.Black
                },
                new Sha
                {
                    CardId = 72,
                    FlowerKind = FlowerKindEnum.Meihua,
                    Number = 8,
                    Color = CardColorEnum.Black
                },
                new Sha
                {
                    CardId = 73,
                    FlowerKind = FlowerKindEnum.Meihua,
                    Number = 9,
                    Color = CardColorEnum.Black
                },
                new Sha
                {
                    CardId = 74,
                    FlowerKind = FlowerKindEnum.Meihua,
                    Number = 10,
                    Color = CardColorEnum.Black
                },
                new Sha
                {
                    CardId = 75,
                    FlowerKind = FlowerKindEnum.Meihua,
                    Number = 8,
                    Color = CardColorEnum.Black
                },
                new Sha
                {
                    CardId = 76,
                    FlowerKind = FlowerKindEnum.Meihua,
                    Number = 9,
                    Color = CardColorEnum.Black
                },
                new Sha
                {
                    CardId = 77,
                    FlowerKind = FlowerKindEnum.Meihua,
                    Number = 10,
                    Color = CardColorEnum.Black
                },

                new Sha
                {
                    CardId = 78,
                    FlowerKind = FlowerKindEnum.Heitao,
                    Number = 2,
                    Color = CardColorEnum.Black
                },
                new Sha
                {
                    CardId = 79,
                    FlowerKind = FlowerKindEnum.Heitao,
                    Number = 3,
                    Color = CardColorEnum.Black
                },
                new Sha
                {
                    CardId = 80,
                    FlowerKind = FlowerKindEnum.Heitao,
                    Number = 4,
                    Color = CardColorEnum.Black
                },
                new Sha
                {
                    CardId = 81,
                    FlowerKind = FlowerKindEnum.Heitao,
                    Number = 5,
                    Color = CardColorEnum.Black
                },
                new Sha
                {
                    CardId = 82,
                    FlowerKind = FlowerKindEnum.Heitao,
                    Number = 5,
                    Color = CardColorEnum.Black
                },
                new Sha
                {
                    CardId = 83,
                    FlowerKind = FlowerKindEnum.Heitao,
                    Number = 6,
                    Color = CardColorEnum.Black
                },
                new Sha
                {
                    CardId = 84,
                    FlowerKind = FlowerKindEnum.Heitao,
                    Number = 7,
                    Color = CardColorEnum.Black
                },
                new Sha
                {
                    CardId = 85,
                    FlowerKind = FlowerKindEnum.Heitao,
                    Number = 8,
                    Color = CardColorEnum.Black
                },
                new Sha
                {
                    CardId = 86,
                    FlowerKind = FlowerKindEnum.Heitao,
                    Number = 9,
                    Color = CardColorEnum.Black
                },
                new Sha
                {
                    CardId = 87,
                    FlowerKind = FlowerKindEnum.Heitao,
                    Number = 10,
                    Color = CardColorEnum.Black
                },
                new Sha
                {
                    CardId = 88,
                    FlowerKind = FlowerKindEnum.Heitao,
                    Number = 8,
                    Color = CardColorEnum.Black
                },
                new Sha
                {
                    CardId = 89,
                    FlowerKind = FlowerKindEnum.Heitao,
                    Number = 9,
                    Color = CardColorEnum.Black
                },
                new Sha
                {
                    CardId = 90,
                    FlowerKind = FlowerKindEnum.Heitao,
                    Number = 10,
                    Color = CardColorEnum.Black
                },
                new Sha
                {
                    CardId = 91,
                    FlowerKind = FlowerKindEnum.Heitao,
                    Number = 11,
                    DisplayNumberText = "J",
                    Color = CardColorEnum.Black
                },

                #endregion

                #region shan

                new Shan
                {
                    CardId = 92,
                    FlowerKind = FlowerKindEnum.Fangkuai,
                    Number = 2,
                    Color = CardColorEnum.Red
                },
                new Shan
                {
                    CardId = 93,
                    FlowerKind = FlowerKindEnum.Fangkuai,
                    Number = 3,
                    Color = CardColorEnum.Red
                },
                new Shan
                {
                    CardId = 94,
                    FlowerKind = FlowerKindEnum.Fangkuai,
                    Number = 4,
                    Color = CardColorEnum.Red
                },
                new Shan
                {
                    CardId = 95,
                    FlowerKind = FlowerKindEnum.Fangkuai,
                    Number = 5,
                    Color = CardColorEnum.Red
                },
                new Shan
                {
                    CardId = 96,
                    FlowerKind = FlowerKindEnum.Fangkuai,
                    Number = 6,
                    Color = CardColorEnum.Red
                },
                new Shan
                {
                    CardId = 97,
                    FlowerKind = FlowerKindEnum.Fangkuai,
                    Number = 7,
                    Color = CardColorEnum.Red
                },
                new Shan
                {
                    CardId = 98,
                    FlowerKind = FlowerKindEnum.Fangkuai,
                    Number = 8,
                    Color = CardColorEnum.Red
                },
                new Shan
                {
                    CardId = 99,
                    FlowerKind = FlowerKindEnum.Fangkuai,
                    Number = 9,
                    Color = CardColorEnum.Red
                },
                new Shan
                {
                    CardId = 100,
                    FlowerKind = FlowerKindEnum.Fangkuai,
                    Number = 10,
                    Color = CardColorEnum.Red
                },
                new Shan
                {
                    CardId = 101,
                    FlowerKind = FlowerKindEnum.Fangkuai,
                    Number = 11,
                    DisplayNumberText = "J",
                    Color = CardColorEnum.Red
                },
                new Shan
                {
                    CardId = 102,
                    FlowerKind = FlowerKindEnum.Fangkuai,
                    Number = 11,
                    DisplayNumberText = "J",
                    Color = CardColorEnum.Red
                },

                new Shan
                {
                    CardId = 103,
                    FlowerKind = FlowerKindEnum.Hongtao,
                    Number = 2,
                    Color = CardColorEnum.Red
                },
                new Shan
                {
                    CardId = 104,
                    FlowerKind = FlowerKindEnum.Hongtao,
                    Number = 2,
                    Color = CardColorEnum.Red
                },
                new Shan
                {
                    CardId = 105,
                    FlowerKind = FlowerKindEnum.Hongtao,
                    Number = 13,
                    DisplayNumberText = "K",
                    Color = CardColorEnum.Red
                },
                new Shan
                {
                    CardId = 106,
                    FlowerKind = FlowerKindEnum.Hongtao,
                    Number = 13,
                    DisplayNumberText = "K",
                    Color = CardColorEnum.Red
                },

                #endregion
            };

            #endregion

            //random the cards.
            return RandomUtil.GetShuffleArray(cardsStack);
        }

        public IEnumerable<CardBase> GenerateNewCardStack(List<CardBase> cards)
        {
            return RandomUtil.GetShuffleArray(cards);
        }
    }
}
using Logic.Cards;
using System;
using System.Collections.Generic;
using System.Text;
using Logic.Model.Cards.BaseCards;
using Logic.Model.Cards.EquipmentCards;
using Logic.Model.Cards.EquipmentCards.Defense;
using Logic.Model.Cards.JinlangCards;

namespace Logic.Util
{
    public class CardStackUtil
    {
        public IEnumerable<CardBase> GenerateNewCardStack()
        {
            #region cardsStack
            List<CardBase> cardsStack = new List<CardBase>()
            {
                #region Tannangquwu
                new Tannangquwu()
                {
                    CardId = 1,
                    FlowerKind= Enums.FlowerKindEnum.Hongtao,
                    Number=3,
                    Color=  Model.Enums.CardColorEnum.Red
                },
                new Tannangquwu()
                {
                    CardId = 2,
                    FlowerKind= Enums.FlowerKindEnum.Hongtao,
                    Number=4,
                    Color=  Model.Enums.CardColorEnum.Red
                },
                new Tannangquwu()
                {
                    CardId = 3,
                    FlowerKind= Enums.FlowerKindEnum.Meihua,
                    Number=3,
                    Color=  Model.Enums.CardColorEnum.Black
                },
                new Tannangquwu()
                {
                    CardId = 4,
                    FlowerKind= Enums.FlowerKindEnum.Meihua,
                    Number=4,
                    Color=  Model.Enums.CardColorEnum.Black
                },
                new Tannangquwu()
                {
                    CardId = 5,
                    FlowerKind= Enums.FlowerKindEnum.Meihua,
                    Number=11,
                    DisplayNumberText="J",
                    Color=  Model.Enums.CardColorEnum.Black
                },
                #endregion

                #region Shoupenglei
                new Shoupenglei()
                {
                    CardId = 6,
                    FlowerKind= Enums.FlowerKindEnum.Fangkuai,
                    Number=12,
                    DisplayNumberText="Q",
                    Color=  Model.Enums.CardColorEnum.Red
                },
                new Shoupenglei()
                {
                    CardId = 7,
                    FlowerKind= Enums.FlowerKindEnum.Meihua,
                    Number=1,
                    DisplayNumberText="A",
                    Color=  Model.Enums.CardColorEnum.Black
                },
                #endregion

                #region Fudichouxin
                new Fudichouxin()
                {
                    CardId = 8,
                    FlowerKind= Enums.FlowerKindEnum.Heitao,
                    Number=3,
                    DisplayNumberText="3",
                    Color=  Model.Enums.CardColorEnum.Black
                },
                 new Fudichouxin()
                {
                    CardId = 9,
                    FlowerKind= Enums.FlowerKindEnum.Heitao,
                    Number=4,
                    Color=  Model.Enums.CardColorEnum.Black
                },
                  new Fudichouxin()
                {
                    CardId = 10,
                    FlowerKind= Enums.FlowerKindEnum.Meihua,
                    Number=3,
                    Color=  Model.Enums.CardColorEnum.Black
                },
                   new Fudichouxin()
                {
                    CardId = 11,
                    FlowerKind= Enums.FlowerKindEnum.Meihua,
                    Number=4,
                    Color=  Model.Enums.CardColorEnum.Black
                },
                    new Fudichouxin()
                {
                    CardId = 12,
                    FlowerKind= Enums.FlowerKindEnum.Fangkuai,
                    Number=12,
                    Color=  Model.Enums.CardColorEnum.Red
                },
                     new Fudichouxin()
                {
                    CardId = 13,
                    FlowerKind= Enums.FlowerKindEnum.Meihua,
                    Number=12,
                    Color=  Model.Enums.CardColorEnum.Black
                },

                #endregion

                #region Huadiweilao
                new Huadiweilao()
                {
                    CardId = 14,
                    FlowerKind= Enums.FlowerKindEnum.Meihua,
                    Number=6,
                    Color=  Model.Enums.CardColorEnum.Black
                },
                 new Huadiweilao()
                {
                    CardId = 15,
                    FlowerKind= Enums.FlowerKindEnum.Heitao,
                    Number=6,
                    Color=  Model.Enums.CardColorEnum.Black
                },
                  new Huadiweilao()
                {
                    CardId = 16,
                    FlowerKind= Enums.FlowerKindEnum.Fangkuai,
                    Number=6,
                    Color=  Model.Enums.CardColorEnum.Black
                },
                #endregion

                #region Yao
                new Yao()
                {
                    CardId = 17,
                    FlowerKind= Enums.FlowerKindEnum.Hongtao,
                    Number=3,
                    Color=  Model.Enums.CardColorEnum.Red
                },
                new Yao()
                {
                    CardId = 18,
                    FlowerKind= Enums.FlowerKindEnum.Hongtao,
                    Number=4,
                    Color=  Model.Enums.CardColorEnum.Red
                },
                new Yao()
                {
                    CardId = 19,
                    FlowerKind= Enums.FlowerKindEnum.Hongtao,
                    Number=6,
                    Color=  Model.Enums.CardColorEnum.Red
                },
                new Yao()
                {
                    CardId = 20,
                    FlowerKind= Enums.FlowerKindEnum.Hongtao,
                    Number=7,
                    Color=  Model.Enums.CardColorEnum.Red
                },
                new Yao()
                {
                    CardId = 21,
                    FlowerKind= Enums.FlowerKindEnum.Hongtao,
                    Number=8,
                    Color=  Model.Enums.CardColorEnum.Red
                },
                new Yao()
                {
                    CardId = 22,
                    FlowerKind= Enums.FlowerKindEnum.Hongtao,
                    Number=9,
                    Color=  Model.Enums.CardColorEnum.Red
                },
                new Yao()
                {
                    CardId = 23,
                    FlowerKind= Enums.FlowerKindEnum.Hongtao,
                    Number=12,
                    DisplayNumberText="Q",
                    Color=  Model.Enums.CardColorEnum.Red
                },
                new Yao()
                {
                    CardId = 24,
                    FlowerKind= Enums.FlowerKindEnum.Hongtao,
                    Number=12,
                    DisplayNumberText="Q",
                    Color=  Model.Enums.CardColorEnum.Red
                },
                #endregion

                #region Fenghuolangyan
                new Fenghuolangyan()
                {
                    CardId = 25,
                    FlowerKind= Enums.FlowerKindEnum.Meihua,
                    Number=7,
                    Color=  Model.Enums.CardColorEnum.Black
                },
                  new Fenghuolangyan()
                {
                    CardId = 26,
                    FlowerKind= Enums.FlowerKindEnum.Heitao,
                    Number=7,
                    Color=  Model.Enums.CardColorEnum.Black
                },
                    new Fenghuolangyan()
                {
                    CardId = 27,
                    FlowerKind= Enums.FlowerKindEnum.Meihua,
                    Number=13,
                    DisplayNumberText="K",
                    Color=  Model.Enums.CardColorEnum.Black
                },
                #endregion

                #region Wanjianqifa
                new Wanjianqifa()
                {
                    CardId = 28,
                    FlowerKind= Enums.FlowerKindEnum.Fangkuai,
                    Number=1,
                    DisplayNumberText="A",
                    Color=  Model.Enums.CardColorEnum.Red
                },
                #endregion

                #region Wuxiekeji
                 new Wuxiekeji()
                {
                    CardId = 29,
                    FlowerKind= Enums.FlowerKindEnum.Hongtao,
                    Number=12,
                    DisplayNumberText="Q",
                    Color=  Model.Enums.CardColorEnum.Red
                },
                  new Wuxiekeji()
                {
                    CardId = 30,
                    FlowerKind= Enums.FlowerKindEnum.Meihua,
                    Number=11,
                    DisplayNumberText="J",
                    Color=  Model.Enums.CardColorEnum.Black
                },
                   new Wuxiekeji()
                {
                    CardId = 31,
                    FlowerKind= Enums.FlowerKindEnum.Heitao,
                    Number=12,
                    DisplayNumberText="Q",
                    Color=  Model.Enums.CardColorEnum.Black
                },
                    new Wuxiekeji()
                {
                    CardId = 32,
                    FlowerKind= Enums.FlowerKindEnum.Heitao,
                    Number=13,
                    DisplayNumberText="K",
                    Color=  Model.Enums.CardColorEnum.Black
                },
                #endregion

                #region Juedou
                new Juedou()
                {
                    CardId = 33,
                    FlowerKind= Enums.FlowerKindEnum.Heitao,
                    Number=1,
                    DisplayNumberText="A",
                    Color=  Model.Enums.CardColorEnum.Black
                },
                 new Juedou()
                {
                    CardId = 34,
                    FlowerKind= Enums.FlowerKindEnum.Hongtao,
                    Number=1,
                    DisplayNumberText="A",
                    Color=  Model.Enums.CardColorEnum.Red
                },
                  new Juedou()
                {
                    CardId = 35,
                    FlowerKind= Enums.FlowerKindEnum.Meihua,
                    Number=1,
                    DisplayNumberText="A",
                    Color=  Model.Enums.CardColorEnum.Black
                } ,
                #endregion

                #region Wugufengdeng
                new Wugufengdeng()
                {
                    CardId = 36,
                    FlowerKind= Enums.FlowerKindEnum.Fangkuai,
                    Number=3,
                    Color=  Model.Enums.CardColorEnum.Red
                },
                new Wugufengdeng()
                {
                    CardId = 37,
                    FlowerKind= Enums.FlowerKindEnum.Fangkuai,
                    Number=4,
                    Color=  Model.Enums.CardColorEnum.Red
                },
                #endregion

                #region Xiuyangshengxi
                 new Xiuyangshengxi()
                {
                    CardId = 38,
                    FlowerKind= Enums.FlowerKindEnum.Fangkuai,
                    Number=1,
                    DisplayNumberText="A",
                    Color=  Model.Enums.CardColorEnum.Red
                },
                #endregion

                #region Wuzhongshengyou
                 new Wuzhongshengyou()
                {
                    CardId = 39,
                    FlowerKind= Enums.FlowerKindEnum.Hongtao,
                    Number=7,
                    Color=  Model.Enums.CardColorEnum.Red
                },
                 new Wuzhongshengyou()
                {
                    CardId = 40,
                    FlowerKind= Enums.FlowerKindEnum.Hongtao,
                    Number=8,
                    Color=  Model.Enums.CardColorEnum.Red
                },
                     new Wuzhongshengyou()
                {
                    CardId = 41,
                    FlowerKind= Enums.FlowerKindEnum.Hongtao,
                    Number=9,
                    Color=  Model.Enums.CardColorEnum.Red
                },
                       new Wuzhongshengyou()
                {
                    CardId = 42,
                    FlowerKind= Enums.FlowerKindEnum.Hongtao,
                    Number=11,
                    DisplayNumberText="J",
                    Color=  Model.Enums.CardColorEnum.Red
                },
                #endregion

                #region Jiedaosharen
                 new Jiedaosharen()
                {
                    CardId = 43,
                    FlowerKind= Enums.FlowerKindEnum.Heitao,
                    Number=12,
                    DisplayNumberText="Q",
                    Color=  Model.Enums.CardColorEnum.Black
                },
                new Jiedaosharen()
                {
                    CardId = 44,
                    FlowerKind= Enums.FlowerKindEnum.Heitao,
                    Number=13,
                    DisplayNumberText="K",
                    Color=  Model.Enums.CardColorEnum.Black
                },
                #endregion,

                #region Yuruyi
                 new Yuruyi()
                {
                    CardId = 45,
                    FlowerKind= Enums.FlowerKindEnum.Heitao,
                    Number=2,
                    Color=  Model.Enums.CardColorEnum.Black
                },
                 new Yuruyi()
                {
                    CardId = 46,
                    FlowerKind= Enums.FlowerKindEnum.Meihua,
                    Number=2,
                    Color=  Model.Enums.CardColorEnum.Black
                },
                #endregion

                #region Yuchangjian
                  new Yuchangjian()
                {
                    CardId = 47,
                    FlowerKind= Enums.FlowerKindEnum.Meihua,
                    Number=6,
                    Color=  Model.Enums.CardColorEnum.Black
                },
                #endregion

                #region Longlindao
                  new Longlindao()
                {
                    CardId = 48,
                    FlowerKind= Enums.FlowerKindEnum.Meihua,
                    Number=2,
                    Color=  Model.Enums.CardColorEnum.Black
                },
                #endregion

                #region Panlonggun
                  new Panlonggun()
                {
                    CardId = 49,
                    FlowerKind= Enums.FlowerKindEnum.Meihua,
                    Number=5,
                    Color=  Model.Enums.CardColorEnum.Black
                },
                #endregion

                #region Luyeqiang
                  new Luyeqiang()
                {
                    CardId = 50,
                    FlowerKind= Enums.FlowerKindEnum.Meihua,
                    Number=12,
                    DisplayNumberText="Q",
                    Color=  Model.Enums.CardColorEnum.Black
                },
                #endregion
                
                #region Bolangchui
                  new Bolangchui()
                {
                    CardId = 51,
                    FlowerKind= Enums.FlowerKindEnum.Fangkuai,
                    Number=5,
                    Color=  Model.Enums.CardColorEnum.Red
                },
                #endregion

                #region Hufu
                  new Hufu()
                {
                    CardId = 52,
                    FlowerKind= Enums.FlowerKindEnum.Heitao,
                    Number=1,
                    DisplayNumberText="A",
                    Color=  Model.Enums.CardColorEnum.Black
                },
                    new Hufu()
                {
                    CardId = 53,
                    FlowerKind= Enums.FlowerKindEnum.Hongtao,
                    Number=1,
                    DisplayNumberText="A",
                    Color=  Model.Enums.CardColorEnum.Red
                },
                #endregion

                #region Bawanggong
                  new Bawanggong()
                {
                    CardId = 54,
                    FlowerKind= Enums.FlowerKindEnum.Heitao,
                    Number=5,
                    Color=  Model.Enums.CardColorEnum.Red
                },
                #endregion

                #region Langyabang
                  new Langyabang()
                {
                    CardId = 55,
                    FlowerKind= Enums.FlowerKindEnum.Hongtao,
                    Number=12,
                    DisplayNumberText="Q",
                    Color=  Model.Enums.CardColorEnum.Red
                },
                #endregion

                #region Jingongma
                new Jingongma()
                {
                    CardId = 56,
                    FlowerKind= Enums.FlowerKindEnum.Hongtao,
                    Number=5,
                    Color=  Model.Enums.CardColorEnum.Red
                },
                  new Jingongma()
                {
                    CardId = 57,
                    FlowerKind= Enums.FlowerKindEnum.Hongtao,
                    Number=13,
                    DisplayNumberText="K",
                    Color=  Model.Enums.CardColorEnum.Red
                },
                   new Jingongma()
                {
                    CardId = 58,
                    FlowerKind= Enums.FlowerKindEnum.Meihua,
                    Number=13,
                    DisplayNumberText="K",
                    Color=  Model.Enums.CardColorEnum.Black
                },
                #endregion

                #region Fangyuma
                new Fangyuma()
                {
                    CardId = 59,
                    FlowerKind= Enums.FlowerKindEnum.Fangkuai,
                    Number=5,
                    Color=  Model.Enums.CardColorEnum.Red
                },
                  new Fangyuma()
                {
                    CardId = 60,
                    FlowerKind= Enums.FlowerKindEnum.Meihua,
                    Number=5,
                    Color=  Model.Enums.CardColorEnum.Black
                },
                   new Fangyuma()
                {
                    CardId = 61,
                    FlowerKind= Enums.FlowerKindEnum.Heitao,
                    Number=5,
                    Color=  Model.Enums.CardColorEnum.Black
                },
                #endregion

                #region Sha
                 new Sha()
                {
                    CardId = 62,
                    FlowerKind= Enums.FlowerKindEnum.Fangkuai,
                    Number=7,
                    Color=  Model.Enums.CardColorEnum.Red
                },
                  new Sha()
                {
                    CardId = 63,
                    FlowerKind= Enums.FlowerKindEnum.Fangkuai,
                    Number=8,
                    Color=  Model.Enums.CardColorEnum.Red
                },
                   new Sha()
                {
                    CardId = 64,
                    FlowerKind= Enums.FlowerKindEnum.Fangkuai,
                    Number=9,
                    Color=  Model.Enums.CardColorEnum.Red
                },
                    new Sha()
                {
                    CardId = 65,
                    FlowerKind= Enums.FlowerKindEnum.Fangkuai,
                    Number=10,
                    Color=  Model.Enums.CardColorEnum.Red
                },
                     new Sha()
                {
                    CardId = 66,
                    FlowerKind= Enums.FlowerKindEnum.Fangkuai,
                    Number=13,
                    DisplayNumberText="K",
                    Color=  Model.Enums.CardColorEnum.Red
                },

                   new Sha()
                {
                    CardId = 67,
                    FlowerKind= Enums.FlowerKindEnum.Hongtao,
                    Number=6,
                    Color=  Model.Enums.CardColorEnum.Red
                },
                   new Sha()
                {
                    CardId = 68,
                    FlowerKind= Enums.FlowerKindEnum.Hongtao,
                    Number=10,
                    Color=  Model.Enums.CardColorEnum.Red
                },
                   new Sha()
                {
                    CardId = 69,
                    FlowerKind= Enums.FlowerKindEnum.Hongtao,
                    Number=10,
                    Color=  Model.Enums.CardColorEnum.Red
                },
                      new Sha()
                {
                    CardId = 70,
                    FlowerKind= Enums.FlowerKindEnum.Hongtao,
                    Number=11,
                    DisplayNumberText="J",
                    Color=  Model.Enums.CardColorEnum.Red
                },

                   new Sha()
                {
                    CardId = 71,
                    FlowerKind= Enums.FlowerKindEnum.Meihua,
                    Number=7,
                    Color=  Model.Enums.CardColorEnum.Black
                },
                   new Sha()
                {
                    CardId = 72,
                    FlowerKind= Enums.FlowerKindEnum.Meihua,
                    Number=8,
                    Color=  Model.Enums.CardColorEnum.Black
                },
                   new Sha()
                {
                    CardId = 73,
                    FlowerKind= Enums.FlowerKindEnum.Meihua,
                    Number=9,
                    Color=  Model.Enums.CardColorEnum.Black
                },
                   new Sha()
                {
                    CardId = 74,
                    FlowerKind= Enums.FlowerKindEnum.Meihua,
                    Number=10,
                    Color=  Model.Enums.CardColorEnum.Black
                },
                       new Sha()
                {
                    CardId = 75,
                    FlowerKind= Enums.FlowerKindEnum.Meihua,
                    Number=8,
                    Color=  Model.Enums.CardColorEnum.Black
                },
                   new Sha()
                {
                    CardId = 76,
                    FlowerKind= Enums.FlowerKindEnum.Meihua,
                    Number=9,
                    Color=  Model.Enums.CardColorEnum.Black
                },
                   new Sha()
                {
                    CardId = 77,
                    FlowerKind= Enums.FlowerKindEnum.Meihua,
                    Number=10,
                    Color=  Model.Enums.CardColorEnum.Black
                },

                   new Sha()
                {
                    CardId = 78,
                    FlowerKind= Enums.FlowerKindEnum.Heitao,
                    Number=2,
                    Color=  Model.Enums.CardColorEnum.Black
                },   new Sha()
                {
                    CardId = 79,
                    FlowerKind= Enums.FlowerKindEnum.Heitao,
                    Number=3,
                    Color=  Model.Enums.CardColorEnum.Black
                },   new Sha()
                {
                    CardId = 80,
                    FlowerKind= Enums.FlowerKindEnum.Heitao,
                    Number=4,
                    Color=  Model.Enums.CardColorEnum.Black
                },   new Sha()
                {
                    CardId = 81,
                    FlowerKind= Enums.FlowerKindEnum.Heitao,
                    Number=5,
                    Color=  Model.Enums.CardColorEnum.Black
                },
                   new Sha()
                {
                    CardId = 82,
                    FlowerKind= Enums.FlowerKindEnum.Heitao,
                    Number=5,
                    Color=  Model.Enums.CardColorEnum.Black
                },
                 new Sha()
                {
                    CardId = 83,
                    FlowerKind= Enums.FlowerKindEnum.Heitao,
                    Number=6,
                    Color=  Model.Enums.CardColorEnum.Black
                },
                new Sha()
                {
                    CardId = 84,
                    FlowerKind= Enums.FlowerKindEnum.Heitao,
                    Number=7,
                    Color=  Model.Enums.CardColorEnum.Black
                },
                   new Sha()
                {
                    CardId = 85,
                    FlowerKind= Enums.FlowerKindEnum.Heitao,
                    Number=8,
                    Color=  Model.Enums.CardColorEnum.Black
                },
                   new Sha()
                {
                    CardId = 86,
                    FlowerKind= Enums.FlowerKindEnum.Heitao,
                    Number=9,
                    Color=  Model.Enums.CardColorEnum.Black
                },
                   new Sha()
                {
                    CardId = 87,
                    FlowerKind= Enums.FlowerKindEnum.Heitao,
                    Number=10,
                    Color=  Model.Enums.CardColorEnum.Black
                },
                       new Sha()
                {
                    CardId = 88,
                    FlowerKind= Enums.FlowerKindEnum.Heitao,
                    Number=8,
                    Color=  Model.Enums.CardColorEnum.Black
                },
                   new Sha()
                {
                    CardId = 89,
                    FlowerKind= Enums.FlowerKindEnum.Heitao,
                    Number=9,
                    Color=  Model.Enums.CardColorEnum.Black
                },
                   new Sha()
                {
                    CardId = 90,
                    FlowerKind= Enums.FlowerKindEnum.Heitao,
                    Number=10,
                    Color=  Model.Enums.CardColorEnum.Black
                },
                   new Sha()
                {
                    CardId = 91,
                    FlowerKind= Enums.FlowerKindEnum.Heitao,
                    Number=11,
                    DisplayNumberText="J",
                    Color=  Model.Enums.CardColorEnum.Black
                },
                #endregion

                #region shan
                 new Shan()
                {
                    CardId = 92,
                    FlowerKind= Enums.FlowerKindEnum.Fangkuai,
                    Number=2,
                    Color=  Model.Enums.CardColorEnum.Red
                },
                  new Shan()
                {
                    CardId = 93,
                    FlowerKind= Enums.FlowerKindEnum.Fangkuai,
                    Number=3,
                    Color=  Model.Enums.CardColorEnum.Red
                },
                   new Shan()
                {
                    CardId = 94,
                    FlowerKind= Enums.FlowerKindEnum.Fangkuai,
                    Number=4,
                    Color=  Model.Enums.CardColorEnum.Red
                },
                      new Shan()
                {
                    CardId = 95,
                    FlowerKind= Enums.FlowerKindEnum.Fangkuai,
                    Number=5,
                    Color=  Model.Enums.CardColorEnum.Red
                },
                    new Shan()
                {
                    CardId = 96,
                    FlowerKind= Enums.FlowerKindEnum.Fangkuai,
                    Number=6,
                    Color=  Model.Enums.CardColorEnum.Red
                },
                     new Shan()
                {
                    CardId = 97,
                    FlowerKind= Enums.FlowerKindEnum.Fangkuai,
                    Number=7,
                    Color=  Model.Enums.CardColorEnum.Red
                },
                      new Shan()
                {
                    CardId = 98,
                    FlowerKind= Enums.FlowerKindEnum.Fangkuai,
                    Number=8,
                    Color=  Model.Enums.CardColorEnum.Red
                },
                       new Shan()
                {
                    CardId = 99,
                    FlowerKind= Enums.FlowerKindEnum.Fangkuai,
                    Number=9,
                    Color=  Model.Enums.CardColorEnum.Red
                },
                        new Shan()
                {
                    CardId = 100,
                    FlowerKind= Enums.FlowerKindEnum.Fangkuai,
                    Number=10,
                    Color=  Model.Enums.CardColorEnum.Red
                },
                         new Shan()
                {
                    CardId = 101,
                    FlowerKind= Enums.FlowerKindEnum.Fangkuai,
                    Number=11,
                    DisplayNumberText="J",
                    Color=  Model.Enums.CardColorEnum.Red
                },
                          new Shan()
                {
                    CardId = 102,
                    FlowerKind= Enums.FlowerKindEnum.Fangkuai,
                    Number=11,
                    DisplayNumberText="J",
                    Color=  Model.Enums.CardColorEnum.Red
                },

               new Shan()
                {
                    CardId = 103,
                    FlowerKind= Enums.FlowerKindEnum.Hongtao,
                    Number=2,
                    Color=  Model.Enums.CardColorEnum.Red
                },
               new Shan()
                {
                    CardId = 104,
                    FlowerKind= Enums.FlowerKindEnum.Hongtao,
                    Number=2,
                    Color=  Model.Enums.CardColorEnum.Red
                },
               new Shan()
                {
                    CardId = 105,
                    FlowerKind= Enums.FlowerKindEnum.Hongtao,
                    Number=13,
                    DisplayNumberText="K",
                    Color=  Model.Enums.CardColorEnum.Red
                },
                new Shan()
                {
                    CardId = 106,
                    FlowerKind= Enums.FlowerKindEnum.Hongtao,
                    Number=13,
                    DisplayNumberText="K",
                    Color=  Model.Enums.CardColorEnum.Red
                },
                #endregion
            };

            #endregion

            //random the cards.
            return RandomUtil.GetShuffleArray<CardBase>(cardsStack);
        }

        public IEnumerable<CardBase> GenerateNewCardStack(List<CardBase> cards)
        {
            return RandomUtil.GetShuffleArray<CardBase>(cards);
        }
    }
}

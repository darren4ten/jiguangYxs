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
                    FlowerKind= Enums.FlowerKindEnum.Hongtao,
                    Number=3,
                    Color=  Model.Enums.CardColorEnum.Red
                },
                new Tannangquwu()
                {
                    FlowerKind= Enums.FlowerKindEnum.Hongtao,
                    Number=4,
                    Color=  Model.Enums.CardColorEnum.Red
                },
                new Tannangquwu()
                {
                    FlowerKind= Enums.FlowerKindEnum.Meihua,
                    Number=3,
                    Color=  Model.Enums.CardColorEnum.Black
                },
                new Tannangquwu()
                {
                    FlowerKind= Enums.FlowerKindEnum.Meihua,
                    Number=4,
                    Color=  Model.Enums.CardColorEnum.Black
                },
                new Tannangquwu()
                {
                    FlowerKind= Enums.FlowerKindEnum.Meihua,
                    Number=11,
                    DisplayNumberText="J",
                    Color=  Model.Enums.CardColorEnum.Black
                },
                #endregion

                #region Shoupenglei
                new Shoupenglei()
                {
                    FlowerKind= Enums.FlowerKindEnum.Fangkuai,
                    Number=12,
                    DisplayNumberText="Q",
                    Color=  Model.Enums.CardColorEnum.Red
                },
                new Shoupenglei()
                {
                    FlowerKind= Enums.FlowerKindEnum.Meihua,
                    Number=1,
                    DisplayNumberText="A",
                    Color=  Model.Enums.CardColorEnum.Black
                },
                #endregion

                #region Fudichouxin
                new Fudichouxin()
                {
                    FlowerKind= Enums.FlowerKindEnum.Heitao,
                    Number=3,
                    DisplayNumberText="3",
                    Color=  Model.Enums.CardColorEnum.Black
                },
                 new Fudichouxin()
                {
                    FlowerKind= Enums.FlowerKindEnum.Heitao,
                    Number=4,
                    Color=  Model.Enums.CardColorEnum.Black
                },
                  new Fudichouxin()
                {
                    FlowerKind= Enums.FlowerKindEnum.Meihua,
                    Number=3,
                    Color=  Model.Enums.CardColorEnum.Black
                },
                   new Fudichouxin()
                {
                    FlowerKind= Enums.FlowerKindEnum.Meihua,
                    Number=4,
                    Color=  Model.Enums.CardColorEnum.Black
                },
                    new Fudichouxin()
                {
                    FlowerKind= Enums.FlowerKindEnum.Fangkuai,
                    Number=12,
                    Color=  Model.Enums.CardColorEnum.Red
                },
                     new Fudichouxin()
                {
                    FlowerKind= Enums.FlowerKindEnum.Meihua,
                    Number=12,
                    Color=  Model.Enums.CardColorEnum.Black
                },

                #endregion

                #region Huadiweilao
                new Huadiweilao()
                {
                    FlowerKind= Enums.FlowerKindEnum.Meihua,
                    Number=6,
                    Color=  Model.Enums.CardColorEnum.Black
                },
                 new Huadiweilao()
                {
                    FlowerKind= Enums.FlowerKindEnum.Heitao,
                    Number=6,
                    Color=  Model.Enums.CardColorEnum.Black
                },
                  new Huadiweilao()
                {
                    FlowerKind= Enums.FlowerKindEnum.Fangkuai,
                    Number=6,
                    Color=  Model.Enums.CardColorEnum.Black
                },
                #endregion

                #region Yao
                new Yao()
                {
                    FlowerKind= Enums.FlowerKindEnum.Hongtao,
                    Number=3,
                    Color=  Model.Enums.CardColorEnum.Red
                },
                new Yao()
                {
                    FlowerKind= Enums.FlowerKindEnum.Hongtao,
                    Number=4,
                    Color=  Model.Enums.CardColorEnum.Red
                },
                new Yao()
                {
                    FlowerKind= Enums.FlowerKindEnum.Hongtao,
                    Number=6,
                    Color=  Model.Enums.CardColorEnum.Red
                },
                new Yao()
                {
                    FlowerKind= Enums.FlowerKindEnum.Hongtao,
                    Number=7,
                    Color=  Model.Enums.CardColorEnum.Red
                },
                new Yao()
                {
                    FlowerKind= Enums.FlowerKindEnum.Hongtao,
                    Number=8,
                    Color=  Model.Enums.CardColorEnum.Red
                },
                new Yao()
                {
                    FlowerKind= Enums.FlowerKindEnum.Hongtao,
                    Number=9,
                    Color=  Model.Enums.CardColorEnum.Red
                },
                new Yao()
                {
                    FlowerKind= Enums.FlowerKindEnum.Hongtao,
                    Number=12,
                    DisplayNumberText="Q",
                    Color=  Model.Enums.CardColorEnum.Red
                },
                new Yao()
                {
                    FlowerKind= Enums.FlowerKindEnum.Hongtao,
                    Number=12,
                    DisplayNumberText="Q",
                    Color=  Model.Enums.CardColorEnum.Red
                },
                #endregion

                #region Fenghuolangyan
                new Fenghuolangyan()
                {
                    FlowerKind= Enums.FlowerKindEnum.Meihua,
                    Number=7,
                    Color=  Model.Enums.CardColorEnum.Black
                },
                  new Fenghuolangyan()
                {
                    FlowerKind= Enums.FlowerKindEnum.Heitao,
                    Number=7,
                    Color=  Model.Enums.CardColorEnum.Black
                },
                    new Fenghuolangyan()
                {
                    FlowerKind= Enums.FlowerKindEnum.Meihua,
                    Number=13,
                    DisplayNumberText="K",
                    Color=  Model.Enums.CardColorEnum.Black
                },
                #endregion

                #region Wanjianqifa
                new Wanjianqifa()
                {
                    FlowerKind= Enums.FlowerKindEnum.Fangkuai,
                    Number=1,
                    DisplayNumberText="A",
                    Color=  Model.Enums.CardColorEnum.Red
                },
                #endregion

                #region Wuxiekeji
                 new Wuxiekeji()
                {
                    FlowerKind= Enums.FlowerKindEnum.Hongtao,
                    Number=12,
                    DisplayNumberText="Q",
                    Color=  Model.Enums.CardColorEnum.Red
                },
                  new Wuxiekeji()
                {
                    FlowerKind= Enums.FlowerKindEnum.Meihua,
                    Number=11,
                    DisplayNumberText="J",
                    Color=  Model.Enums.CardColorEnum.Black
                },
                   new Wuxiekeji()
                {
                    FlowerKind= Enums.FlowerKindEnum.Heitao,
                    Number=12,
                    DisplayNumberText="Q",
                    Color=  Model.Enums.CardColorEnum.Black
                },
                    new Wuxiekeji()
                {
                    FlowerKind= Enums.FlowerKindEnum.Heitao,
                    Number=13,
                    DisplayNumberText="K",
                    Color=  Model.Enums.CardColorEnum.Black
                },
                #endregion

                #region Juedou
                new Juedou()
                {
                    FlowerKind= Enums.FlowerKindEnum.Heitao,
                    Number=1,
                    DisplayNumberText="A",
                    Color=  Model.Enums.CardColorEnum.Black
                },
                 new Juedou()
                {
                    FlowerKind= Enums.FlowerKindEnum.Hongtao,
                    Number=1,
                    DisplayNumberText="A",
                    Color=  Model.Enums.CardColorEnum.Red
                },
                  new Juedou()
                {
                    FlowerKind= Enums.FlowerKindEnum.Meihua,
                    Number=1,
                    DisplayNumberText="A",
                    Color=  Model.Enums.CardColorEnum.Black
                } ,
                #endregion

                #region Wugufengdeng
                new Wugufengdeng()
                {
                    FlowerKind= Enums.FlowerKindEnum.Fangkuai,
                    Number=3,
                    Color=  Model.Enums.CardColorEnum.Red
                },
                new Wugufengdeng()
                {
                    FlowerKind= Enums.FlowerKindEnum.Fangkuai,
                    Number=4,
                    Color=  Model.Enums.CardColorEnum.Red
                },
                #endregion

                #region Xiuyangshengxi
                 new Xiuyangshengxi()
                {
                    FlowerKind= Enums.FlowerKindEnum.Fangkuai,
                    Number=1,
                    DisplayNumberText="A",
                    Color=  Model.Enums.CardColorEnum.Red
                },
                #endregion

                #region Wuzhongshengyou
                 new Wuzhongshengyou()
                {
                    FlowerKind= Enums.FlowerKindEnum.Hongtao,
                    Number=7,
                    Color=  Model.Enums.CardColorEnum.Red
                },
                 new Wuzhongshengyou()
                {
                    FlowerKind= Enums.FlowerKindEnum.Hongtao,
                    Number=8,
                    Color=  Model.Enums.CardColorEnum.Red
                },
                     new Wuzhongshengyou()
                {
                    FlowerKind= Enums.FlowerKindEnum.Hongtao,
                    Number=9,
                    Color=  Model.Enums.CardColorEnum.Red
                },
                       new Wuzhongshengyou()
                {
                    FlowerKind= Enums.FlowerKindEnum.Hongtao,
                    Number=11,
                    DisplayNumberText="J",
                    Color=  Model.Enums.CardColorEnum.Red
                },
                #endregion

                #region Jiedaosharen
                 new Jiedaosharen()
                {
                    FlowerKind= Enums.FlowerKindEnum.Heitao,
                    Number=12,
                    DisplayNumberText="Q",
                    Color=  Model.Enums.CardColorEnum.Black
                },
                new Jiedaosharen()
                {
                    FlowerKind= Enums.FlowerKindEnum.Heitao,
                    Number=13,
                    DisplayNumberText="K",
                    Color=  Model.Enums.CardColorEnum.Black
                },
                #endregion,

                #region Yuruyi
                 new Yuruyi()
                {
                    FlowerKind= Enums.FlowerKindEnum.Heitao,
                    Number=2,
                    Color=  Model.Enums.CardColorEnum.Black
                },
                 new Yuruyi()
                {
                    FlowerKind= Enums.FlowerKindEnum.Meihua,
                    Number=2,
                    Color=  Model.Enums.CardColorEnum.Black
                },
                #endregion

                #region Yuchangjian
                  new Yuchangjian()
                {
                    FlowerKind= Enums.FlowerKindEnum.Meihua,
                    Number=6,
                    Color=  Model.Enums.CardColorEnum.Black
                },
                #endregion

                #region Longlindao
                  new Longlindao()
                {
                    FlowerKind= Enums.FlowerKindEnum.Meihua,
                    Number=2,
                    Color=  Model.Enums.CardColorEnum.Black
                },
                #endregion

                #region Panlonggun
                  new Panlonggun()
                {
                    FlowerKind= Enums.FlowerKindEnum.Meihua,
                    Number=5,
                    Color=  Model.Enums.CardColorEnum.Black
                },
                #endregion

                #region Luyeqiang
                  new Luyeqiang()
                {
                    FlowerKind= Enums.FlowerKindEnum.Meihua,
                    Number=12,
                    DisplayNumberText="Q",
                    Color=  Model.Enums.CardColorEnum.Black
                },
                #endregion
                
                #region Bolangchui
                  new Bolangchui()
                {
                    FlowerKind= Enums.FlowerKindEnum.Fangkuai,
                    Number=5,
                    Color=  Model.Enums.CardColorEnum.Red
                },
                #endregion

                #region Hufu
                  new Hufu()
                {
                    FlowerKind= Enums.FlowerKindEnum.Heitao,
                    Number=1,
                    DisplayNumberText="A",
                    Color=  Model.Enums.CardColorEnum.Black
                },
                    new Hufu()
                {
                    FlowerKind= Enums.FlowerKindEnum.Hongtao,
                    Number=1,
                    DisplayNumberText="A",
                    Color=  Model.Enums.CardColorEnum.Red
                },
                #endregion

                #region Bawanggong
                  new Bawanggong()
                {
                    FlowerKind= Enums.FlowerKindEnum.Heitao,
                    Number=5,
                    Color=  Model.Enums.CardColorEnum.Red
                },
                #endregion

                #region Langyabang
                  new Langyabang()
                {
                    FlowerKind= Enums.FlowerKindEnum.Hongtao,
                    Number=12,
                    DisplayNumberText="Q",
                    Color=  Model.Enums.CardColorEnum.Red
                },
                #endregion

                #region Jingongma
                new Jingongma()
                {
                    FlowerKind= Enums.FlowerKindEnum.Hongtao,
                    Number=5,
                    Color=  Model.Enums.CardColorEnum.Red
                },
                  new Jingongma()
                {
                    FlowerKind= Enums.FlowerKindEnum.Hongtao,
                    Number=13,
                    DisplayNumberText="K",
                    Color=  Model.Enums.CardColorEnum.Red
                },
                   new Jingongma()
                {
                    FlowerKind= Enums.FlowerKindEnum.Meihua,
                    Number=13,
                    DisplayNumberText="K",
                    Color=  Model.Enums.CardColorEnum.Black
                },
                #endregion

                #region Fangyuma
                new Fangyuma()
                {
                    FlowerKind= Enums.FlowerKindEnum.Fangkuai,
                    Number=5,
                    Color=  Model.Enums.CardColorEnum.Red
                },
                  new Fangyuma()
                {
                    FlowerKind= Enums.FlowerKindEnum.Meihua,
                    Number=5,
                    Color=  Model.Enums.CardColorEnum.Black
                },
                   new Fangyuma()
                {
                    FlowerKind= Enums.FlowerKindEnum.Heitao,
                    Number=5,
                    Color=  Model.Enums.CardColorEnum.Black
                },
                #endregion

                #region Sha
                 new Sha()
                {
                    FlowerKind= Enums.FlowerKindEnum.Fangkuai,
                    Number=7,
                    Color=  Model.Enums.CardColorEnum.Red
                },
                  new Sha()
                {
                    FlowerKind= Enums.FlowerKindEnum.Fangkuai,
                    Number=8,
                    Color=  Model.Enums.CardColorEnum.Red
                },
                   new Sha()
                {
                    FlowerKind= Enums.FlowerKindEnum.Fangkuai,
                    Number=9,
                    Color=  Model.Enums.CardColorEnum.Red
                },
                    new Sha()
                {
                    FlowerKind= Enums.FlowerKindEnum.Fangkuai,
                    Number=10,
                    Color=  Model.Enums.CardColorEnum.Red
                },
                     new Sha()
                {
                    FlowerKind= Enums.FlowerKindEnum.Fangkuai,
                    Number=13,
                    DisplayNumberText="K",
                    Color=  Model.Enums.CardColorEnum.Red
                },

                   new Sha()
                {
                    FlowerKind= Enums.FlowerKindEnum.Hongtao,
                    Number=6,
                    Color=  Model.Enums.CardColorEnum.Red
                },
                   new Sha()
                {
                    FlowerKind= Enums.FlowerKindEnum.Hongtao,
                    Number=10,
                    Color=  Model.Enums.CardColorEnum.Red
                },
                   new Sha()
                {
                    FlowerKind= Enums.FlowerKindEnum.Hongtao,
                    Number=10,
                    Color=  Model.Enums.CardColorEnum.Red
                },
                      new Sha()
                {
                    FlowerKind= Enums.FlowerKindEnum.Hongtao,
                    Number=11,
                    DisplayNumberText="J",
                    Color=  Model.Enums.CardColorEnum.Red
                },

                   new Sha()
                {
                    FlowerKind= Enums.FlowerKindEnum.Meihua,
                    Number=7,
                    Color=  Model.Enums.CardColorEnum.Black
                },
                   new Sha()
                {
                    FlowerKind= Enums.FlowerKindEnum.Meihua,
                    Number=8,
                    Color=  Model.Enums.CardColorEnum.Black
                },
                   new Sha()
                {
                    FlowerKind= Enums.FlowerKindEnum.Meihua,
                    Number=9,
                    Color=  Model.Enums.CardColorEnum.Black
                },
                   new Sha()
                {
                    FlowerKind= Enums.FlowerKindEnum.Meihua,
                    Number=10,
                    Color=  Model.Enums.CardColorEnum.Black
                },
                       new Sha()
                {
                    FlowerKind= Enums.FlowerKindEnum.Meihua,
                    Number=8,
                    Color=  Model.Enums.CardColorEnum.Black
                },
                   new Sha()
                {
                    FlowerKind= Enums.FlowerKindEnum.Meihua,
                    Number=9,
                    Color=  Model.Enums.CardColorEnum.Black
                },
                   new Sha()
                {
                    FlowerKind= Enums.FlowerKindEnum.Meihua,
                    Number=10,
                    Color=  Model.Enums.CardColorEnum.Black
                },

                   new Sha()
                {
                    FlowerKind= Enums.FlowerKindEnum.Heitao,
                    Number=2,
                    Color=  Model.Enums.CardColorEnum.Black
                },   new Sha()
                {
                    FlowerKind= Enums.FlowerKindEnum.Heitao,
                    Number=3,
                    Color=  Model.Enums.CardColorEnum.Black
                },   new Sha()
                {
                    FlowerKind= Enums.FlowerKindEnum.Heitao,
                    Number=4,
                    Color=  Model.Enums.CardColorEnum.Black
                },   new Sha()
                {
                    FlowerKind= Enums.FlowerKindEnum.Heitao,
                    Number=5,
                    Color=  Model.Enums.CardColorEnum.Black
                },
                   new Sha()
                {
                    FlowerKind= Enums.FlowerKindEnum.Heitao,
                    Number=5,
                    Color=  Model.Enums.CardColorEnum.Black
                },
                 new Sha()
                {
                    FlowerKind= Enums.FlowerKindEnum.Heitao,
                    Number=6,
                    Color=  Model.Enums.CardColorEnum.Black
                },
                new Sha()
                {
                    FlowerKind= Enums.FlowerKindEnum.Heitao,
                    Number=7,
                    Color=  Model.Enums.CardColorEnum.Black
                },
                   new Sha()
                {
                    FlowerKind= Enums.FlowerKindEnum.Heitao,
                    Number=8,
                    Color=  Model.Enums.CardColorEnum.Black
                },
                   new Sha()
                {
                    FlowerKind= Enums.FlowerKindEnum.Heitao,
                    Number=9,
                    Color=  Model.Enums.CardColorEnum.Black
                },
                   new Sha()
                {
                    FlowerKind= Enums.FlowerKindEnum.Heitao,
                    Number=10,
                    Color=  Model.Enums.CardColorEnum.Black
                },
                       new Sha()
                {
                    FlowerKind= Enums.FlowerKindEnum.Heitao,
                    Number=8,
                    Color=  Model.Enums.CardColorEnum.Black
                },
                   new Sha()
                {
                    FlowerKind= Enums.FlowerKindEnum.Heitao,
                    Number=9,
                    Color=  Model.Enums.CardColorEnum.Black
                },
                   new Sha()
                {
                    FlowerKind= Enums.FlowerKindEnum.Heitao,
                    Number=10,
                    Color=  Model.Enums.CardColorEnum.Black
                },
                   new Sha()
                {
                    FlowerKind= Enums.FlowerKindEnum.Heitao,
                    Number=11,
                    DisplayNumberText="J",
                    Color=  Model.Enums.CardColorEnum.Black
                },
                #endregion

                #region shan
                 new Shan()
                {
                    FlowerKind= Enums.FlowerKindEnum.Fangkuai,
                    Number=2,
                    Color=  Model.Enums.CardColorEnum.Red
                },
                  new Shan()
                {
                    FlowerKind= Enums.FlowerKindEnum.Fangkuai,
                    Number=3,
                    Color=  Model.Enums.CardColorEnum.Red
                },
                   new Shan()
                {
                    FlowerKind= Enums.FlowerKindEnum.Fangkuai,
                    Number=4,
                    Color=  Model.Enums.CardColorEnum.Red
                },
                      new Shan()
                {
                    FlowerKind= Enums.FlowerKindEnum.Fangkuai,
                    Number=5,
                    Color=  Model.Enums.CardColorEnum.Red
                },
                    new Shan()
                {
                    FlowerKind= Enums.FlowerKindEnum.Fangkuai,
                    Number=6,
                    Color=  Model.Enums.CardColorEnum.Red
                },
                     new Shan()
                {
                    FlowerKind= Enums.FlowerKindEnum.Fangkuai,
                    Number=7,
                    Color=  Model.Enums.CardColorEnum.Red
                },
                      new Shan()
                {
                    FlowerKind= Enums.FlowerKindEnum.Fangkuai,
                    Number=8,
                    Color=  Model.Enums.CardColorEnum.Red
                },
                       new Shan()
                {
                    FlowerKind= Enums.FlowerKindEnum.Fangkuai,
                    Number=9,
                    Color=  Model.Enums.CardColorEnum.Red
                },
                        new Shan()
                {
                    FlowerKind= Enums.FlowerKindEnum.Fangkuai,
                    Number=10,
                    Color=  Model.Enums.CardColorEnum.Red
                },
                         new Shan()
                {
                    FlowerKind= Enums.FlowerKindEnum.Fangkuai,
                    Number=11,
                    DisplayNumberText="J",
                    Color=  Model.Enums.CardColorEnum.Red
                },
                          new Shan()
                {
                    FlowerKind= Enums.FlowerKindEnum.Fangkuai,
                    Number=11,
                    DisplayNumberText="J",
                    Color=  Model.Enums.CardColorEnum.Red
                },

               new Shan()
                {
                    FlowerKind= Enums.FlowerKindEnum.Hongtao,
                    Number=2,
                    Color=  Model.Enums.CardColorEnum.Red
                },
               new Shan()
                {
                    FlowerKind= Enums.FlowerKindEnum.Hongtao,
                    Number=2,
                    Color=  Model.Enums.CardColorEnum.Red
                },
               new Shan()
                {
                    FlowerKind= Enums.FlowerKindEnum.Hongtao,
                    Number=13,
                    DisplayNumberText="K",
                    Color=  Model.Enums.CardColorEnum.Red
                },
                new Shan()
                {
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

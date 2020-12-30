using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Logic.ActionManger;
using Logic.Cards;
using Logic.Enums;
using Logic.GameLevel;
using Logic.GameLevel.Levels;
using Logic.Model.Cards.BaseCards;
using Logic.Model.Cards.JinlangCards;
using Logic.Model.Cards.MutedCards;
using Logic.Model.Enums;
using Logic.Model.Hero.Presizdent;
using Logic.Model.Player;
using Logic.Model.Skill;
using Logic.Model.Skill.SubSkill;
using NUnit.Framework;

namespace Tests.Card
{
    [TestFixture]
    public class HuadiweilaoTest
    {
        [Test]
        public async Task HuadiweilaoTest_Success()
        {
            var gameLevel1 = new GameLevel1();
            var qianghua1 = new Qianghua(5, 30);
            var shatan1 = new Shatan(5, 50);
            var star2Xiangyu = new PlayerHero(2, new Xiangyu(), null,
                new List<SkillBase>(){
                    shatan1
                });
            var star3Zhuyuanzhang = new PlayerHero(3, new Zhuyuanzhang(), null,
                new List<SkillBase>(){
                    new Qianghua(1,50),
                    new Xixue(5,50),
                });
            var star3Zhuyuanzhang1 = new PlayerHero(3, new Zhuyuanzhang(), null,
                new List<SkillBase>(){
                    new Xixue(5,50),
                });

            var player1 = new Player(gameLevel1, new AiActionManager(), new List<PlayerHero>() { star2Xiangyu })
            {
                PlayerId = 1,
                RoundContext = new RoundContext()
                {
                    AttackDynamicFactor = AttackDynamicFactor.GetDefaultDeltaAttackFactor()
                }
            };
            var player2 = new Player(gameLevel1, new AiActionManager(), new List<PlayerHero>() { star3Zhuyuanzhang })
            {
                PlayerId = 2
            };
            gameLevel1.OnLoad(player1, new List<Player>() { player1, player2 });
            player1.Init();
            player2.Init();
            var cardToPlay = new Huadiweilao();
            await player1.AddCardInHand(cardToPlay);
            await player1.AddCardInHand(new Sha());
            await player2.AddCardsInHand(new List<CardBase>() { new Sha(), new Shan() });

            Assert.AreEqual(2, player1.CardsInHand.Count);
            Assert.AreEqual(2, player2.CardsInHand.Count);

            var response = await cardToPlay.PlayCard(new CardRequestContext()
            {
                TargetPlayers = new List<Player>()
                {
                    player2
                }
            }, player1.RoundContext);

            gameLevel1.GlobalEventBus.ListenEvent(Guid.NewGuid(), gameLevel1.HostPlayerHero, EventTypeEnum.AfterPanding, (
                async (context, roundContext, responseContext) =>
                {
                    responseContext.ResponseResult = ResponseResultEnum.Failed;
                    var c = responseContext.Cards.FirstOrDefault();
                    if (c?.FlowerKind == FlowerKindEnum.Hongtao)
                    {
                        responseContext.Cards = new List<CardBase>()
                        {
                            new ChangedCard(new List<CardBase>(){c}, c)
                            {
                                CardChangeType = CardChangeTypeEnum.Changed,
                                FlowerKind=FlowerKindEnum.Meihua
                            }
                        };
                        responseContext.Message = "测试强制改变判定结果为梅花。";
                        Console.WriteLine($"测试强制改变判定结果为梅花，原始花色为：{c.FlowerKind}");
                    }
                }));
            await player2.StartMyRound();
            Assert.AreEqual(1, player1.CardsInHand.Count);
            Assert.AreEqual(4, player2.CardsInHand.Count);
        }
    }
}

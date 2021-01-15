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
    public class ShoupengleiTest
    {
        private GameLevelBase gameLevel1;
        private Player player1;
        private Player player2;
        [SetUp]
        public void Init()
        {
            gameLevel1 = new GameLevel1();
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

            player1 = new Player(gameLevel1, new AiActionManager(), new List<PlayerHero>() { star2Xiangyu })
            {
                PlayerId = 1,
                RoundContext = new RoundContext()
                {
                    AttackDynamicFactor = AttackDynamicFactor.GetDefaultDeltaAttackFactor()
                }
            };
            player2 = new Player(gameLevel1, new AiActionManager(), new List<PlayerHero>() { star3Zhuyuanzhang })
            {
                PlayerId = 2
            };
            gameLevel1.OnLoad(player1, new List<Player>() { player1, player2 });
            player1.Init();
            player2.Init();
        }
        [Test]
        public async Task ShoupengleiTest_Success()
        {

            var cardToPlay = new Shoupenglei();
            await player1.AddCardInHand(cardToPlay);
            await player1.AddCardInHand(new Sha());
            await player2.AddCardsInHand(new List<CardBase>() { new Sha(), new Shan() });

            Assert.AreEqual(2, player1.CardsInHand.Count);
            Assert.AreEqual(2, player2.CardsInHand.Count);

            var response = await cardToPlay.PlayCard(new CardRequestContext(), player1.RoundContext);
            Assert.AreEqual(5, player1.CurrentPlayerHero.CurrentLife);
            Assert.AreEqual(6, player2.CurrentPlayerHero.CurrentLife);

            var triggerTimes = 0;
            gameLevel1.GlobalEventBus.ListenEvent(Guid.NewGuid(), gameLevel1.HostPlayerHero, EventTypeEnum.AfterPanding, (
                async (context, roundContext, responseContext) =>
                {
                    if (triggerTimes == 0)
                    {
                        //第1次触发判定时，让判定牌强制为红桃
                        if (responseContext.ResponseResult == ResponseResultEnum.Success)
                        {
                            responseContext.ResponseResult = ResponseResultEnum.Failed;
                            var c = responseContext.Cards.FirstOrDefault();
                            responseContext.Cards = new List<CardBase>()
                            {
                                new ChangedCard(new List<CardBase>(){c}, c)
                                {
                                    CardChangeType = CardChangeTypeEnum.Changed,
                                    Number = 13,
                                    FlowerKind=FlowerKindEnum.Heitao
                                }
                            };
                            Console.WriteLine($"测试强制改变判定结果为红桃，原始牌为：{c}");
                        }
                    }
                    else if (triggerTimes == 1)
                    {
                        //第2次触发判定时，让判定牌强制为黑桃2
                        if (responseContext.ResponseResult == ResponseResultEnum.Failed)
                        {
                            responseContext.ResponseResult = ResponseResultEnum.Success;
                            var c = responseContext.Cards.FirstOrDefault();
                            var newCard = new ChangedCard(new List<CardBase>() { c }, c)
                            {
                                CardChangeType = CardChangeTypeEnum.Changed,
                                Number = 2,
                                FlowerKind = FlowerKindEnum.Heitao
                            };
                            responseContext.Cards = new List<CardBase>()
                            {
                                newCard
                            };
                            Console.WriteLine($"测试强制改变判定结果为黑桃桃{newCard}，原始牌为：{c}");
                        }
                    }

                    triggerTimes++;
                }));
            await player1.StartStep_EnterMyRound();
            await player2.StartStep_EnterMyRound();
            Assert.AreEqual(5, player1.CurrentPlayerHero.CurrentLife);
            Assert.AreEqual(3, player2.CurrentPlayerHero.CurrentLife);
        }

        [Test]
        public async Task ShoupengleiTest_NotExplode()
        {

            var cardToPlay = new Shoupenglei();
            await player1.AddCardInHand(cardToPlay);
            await player1.AddCardInHand(new Sha());
            await player2.AddCardsInHand(new List<CardBase>() { new Sha(), new Shan() });

            Assert.AreEqual(2, player1.CardsInHand.Count);
            Assert.AreEqual(2, player2.CardsInHand.Count);

            var response = await cardToPlay.PlayCard(new CardRequestContext(), player1.RoundContext);
            Assert.AreEqual(5, player1.CurrentPlayerHero.CurrentLife);
            Assert.AreEqual(6, player2.CurrentPlayerHero.CurrentLife);

            gameLevel1.GlobalEventBus.ListenEvent(Guid.NewGuid(), gameLevel1.HostPlayerHero, EventTypeEnum.AfterPanding, (
                async (context, roundContext, responseContext) =>
                {
                    //第1次触发判定时，让判定牌强制为红桃
                    responseContext.ResponseResult = ResponseResultEnum.Failed;
                    var c = responseContext.Cards.FirstOrDefault();
                    responseContext.Cards = new List<CardBase>()
                            {
                                new ChangedCard(new List<CardBase>(){c}, c)
                                {
                                    CardChangeType = CardChangeTypeEnum.Changed,
                                    Number = 13,
                                    FlowerKind=FlowerKindEnum.Heitao
                                }
                            };
                    Console.WriteLine($"测试强制改变判定结果为红桃，原始牌为：{c}");
                }));
            await player1.StartStep_EnterMyRound();
            await player2.StartStep_EnterMyRound();
            Assert.AreEqual(5, player1.CurrentPlayerHero.CurrentLife);
        }
    }
}

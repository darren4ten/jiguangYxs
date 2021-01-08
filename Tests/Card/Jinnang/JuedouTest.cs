using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;
using Logic.ActionManger;
using Logic.Enums;
using Logic.GameLevel;
using Logic.GameLevel.Levels;
using Logic.Model.Cards.BaseCards;
using Logic.Model.Cards.JinlangCards;
using Logic.Model.Enums;
using Logic.Model.Hero.Officer;
using Logic.Model.Hero.Presizdent;
using Logic.Model.Player;
using Logic.Model.Skill;
using Logic.Model.Skill.SubSkill;
using NUnit.Framework;

namespace Tests.Card
{
    [TestFixture]
    public class JuedouTest
    {
        private GameLevel1 gameLevel1;
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
                    qianghua1,
                    shatan1
                });
            var star3Zhuyuanzhang = new PlayerHero(3, new Zhuyuanzhang(), null,
                new List<SkillBase>(){
                    new Qianghua(1,50),
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

            gameLevel1.OnLoad(player1, new List<Player>() { player2 });
            player1.Init();
            player2.Init();
        }

        [Test]
        public void TestJuedou_Xiangyu_Success()
        {
            var cardJuedou = new Juedou().AttachPlayerContext(new PlayerContext() { Player = player1, GameLevel = gameLevel1 });
            player1.CardsInHand.Add(cardJuedou);
            player1.CardsInHand.Add(new Sha().AttachPlayerContext(new PlayerContext() { Player = player1, GameLevel = gameLevel1 }));
            player2.CardsInHand.Add(new Sha().AttachPlayerContext(new PlayerContext() { Player = player2, GameLevel = gameLevel1 }));
            player2.CardsInHand.Add(new Sha().AttachPlayerContext(new PlayerContext() { Player = player2, GameLevel = gameLevel1 }));

            Assert.AreEqual(5, player1.GetCurrentPlayerHero().BaseAttackFactor.MaxLife);
            Assert.AreEqual(6, player2.GetCurrentPlayerHero().BaseAttackFactor.MaxLife);

            var response = cardJuedou.PlayCard(new CardRequestContext()
            {
                //AttackType = AttackTypeEnum.Juedou,
                //CardType = CardTypeEnum.Any,
                //RequestCard = new Shan(),
                TargetPlayers = new List<Player>()
                    {
                        player2
                    }
            }, player1.RoundContext).GetAwaiter().GetResult();
            Console.WriteLine($"Player1的手牌数：" + player1.CardsInHand.Count);
            Assert.AreEqual(5, player1.GetCurrentPlayerHero().CurrentLife);
            Assert.AreEqual(5, player2.GetCurrentPlayerHero().CurrentLife);
        }

        [Test]
        public void TestJuedou_Xiangyu_Success1()
        {
            var cardJuedou = new Juedou().AttachPlayerContext(new PlayerContext() { Player = player2, GameLevel = gameLevel1 });
            player1.CardsInHand.Add(new Sha().AttachPlayerContext(new PlayerContext() { Player = player2, GameLevel = gameLevel1 }));
            player2.CardsInHand.Add(cardJuedou);
            player2.CardsInHand.Add(new Sha().AttachPlayerContext(new PlayerContext() { Player = player2, GameLevel = gameLevel1 }));
            player2.CardsInHand.Add(new Sha().AttachPlayerContext(new PlayerContext() { Player = player2, GameLevel = gameLevel1 }));

            Assert.AreEqual(5, player1.GetCurrentPlayerHero().BaseAttackFactor.MaxLife);
            Assert.AreEqual(1, player1.CardsInHand.Count);
            Assert.AreEqual(3, player2.CardsInHand.Count); 
            Assert.AreEqual(6, player2.GetCurrentPlayerHero().BaseAttackFactor.MaxLife);
            //没有霸王的决斗有霸王的场景
            var response = cardJuedou.PlayCard(new CardRequestContext()
            {
                TargetPlayers = new List<Player>()
                {
                    player1
                }
            }, player1.RoundContext).GetAwaiter().GetResult();
            Console.WriteLine($"Player1的手牌数：" + player1.CardsInHand.Count);
            Assert.AreEqual(4, player1.GetCurrentPlayerHero().CurrentLife);
            Assert.AreEqual(0, player1.CardsInHand.Count);
            Assert.AreEqual(0, player2.CardsInHand.Count); 
            Assert.AreEqual(6, player2.GetCurrentPlayerHero().CurrentLife);
        }

        [Test]
        public void TestJuedou_Failed()
        {
            var cardJuedou = new Juedou().AttachPlayerContext(new PlayerContext() { Player = player1, GameLevel = gameLevel1 });
            player1.CardsInHand.Add(cardJuedou);
            player1.CardsInHand.Add(new Sha().AttachPlayerContext(new PlayerContext() { Player = player2, GameLevel = gameLevel1 }));
            player2.CardsInHand.Add(new Sha().AttachPlayerContext(new PlayerContext() { Player = player2, GameLevel = gameLevel1 }));

            Assert.AreEqual(5, player1.GetCurrentPlayerHero().BaseAttackFactor.MaxLife);
            Assert.AreEqual(6, player2.GetCurrentPlayerHero().BaseAttackFactor.MaxLife);

            var response = cardJuedou.PlayCard(new CardRequestContext()
            {
                //AttackType = AttackTypeEnum.Juedou,
                //CardType = CardTypeEnum.Any,
                //RequestCard = new Shan(),
                TargetPlayers = new List<Player>()
                    {
                        player2
                    }
            }, player1.RoundContext).GetAwaiter().GetResult();
            Console.WriteLine($"Player1的手牌数：" + player1.CardsInHand.Count);
            Assert.AreEqual(5, player1.GetCurrentPlayerHero().CurrentLife);
            Assert.AreEqual(5, player2.GetCurrentPlayerHero().CurrentLife);
        }

        [Test]
        public async Task TestJuedou_Failed1()
        {
            var star2Chengyaojin = new PlayerHero(2, new Chengyaojin(), null,
                new List<SkillBase>()
                {
                });
            player1 = new Player(gameLevel1, new AiActionManager(), new List<PlayerHero>() { star2Chengyaojin })
            {
                PlayerId = 1,
                RoundContext = new RoundContext()
                {
                    AttackDynamicFactor = AttackDynamicFactor.GetDefaultDeltaAttackFactor()
                }
            };
            player1.Init();
            var cardJuedou = new Juedou().AttachPlayerContext(new PlayerContext() { Player = player1, GameLevel = gameLevel1 });
            player1.CardsInHand.Add(cardJuedou);
            await player1.AddCardInHand(new Sha());
            await player2.AddCardInHand(new Sha());

            Assert.AreEqual(5, player1.GetCurrentPlayerHero().BaseAttackFactor.MaxLife);
            Assert.AreEqual(2, player1.CardsInHand.Count);
            Assert.AreEqual(1, player2.CardsInHand.Count);
            Assert.AreEqual(6, player2.GetCurrentPlayerHero().BaseAttackFactor.MaxLife);

            var response = cardJuedou.PlayCard(new CardRequestContext()
            {
                //AttackType = AttackTypeEnum.Juedou,
                //CardType = CardTypeEnum.Any,
                //RequestCard = new Shan(),
                TargetPlayers = new List<Player>()
                {
                    player2
                }
            }, player1.RoundContext).GetAwaiter().GetResult();
            Console.WriteLine($"Player1的手牌数：" + player1.CardsInHand.Count);
            Assert.AreEqual(5, player1.GetCurrentPlayerHero().CurrentLife);
            Assert.AreEqual(0, player1.CardsInHand.Count);
            Assert.AreEqual(0, player2.CardsInHand.Count);
            Assert.AreEqual(5, player2.GetCurrentPlayerHero().CurrentLife);
        }

        [Test]
        public async Task TestJuedou_Failed2()
        {
            var star2Chengyaojin = new PlayerHero(2, new Chengyaojin(), null,
                new List<SkillBase>()
                {
                });
            player1 = new Player(gameLevel1, new AiActionManager(), new List<PlayerHero>() { star2Chengyaojin })
            {
                PlayerId = 1,
                RoundContext = new RoundContext()
                {
                    AttackDynamicFactor = AttackDynamicFactor.GetDefaultDeltaAttackFactor()
                }
            };
            player1.Init();
            var cardJuedou = new Juedou().AttachPlayerContext(new PlayerContext() { Player = player1, GameLevel = gameLevel1 });
            player1.CardsInHand.Add(cardJuedou);
            await player1.AddCardInHand(new Sha());
            await player2.AddCardInHand(new Sha());
            await player2.AddCardInHand(new Sha());

            Assert.AreEqual(5, player1.GetCurrentPlayerHero().BaseAttackFactor.MaxLife);
            Assert.AreEqual(2, player1.CardsInHand.Count);
            Assert.AreEqual(2, player2.CardsInHand.Count);
            Assert.AreEqual(6, player2.GetCurrentPlayerHero().BaseAttackFactor.MaxLife);

            var response = cardJuedou.PlayCard(new CardRequestContext()
            {
                //AttackType = AttackTypeEnum.Juedou,
                //CardType = CardTypeEnum.Any,
                //RequestCard = new Shan(),
                TargetPlayers = new List<Player>()
                {
                    player2
                }
            }, player1.RoundContext).GetAwaiter().GetResult();
            Console.WriteLine($"Player1的手牌数：" + player1.CardsInHand.Count);
            Assert.AreEqual(4, player1.GetCurrentPlayerHero().CurrentLife);
            Assert.AreEqual(0, player1.CardsInHand.Count);
            Assert.AreEqual(0, player2.CardsInHand.Count);
            Assert.AreEqual(6, player2.GetCurrentPlayerHero().CurrentLife);
        }
    }
}

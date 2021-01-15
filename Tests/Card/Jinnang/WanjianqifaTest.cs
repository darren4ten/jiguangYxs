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
using Logic.Model.Hero.Presizdent;
using Logic.Model.Player;
using Logic.Model.Skill;
using Logic.Model.Skill.SubSkill;
using NUnit.Framework;

namespace Tests.Card
{
    [TestFixture]
    public class WanjianqifaTest
    {
        [Test]
        public async Task TestWanjianqifaTest_Success()
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
            var player3 = new Player(gameLevel1, new AiActionManager(), new List<PlayerHero>() { star3Zhuyuanzhang1 })
            {
                PlayerId = 3
            };

            gameLevel1.OnLoad(player1, new List<Player>() { player2, player3 });
            player1.Init();
            player2.Init();
            player3.Init();
            var cardToPlay = new Wanjianqifa().AttachPlayerContext(new PlayerContext() { Player = player1, GameLevel = gameLevel1 });
            player1.CardsInHand.Add(cardToPlay);
            player1.CardsInHand.Add(new Sha().AttachPlayerContext(new PlayerContext() { Player = player2, GameLevel = gameLevel1 }));
            player2.CardsInHand.Add(new Sha().AttachPlayerContext(new PlayerContext() { Player = player2, GameLevel = gameLevel1 }));
            player2.CardsInHand.Add(new Sha().AttachPlayerContext(new PlayerContext() { Player = player2, GameLevel = gameLevel1 }));
            player3.CardsInHand.Add(new Shan().AttachPlayerContext(new PlayerContext() { Player = player3, GameLevel = gameLevel1 }));
            player3.CardsInHand.Add(new Shoupenglei().AttachPlayerContext(new PlayerContext() { Player = player3, GameLevel = gameLevel1 }));

            Assert.AreEqual(6, player2.CurrentPlayerHero.CurrentLife);
            Assert.AreEqual(6, player3.CurrentPlayerHero.CurrentLife);

            var response = await cardToPlay.PlayCard(new CardRequestContext(), player1.RoundContext);

            Assert.AreEqual(5, player2.CurrentPlayerHero.CurrentLife);
            Assert.AreEqual(6, player3.CurrentPlayerHero.CurrentLife);
            Assert.AreEqual(1, player1.CardsInHand.Count);
            Assert.AreEqual(2, player2.CardsInHand.Count);
            Assert.AreEqual(1, player3.CardsInHand.Count);
        }
    }
}

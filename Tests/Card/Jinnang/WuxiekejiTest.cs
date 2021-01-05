using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;
using Logic.ActionManger;
using Logic.Cards;
using Logic.Enums;
using Logic.GameLevel;
using Logic.GameLevel.Levels;
using Logic.Model.Cards.BaseCards;
using Logic.Model.Cards.JinlangCards;
using Logic.Model.Enums;
using Logic.Model.Hero.Presizdent;
using Logic.Model.Player;
using Logic.Model.RequestResponse.Request;
using Logic.Model.Skill;
using Logic.Model.Skill.SubSkill;
using NUnit.Framework;

namespace Tests.Card
{
    [TestFixture]
    public class WuxiekejiTest
    {
        [Test]
        public async Task WuxiekejiTest_Success()
        {
            var gameLevel1 = new GameLevel1();
            var qianghua1 = new Qianghua(5, 30);
            var shatan1 = new Shatan(5, 50);
            var star2Xiangyu = new PlayerHero(2, new Xiangyu(), null,
                new List<SkillBase>(){
                    //shatan1
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
                GroupId = Guid.NewGuid(),
                RoundContext = new RoundContext()
                {
                    AttackDynamicFactor = AttackDynamicFactor.GetDefaultDeltaAttackFactor()
                }
            };
            var player2 = new Player(gameLevel1, new AiActionManager(), new List<PlayerHero>() { star3Zhuyuanzhang })
            {
                GroupId = Guid.NewGuid(),
                PlayerId = 2
            };
            var player3 = new Player(gameLevel1, new AiActionManager(), new List<PlayerHero>() { star3Zhuyuanzhang1 })
            {
                GroupId = Guid.NewGuid(),
                PlayerId = 3
            };

            gameLevel1.OnLoad(player1, new List<Player>() { player2, player3 });
            player1.Init();
            player2.Init();
            player3.Init();
            var cardToPlay = new Sha();
            var cardToPlay1 = new Fenghuolangyan();
            await player1.AddCardInHand(cardToPlay);
            await player1.AddCardInHand(cardToPlay1);
            await player2.AddCardsInHand(new List<CardBase>()
            {
                new Shan(),
                new Wuxiekeji()
            });
            await player3.AddCardsInHand(new List<CardBase>()
            {
                new Shoupenglei(),
            });
            //让player2掉血4.
            var tmpRoundContext = new RoundContext()
            {
                AttackDynamicFactor = AttackDynamicFactor.GetDefaultDeltaAttackFactor()
            };
            tmpRoundContext.AttackDynamicFactor.Damage.ShaDamage += 3;
            await cardToPlay.PlayCard(new CardRequestContext()
            {
                TargetPlayers = new List<Player>()
                {
                    player2
                }
            }, tmpRoundContext);

            Assert.AreEqual(1, player1.CardsInHand.Count);
            Assert.AreEqual(2, player2.CardsInHand.Count);
            Assert.AreEqual(1, player3.CardsInHand.Count);
            Assert.AreEqual(2, player2.GetCurrentPlayerHero().CurrentLife);
            Assert.AreEqual(6, player3.GetCurrentPlayerHero().CurrentLife);

            var response1 = await cardToPlay1.PlayCard(new CardRequestContext(), player1.RoundContext);

            Assert.AreEqual(0, player1.CardsInHand.Count);
            Assert.AreEqual(1, player2.CardsInHand.Count);
            Assert.AreEqual(1, player3.CardsInHand.Count);
            Assert.AreEqual(2, player2.GetCurrentPlayerHero().CurrentLife);
            Assert.AreEqual(5, player3.GetCurrentPlayerHero().CurrentLife);
        }
    }
}

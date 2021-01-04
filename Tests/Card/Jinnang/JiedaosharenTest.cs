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
using Logic.Model.Cards.EquipmentCards;
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
    public class JiedaosharenTest
    {
        [Test]
        public async Task JiedaosharenTest_Success()
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
                    //new Qianghua(1,50),
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
            var cardToPlay = new Jiedaosharen();
            var cardToPlay1 = new Jiedaosharen();
            await player1.AddCardInHand(cardToPlay);
            await player1.AddCardInHand(cardToPlay1);
            await player1.AddCardInHand(new Sha());
            await player2.AddCardsInHand(new List<CardBase>() { new Sha(), new Shan() });
            await player2.AddEquipment(new Panlonggun());
            Assert.AreEqual(3, player1.CardsInHand.Count);
            Assert.AreEqual(2, player2.CardsInHand.Count);
            Assert.AreEqual(5, player1.GetCurrentPlayerHero().CurrentLife);

            var response = await cardToPlay.PlayCard(new CardRequestContext()
            {
                TargetPlayers = new List<Player>()
                {
                    player2,player1
                }
            }, player1.RoundContext);

            Assert.AreEqual(2, player1.CardsInHand.Count);
            Assert.AreEqual(4, player1.GetCurrentPlayerHero().CurrentLife);
            Assert.AreEqual(1, player2.CardsInHand.Count);
            Assert.AreEqual(1, player2.EquipmentSet.Count);

            var response1 = await cardToPlay1.PlayCard(new CardRequestContext()
            {
                TargetPlayers = new List<Player>()
                {
                    player2,player1
                }
            }, player1.RoundContext);

            Assert.AreEqual(2, player1.CardsInHand.Count);
            Assert.AreEqual(4, player1.GetCurrentPlayerHero().CurrentLife);
            Assert.AreEqual(1, player2.CardsInHand.Count);
            Assert.AreEqual(0, player2.EquipmentSet.Count);
        }
    }
}

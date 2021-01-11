using System;
using System.Collections.Generic;
using System.Linq;
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
    public class TannangquwuTest
    {
        [Test]
        public async Task TestTannangquwu_Success()
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

            player1.Init();
            player2.Init();
            gameLevel1.OnLoad(player1, new List<Player>() { player2 });
            var cardToPlay = new Tannangquwu().AttachPlayerContext(new PlayerContext() { Player = player1, GameLevel = gameLevel1 });
            player1.CardsInHand.Add(cardToPlay);
            var p2Sha = new Sha();
            await player2.AddCardInHand(p2Sha);

            var response = await cardToPlay.PlayCard(CardRequestContext.GetBaseCardRequestContext(new List<Player>() { player2 }), player1.RoundContext);
            Console.WriteLine($"Player1的手牌数：" + player1.CardsInHand.Count);
            Assert.AreEqual(true, gameLevel1.TempCardDesk.Cards.Any(c => c == cardToPlay), "探囊取物应该被放入弃牌堆");
            Assert.AreEqual(false, gameLevel1.TempCardDesk.Cards.Any(c => c == p2Sha), "该“杀”不应该被放入弃牌堆。");
            Assert.AreEqual(1, player1.CardsInHand.Count);
            Assert.AreNotEqual(null, player1.CardsInHand.First().PlayerContext);
            Assert.AreEqual(player1.PlayerId, player1.CardsInHand.First().PlayerContext.Player.PlayerId);
            Assert.AreEqual(0, player2.CardsInHand.Count);
        }
    }
}

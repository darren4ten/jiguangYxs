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
using Logic.Model.Cards.EquipmentCards;
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
    public class BolangchuiTest
    {
        [Test]
        public async Task BolangchuiTest_Success()
        {
            var gameLevel1 = new GameLevel1();
            var qianghua1 = new Qianghua(5, 30);
            var shatan1 = new Shatan(5, 50);
            var star2Chengyaojin = new PlayerHero(2, new Chengyaojin(), null,
                new List<SkillBase>(){
                    new Xixue(5,50),
                });
            var star3Zhuyuanzhang = new PlayerHero(3, new Zhuyuanzhang(), null,
                new List<SkillBase>(){
                    new Qianghua(1,50),
                    new Xixue(5,50),
                });
            var player1 = new Player(gameLevel1, new AiActionManager(), new List<PlayerHero>() { star2Chengyaojin })
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
            var bolangchui = new Bolangchui() { CardId = 3, Color = CardColorEnum.Black, Number = 7, FlowerKind = FlowerKindEnum.Heitao }.AttachPlayerContext(new PlayerContext() { Player = player1, GameLevel = gameLevel1 });
            var cardSha = new Sha() { CardId = 1, Color = CardColorEnum.Red, Number = 10, FlowerKind = FlowerKindEnum.Hongtao }.AttachPlayerContext(new PlayerContext() { Player = player1, GameLevel = gameLevel1 });
            player1.CardsInHand.Add(bolangchui);
            player1.CardsInHand.Add(cardSha);
            player1.CardsInHand.Add(new Shoupenglei().AttachPlayerContext(new PlayerContext() { Player = player1, GameLevel = gameLevel1 }));
            player1.CardsInHand.Add(new Fenghuolangyan().AttachPlayerContext(new PlayerContext() { Player = player1, GameLevel = gameLevel1 }));
            player1.CardsInHand.Add(new Yao().AttachPlayerContext(new PlayerContext() { Player = player1, GameLevel = gameLevel1 }));

            player2.CardsInHand.Add(new Sha() { CardId = 2, Color = CardColorEnum.Black, Number = 8, FlowerKind = FlowerKindEnum.Heitao }.AttachPlayerContext(new PlayerContext() { Player = player2, GameLevel = gameLevel1 }));
            player2.CardsInHand.Add(new Shan().AttachPlayerContext(new PlayerContext() { Player = player2, GameLevel = gameLevel1 }));
          
            Assert.AreEqual(0, player1.EquipmentSet.Count);
            //装备博浪锤
            var response = await bolangchui.PlayCard(new CardRequestContext() { }, player1.RoundContext);
            Assert.AreEqual(1, player1.EquipmentSet.Count);
            Assert.AreEqual(4, player1.CardsInHand.Count);
            Assert.AreEqual(6, player2.GetCurrentPlayerHero().CurrentLife);
            //攻击
            var shaResponse = await cardSha.PlayCard(new CardRequestContext()
            {
                RequestId = Guid.NewGuid(),
                TargetPlayers = new List<Player>() { player2 }
            }, player1.RoundContext);

            Console.WriteLine($"Player1的手牌数：" + player1.CardsInHand.Count);
            Assert.AreEqual(1, player1.CardsInHand.Count);
            Assert.AreEqual(1, player2.CardsInHand.Count);
            Assert.AreEqual(5, player2.GetCurrentPlayerHero().CurrentLife);
        }
    }
}

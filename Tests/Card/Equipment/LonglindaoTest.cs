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
using Logic.Model.Hero.Presizdent;
using Logic.Model.Player;
using Logic.Model.Skill;
using Logic.Model.Skill.SubSkill;
using NUnit.Framework;

namespace Tests.Card
{
    [TestFixture]
    public class LonglindaoTest
    {
        [Test]
        public async Task LonglindaoTest_Success()
        {
            var gameLevel1 = new GameLevel1();
            var qianghua1 = new Qianghua(5, 30);
            var shatan1 = new Shatan(5, 50);
            var star2Xiangyu = new PlayerHero(2, new Xiangyu(), null,
                new List<SkillBase>(){
                    new Xixue(5,50),
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
          
            var longlindao = new Longlindao() { CardId = 3, Color = CardColorEnum.Black, Number = 7, FlowerKind = FlowerKindEnum.Heitao }.AttachPlayerContext(new PlayerContext() { Player = player1, GameLevel = gameLevel1 });
            var cardSha = new Sha() { CardId = 1, Color = CardColorEnum.Red, Number = 10, FlowerKind = FlowerKindEnum.Hongtao }.AttachPlayerContext(new PlayerContext() { Player = player1, GameLevel = gameLevel1 });
            var cardSha1 = new Sha() { CardId = 5, Color = CardColorEnum.Red, Number = 8, FlowerKind = FlowerKindEnum.Hongtao }.AttachPlayerContext(new PlayerContext() { Player = player1, GameLevel = gameLevel1 });
            var cardSha2 = new Sha() { CardId = 6, Color = CardColorEnum.Red, Number = 8, FlowerKind = FlowerKindEnum.Fangkuai }.AttachPlayerContext(new PlayerContext() { Player = player1, GameLevel = gameLevel1 });
            player1.CardsInHand.Add(longlindao);
            player1.CardsInHand.Add(cardSha);
            player1.CardsInHand.Add(cardSha1);
            player1.CardsInHand.Add(cardSha2);

            player2.CardsInHand.Add(new Sha() { CardId = 2, Color = CardColorEnum.Black, Number = 8, FlowerKind = FlowerKindEnum.Heitao }.AttachPlayerContext(new PlayerContext() { Player = player2, GameLevel = gameLevel1 }));
            player2.CardsInHand.Add(new Shan().AttachPlayerContext(new PlayerContext() { Player = player2, GameLevel = gameLevel1 }));
            var hufu = new Hufu().AttachPlayerContext(new PlayerContext() { Player = player2, GameLevel = gameLevel1 });
            Assert.AreEqual(0, player1.EquipmentSet.Count);
            Assert.AreEqual(0, player2.EquipmentSet.Count);
            //装备龙鳞刀
            var response = await longlindao.PlayCard(new CardRequestContext() { }, player1.RoundContext);
            var response1 = await hufu.PlayCard(new CardRequestContext() { }, player2.RoundContext);
            Assert.AreEqual(1, player1.EquipmentSet.Count);
            Assert.AreEqual(1, player2.EquipmentSet.Count);
            Assert.AreEqual(3, player1.CardsInHand.Count);
            //攻击
            var shaResponse = await cardSha.PlayCard(new CardRequestContext()
            {
                RequestId = Guid.NewGuid(),
                TargetPlayers = new List<Player>() { player2 }
            }, player1.RoundContext);

            Console.WriteLine($"Player1的手牌数：" + player1.CardsInHand.Count);
            Assert.AreEqual(2, player1.CardsInHand.Count);
            Assert.AreEqual(1, player2.CardsInHand.Count + player2.EquipmentSet.Count + player2.Marks.Count(p => p.MarkType == MarkTypeEnum.Card));
            Assert.AreEqual(1, player1.EquipmentSet.Count);
            Assert.AreEqual(0, player2.EquipmentSet.Count);
            //继续出杀，应该卸载最后一张牌
            var shaResponse1 = await cardSha1.PlayCard(new CardRequestContext()
            {
                RequestId = Guid.NewGuid(),
                TargetPlayers = new List<Player>() { player2 }
            }, player1.RoundContext);
            Assert.AreEqual(1, player1.CardsInHand.Count);
            Assert.AreEqual(0, player2.CardsInHand.Count);
            Assert.AreEqual(1, player1.EquipmentSet.Count);
            Assert.AreEqual(0, player2.EquipmentSet.Count);

            //继续出杀
            var shaResponse2 = await cardSha2.PlayCard(new CardRequestContext()
            {
                RequestId = Guid.NewGuid(),
                TargetPlayers = new List<Player>() { player2 }
            }, player1.RoundContext);
            Assert.AreEqual(0, player1.CardsInHand.Count);
            Assert.AreEqual(0, player2.CardsInHand.Count);
            Assert.AreEqual(5, player2.GetCurrentPlayerHero().CurrentLife);
        }
    }
}

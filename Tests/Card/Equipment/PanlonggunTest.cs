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
    public class PanlonggunTest
    {
        [Test]
        public async Task PanlonggunTest_Success()
        {
            var gameLevel1 = new GameLevel1();
            gameLevel1.OnLoad();
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
            var sha = new Sha() { CardId = 1, Number = 10, FlowerKind = FlowerKindEnum.Hongtao }.AttachPlayerContext(new PlayerContext() { Player = player1, GameLevel = gameLevel1 });
            var panlonggun = new Panlonggun() { CardId = 3, Color = CardColorEnum.Black, Number = 7, FlowerKind = FlowerKindEnum.Heitao }.AttachPlayerContext(new PlayerContext() { Player = player1, GameLevel = gameLevel1 });
            player1.CardsInHand.Add(panlonggun);
            player1.CardsInHand.Add(sha);
            player1.CardsInHand.Add(new Sha() { CardId = 11, Number = 10, FlowerKind = FlowerKindEnum.Heitao }.AttachPlayerContext(new PlayerContext() { Player = player1, GameLevel = gameLevel1 }));
            player1.CardsInHand.Add(new Shan() { CardId = 12, Number = 8, FlowerKind = FlowerKindEnum.Heitao }.AttachPlayerContext(new PlayerContext() { Player = player1, GameLevel = gameLevel1 }));

            player2.CardsInHand.Add(new Shoupenglei() { CardId = 2, Color = CardColorEnum.Black, Number = 8, FlowerKind = FlowerKindEnum.Heitao }.AttachPlayerContext(new PlayerContext() { Player = player2, GameLevel = gameLevel1 }));
            player2.CardsInHand.Add(new Shan().AttachPlayerContext(new PlayerContext() { Player = player2, GameLevel = gameLevel1 }));

            //装备盘龙棍
            var response = await panlonggun.PlayCard(new CardRequestContext() { }, player1.RoundContext);
            Assert.AreEqual(3, player1.CardsInHand.Count);
            Assert.AreEqual(2, player2.CardsInHand.Count);
            //攻击
            var shaResponse = await sha.PlayCard(new CardRequestContext()
            {
                RequestId = Guid.NewGuid(),
                TargetPlayers = new List<Player>() { player2 }
            }, sha.PlayerContext.Player.RoundContext);

            Console.WriteLine($"Player1的手牌数：" + player1.CardsInHand.Count);
            Assert.AreEqual(ResponseResultEnum.Success, shaResponse.ResponseResult);
            Assert.AreEqual(1, player1.CardsInHand.Count);
            Assert.AreEqual(1, player2.CardsInHand.Count);
            Assert.AreEqual(5, player2.GetCurrentPlayerHero().CurrentLife);
        }
    }
}

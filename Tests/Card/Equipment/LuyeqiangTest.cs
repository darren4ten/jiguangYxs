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
    public class LuyeqiangTest
    {
        [Test]
        public async Task LuyeqiangTest_Success()
        {
            try
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

            gameLevel1.OnLoad(player1, new List<Player>() { player2 });
                player1.Init();
                player2.Init();
                var juedou = new Juedou() { CardId = 1, Color = CardColorEnum.Red, Number = 10, FlowerKind = FlowerKindEnum.Hongtao }.AttachPlayerContext(new PlayerContext() { Player = player1, GameLevel = gameLevel1 });
                player1.CardsInHand.Add(juedou);

                var luyeqiang = new Luyeqiang() { CardId = 3, Color = CardColorEnum.Black, Number = 7, FlowerKind = FlowerKindEnum.Heitao }.AttachPlayerContext(new PlayerContext() { Player = player2, GameLevel = gameLevel1 });
                player2.CardsInHand.Add(luyeqiang);
                player2.CardsInHand.Add(new Shoupenglei() { CardId = 2, Color = CardColorEnum.Black, Number = 8, FlowerKind = FlowerKindEnum.Heitao }.AttachPlayerContext(new PlayerContext() { Player = player2, GameLevel = gameLevel1 }));
                player2.CardsInHand.Add(new Shan().AttachPlayerContext(new PlayerContext() { Player = player2, GameLevel = gameLevel1 }));

                //装备芦叶枪
                var response = await luyeqiang.PlayCard(new CardRequestContext() { }, player2.RoundContext);
                Assert.AreEqual(1, player1.CardsInHand.Count);
                Assert.AreEqual(2, player2.CardsInHand.Count);
                //攻击
                var juedouResponse = await juedou.PlayCard(new CardRequestContext()
                {
                    RequestId = Guid.NewGuid(),
                    TargetPlayers = new List<Player>() { player2 }
                }, player1.RoundContext);

                Console.WriteLine($"Player1的手牌数：" + player1.CardsInHand.Count);
                Assert.AreEqual(ResponseResultEnum.Success, juedouResponse.ResponseResult);
                Assert.AreEqual(0, player1.CardsInHand.Count);
                Assert.AreEqual(0, player2.CardsInHand.Count);
                Assert.AreEqual(6, player2.GetCurrentPlayerHero().CurrentLife);
            }
            catch (Exception e)
            {
                Console.WriteLine(e);
                throw;
            }
        }
    }
}

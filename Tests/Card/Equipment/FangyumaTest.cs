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
    public class FangyumaTest
    {
        private GameLevelBase gameLevel1;
        private Player player1;
        private Player player2;
        private Player player3;
        private Player player4;
        [SetUp]
        public void Init()
        {
            gameLevel1 = new GameLevel1();
            var qianghua1 = new Qianghua(5, 30);
            var shatan1 = new Shatan(5, 50);
            var star2Zyz = new PlayerHero(2, new Zhuyuanzhang(), null,
                new List<SkillBase>(){
                    new Xixue(5,50),
                });
            var star3Zhuyuanzhang = new PlayerHero(3, new Zhuyuanzhang(), null,
                new List<SkillBase>(){
                    new Qianghua(1,50),
                    new Xixue(5,50),
                });
            var star3Zhuyuanzhang1 = new PlayerHero(3, new Zhuyuanzhang(), null,
                new List<SkillBase>(){
                    new Qianghua(1,50),
                    new Xixue(5,50),
                });
            var star3Zhuyuanzhang2 = new PlayerHero(3, new Zhuyuanzhang(), null,
                new List<SkillBase>(){
                    new Qianghua(1,50),
                    new Xixue(5,50),
                });
            player1 = new Player(gameLevel1, new AiActionManager(), new List<PlayerHero>() { star2Zyz })
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
            player3 = new Player(gameLevel1, new AiActionManager(), new List<PlayerHero>() { star3Zhuyuanzhang1 })
            {
                PlayerId = 3
            };
            player4 = new Player(gameLevel1, new AiActionManager(), new List<PlayerHero>() { star3Zhuyuanzhang2 })
            {
                PlayerId = 4
            };

            gameLevel1.OnLoad(player1, new List<Player>() { player2, player3, player4 });
            player1.Init();
            player2.Init();
            player3.Init();
            player4.Init();
        }
        [Test]
        public async Task FangyumaTest_Success()
        {
            var sha = new Sha() { CardId = 1, Color = CardColorEnum.Red, Number = 10, FlowerKind = FlowerKindEnum.Hongtao }.AttachPlayerContext(new PlayerContext() { Player = player1, GameLevel = gameLevel1 });
            player1.CardsInHand.Add(sha);

            var fangyuma = new Fangyuma() { CardId = 3, Color = CardColorEnum.Black, Number = 7, FlowerKind = FlowerKindEnum.Heitao }.AttachPlayerContext(new PlayerContext() { Player = player2, GameLevel = gameLevel1 });
            player2.CardsInHand.Add(fangyuma);

            var dist12 = gameLevel1.GetPlayersDistance(player1, player2);
            var dist13 = gameLevel1.GetPlayersDistance(player1, player3);
            var dist14 = gameLevel1.GetPlayersDistance(player1, player4);
            Assert.AreEqual(0, dist12.ShaDistance);
            Assert.AreEqual(0, dist12.TannangDistance);
            Assert.AreEqual(0, dist12.ShaDistanceWithoutWeapon);

            Assert.AreEqual(1, dist13.ShaDistance);
            Assert.AreEqual(1, dist13.TannangDistance);
            Assert.AreEqual(1, dist13.ShaDistanceWithoutWeapon);

            Assert.AreEqual(0, dist14.ShaDistance);
            Assert.AreEqual(0, dist14.TannangDistance);
            Assert.AreEqual(0, dist14.ShaDistanceWithoutWeapon);

            Assert.AreEqual(true, await player1.IsAvailableForPlayer(player2, new Sha(), AttackTypeEnum.Sha));
            Assert.AreEqual(false, await player1.IsAvailableForPlayer(player3, new Sha(), AttackTypeEnum.Sha));
            Assert.AreEqual(true, await player1.IsAvailableForPlayer(player4, new Sha(), AttackTypeEnum.Sha));
            //装备芦叶枪
            var response = await fangyuma.PlayCard(new CardRequestContext() { }, player2.RoundContext);


            var dist120 = gameLevel1.GetPlayersDistance(player1, player2);
            var dist130 = gameLevel1.GetPlayersDistance(player1, player3);
            var dist140 = gameLevel1.GetPlayersDistance(player1, player4);
            Assert.AreEqual(1, dist120.ShaDistance);
            Assert.AreEqual(1, dist120.TannangDistance);
            Assert.AreEqual(1, dist120.ShaDistanceWithoutWeapon);

            Assert.AreEqual(1, dist130.ShaDistance);
            Assert.AreEqual(1, dist130.TannangDistance);
            Assert.AreEqual(1, dist130.ShaDistanceWithoutWeapon);

            Assert.AreEqual(0, dist140.ShaDistance);
            Assert.AreEqual(0, dist140.TannangDistance);
            Assert.AreEqual(0, dist140.ShaDistanceWithoutWeapon);
            Assert.AreEqual(false, await player1.IsAvailableForPlayer(player2, new Sha(), AttackTypeEnum.Sha));
            Assert.AreEqual(false, await player1.IsAvailableForPlayer(player3, new Sha(), AttackTypeEnum.Sha));
            Assert.AreEqual(true, await player1.IsAvailableForPlayer(player4, new Sha(), AttackTypeEnum.Sha));
        }

    }
}

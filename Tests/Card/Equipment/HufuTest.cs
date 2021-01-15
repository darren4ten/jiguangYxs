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
using Logic.Model.Hero.Presizdent;
using Logic.Model.Player;
using Logic.Model.Skill;
using Logic.Model.Skill.SubSkill;
using NUnit.Framework;

namespace Tests.Card
{
    [TestFixture]
    public class HufuTest
    {
        private GameLevelBase _gameLevel;
        private Player _player1;
        private Player _player2;
        #region init
        [SetUp]
        public void Init()
        {
            _gameLevel = new GameLevel1();
            var qianghua1 = new Qianghua(5, 30);
            var shatan1 = new Shatan(5, 50);
            var star2Xiangyu = new PlayerHero(2, new Xiangyu(), null,
                new List<SkillBase>()
                {
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

            _player1 = new Player(_gameLevel, new AiActionManager(), new List<PlayerHero>() { star2Xiangyu })
            {
                PlayerId = 1,
                GroupId = Guid.NewGuid(),
                RoundContext = new RoundContext()
                {
                    AttackDynamicFactor = AttackDynamicFactor.GetDefaultDeltaAttackFactor()
                }
            };
            _player2 = new Player(_gameLevel, new AiActionManager(), new List<PlayerHero>() { star3Zhuyuanzhang })
            {
                GroupId = Guid.NewGuid(),
                PlayerId = 2
            };

            _gameLevel.OnLoad(_player1, new List<Player>() { _player2, });
            _player1.Init();
            _player2.Init();
        }

        #endregion
        [Test]
        public async Task HufuShaMultipleTimes_Success()
        {

            var cardToPlay = new Hufu();
            await _player1.AddCardInHand(cardToPlay);
            await _player1.AddCardsInHand(new List<CardBase>()
            {
                new Sha(),
                new Sha(),
                new Sha()
            });
            await _player2.AddCardsInHand(new List<CardBase>()
            {
                new Shan(),
                new Shan(),
                new Shan(),
            });
            Assert.AreEqual(4, _player1.CardsInHand.Count);
            Assert.AreEqual(3, _player2.CardsInHand.Count);
            Assert.AreEqual(6, _player2.CurrentPlayerHero.CurrentLife);

            await _player1.StartStep_PlayCard();

            Assert.AreEqual(0, _player1.CardsInHand.Count);
            Assert.AreEqual(1, _player2.CardsInHand.Count);
            Assert.AreEqual(4, _player2.CurrentPlayerHero.CurrentLife);
        }
    }
}

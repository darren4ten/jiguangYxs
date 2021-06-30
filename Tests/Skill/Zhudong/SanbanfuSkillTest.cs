using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Logic.ActionManger;
using Logic.Cards;
using Logic.GameLevel;
using Logic.GameLevel.Levels;
using Logic.Model.Cards.BaseCards;
using Logic.Model.Hero.Officer;
using Logic.Model.Hero.Presizdent;
using Logic.Model.Player;
using Logic.Model.Skill;
using Logic.Model.Skill.Interface;
using Logic.Model.Skill.SubSkill;
using Logic.Model.Skill.Zhudong;
using NUnit.Framework;

namespace Tests.Skill.Zhudong
{
    [TestFixture]
    public class SanbanfuSkillTest
    {
        private GameLevelBase _gameLevel;
        private Player _player1;
        private Player _player2;

        #region Init
        [SetUp]
        public void Init()
        {
            _gameLevel = new GameLevel1();
            var shatan1 = new Shatan(5, 50);
            var star2Chengyaojin = new PlayerHero(2, new Chengyaojin(), null,
                new List<SkillBase>()
                {
                    //shatan1
                });
            var star3Chengyaojin = new PlayerHero(3, new Chengyaojin(), null,
                new List<SkillBase>(){
                    new Qianghua(1,50),
                    new Xixue(5,50),
                });

            _player1 = new Player(_gameLevel, new AiActionManager(), new List<PlayerHero>() { star2Chengyaojin })
            {
                PlayerId = 1,
                GroupId = Guid.NewGuid(),
                RoundContext = new RoundContext()
                {
                    AttackDynamicFactor = AttackDynamicFactor.GetDefaultDeltaAttackFactor()
                }
            };
            _player2 = new Player(_gameLevel, new AiActionManager(), new List<PlayerHero>() { star3Chengyaojin })
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
        public async Task Sanbanfu_Success_NoShan()
        {
            var sanbanfuSkill = _player1.CurrentPlayerHero.GetAllMainSkills().FirstOrDefault(p => p is ISkillButton) as ISkillButton;
            Assert.IsNotNull(sanbanfuSkill);
            await _player1.StartStep_EnterMyRound();
            Assert.AreEqual(true, sanbanfuSkill.IsEnabled());
            var btnInfo = sanbanfuSkill.GetButtonInfo();
            await _player1.AddCardsInHand(new List<CardBase>()
            {
                new Sha(),
                new Shan()
            });
            //对方无闪
            Assert.AreEqual(true, btnInfo.IsEnabled);
            await btnInfo.OnClick(new CardRequestContext(), _player1.RoundContext, new CardResponseContext());
            Assert.AreEqual(4, _player2.CurrentPlayerHero.CurrentLife);
            Assert.AreEqual(0, _player1.CardsInHand.Count);
            //触发过三板斧之后不会在
            Assert.AreEqual(false, sanbanfuSkill.IsEnabled(), "回合内触发过三板斧就不会再触发了");
        }

        [Test]
        public async Task Sanbanfu_Success_1Shan()
        {
            var sanbanfuSkill = _player1.CurrentPlayerHero.GetAllMainSkills().FirstOrDefault(p => p is ISkillButton) as ISkillButton;
            Assert.IsNotNull(sanbanfuSkill);
            await _player1.StartStep_EnterMyRound();
            Assert.AreEqual(true, sanbanfuSkill.IsEnabled());
            var btnInfo = sanbanfuSkill.GetButtonInfo();
            await _player1.AddCardsInHand(new List<CardBase>()
            {
                new Sha(),
                new Shan()
            });
            await _player2.AddCardsInHand(new List<CardBase>()
            {
                new Shan()
            });

            //对方1闪
            Assert.AreEqual(true, btnInfo.IsEnabled);
            await btnInfo.OnClick(new CardRequestContext(), _player1.RoundContext, new CardResponseContext());
            Assert.AreEqual(4, _player1.CurrentPlayerHero.CurrentLife);
            Assert.AreEqual(1, _player1.CardsInHand.Count);
            Assert.AreEqual(5, _player2.CurrentPlayerHero.CurrentLife);
            //触发过三板斧之后不会在
            Assert.AreEqual(false, sanbanfuSkill.IsEnabled(), "回合内触发过三板斧就不会再触发了");
        }
    }
}

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
    public class TannangquwuTest
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
            var star3Zhuyuanzhang = new PlayerHero(3, new Chengyaojin(), null,
                new List<SkillBase>(){
                    new Qianghua(1,50),
                    new Xixue(5,50),
                });
            var star3Zhuyuanzhang1 = new PlayerHero(3, new Chengyaojin(), null,
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
        public async Task TestTannangquwu_Success()
        {
            _gameLevel.OnLoad(_player1, new List<Player>() { _player2 });
            var cardToPlay = new Tannangquwu().AttachPlayerContext(new PlayerContext() { Player = _player1, GameLevel = _gameLevel });
            _player1.CardsInHand.Add(cardToPlay);
            var p2Sha = new Sha();
            await _player2.AddCardInHand(p2Sha);

            var response = await cardToPlay.PlayCard(CardRequestContext.GetBaseCardRequestContext(new List<Player>() { _player2 }), _player1.RoundContext);
            Console.WriteLine($"Player1的手牌数：" + _player1.CardsInHand.Count);
            Assert.AreEqual(true, _gameLevel.TempCardDesk.Cards.Any(c => c == cardToPlay), "探囊取物应该被放入弃牌堆");
            Assert.AreEqual(false, _gameLevel.TempCardDesk.Cards.Any(c => c == p2Sha), "该“杀”不应该被放入弃牌堆。");
            Assert.AreEqual(1, _player1.CardsInHand.Count);
            Assert.AreNotEqual(null, _player1.CardsInHand.First().PlayerContext);
            Assert.AreEqual(_player1.PlayerId, _player1.CardsInHand.First().PlayerContext.Player.PlayerId);
            Assert.AreEqual(0, _player2.CardsInHand.Count);
        }

        [Test]
        public async Task TestTannangquwu_Equipment_Success()
        {
            _gameLevel.OnLoad(_player1, new List<Player>() { _player2 });
            var panlonggun = new Panlonggun();
            var cardToPlay = new Tannangquwu();
            await _player1.AddCardInHand(panlonggun);
            await _player1.AddCardInHand(cardToPlay);
            await panlonggun.PlayCard(CardRequestContext.GetBaseCardRequestContext(null),
                    panlonggun.PlayerContext.Player.RoundContext);
            var luyeqiang = new Luyeqiang();
            await _player2.AddCardInHand(luyeqiang);
            await luyeqiang.PlayCard(CardRequestContext.GetBaseCardRequestContext(null),
                luyeqiang.PlayerContext.Player.RoundContext);

            var response = await cardToPlay.PlayCard(CardRequestContext.GetBaseCardRequestContext(new List<Player>() { _player2 }), _player1.RoundContext);
            Console.WriteLine($"Player1的手牌数：" + _player1.CardsInHand.Count);
            Assert.AreEqual(true, _gameLevel.TempCardDesk.Cards.Any(c => c == cardToPlay), "探囊取物应该被放入弃牌堆");
            Assert.AreEqual(false, _gameLevel.TempCardDesk.Cards.Any(c => c == luyeqiang), "该“芦叶枪”不应该被放入弃牌堆。");
            Assert.AreEqual(1, _player1.CardsInHand.Count);
            Assert.AreNotEqual(null, _player1.CardsInHand.First().PlayerContext);
            Assert.AreEqual(_player1.PlayerId, _player1.CardsInHand.First().PlayerContext.Player.PlayerId);
            Assert.AreEqual(0, _player2.CardsInHand.Count);
        }
    }
}

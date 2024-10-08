﻿using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;
using Logic.ActionManger;
using Logic.GameLevel;
using Logic.GameLevel.Levels;
using Logic.Model.Cards.BaseCards;
using Logic.Model.Cards.EquipmentCards;
using Logic.Model.Cards.JinlangCards;
using Logic.Model.Hero.Presizdent;
using Logic.Model.Player;
using Logic.Model.Skill;
using Logic.Model.Skill.SubSkill;
using NUnit.Framework;

namespace Tests.Game
{
    [TestFixture]
    public class GameTest
    {
        private GameLevelBase _gameLevel;
        private Player _player1;
        private Player _player2;
        private Player _player3;
        #region init
        [SetUp]
        public void Init()
        {
            _gameLevel = new GameLevel1();

            var star2Xiangyu = new PlayerHero(2, new Xiangyu(), null,
                new List<SkillBase>()
                {
                    new Qianghua(5, 30),
                    new Shatan(5, 50)
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
            _player3 = new Player(_gameLevel, new AiActionManager(), new List<PlayerHero>() { star3Zhuyuanzhang1 })
            {
                GroupId = _player2.GroupId,
                PlayerId = 3
            };

            _player1.Init();
            _player2.Init();
            _player3.Init();
        }

        #endregion
        [Test]
        public async Task GameOver_Success()
        {

            //游戏正常结束，且我方胜利
            Assert.DoesNotThrowAsync(async () =>
            {
                await _gameLevel.Start(_player1, new List<Player>() { _player2 }, (delegate
                     {
                         var rc = new RoundContext()
                         {
                             AttackDynamicFactor = AttackDynamicFactor.GetDefaultDeltaAttackFactor()
                         };
                         rc.AttackDynamicFactor.Damage.ShaDamage += 6;
                         var sha = new Sha();
                         _player1.AddCardInHand((sha)).GetAwaiter().GetResult();
                         var res = sha.PlayCard(CardRequestContext.GetBaseCardRequestContext(new List<Player>() { _player2 }), rc).GetAwaiter().GetResult();
                         Console.WriteLine(res);
                     }));
            });
        }

        [Test]
        public async Task EquipNewEquipment_Success()
        {
            Assert.DoesNotThrowAsync(async () =>
            {
                await _gameLevel.Start(_player1, new List<Player>() { _player2 }, action: (async delegate
               {
                   var rc = new RoundContext()
                   {
                       AttackDynamicFactor = AttackDynamicFactor.GetDefaultDeltaAttackFactor()
                   };
                   await _player2.AddCardInHand(new Hufu());
                   var tannang = new Tannangquwu();
                   await _player1.AddCardInHand(tannang);
                   await tannang.PlayCard(CardRequestContext.GetBaseCardRequestContext(new List<Player>() { _player2 }), null);
                   await _player1.AddEquipment(new Panlonggun());

                   rc.AttackDynamicFactor.Damage.ShaDamage += 6;
                   var sha = new Sha();
                   _player1.AddCardInHand((sha)).GetAwaiter().GetResult();
                   var res = await sha.PlayCard(CardRequestContext.GetBaseCardRequestContext(new List<Player>() { _player2 }), rc);
                   Console.WriteLine(res);
               }));
            });
        }
    }
}

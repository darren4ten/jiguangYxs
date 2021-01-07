using System;
using System.Collections.Generic;
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
using Logic.Model.RequestResponse.Request;
using Logic.Model.Skill;
using Logic.Model.Skill.SubSkill;
using NUnit.Framework;

namespace Tests.Card
{
    [TestFixture]
    public class WuxiekejiTest
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
            _player3 = new Player(_gameLevel, new AiActionManager(), new List<PlayerHero>() { star3Zhuyuanzhang1 })
            {
                GroupId = Guid.NewGuid(),
                PlayerId = 3
            };

            _gameLevel.OnLoad(_player1, new List<Player>() { _player2, _player3 });
            _player1.Init();
            _player2.Init();
            _player3.Init();
        }

        #endregion
        [Test]
        public async Task WuxiekejiTest_Fenghuolangyan_Success()
        {

            var cardToPlay = new Sha();
            var cardToPlay1 = new Fenghuolangyan();
            await _player1.AddCardInHand(cardToPlay);
            await _player1.AddCardInHand(cardToPlay1);
            await _player2.AddCardsInHand(new List<CardBase>()
            {
                new Shan(),
                new Wuxiekeji()
            });
            await _player3.AddCardsInHand(new List<CardBase>()
            {
                new Shoupenglei(),
            });
            //让player2掉血4.
            var tmpRoundContext = new RoundContext()
            {
                AttackDynamicFactor = AttackDynamicFactor.GetDefaultDeltaAttackFactor()
            };
            tmpRoundContext.AttackDynamicFactor.Damage.ShaDamage += 3;
            await cardToPlay.PlayCard(new CardRequestContext()
            {
                TargetPlayers = new List<Player>()
                {
                    _player2
                }
            }, tmpRoundContext);

            Assert.AreEqual(1, _player1.CardsInHand.Count);
            Assert.AreEqual(2, _player2.CardsInHand.Count);
            Assert.AreEqual(1, _player3.CardsInHand.Count);
            Assert.AreEqual(2, _player2.GetCurrentPlayerHero().CurrentLife);
            Assert.AreEqual(6, _player3.GetCurrentPlayerHero().CurrentLife);

            var response1 = await cardToPlay1.PlayCard(new CardRequestContext(), _player1.RoundContext);

            Assert.AreEqual(0, _player1.CardsInHand.Count);
            Assert.AreEqual(1, _player2.CardsInHand.Count);
            Assert.AreEqual(1, _player3.CardsInHand.Count);
            Assert.AreEqual(2, _player2.GetCurrentPlayerHero().CurrentLife);
            Assert.AreEqual(5, _player3.GetCurrentPlayerHero().CurrentLife);
        }

        [Test]
        public async Task WuxiekejiTest_Wanjianqifa_Success()
        {

            var cardToPlay = new Sha();
            var cardToPlay1 = new Wanjianqifa();
            await _player1.AddCardInHand(cardToPlay);
            await _player1.AddCardInHand(cardToPlay1);
            await _player2.AddCardsInHand(new List<CardBase>()
            {
                new Panlonggun(),
                new Wuxiekeji()
            });
            await _player3.AddCardsInHand(new List<CardBase>()
            {
                new Shoupenglei(),
            });
            //让player2掉血4.
            var tmpRoundContext = new RoundContext()
            {
                AttackDynamicFactor = AttackDynamicFactor.GetDefaultDeltaAttackFactor()
            };
            tmpRoundContext.AttackDynamicFactor.Damage.ShaDamage += 3;
            await cardToPlay.PlayCard(new CardRequestContext()
            {
                TargetPlayers = new List<Player>()
                {
                    _player2
                }
            }, tmpRoundContext);

            Assert.AreEqual(1, _player1.CardsInHand.Count);
            Assert.AreEqual(2, _player2.CardsInHand.Count);
            Assert.AreEqual(1, _player3.CardsInHand.Count);
            Assert.AreEqual(2, _player2.GetCurrentPlayerHero().CurrentLife);
            Assert.AreEqual(6, _player3.GetCurrentPlayerHero().CurrentLife);

            var response1 = await cardToPlay1.PlayCard(new CardRequestContext(), _player1.RoundContext);

            Assert.AreEqual(0, _player1.CardsInHand.Count);
            Assert.AreEqual(1, _player2.CardsInHand.Count);
            Assert.AreEqual(1, _player3.CardsInHand.Count);
            Assert.AreEqual(2, _player2.GetCurrentPlayerHero().CurrentLife);
            Assert.AreEqual(5, _player3.GetCurrentPlayerHero().CurrentLife);
        }


        /// <summary>
        /// 多个无懈可击（3个无懈可击）
        /// </summary>
        /// <returns></returns>
        [Test]
        public async Task WuxiekejiTest_Tannangquwu_MultipleWuxie_Success()
        {

            var cardToPlay = new Tannangquwu();
            await _player1.AddCardInHand(cardToPlay);
            await _player1.AddCardInHand(new Wuxiekeji());
            await _player2.AddCardsInHand(new List<CardBase>()
            {
                new Panlonggun(),
                new Wuxiekeji()
            });
            await _player3.AddCardsInHand(new List<CardBase>()
            {
                new Shoupenglei(),
            });

            Assert.AreEqual(2, _player1.CardsInHand.Count);
            Assert.AreEqual(2, _player2.CardsInHand.Count);
            Assert.AreEqual(1, _player3.CardsInHand.Count);

            var response = await cardToPlay.PlayCard(new CardRequestContext()
            {
                TargetPlayers = new List<Player>()
                {
                    _player2
                }
            }, _player1.RoundContext);

            Assert.AreEqual(1, _player1.CardsInHand.Count);
            Assert.AreEqual(0, _player2.CardsInHand.Count);
            Assert.AreEqual(1, _player3.CardsInHand.Count);
        }

        /// <summary>
        /// 多个无懈可击（4个无懈可击）
        /// </summary>
        /// <returns></returns>
        [Test]
        public async Task WuxiekejiTest_Tannangquwu_MultipleWuxie_Failed()
        {
            _player3.GroupId = _player2.GroupId;
            var cardToPlay = new Tannangquwu();
            await _player1.AddCardInHand(cardToPlay);
            await _player1.AddCardInHand(new Wuxiekeji());
            await _player2.AddCardsInHand(new List<CardBase>()
            {
                new Panlonggun(),
                new Wuxiekeji()
            });
            await _player3.AddCardsInHand(new List<CardBase>()
            {
                new Shoupenglei(),
                new Wuxiekeji()
            });

            Assert.AreEqual(2, _player1.CardsInHand.Count);
            Assert.AreEqual(2, _player2.CardsInHand.Count);
            Assert.AreEqual(2, _player3.CardsInHand.Count);

            var response = await cardToPlay.PlayCard(new CardRequestContext()
            {
                TargetPlayers = new List<Player>()
                {
                    _player2
                }
            }, _player1.RoundContext);

            Assert.AreEqual(0, _player1.CardsInHand.Count);
            Assert.AreEqual(1, _player2.CardsInHand.Count);
            Assert.AreEqual(1, _player3.CardsInHand.Count);
        }

        [Test]
        public async Task WuxiekejiTest_Tannangquwu_Success()
        {

            var cardToPlay = new Tannangquwu();
            await _player1.AddCardInHand(cardToPlay);
            await _player2.AddCardsInHand(new List<CardBase>()
            {
                new Panlonggun(),
                new Wuxiekeji()
            });
            await _player3.AddCardsInHand(new List<CardBase>()
            {
                new Shoupenglei(),
            });

            Assert.AreEqual(1, _player1.CardsInHand.Count);
            Assert.AreEqual(2, _player2.CardsInHand.Count);
            Assert.AreEqual(1, _player3.CardsInHand.Count);

            var response = await cardToPlay.PlayCard(new CardRequestContext()
            {
                TargetPlayers = new List<Player>()
                {
                    _player2
                }
            }, _player1.RoundContext);

            Assert.AreEqual(0, _player1.CardsInHand.Count);
            Assert.AreEqual(1, _player2.CardsInHand.Count);
            Assert.AreEqual(1, _player3.CardsInHand.Count);
        }

        [Test]
        public async Task WuxiekejiTest_Fudichouxin_Success()
        {

            var cardToPlay = new Fudichouxin();
            await _player1.AddCardInHand(cardToPlay);
            await _player2.AddCardsInHand(new List<CardBase>()
            {
                new Panlonggun(),
                new Wuxiekeji()
            });
            await _player3.AddCardsInHand(new List<CardBase>()
            {
                new Shoupenglei(),
            });

            Assert.AreEqual(1, _player1.CardsInHand.Count);
            Assert.AreEqual(2, _player2.CardsInHand.Count);
            Assert.AreEqual(1, _player3.CardsInHand.Count);

            var response = await cardToPlay.PlayCard(new CardRequestContext()
            {
                TargetPlayers = new List<Player>()
                {
                    _player2
                }
            }, _player1.RoundContext);

            Assert.AreEqual(0, _player1.CardsInHand.Count);
            Assert.AreEqual(1, _player2.CardsInHand.Count);
            Assert.AreEqual(1, _player3.CardsInHand.Count);
        }

        [Test]
        public async Task WuxiekejiTest_Juedou_Success()
        {

            var cardToPlay = new Juedou();
            await _player1.AddCardInHand(cardToPlay);
            await _player2.AddCardsInHand(new List<CardBase>()
            {
                new Panlonggun(),
                new Wuxiekeji()
            });
            await _player3.AddCardsInHand(new List<CardBase>()
            {
                new Shoupenglei(),
            });

            Assert.AreEqual(1, _player1.CardsInHand.Count);
            Assert.AreEqual(2, _player2.CardsInHand.Count);
            Assert.AreEqual(1, _player3.CardsInHand.Count);

            var response = await cardToPlay.PlayCard(new CardRequestContext()
            {
                TargetPlayers = new List<Player>()
                {
                    _player2
                }
            }, _player1.RoundContext);

            Assert.AreEqual(0, _player1.CardsInHand.Count);
            Assert.AreEqual(1, _player2.CardsInHand.Count);
            Assert.AreEqual(1, _player3.CardsInHand.Count);
        }

        [Test]
        public async Task WuxiekejiTest_Juedou_MultipleWuxiekeji_Success()
        {

            var cardToPlay = new Juedou();
            //让Player2和3变成同一阵营
            _player3.GroupId = _player2.GroupId;
            await _player1.AddCardInHand(cardToPlay);
            await _player2.AddCardsInHand(new List<CardBase>()
            {
                new Panlonggun(),
                new Wuxiekeji()
            });
            await _player3.AddCardsInHand(new List<CardBase>()
            {
                new Shoupenglei(),
                new Wuxiekeji()
            });

            Assert.AreEqual(1, _player1.CardsInHand.Count);
            Assert.AreEqual(2, _player2.CardsInHand.Count);
            Assert.AreEqual(2, _player3.CardsInHand.Count);

            var response = await cardToPlay.PlayCard(new CardRequestContext()
            {
                TargetPlayers = new List<Player>()
                {
                    _player2
                }
            }, _player1.RoundContext);

            Assert.AreEqual(0, _player1.CardsInHand.Count);
            Assert.AreEqual(1, _player2.CardsInHand.Count);
            Assert.AreEqual(2, _player3.CardsInHand.Count);
        }

        [Test]
        public async Task WuxiekejiTest_Juedou_MultipleWuxiekeji_lowBlood_Success()
        {

            var cardToPlay = new Juedou();
            var sha = new Sha();
            //让Player2和3变成同一阵营
            _player3.GroupId = _player2.GroupId;
            await _player1.AddCardInHand(cardToPlay);
            await _player1.AddCardInHand(sha);
            await _player2.AddCardsInHand(new List<CardBase>()
            {
                new Panlonggun(),
                new Wuxiekeji()
            });
            await _player3.AddCardsInHand(new List<CardBase>()
            {
                new Shoupenglei(),
                new Wuxiekeji()
            });

            //让player2掉血4.
            var tmpRoundContext = new RoundContext()
            {
                AttackDynamicFactor = AttackDynamicFactor.GetDefaultDeltaAttackFactor()
            };
            tmpRoundContext.AttackDynamicFactor.Damage.ShaDamage += 3;
            await sha.PlayCard(new CardRequestContext()
            {
                TargetPlayers = new List<Player>()
                {
                    _player2
                }
            }, tmpRoundContext);

            Assert.AreEqual(1, _player1.CardsInHand.Count);
            Assert.AreEqual(2, _player2.CardsInHand.Count);
            Assert.AreEqual(2, _player3.CardsInHand.Count);

            var response = await cardToPlay.PlayCard(new CardRequestContext()
            {
                TargetPlayers = new List<Player>()
                {
                    _player2
                }
            }, _player1.RoundContext);

            Assert.AreEqual(0, _player1.CardsInHand.Count);
            //无懈可击可能是被player2出的，也可能是被player3出的。
            if (_player2.CardsInHand.Count == 1)
            {
                Assert.AreEqual(2, _player3.CardsInHand.Count);
            }
            else
            {
                Assert.AreEqual(1, _player3.CardsInHand.Count);
            }
        }
    }
}

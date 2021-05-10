using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;
using Logic.ActionManger;
using Logic.GameLevel;
using Logic.GameLevel.Levels;
using Logic.Model.Hero.Presizdent;
using Logic.Model.Player;
using Logic.Model.Skill;
using Logic.Model.Skill.SubSkill;

namespace JiguangYxsConsole
{
    class GameTest
    {
        private GameLevelBase _gameLevel;
        private Player _player1;
        private Player _player2;
        private Player _player3;
        #region init all AI players game.
        public void Init_AllAI()
        {
            _gameLevel = new GameLevel1();

            var star2Xiangyu = new PlayerHero(5, new Xiangyu(), null,
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

            //_gameLevel.OnLoad(_player1, new List<Player>() { _player2, _player3 });
            _player1.Init();
            _player2.Init();
            _player3.Init();
        }

        #endregion

        #region init one human and 2 AI players game.
        public void Init_OneHuman2AI()
        {
            _gameLevel = new GameLevel2();
            _gameLevel.Start();

            var star2Xiangyu = new PlayerHero(5, new Xiangyu(), null,
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

            //_gameLevel.OnLoad(_player1, new List<Player>() { _player2, _player3 });
            _player1.Init();
            _player2.Init();
            _player3.Init();
        }

        #endregion
        public async Task RunGame(int level)
        {
            if (level <= 1)
            {
                Init_AllAI();
                await _gameLevel.Start(_player1, new List<Player>() { _player2, _player3 });
            }
            else if (level == 2)
            {
                _gameLevel = new GameLevel2();
                await _gameLevel.Start();
            }

        }

        public async Task RunGameLevel()
        {
            Init_AllAI();
            await _gameLevel.Start();
        }
    }
}

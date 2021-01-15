﻿using System;
using Logic.Model.Hero.Civilian;
using Logic.Model.Hero.Presizdent;
using Logic.Model.Player;
using System.Collections.Generic;
using System.Threading.Tasks;
using Logic.ActionManger;
using Logic.Model.Skill;
using Logic.Model.Skill.SubSkill;

namespace Logic.GameLevel.Levels
{
    public class GameLevel1 : GameLevelBase
    {
        public GameLevel1()
        {
            Name = "Level 1";
            Description = "Level 1 test.";
        }


        public override async Task Start(Action action = null)
        {
            var star5Xiangyu = new PlayerHero(5, new Xiangyu(), null,
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

            var player1 = new Player(this, new AiActionManager(), new List<PlayerHero>() { star5Xiangyu })
            {
                PlayerId = 1,
                GroupId = Guid.NewGuid(),
                RoundContext = new RoundContext()
                {
                    AttackDynamicFactor = AttackDynamicFactor.GetDefaultDeltaAttackFactor()
                }
            };
            var player2 = new Player(this, new AiActionManager(), new List<PlayerHero>() { star3Zhuyuanzhang })
            {
                GroupId = Guid.NewGuid(),
                PlayerId = 2
            };
            var player3 = new Player(this, new AiActionManager(), new List<PlayerHero>() { star3Zhuyuanzhang1 })
            {
                GroupId = player2.GroupId,
                PlayerId = 3
            };

            player1.Init();
            player2.Init();
            player3.Init();
            await Start(player1, new List<Player>() { player2, player3 }, action);
        }
    }
}

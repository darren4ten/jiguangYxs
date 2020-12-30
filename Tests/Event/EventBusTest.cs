using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;
using Logic.ActionManger;
using Logic.Event;
using Logic.GameLevel;
using Logic.GameLevel.Levels;
using Logic.Model.Enums;
using Logic.Model.Hero.Presizdent;
using Logic.Model.Player;
using Logic.Model.Skill;
using Logic.Model.Skill.SubSkill;
using NUnit.Framework;

namespace Tests.Event
{
    public class EventBusTest
    {
        EventBus eventBus = EventBus.GetInstance();

        [Test]
        public void Test()
        {
            var evtId = Guid.NewGuid();
            var gameLevel1 = new GameLevel1();
            var star2Xiangyu = new PlayerHero(2, new Xiangyu(), null,
                new List<SkillBase>(){
                                        new Shatan(1,1)
                                    });
            var player1 = new Player(gameLevel1, new AiActionManager(), new List<PlayerHero>() { star2Xiangyu })
            {
                PlayerId = 1
            };
            string msg = "";
            eventBus.ListenEvent(evtId, star2Xiangyu, Logic.Model.Enums.EventTypeEnum.AfterShaSuccess, (
                async (context, roundContext, responseContext) =>
                {
                    msg = "触发杀贪";
                    Console.WriteLine(msg);
                    await Task.FromResult("");
                }));

            var t = Task.Run(async () =>
              {
                  await eventBus.TriggerEvent(EventTypeEnum.AfterShaSuccess, player1.GetCurrentPlayerHero(), new CardRequestContext(),
                      new RoundContext(), null);
              });
            Task.WaitAll(t);
            Assert.AreEqual("触发杀贪", msg);
        }
    }
}

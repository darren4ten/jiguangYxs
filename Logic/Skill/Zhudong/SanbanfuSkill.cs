using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Logic.Cards;
using Logic.Enums;
using Logic.GameLevel;
using Logic.Log;
using Logic.Model.Cards.BaseCards;
using Logic.Model.Cards.MutedCards;
using Logic.Model.Enums;
using Logic.Model.Player;
using Logic.Model.RequestResponse.Request;
using Logic.Model.Skill.Interface;

namespace Logic.Model.Skill.Zhudong
{
    /// <summary>
    ///     三板斧
    /// </summary>
    public class SanbanfuSkill : ZhudongSkillBase, ISkillButton, IEnhanceSha
    {
        private Guid _evtRequest = Guid.NewGuid();
        private Guid _evtShaSuccess = Guid.NewGuid();
        private Guid _evtShaFailed = Guid.NewGuid();
        public SanbanfuSkill()
        {
            Name = "Sanbanfu";
            DisplayName = "三板斧";
        }

        public int EnhancePriority()
        {
            return 100;
        }

        public bool IsEnabled()
        {
            //如果可以出杀且技能触发次数<1才能enable
            if (Sha.CanBePlayed(PlayerHero.PlayerContext))
            {
                if (PlayerHero.PlayerContext.Player.RoundContext == null) return false;

                var triggerTimes = GetZhudongTriggerTimes();

                if (triggerTimes <= 1)
                {
                    return true;
                }
            }

            return false;
        }

        public override async Task LoadSkill(PlayerHero playerHero)
        {
            await base.LoadSkill(playerHero);
            //监听事件
            //对于三板斧技能，请求中需要出牌数最大值为2，最小值为1
            PlayerHero.PlayerContext.GameLevel.GlobalEventBus.
                ListenEvent(_evtRequest, PlayerHero, Logic.Model.Enums.EventTypeEnum.BeforeSha, (
                    async (reqContext, roundContext, responseContext) =>
                    {
                        if (reqContext.AttackType == AttackTypeEnum.Sanbanfu)
                        {
                            reqContext.MinCardCountToPlay = 1;
                            reqContext.MaxCardCountToPlay = 2;
                        }
                        await Task.FromResult(0);
                    }));
            //监听杀成功事件，接管默认的杀成功事件。
            //如果杀成功且出的闪的数量是1，则敌我都掉1血我方掉1血;否则敌方掉2血。
            PlayerHero.PlayerContext.GameLevel.GlobalEventBus.
                ListenEvent(_evtShaSuccess, PlayerHero, Logic.Model.Enums.EventTypeEnum.BeforeShaSuccess, (
                    async (reqContext, roundContext, responseContext) =>
                    {
                        if (reqContext.AttackType == AttackTypeEnum.Sanbanfu)
                        {
                            //没出闪，则对方掉血+1，己方弃掉一张牌；
                            if (responseContext.Cards.Count == 0)
                            {
                                reqContext.AttackDynamicFactor.Damage.ShaDamage += 1;
                                if (PlayerHero.PlayerContext.Player.CardsInHand.Any())
                                {
                                    var cardToThrowRes = await PlayerHero.PlayerContext.Player.ActionManager.OnRequestResponseCard(new CardRequestContext()
                                    {
                                        Message = "请弃一张手牌",
                                        CardScope = CardScopeEnum.InHand,
                                        MinCardCountToPlay = 1,
                                        MaxCardCountToPlay = 1,
                                        TargetPlayers = new List<Player.Player>() { PlayerHero.PlayerContext.Player },
                                        AttackType = AttackTypeEnum.ThrowCard
                                    });
                                    if (cardToThrowRes.ResponseResult == ResponseResultEnum.Failed)
                                    {
                                        throw new Exception("三板斧成功必须弃掉一张牌");
                                    }

                                    await PlayerHero.PlayerContext.Player.RemoveCardsInHand(cardToThrowRes.Cards, null, null, null);
                                }
                            }

                        }
                    }));
            //监听杀失败事件，如果杀失败，则自己掉1血
            PlayerHero.PlayerContext.GameLevel.GlobalEventBus.
                ListenEvent(_evtShaSuccess, PlayerHero, Logic.Model.Enums.EventTypeEnum.AfterShaFailed, (
                    async (reqContext, roundContext, responseContext) =>
                    {
                        if (reqContext.AttackType == AttackTypeEnum.Sanbanfu)
                        {
                            if (responseContext.Cards.Count == 2)
                            {
                                await SelfLoseLife();
                            }
                            //对方出闪，则敌我各掉血1
                            else if (responseContext.Cards.Count == 1)
                            {
                                await SelfLoseLife();
                                var req = CardRequestContext.GetBaseCardRequestContext(null);
                                req.AttackDynamicFactor.Damage.ShaDamage = 1;
                                req.AttackType = AttackTypeEnum.None;
                                await reqContext.TargetPlayers.First().CurrentPlayerHero.LoseLife(new LoseLifeRequest()
                                {
                                    DamageType = DamageTypeEnum.Sanbanfu,
                                    RequestTaskCompletionSource = new TaskCompletionSource<CardResponseContext>(),
                                    CardRequestContext = req,
                                });
                            }
                        }
                    }));
        }

        public SkillButtonInfo GetButtonInfo()
        {
            var player = PlayerHero.PlayerContext.Player;
            return new SkillButtonInfo
            {
                Text = "三板斧",
                Description = "",
                SkillType = SkillType(),
                BtnEnableCheck = IsEnabled,
                OnClick = async (context, roundContext, responseContext) =>
                {
                    //请求出杀
                    var res = await player.ResponseCard(new CardRequestContext
                    {
                        CardScope = CardScopeEnum.InHand,
                        AttackType = AttackTypeEnum.Sha,
                        RequestCard = new Sha(),
                        MaxCardCountToPlay = 1,
                        MinCardCountToPlay = 1,
                        SrcPlayer = player,
                        TargetPlayers = new List<Player.Player> { player }
                    }, responseContext, roundContext);
                    if (res.ResponseResult == ResponseResultEnum.Success || res.Cards != null && res.Cards.Count >= 1)
                    {
                        var sha = res.Cards.First();
                        var request = CardRequestContext.GetBaseCardRequestContext(null);
                        request.AttackType = AttackTypeEnum.Sanbanfu;
                        bool triggered = await sha.Popup(request);
                        if (triggered)
                        {
                            Console.WriteLine(
                                $"{PlayerHero.PlayerContext.Player.PlayerId}的【{PlayerHero.PlayerContext.Player.CurrentPlayerHero.Hero.DisplayName}】发动三板斧。");
                            PlayerHero.PlayerContext.GameLevel.LogManager.LogAction(
                                new RichTextParagraph(
                                    new RichTextWrapper($"{player.PlayerId}【{player.CurrentPlayerHero.Hero.DisplayName}】",
                                        RichTextWrapper.GetColor(ColorEnum.Blue)),
                                    new RichTextWrapper("发动"),
                                    new RichTextWrapper("三板斧", RichTextWrapper.GetColor(ColorEnum.Red)),
                                    new RichTextWrapper("。")
                                ));
                        }

                    }
                    else
                    {
                        responseContext.ResponseResult = ResponseResultEnum.Failed;
                    }

                    //触发时，提示选择一个攻击目标
                    await Task.FromResult(0);
                }
            };
        }

        public override Task UnLoadSkill()
        {
            //注销监听事件
            PlayerHero.PlayerContext.GameLevel.GlobalEventBus.RemoveEventListener(EventTypeEnum.BeforeSha, _evtRequest);
            PlayerHero.PlayerContext.GameLevel.GlobalEventBus.RemoveEventListener(EventTypeEnum.BeforeShaSuccess, _evtShaSuccess);
            PlayerHero.PlayerContext.GameLevel.GlobalEventBus.RemoveEventListener(EventTypeEnum.AfterShaFailed, _evtShaFailed);
            return base.UnLoadSkill();
        }

        public override SkillTypeEnum SkillType()
        {
            return SkillTypeEnum.SanBanfu;
        }

        private async Task SelfLoseLife()
        {
            var req = CardRequestContext.GetBaseCardRequestContext(null);
            req.AttackDynamicFactor.Damage.ShaDamage = 1;
            req.AttackType = AttackTypeEnum.None;
            await PlayerHero.LoseLife(new LoseLifeRequest()
            {
                DamageType = DamageTypeEnum.None,
                RequestTaskCompletionSource = new TaskCompletionSource<CardResponseContext>(),
                CardRequestContext = req,
                SrcRoundContext = PlayerHero.PlayerContext.Player.RoundContext
            });
        }
    }
}
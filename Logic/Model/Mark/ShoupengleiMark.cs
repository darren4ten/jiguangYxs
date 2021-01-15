using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Logic.Enums;
using Logic.GameLevel;
using Logic.Model.Cards.JinlangCards;
using Logic.Model.Enums;
using Logic.Model.RequestResponse.Request;

namespace Logic.Model.Mark
{
    /// <summary>
    /// 手捧雷标记
    /// </summary>
    public class ShoupengleiMark : MarkBase
    {
        private Guid eventId;
        public ShoupengleiMark()
        {
            MarkStatus = Enums.MarkStatusEnum.NotStarted;
            MarkType = Enums.MarkTypeEnum.Card;
            Description = "判定黑桃2-9则爆炸";
        }

        public override void Init()
        {
            eventId = Guid.NewGuid();
            PlayerContext.GameLevel.GlobalEventBus.ListenEvent(eventId, PlayerContext.Player.CurrentPlayerHero, EventTypeEnum.BeforeEnterMyRound, (
                async (context, roundContext, responseContext) =>
                {
                    context.AttackDynamicFactor = context.AttackDynamicFactor ??
                                                             AttackDynamicFactor.GetDefaultBaseAttackFactor();
                    //检查是否有无懈可击
                    var wxResponse = await PlayerContext.GameLevel.GroupRequestWithConfirm(new CardRequestContext()
                    {
                        RequestCard = new Wuxiekeji(),
                        SrcPlayer = PlayerContext.Player,
                        TargetPlayers = PlayerContext.GameLevel.Players
                    });
                    if (wxResponse.ResponseResult == Enums.ResponseResultEnum.Wuxiekeji)
                    {
                        wxResponse.ResponseResult = Enums.ResponseResultEnum.Success;
                        wxResponse.Message = "请求被无懈可击";
                        //无懈可击之后，将该标记转移到下一个或者的玩家
                        await MoveMark();
                        return;
                    }

                    //继续判定
                    var pandingResponse = await PlayerContext.GameLevel.Panding(context, (card => card.FlowerKind == FlowerKindEnum.Heitao && card.Number >= 2 && card.Number <= 9));
                    if (pandingResponse.ResponseResult == Enums.ResponseResultEnum.Cancelled)
                    {
                        responseContext.ResponseResult = Enums.ResponseResultEnum.Cancelled;
                        responseContext.Message = "请求被取消，停止判定";
                        return;
                    }
                    else if (pandingResponse.ResponseResult == Enums.ResponseResultEnum.Failed)
                    {
                        responseContext.ResponseResult = Enums.ResponseResultEnum.Success;
                        responseContext.Message = $"手捧雷判定不生效。";
                        Console.WriteLine($"【手捧雷】判定不生效，判定牌为【{pandingResponse.Cards.FirstOrDefault()}】");
                        //转移标记
                        await MoveMark();
                        return;
                    }
                    else if (pandingResponse.ResponseResult == Enums.ResponseResultEnum.Success)
                    {
                        Console.WriteLine($"【手捧雷】判定生效，判定牌为【{pandingResponse.Cards.FirstOrDefault()}】");
                        //判定生效，爆炸
                        await PlayerContext.Player.CurrentPlayerHero.LoseLife(new LoseLifeRequest()
                        {
                            CardRequestContext = context,
                            CardResponseContext = responseContext,
                            DamageType = DamageTypeEnum.Shoupenglei,
                            SrcRoundContext = roundContext
                        });
                        //移除标记
                        await PlayerContext.Player.RemoveMark(this);
                    }
                    else
                    {
                        Console.WriteLine("[手捧雷]未处理的response:" + pandingResponse.ResponseResult);
                    }
                }));
        }

        public override void Reset()
        {
            PlayerContext.GameLevel.GlobalEventBus.RemoveEventListener(EventTypeEnum.BeforeEnterMyRound, eventId);
        }

        private async Task MoveMark()
        {
            //需要判断下一个player身上是否已经有手捧雷了，如果有，则去找下下一个玩家
            var nextPlayer = PlayerContext.Player.GetNextPlayer(false);
            while (nextPlayer != null && nextPlayer != PlayerContext.Player)
            {
                if (nextPlayer.Marks?.Any(m => m is ShoupengleiMark) == true)
                {
                    nextPlayer = nextPlayer.GetNextPlayer(false);
                    continue;
                }
                break;
            }
            await PlayerContext.Player.MoveMark(nextPlayer, this);
            Console.WriteLine($"手捧雷【{this.Cards.FirstOrDefault()}】转移到{nextPlayer.PlayerId}【{nextPlayer.CurrentPlayerHero.Hero.DisplayName}】");
        }
    }
}

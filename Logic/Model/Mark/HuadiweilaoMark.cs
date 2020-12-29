using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;
using Logic.Enums;
using Logic.Model.Cards.JinlangCards;
using Logic.Model.Enums;

namespace Logic.Model.Mark
{
    /// <summary>
    /// 画地为牢标记
    /// </summary>
    public class HuadiweilaoMark : MarkBase
    {
        private Guid eventId;
        public HuadiweilaoMark()
        {
            MarkStatus = Enums.MarkStatusEnum.NotStarted;
            MarkType = Enums.MarkTypeEnum.Card;
            Description = "画地为牢";
        }

        public override void Init()
        {
            eventId = Guid.NewGuid();
            PlayerContext.GameLevel.GlobalEventBus.ListenEvent(eventId, PlayerContext.Player.GetCurrentPlayerHero(), EventTypeEnum.BeforeEnterMyRound, (
                async (context, roundContext, responseContext) =>
                {
                    //检查是否有无懈可击
                    var wxResponse = await JinnangBase.GroupRequestWuxiekeji(context, responseContext, roundContext);
                    if (wxResponse.ResponseResult == Enums.ResponseResultEnum.Wuxiekeji)
                    {
                        wxResponse.ResponseResult = Enums.ResponseResultEnum.Success;
                        wxResponse.Message = "请求被无懈可击";
                        //无懈可击之后，将该标记删掉
                        await RemoveMark();
                        return;
                    }

                    //继续判定
                    var pandingResponse = await PlayerContext.GameLevel.Panding(context, (card => card.FlowerKind == FlowerKindEnum.Hongtao));
                    if (pandingResponse.ResponseResult == Enums.ResponseResultEnum.Cancelled)
                    {
                        responseContext.ResponseResult = Enums.ResponseResultEnum.Cancelled;
                        responseContext.Message = "请求被取消，停止判定";
                        return;
                    }
                    else if (pandingResponse.ResponseResult == Enums.ResponseResultEnum.Success)
                    {
                        responseContext.ResponseResult = Enums.ResponseResultEnum.Success;
                        responseContext.Message = $"画地为牢判定不生效。";
                        //移除标记
                        await RemoveMark();
                        return;
                    }
                    else if (pandingResponse.ResponseResult == Enums.ResponseResultEnum.Failed)
                    {
                        //判定生效，画地为牢
                        PlayerContext.Player.RoundContext.AttackDynamicFactor.SkipOption.ShouldSkipPlayCard = true;
                        //移除标记
                        await RemoveMark();
                    }
                    else
                    {
                        Console.WriteLine("[画地为牢]未处理的response:" + pandingResponse.ResponseResult);
                    }
                }));
        }

        public override void Reset()
        {
            PlayerContext.GameLevel.GlobalEventBus.RemoveEventListener(EventTypeEnum.BeforeEnterMyRound, eventId);
        }

        private async Task RemoveMark()
        {
            await PlayerContext.Player.RemoveMark(this);
            Reset();
            PlayerContext = null;
        }
    }
}

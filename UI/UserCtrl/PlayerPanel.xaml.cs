using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Documents;
using System.Windows.Input;
using JgYxs.UI.Model;
using Logic.Cards;
using Logic.GameLevel;
using Logic.Model.Enums;
using Logic.Model.Player;

namespace JgYxs.UI.UserCtrl
{
    /// <summary>
    /// Interaction logic for PlayerPanel.xaml
    /// </summary>
    public partial class PlayerPanel : UserControl
    {
        private GameDataContext GameDataContext => (GameDataContext)DataContext;
        private Player Player => GameDataContext.CurrentPlayer;

        public PlayerPanel()
        {
            //Player = (Player)DataContext;
            InitializeComponent();
        }

        protected override void OnInitialized(EventArgs e)
        {
            base.OnInitialized(e);
        }

        protected void RefreshCardPopoutStatus(CardBase card)
        {
            var item = CardsInHand.ItemContainerGenerator.ContainerFromItem(card) as ListBoxItem;
            if (card.IsPopout)
            {
                item.Margin = new Thickness(item.Margin.Left, item.Margin.Top - 5, item.Margin.Right,
                    item.Margin.Bottom + 5);
            }
            else
            {
                item.Margin = new Thickness(item.Margin.Left, item.Margin.Top + 5, item.Margin.Right,
                    item.Margin.Bottom - 5);
            }
        }

        public async void OnCardInHandClicked(object sender, RoutedEventArgs routedEventArgs)
        {
            //手牌被单击，则检查手牌是否可以打出
            var cardCtrl = (ListBoxItem)sender;
            var card = (CardBase)cardCtrl.DataContext;
            //检查最大可以弹出的手牌数 maxPopCardCount（默认1）
            // maxPopCardCount=CardRequestContext!=null && CardRequestContext.MaxCardCountToPlay>1?CardRequestContext.MaxCardCountToPlay:1;
            //检查当前已弹出的牌数是否超过了maxPopCardCount,如果是，则将最先弹出的卡牌收回；如果没有，则直接将该牌弹出
            var recentRequest = Player.CardRequestContexts?.LastOrDefault();
            var maxPopCardCount = recentRequest != null && recentRequest.MaxCardCountToPlay > 1 ? recentRequest.MaxCardCountToPlay : 1;
            if (!card.IsPopout)
            {
                if (Player.CardsInHand.Count(c => c.IsPopout) >= maxPopCardCount)
                {
                    var firstCard = Player.CardsInHand.First(c => c.IsPopout);
                    firstCard.IsPopout = false;
                    RefreshCardPopoutStatus(firstCard);
                }
                card.IsPopout = true;
            }
            else
            {
                card.IsPopout = false;
            }
            RefreshCardPopoutStatus(card);
            Player.PlayerUiState.RestoreSelectStatus(SelectStatusEnum.Idle);

            var cards = Player.CardsInHand.Where(p => p.IsPopout).ToList();
            //检查弹出的牌是否可以被打出，即检查弹出的牌是否是CardRequestContext需要的或者RoundContext的需求，
            //  如果不符合，则ActionBar的DisplayMessage需要提示出牌不合理，只显示取消按钮
            //  如果符合，则显示确认和取消按钮
            //被动出牌
            if (recentRequest != null)
            {
                if (recentRequest.IsMatch(cards))
                {
                    //继续检查出牌是否符合要求，如果符合要求
                    //提示确认、取消
                    Player.PlayerUiState.SetupOkCancelActionBar(recentRequest.RequestTaskCompletionSource, "确定出牌", "确定", "取消", (async () =>
                        {
                            recentRequest.RequestTaskCompletionSource.SetResult(new CardResponseContext()
                            {
                                Cards = cards,
                                ResponseResult = ResponseResultEnum.Success
                            });
                            return await Task.FromResult(true);
                        }));
                }
                else
                {
                    //提示还需要选择至少MaxCardCountToPlay-MinCardCountToPlay 张牌
                    Player.PlayerUiState.SetupOkCancelActionBar(recentRequest.RequestTaskCompletionSource, "选择的牌不可以被打出", null, "取消");
                }

                await recentRequest.RequestTaskCompletionSource.Task;
            }
            //主动出牌
            else
            {
                if (Player.IsInZhudongMode())
                {
                    var firstCard = cards.FirstOrDefault();
                    if (firstCard != null)
                    {
                        if (firstCard.CanBePlayed())
                        {
                            //var popTcs = new CancellationTokenSource();
                            var popRes = await firstCard.Popup();
                            //Player.PlayerUiState.SetupOkCancelActionBar(Player.RoundContext.RoundTaskCompletionSource, "确定出牌", "确定", "取消", (async () =>
                            //    {
                            //        Player.RoundContext.RoundTaskCompletionSource.SetResult(new CardResponseContext()
                            //        {
                            //            Cards = cards,
                            //            ResponseResult = ResponseResultEnum.Success
                            //        });
                            //        return await Task.FromResult(true);
                            //    }));
                        }
                        else
                        {
                            //提示还需要选择至少MaxCardCountToPlay-MinCardCountToPlay 张牌
                            Player.PlayerUiState.SetupOkCancelActionBar(Player.RoundContext?.RoundTaskCompletionSource, "选择的牌不可以被打出", null, "结束出牌");
                        }

                    }
                    else
                    {
                        //提示还需要选择至少MaxCardCountToPlay-MinCardCountToPlay 张牌
                        Player.PlayerUiState.SetupOkCancelActionBar(Player.RoundContext?.RoundTaskCompletionSource, "请出牌", null, "结束出牌");
                    }
                    if (Player.RoundContext?.RoundTaskCompletionSource != null)
                    {
                        await Player.RoundContext.RoundTaskCompletionSource.Task;
                    }
                }
                else
                {
                    Player.PlayerUiState.SetupOkCancelActionBar(null, "选择的牌不可以被打出", null, null);
                }

               
            }
        }
    }
}

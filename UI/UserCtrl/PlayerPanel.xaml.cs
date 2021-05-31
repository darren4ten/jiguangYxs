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
            Player.PlayerUiState.DefaultCardInHandClicked(card, RefreshCardPopoutStatus);
        }
    }
}

using System.Windows;
using System.Windows.Controls;
using Logic.Cards;

namespace JgYxs.UI.UserCtrl
{
    /// <summary>
    /// Interaction logic for Card.xaml
    /// </summary>
    public partial class Card : UserControl
    {
        public CardBase CurCard => (CardBase)DataContext;
        private double _initMarginTop;
        public CardBase GetCard()
        {
            return (CardBase)DataContext;
        }

        public Card()
        {
            InitializeComponent();
            _initMarginTop = this.Margin.Top;
        }

        public double CurrentMarginTop => CurCard.IsPopout ? _initMarginTop - 5 : _initMarginTop;

        private void ButtonBase_OnClick(object sender, RoutedEventArgs e)
        {
            //////单击卡牌时，
            //var card = GetCard();
            //var mt = IsCardPopup ? this.Margin.Top - 5 : this.Margin.Top + 5;
            //var mb = IsCardPopup ? this.Margin.Top + 5 : this.Margin.Top - 5;
            //this.Margin = new Thickness(this.Margin.Left, mt, this.Margin.Right, mb);
            //IsCardPopup = !IsCardPopup;
            ////MessageBox.Show($"You clicked {card.Number} {card.CardType}:{card.Name}");
        }
    }
}

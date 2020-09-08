using System;
using System.Collections.Generic;
using System.Text;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Navigation;
using System.Windows.Shapes;
using Logic.Cards;

namespace UI.UserCtrl
{
    /// <summary>
    /// Interaction logic for Card.xaml
    /// </summary>
    public partial class Card : UserControl
    {
        private bool IsCardPopup = false;
        //public Card(CardBase card)
        //{
        //    InitializeComponent();
        //    this.DataContext = card;
        //}
        public CardBase GetCard()
        {
            return (CardBase)DataContext;
        }

        public Card()
        {
            InitializeComponent();
        }


        private void ButtonBase_OnClick(object sender, RoutedEventArgs e)
        {
            var card = GetCard();
            var mt = IsCardPopup ? this.Margin.Top - 5 : this.Margin.Top + 5;
            var mb = IsCardPopup ? this.Margin.Top + 5 : this.Margin.Top - 5;
            this.Margin = new Thickness(this.Margin.Left, mt, this.Margin.Right, mb);
            IsCardPopup = !IsCardPopup;
            //MessageBox.Show($"You clicked {card.Number} {card.CardType}:{card.Name}");
        }
    }
}

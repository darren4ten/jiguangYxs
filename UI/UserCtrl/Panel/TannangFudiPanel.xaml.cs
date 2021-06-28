using System.Linq;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Media;
using Logic.Cards;
using Logic.GameLevel.Panel;
using Logic.Model.Player;

namespace JgYxs.UI.UserCtrl.Panel
{
    /// <summary>
    /// Interaction logic for CardPanel.xaml
    /// </summary>
    public partial class TannangFudiPanel : UserControl
    {

        public TannangFudiPanel()
        {
            InitializeComponent();
        }

        private PanelBase panel => (PanelBase)DataContext;

        protected void RefreshCardPopoutStatus(Button item, CardBase card)
        {
            if (card.IsPopout)
            {
                item.BorderThickness = new Thickness(2, 2, 2, 2);
                item.BorderBrush = new SolidColorBrush(Color.FromRgb(232, 232, 26));
            }
            else
            {

                item.BorderThickness = new Thickness(0, 0, 0, 0);
                item.BorderBrush = new SolidColorBrush(Color.FromRgb(11, 11, 11));
            }

        }

        private static T FindParent<T>(DependencyObject dependencyObject) where T : DependencyObject
        {
            var parent = VisualTreeHelper.GetParent(dependencyObject);
            if (parent == null) return null;
            var parentT = parent as T;
            return parentT ?? FindParent<T>(parent);
        }

        private async void OnCardInHandClicked(object sender, RoutedEventArgs e)
        {
            var cardCtrl = (ListBoxItem)sender;
            //var lb= FindParent<ListBox>(cardCtrl);
            var pCard = (PanelCard)cardCtrl.DataContext;
            var btn = e.OriginalSource is Button button ? button : null;
            await panel.OnClickedHandler(pCard.Card, (c =>
              {
                  RefreshCardPopoutStatus(btn, c);
              }));
        }
    }
}

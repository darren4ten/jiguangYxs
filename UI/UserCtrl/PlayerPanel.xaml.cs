using System.Windows;
using System.Windows.Controls;
using JgYxs.UI.Model;

namespace JgYxs.UI.UserCtrl
{
    /// <summary>
    /// Interaction logic for CurrentPlayerPanel.xaml
    /// </summary>
    public partial class PlayerPanel : UserControl
    {
        public UIState GetUiAction()
        {
            return (UIState)DataContext;
        }
        public PlayerPanel()
        {

            InitializeComponent();
        }

        private void Btn1Click(object sender, RoutedEventArgs e)
        {
            GetUiAction()?.BtnAction1?.BtnRoutedEventHandler?.Invoke(sender, e);
        }

        private void Btn2Click(object sender, RoutedEventArgs e)
        {
            GetUiAction()?.BtnAction2?.BtnRoutedEventHandler?.Invoke(sender, e);
        }
    }
}

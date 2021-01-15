using System;
using System.Windows;
using System.Windows.Controls;
using JgYxs.UI.Model;
using Logic.Model.Player;

namespace JgYxs.UI.UserCtrl
{
    /// <summary>
    /// Interaction logic for PlayerPanel.xaml
    /// </summary>
    public partial class PlayerPanel : UserControl
    {
        Player Player { get; set; }
        public UIState GetUiAction()
        {
            return (UIState)DataContext;
        }
        public PlayerPanel()
        {
            //Player = (Player)DataContext;
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

        protected override void OnInitialized(EventArgs e)
        {
            base.OnInitialized(e);
        }
    }
}

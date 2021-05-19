using System;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Input;
using System.Windows.Media;
using Logic.Model.Hero;
using Logic.Model.Player;

namespace JgYxs.UI.UserCtrl
{
    /// <summary>
    /// Interaction logic for Hero.xaml
    /// </summary>
    public partial class Hero : UserControl
    {
        public Player Player { get; set; }
        public bool ShowRight { get; set; }

        public Hero()
        {
            if (DataContext != null)
            {
                Player = (Player)DataContext;
            }

            TestStr = "asdfasfasdfsdf";
            this.Loaded += Hero_Loaded;
            InitializeComponent();
        }

        private void Hero_Loaded(object sender, RoutedEventArgs e)
        {
            var parent = VisualTreeHelper.GetParent(this);
            if (parent != null && parent is Window)
            {
                var pWin = (Window)parent;
                Vector offset = VisualTreeHelper.GetOffset(this);
                ShowRight = pWin.Width - offset.X < this.Width + 200;
                TestStr = (this.Width + offset.X) + (ShowRight ? "Right" : "Left");
            }

        }

        public string TestStr { get; set; }

        protected override void OnInitialized(EventArgs e)
        {
            base.OnInitialized(e);
        }

        private async void OnPlayerClicked(object sender, MouseButtonEventArgs e)
        {
            var dt = ((Image)sender).DataContext;
            var player = (Player)dt;
            if (player.PlayerUiState.OnPlayerClicked != null)
            {
                await player.PlayerUiState.OnPlayerClicked();
            }
        }
    }
}

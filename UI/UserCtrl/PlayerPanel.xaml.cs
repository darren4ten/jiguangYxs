using System;
using System.Windows;
using System.Windows.Controls;
using JgYxs.UI.Model;
using Logic.GameLevel;
using Logic.Model.Player;

namespace JgYxs.UI.UserCtrl
{
    /// <summary>
    /// Interaction logic for PlayerPanel.xaml
    /// </summary>
    public partial class PlayerPanel : UserControl
    {
        Player Player { get; set; }
    
        public PlayerPanel()
        {
            //Player = (Player)DataContext;
            InitializeComponent();
        }

        protected override void OnInitialized(EventArgs e)
        {
            base.OnInitialized(e);
        }
    }
}

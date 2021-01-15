using System;
using System.Windows.Controls;
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

        //public Player CurPlayer => DataContext == null ? null : (Player)DataContext;
        //public PlayerHero PlayerHero => CurPlayer == null ? null : CurPlayer.CurrentPlayerHero;
        //public HeroBase CurHero => PlayerHero == null ? null : PlayerHero.Hero;
        public Hero()
        {
            if (DataContext != null)
            {
                Player = (Player)DataContext;
            }
            InitializeComponent();
        }

        protected override void OnInitialized(EventArgs e)
        {
            base.OnInitialized(e);
        }
    }
}

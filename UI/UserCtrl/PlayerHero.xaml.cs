using System.Windows.Controls;

namespace JgYxs.UI.UserCtrl
{
    /// <summary>
    /// Interaction logic for PlayerHero.xaml
    /// </summary>
    public partial class PlayerHero : UserControl
    {
        public Model.PlayerHero PlayerHeroObj { get; set; }
        public PlayerHero()
        {
            InitializeComponent();
            this.DataContext = PlayerHeroObj;
            //this.DataContext = new Model.PlayerHero()
            //{
            //    CurrentLife = 11,
            //    MaxLife = 12,
            //    Cards = new List<CardBase>()
            //};
        }
    }
}

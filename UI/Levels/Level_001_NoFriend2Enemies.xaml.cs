using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Shapes;
using Logic.Cards;
using Logic.GameLevel;
using Logic.GameLevel.Levels;
using Logic.Model.Cards.BaseCards;
using Logic.Model.Player;

namespace UI.Levels
{
    /// <summary>
    /// Interaction logic for Level_001_NoFriend2Enemies.xaml
    /// </summary>
    public partial class Level_001_NoFriend2Enemies : Window
    {
        private GameLevelBase _gameLevel;
        public Player CurrentPlayer { get; set; }
        public Player Player2 { get; set; }
        public Player Player3 { get; set; }
        public CardBase TestCard { get; set; }
        public int testNumber { get; set; }
        public Level_001_NoFriend2Enemies()
        {
            this.DataContext = this;
            testNumber = 999;
            TestCard = new Sha() { Number = 10 };
            InitializeComponent();
            TestCard.Number = 66;
        }

        protected override void OnInitialized(EventArgs e)
        {
            _gameLevel = new GameLevel1();
            this.Dispatcher.BeginInvoke(new Action(async () =>
            {
                await _gameLevel.Start((() =>
                {
                    CurrentPlayer = _gameLevel.CurrentPlayer;
                    Player2 = CurrentPlayer.GetNextPlayer(false);
                    Player3 = Player2.GetNextPlayer(false);
                  var  TestCard1 = CurrentPlayer.CardsInHand.FirstOrDefault();
                    TestCard.Number = TestCard1.Number;
                }));
            }));
            base.OnInitialized(e);
        }

    }
}

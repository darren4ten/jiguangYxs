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
using JgYxs.UI.Model;
using Logic.ActionManger;
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
        public GameDataContext GameDataContext { get; set; } = new GameDataContext()
        {
            CurrentPlayer = new Player(new GameLevel1(), new AiActionManager(), new List<Logic.Model.Player.PlayerHero>())
        };

        public List<string> Names { get; set; }
        public string TestName { get; set; }
        public CardBase TestCard { get; set; }
        public Level_001_NoFriend2Enemies()
        {
            InitializeComponent();
        }

        public Level_001_NoFriend2Enemies(GameDataContext game)
        {
            this.GameDataContext = game;
            Names = new List<string>() { "Zhangsan", "Lisi" };
            TestName = "Lucy";
        
            InitializeComponent();
        }

        protected override void OnInitialized(EventArgs e)
        {
            CurPlayerPanel.DataContext = GameDataContext;
            Player2.DataContext = GameDataContext.Player2;
            Player3.DataContext = GameDataContext.Player3;
            GameDataContext.GameLevel.LogManager.AttachRecevier(HistoryPanel);
            RemainCards.DataContext = GameDataContext.GameLevel;
            TempCardDesk.DataContext = GameDataContext.GameLevel.TempCardDesk;
            base.OnInitialized(e);
        }
    }
}

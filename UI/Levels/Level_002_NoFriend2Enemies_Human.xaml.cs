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
using JgYxs.UI.Levels;
using JgYxs.UI.Model;
using Logic.ActionManger;
using Logic.Cards;
using Logic.GameLevel;
using Logic.GameLevel.Levels;
using Logic.Model.Cards.BaseCards;
using Logic.Model.Cards.EquipmentCards;
using Logic.Model.Cards.JinlangCards;
using Logic.Model.Mark;
using Logic.Model.Player;

namespace UI.Levels
{
    /// <summary>
    /// Interaction logic for Level_001_NoFriend2Enemies.xaml
    /// </summary>
    public partial class Level_002_NoFriend2Enemies_Human : LevelWindow
    {
        public Level_002_NoFriend2Enemies_Human()
        {
            InitializeComponent();
        }

        public Level_002_NoFriend2Enemies_Human(GameDataContext game)
        {
            this.GameDataContext = game;
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
            ActionBar.DataContext = GameDataContext.GameLevel.CurrentPlayer.PlayerUiState.ActionBar;
            //GameDataContext.Player2.AddMark(new ShoupengleiMark()).GetAwaiter().GetResult();
            //GameDataContext.Player2.AddMark(new HuadiweilaoMark()).GetAwaiter().GetResult();

            GameDataContext.CurrentPlayer.AddCardsInHand(new List<CardBase>()
            {
                new Fudichouxin(),
                new Tannangquwu(),
                new Bolangchui(),
                new Sha(),
                new Shan()
            }).GetAwaiter().GetResult();
            GameDataContext.Player2.AddCardsInHand(new List<CardBase>()
            {
                new Juedou(),
                new Wuxiekeji(),
                new Fangyuma(),
                new Shan()
            }).GetAwaiter().GetResult();

            base.OnInitialized(e);
        }
    }
}

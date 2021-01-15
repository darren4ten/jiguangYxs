using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using JgYxs.UI.Model;
using Logic.Cards;
using Logic.GameLevel;
using Logic.GameLevel.Levels;
using Logic.Model.Cards.BaseCards;
using Logic.Model.Player;
using UI.Levels;

namespace JgYxs.UI
{
    /// <summary>
    /// Interaction logic for MainWindow.xaml
    /// </summary>
    public partial class MainWindow : Window
    {
        public MainWindow()
        {
            InitializeComponent();
            Init();
        }

        public void Init()
        {
            GameDataContext gameDataContext = new GameDataContext() { TestStr="ATestString"};
            gameDataContext.GameLevel = new GameLevel1();
            this.Dispatcher.BeginInvoke(new Action(async () =>
            {
                await gameDataContext.GameLevel.Start((() =>
                {
                    gameDataContext.CurrentPlayer = gameDataContext.GameLevel.CurrentPlayer;
                    gameDataContext.Player2 = gameDataContext.CurrentPlayer.GetNextPlayer(false);
                    gameDataContext.Player3 = gameDataContext.Player2.GetNextPlayer(false);
                    var level1 = new Level_001_NoFriend2Enemies(gameDataContext);
                    this.Hide();
                    level1.Show();
                }));
            }));
        }
    }
}

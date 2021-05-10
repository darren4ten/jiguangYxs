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

            //var testWindow = new TestWindow();
            //testWindow.Show();
            //Close();
            //return;
            InitLevel(2);
        }

        private void InitLevel(int level)
        {
            GameDataContext gameDataContext = new GameDataContext() { TestStr = "ATestString" };
            if (level == 1)
            {
                gameDataContext.GameLevel = new GameLevel1();
                this.Dispatcher.BeginInvoke(new Action(async () =>
                {
                    await gameDataContext.GameLevel.Start((() =>
                    {
                        gameDataContext.CurrentPlayer = gameDataContext.GameLevel.CurrentPlayer;
                        gameDataContext.Player2 = gameDataContext.CurrentPlayer.GetNextPlayer(false);
                        gameDataContext.Player3 = gameDataContext.Player2.GetNextPlayer(false);
                        var level1 = new Level_001_NoFriend2Enemies(gameDataContext);
                        this.Close();
                        level1.Show();
                    }));
                }));
            }
            else if (level == 2)
            {
                gameDataContext.GameLevel = new GameLevel2();
                this.Dispatcher.BeginInvoke(new Action(async () =>
                {
                    await gameDataContext.GameLevel.Start((() =>
                    {
                        gameDataContext.CurrentPlayer = gameDataContext.GameLevel.CurrentPlayer;
                        gameDataContext.Player2 = gameDataContext.CurrentPlayer.GetNextPlayer(false);
                        gameDataContext.Player3 = gameDataContext.Player2.GetNextPlayer(false);
                        var level1 = new Level_002_NoFriend2Enemies_Human(gameDataContext);
                        this.Close();
                        level1.Show();
                    }));
                }));
            }

        }
    }
}

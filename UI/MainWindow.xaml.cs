using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using JgYxs.UI.Model;
using JgYxs.UI.UserCtrl.Panel;
using Logic.Cards;
using Logic.GameLevel;
using Logic.GameLevel.Levels;
using Logic.GameLevel.Panel;
using Logic.Model.Cards.BaseCards;
using Logic.Model.Cards.EquipmentCards;
using Logic.Model.Cards.JinlangCards;
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

            //Test();
            InitLevel(2);
        }

        private void Test()
        {
            var tn = new TannangFudiPanel();
            tn.DataContext = new PanelBase()
            {
                DisplayMessage = "探囊取物",
                InHandCards = new ObservableCollection<PanelCard>()
                {
                    new PanelCard(new Sha(),true),
                    new PanelCard(new Shoupenglei(),true),
                },
                EquipmentCards = new ObservableCollection<PanelCard>()
                {
                    new PanelCard(new Luyeqiang(),true),
                    new PanelCard(new Hufu(),true),
                },
                OnClickedHandler =  (async (card, action) =>
                {
                    card.IsPopout = !card.IsPopout;
                    action(card);
                    await Task.FromResult(0);
                })
            };

            var win = new Window()
            {
                Content = tn,
                Title = "探囊取物",
                Width = tn.Width,
                Height = tn.Height,
                WindowStartupLocation = WindowStartupLocation.CenterScreen
            };
            win.ShowDialog();
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
                        level1.Left = 0;
                        level1.Top = 300;
                        this.Close();
                        level1.Show();
                    }));
                }));
            }

        }
    }
}

using System;
using System.Collections.Generic;
using System.Text;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Shapes;
using Logic.GameLevel;
using Logic.GameLevel.Levels;

namespace UI.Levels
{
    /// <summary>
    /// Interaction logic for Level_001_NoFriend2Enemies.xaml
    /// </summary>
    public partial class Level_001_NoFriend2Enemies : Window
    {
        private GameLevelBase _gameLevel;

        public Level_001_NoFriend2Enemies()
        {
            InitGame();
            InitializeComponent();
        }

        public void InitGame()
        {
            _gameLevel = new GameLevel1();
        }
    }
}

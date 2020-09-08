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
using System.Windows.Navigation;
using System.Windows.Shapes;
using WpfApp1.Model;
using WpfApp1.UserCtrl;
using PlayerHero = WpfApp1.Model.PlayerHero;

namespace UI
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

        private void Init()
        {
            var uiAction = new UIState()
            {
                BtnAction1 = new BtnAction()
                {
                    BtnText = "OK",
                    IsVisible = Visibility.Visible,
                    BtnRoutedEventHandler = (obj, e) => { MessageBox.Show("Click btn1"); }
                },
                BtnAction2 = new BtnAction()
                {
                    BtnText = "Cancel",
                    IsVisible = Visibility.Visible,
                },
                PlayerHero = new PlayerHero
                {
                    MaxLife = 5,
                    CurrentLife = 4,
                    Cards = new List<CardBase>()
                    {
                        new CardBase()
                        {
                            Number=2,
                        }
                    }
                }
            };
            CurrentPlayerPanel.DataContext = uiAction;
        }
    }
}

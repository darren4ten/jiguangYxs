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
using System.Windows.Navigation;
using System.Windows.Shapes;
using WpfApp1.Model;

namespace WpfApp1.UserCtrl
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

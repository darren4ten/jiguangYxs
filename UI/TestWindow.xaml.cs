using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Text;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Shapes;
using Logic.Cards;
using Logic.Enums;
using Logic.Model.Cards.EquipmentCards;
using Logic.Model.Cards.EquipmentCards.Defense;
using Logic.Util.Extension;

namespace JgYxs.UI
{
    /// <summary>
    /// Interaction logic for TestWindow.xaml
    /// </summary>
    public partial class TestWindow : Window
    {
        public ObservableCollection<CardBase> EquipmentSet { get; set; } = new ObservableCollection<CardBase>();
        public TestWindow()
        {
            this.DataContext = EquipmentSet;

            InitializeComponent();
        }

        protected override void OnInitialized(EventArgs e)
        {
            EquipmentSet.AddRange( new List<CardBase>(){
                new Langyabang() { Number = 1, FlowerKind = FlowerKindEnum.Hongtao },
                new Yuruyi() { Number = 8, FlowerKind = FlowerKindEnum.Fangkuai },
                new Jingongma() { Number = 13, FlowerKind = FlowerKindEnum.Meihua },
                new Fangyuma() { Number = 10, FlowerKind = FlowerKindEnum.Heitao }
            });
            LvEquipment.ItemsSource = EquipmentSet;
            base.OnInitialized(e);
        }
    }
}

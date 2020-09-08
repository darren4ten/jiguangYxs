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
    /// Interaction logic for CurrentPlayerPanel.xaml
    /// </summary>
    public partial class CurrentPlayerPanel : UserControl
    {
        public UIState GetUiAction()
        {
            return (UIState)DataContext;
        }
        public CurrentPlayerPanel()
        {

            InitializeComponent();
        }

        private void Btn1Click(object sender, RoutedEventArgs e)
        {
            GetUiAction()?.BtnAction1?.BtnRoutedEventHandler?.Invoke(sender, e);
        }

        private void Btn2Click(object sender, RoutedEventArgs e)
        {
            GetUiAction()?.BtnAction2?.BtnRoutedEventHandler?.Invoke(sender, e);
        }
    }
}

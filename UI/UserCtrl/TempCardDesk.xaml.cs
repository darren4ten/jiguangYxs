using System;
using System.Collections.Generic;
using System.Collections.Specialized;
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

namespace JgYxs.UI.UserCtrl
{
    /// <summary>
    /// Interaction logic for TempCardDesk.xaml
    /// </summary>
    public partial class TempCardDesk : UserControl
    {
        public TempCardDesk()
        {
            InitializeComponent();
            ((INotifyCollectionChanged)LbDesk.Items).CollectionChanged += OnTempCardsSizeChanged
                ;
        }

        private void OnTempCardsSizeChanged(object sender, NotifyCollectionChangedEventArgs e)
        {
            if (LbDesk.Items.Count > 0)
            {
                LbDesk.ScrollIntoView(LbDesk.Items[^1]);
            }
        }
    }
}

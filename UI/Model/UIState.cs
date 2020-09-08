using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Runtime.CompilerServices;
using System.Text;
using System.Windows;

namespace WpfApp1.Model
{
    public class UIState
    {
        public string Tip { get; set; }
        public BtnAction BtnAction1 { get; set; }
        public BtnAction BtnAction2 { get; set; }

        public PlayerHero PlayerHero { get; set; }
    }

    public class BtnAction : INotifyPropertyChanged
    {
        public string BtnText { get; set; }

        public Visibility IsVisible { get; set; }


        public Action<object, RoutedEventArgs> BtnRoutedEventHandler { get; set; }

        public event PropertyChangedEventHandler PropertyChanged;

        protected virtual void OnPropertyChanged([CallerMemberName] string propertyName = null)
        {
            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propertyName));
        }
    }
}

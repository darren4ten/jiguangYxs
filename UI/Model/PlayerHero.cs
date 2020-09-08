using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.ComponentModel;
using System.Runtime.CompilerServices;
using System.Text;
using Logic.Cards;

namespace WpfApp1.Model
{
    public class PlayerHero : INotifyPropertyChanged
    {
        private int maxLife;
        private int currentLife;
        public int MaxLife
        {
            get { return maxLife; }
            set
            {
                maxLife = value;
                OnPropertyChanged("MaxLife");
            }
        }
        public int CurrentLife
        {
            get { return currentLife; }
            set
            {
                currentLife = value;
                OnPropertyChanged("CurrentLife");
            }
        }

        public ObservableCollection<CardBase> CardsInHand { get; set; }
        public List<CardBase> Cards { get; set; }
        public event PropertyChangedEventHandler PropertyChanged;

        protected virtual void OnPropertyChanged([CallerMemberName] string propertyName = null)
        {
            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propertyName));
        }
    }
}

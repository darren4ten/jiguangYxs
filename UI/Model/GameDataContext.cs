using Logic.Annotations;
using Logic.GameLevel;
using Logic.Model.Player;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Runtime.CompilerServices;
using System.Text;

namespace JgYxs.UI.Model
{
    public class GameDataContext : INotifyPropertyChanged
    {
        GameLevelBase _GameLevel;
        public GameLevelBase GameLevel
        {
            get { return _GameLevel; }
            set
            {
                _GameLevel = value;
            }
        }

        Player _CurrentPlayer;
        public Player CurrentPlayer
        {
            get { return _CurrentPlayer; }
            set
            {
                _CurrentPlayer = value;
            }
        }
        Player _Player2;
        public Player Player2
        {
            get { return _Player2; }
            set
            {
                _Player2 = value;
            }
        }
        Player _Player3;
        public Player Player3
        {
            get { return _Player3; }
            set
            {
                _Player3 = value;
            }
        }

        string _TestStr;
        public string TestStr
        {
            get { return _TestStr; }
            set
            {
                _TestStr = value;
            }
        }

        public event PropertyChangedEventHandler PropertyChanged;

        [NotifyPropertyChangedInvocator]
        protected virtual void OnPropertyChanged([CallerMemberName] string propertyName = null)
        {
            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propertyName));
        }
    }
}

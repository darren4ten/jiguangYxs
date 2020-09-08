using Logic.Enums;
using Logic.Model.Enums;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Runtime.CompilerServices;
using System.Text;

namespace Logic.Cards
{
    public abstract class CardBase : INotifyPropertyChanged
    {
        public int CardId { get; set; }
        public string Name { get; set; }
        /// <summary>
        /// The displayname of the card, eg: sha, shan
        /// </summary>
        public string DisplayName { get; set; }

        public string DisplayNumberText { get; set; }

        public string Description { get; set; }
        public FlowerKindEnum FlowerKind { get; set; }
        public CardTypeEnum CardType { get; set; }
        public CardColorEnum Color { get; set; }
        public string Image { get; set; }
        public int Number { get; set; }

        public CardTargetTypeEnum TargetType { get; set; }

        public List<Object> Targets { get; set; }

        protected CardContext CardContext { get; set; }

        public void SetCardContext(CardContext cardContext)
        {
            this.CardContext = cardContext;
        }

        /// <summary>
        /// Whether the card can be played or not.
        /// </summary>
        public abstract bool CanBePlayedFunc();

        public abstract void TriggerResultFunc();

        public virtual void PlayCard()
        {
            if (!CanBePlayedFunc())
            {
                Console.WriteLine("Card cannot be played.");
                return;
            }
        }

        public virtual bool IsTargetSelectable(CardContext cardContext)
        {
            return false;
        }

        //public abstract void ThrowCard();

        public event PropertyChangedEventHandler PropertyChanged;

        protected virtual void OnPropertyChanged([CallerMemberName] string propertyName = null)
        {
            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propertyName));
        }
    }
}

using Logic.Cards;
using Logic.Util;
using System;
using System.Collections.Generic;
using System.Text;
using System.Linq;

namespace Logic.GameLevel
{
    public abstract class GameLevelBase
    {
        public int LevelId { get; set; }

        public string Name { get; set; }
        public string Description { get; set; }

        protected List<CardBase> UnUsedCardStack { get; set; }
        protected List<CardBase> UsedCardStack { get; set; }

        protected List<Player.Player> Players { get; set; }

        protected virtual void LoadCards()
        {
            this.UnUsedCardStack = new CardStackUtil().GenerateNewCardStack().ToList();
            this.UsedCardStack = new List<CardBase>();
        }
        protected virtual void LoadPlayers()
        {
            this.Players = new List<Player.Player>();
        }

        public virtual void OnLoad()
        {
            this.LoadCards();
            this.LoadPlayers();
        }

        public virtual void Start()
        {
            Console.WriteLine("Game Started!");
        }

        public virtual void End()
        {
            this.Players = null;
            this.UnUsedCardStack = null;
            this.UsedCardStack = null;
            //Play animation
            Console.WriteLine("Game over!");
        }
    }
}

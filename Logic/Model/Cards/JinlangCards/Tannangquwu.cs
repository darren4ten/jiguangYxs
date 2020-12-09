﻿using System;
using System.Threading.Tasks;
using Logic.Cards;

namespace Logic.Model.Cards.JinlangCards
{
    /// <summary>
    /// 探囊取物
    /// </summary>
    public class Tannangquwu : JinnangBase
    {
        public Tannangquwu()
        {
            this.Description = "探囊取物";
            this.Name = "Tannangquwu";
            this.DisplayName = "探囊取物";
        }

        public override bool CanBePlayed()
        {
            return true;
        }

        public override Task Popup()
        {
            throw new NotImplementedException();
        }
    }
}

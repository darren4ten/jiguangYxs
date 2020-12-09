﻿using System;
using System.Threading.Tasks;
using Logic.Cards;

namespace Logic.Model.Cards.JinlangCards
{
    /// <summary>
    /// 休养生息
    /// </summary>
    public class Xiuyangshengxi : JinnangBase
    {
        public Xiuyangshengxi()
        {
            this.Description = "休养生息";
            this.Name = "Xiuyangshengxi";
            this.DisplayName = "休养生息";
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

﻿using System;
using System.Threading.Tasks;
using Logic.Cards;

namespace Logic.Model.Cards.JinlangCards
{
    /// <summary>
    /// 无懈可击
    /// </summary>
    public class Wuxiekeji : JinnangBase
    {
        public Wuxiekeji()
        {
            this.Description = "无懈可击";
            this.Name = "Wuxiekeji";
            this.DisplayName = "无懈可击";
        }

        public override Task Popup()
        {
            throw new NotImplementedException();
        }
    }
}

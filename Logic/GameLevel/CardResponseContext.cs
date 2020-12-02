using System;
using System.Collections.Generic;
using System.Text;
using Logic.Cards;
using Logic.Model.Enums;

namespace Logic.GameLevel
{
    /// <summary>
    /// 响应上下文
    /// </summary>
    public class CardResponseContext
    {
        public List<CardBase> Cards { get; set; }

        /// <summary>
        /// 响应结果
        /// </summary>
        public ResponseResultEnum ResponseResult { get; set; }
    }
}

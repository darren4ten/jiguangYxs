using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;
using Logic.Model.Enums;

namespace Logic.Model.Player
{
    public class PlayerUIState
    {
        public ToastMessage ToastMessage { get; set; }

        public DisplayMessage DisplayMessage { get; set; }

        public BtnAction BtnAction1 { get; set; }
        public BtnAction BtnAction2 { get; set; }

        public SelectStatusEnum SelectStatus { get; set; }

        public string BoarderColor { get; set; }

        public string BackgroundColor { get; set; }

        /// <summary>
        /// UI绑定的Player
        /// </summary>
        public Player BindPlayer { get; }

        /// <summary>
        /// 当前Player选中的目标
        /// </summary>
        public List<Player> SelectedTargets { get; }

        public PlayerUIState(Player bindPlayer)
        {
            BindPlayer = bindPlayer;
        }
    }

    public class BtnAction
    {
        public string BtnText { get; set; }
        public bool IsVisible { get; set; }

        public Func<Task> BtnRoutedEventHandler { get; set; }
    }

    public class DisplayMessage
    {
        public bool IsVisible { get; set; }

        public string Content { get; set; }
    }

    public class ToastMessage
    {
        public bool IsVisible { get; set; }

        /// <summary>
        /// 持续毫秒数
        /// </summary>
        public int DurationMiliSeconds { get; set; }

        /// <summary>
        /// 具体文本内容
        /// </summary>
        public string Content { get; set; }
    }
}

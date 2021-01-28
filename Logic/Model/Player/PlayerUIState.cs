using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Runtime.CompilerServices;
using System.Text;
using System.Threading.Tasks;
using Logic.Annotations;
using Logic.GameLevel;
using Logic.Model.Enums;

namespace Logic.Model.Player
{
    public delegate Task<bool?> BtnRoutedEventHandler();
    public class PlayerUIState : INotifyPropertyChanged
    {
        public ActionBar ActionBar { get; set; }

        public SelectStatusEnum SelectStatus { get; set; }

        public string BoarderColor { get; set; }

        public string BackgroundColor { get; set; }

        /// <summary>
        /// UI绑定的Player
        /// </summary>
        public Player BindPlayer { get; }

        public PlayerUIState(Player bindPlayer)
        {
            BindPlayer = bindPlayer;
        }

        public event PropertyChangedEventHandler PropertyChanged;

        [NotifyPropertyChangedInvocator]
        protected virtual void OnPropertyChanged([CallerMemberName] string propertyName = null)
        {
            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propertyName));
        }

        /// <summary>
        /// 设置UIstate为一个有“确认”,“取消”按钮的对象
        /// </summary>
        /// <param name="displayMessage"></param>
        /// <returns></returns>
        public void SetupOkCancelActionBar(TaskCompletionSource<CardResponseContext> tcs, string displayMessage, BtnRoutedEventHandler btn1EventHandler = null, BtnRoutedEventHandler btn2EventHandler = null)
        {
            ActionBar = new ActionBar();
            ActionBar.Visiable = true;
            ActionBar.DisplayMessage = new DisplayMessage()
            {
                Content = displayMessage,
                IsVisible = true
            };
            ActionBar.BtnAction1 = new BtnAction()
            {
                BtnText = "确认",
                IsVisible = true,
                BtnRoutedEventHandler = async () =>
                {
                    ActionBar.Visiable = false;
                    if (btn1EventHandler == null)
                    {
                        tcs.SetResult(new CardResponseContext() { ResponseResult = ResponseResultEnum.Success });
                    }
                    else
                    {
                        return await btn1EventHandler();
                    }
                    return await Task.FromResult(true);
                }
            };
            ActionBar.BtnAction2 = new BtnAction()
            {
                BtnText = "取消",
                IsVisible = true,
                BtnRoutedEventHandler = async () =>
                {
                    ActionBar.Visiable = false;
                    if (btn2EventHandler == null)
                    {
                        tcs.SetResult(new CardResponseContext() { ResponseResult = ResponseResultEnum.Cancelled });
                    }
                    else
                    {
                        return await btn2EventHandler();
                    }
                    return await Task.FromResult(false);
                }
            };
        }
    }

    public class ActionBar : INotifyPropertyChanged
    {
        public Guid ActionId { get; set; }
        public bool Visiable { get; set; }
        public ToastMessage ToastMessage { get; set; }

        public DisplayMessage DisplayMessage { get; set; }

        public BtnAction BtnAction1 { get; set; }
        public BtnAction BtnAction2 { get; set; }
        public event PropertyChangedEventHandler PropertyChanged;

        [NotifyPropertyChangedInvocator]
        protected virtual void OnPropertyChanged([CallerMemberName] string propertyName = null)
        {
            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propertyName));
        }
    }

    public class BtnAction : INotifyPropertyChanged
    {
        public string BtnText { get; set; }
        public bool IsVisible { get; set; }

        public BtnRoutedEventHandler BtnRoutedEventHandler { get; set; }

        public event PropertyChangedEventHandler PropertyChanged;

        [NotifyPropertyChangedInvocator]
        protected virtual void OnPropertyChanged([CallerMemberName] string propertyName = null)
        {
            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propertyName));
        }
    }

    public class DisplayMessage : INotifyPropertyChanged
    {
        public bool IsVisible { get; set; }

        public string Content { get; set; }
        public event PropertyChangedEventHandler PropertyChanged;

        [NotifyPropertyChangedInvocator]
        protected virtual void OnPropertyChanged([CallerMemberName] string propertyName = null)
        {
            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propertyName));
        }
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

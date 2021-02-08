using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Runtime.CompilerServices;
using System.Text;
using System.Threading.Tasks;
using Logic.Annotations;
using Logic.GameLevel;
using Logic.Model.Enums;
using Logic.Model.RequestResponse.Request;

namespace Logic.Model.Player
{
    public delegate Task<bool?> BtnRoutedEventHandler();
    public delegate Task<bool?> CardEventHandler(object sender);
    public class PlayerUIState : INotifyPropertyChanged
    {
        public ActionBar ActionBar { get; set; }

        public SelectStatusEnum SelectStatus { get; set; }

        public string BoarderColor { get; set; }

        public string BackgroundColor { get; set; }

        /// <summary>
        /// 玩家被单击的事件
        /// </summary>
        public BtnRoutedEventHandler OnPlayerClicked { get; set; }

        /// <summary>
        /// 手牌被单击的事件
        /// </summary>
        public CardEventHandler OnCardInHandClicked { get; set; }

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
        public void SetupOkCancelActionBar(TaskCompletionSource<CardResponseContext> tcs, string displayMessage, string okText, string cancelText, BtnRoutedEventHandler btn1EventHandler = null, BtnRoutedEventHandler btn2EventHandler = null)
        {
            ActionBar = new ActionBar();
            ActionBar.Visiable = true;
            ActionBar.DisplayMessage = new DisplayMessage()
            {
                Content = displayMessage,
                IsVisible = true
            };
            if (string.IsNullOrEmpty(okText))
            {
                ActionBar.BtnAction1 = null;
            }
            else
            {
                ActionBar.BtnAction1 = new BtnAction()
                {
                    BtnText = okText,
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
            }

            if (string.IsNullOrEmpty(cancelText))
            {
                ActionBar.BtnAction2 = null;
            }
            else
            {
                ActionBar.BtnAction2 = new BtnAction()
                {
                    BtnText = cancelText,
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

        /// <summary>
        /// 恢复所有人的选中状态为指定状态
        /// </summary>
        /// <param name="status"></param>
        public void RestoreSelectStatus(SelectStatusEnum status)
        {
            BindPlayer.GameLevel.Players.ForEach(p =>
            {
                if (p.PlayerUiState.SelectStatus != status)
                {
                    p.PlayerUiState.SelectStatus = status;
                }
            });
        }

        /// <summary>
        /// 高亮可选择的目标
        /// </summary>
        /// <param name="request"></param>
        /// <param name="clickedAction">点击之后的动作</param>
        public async Task<CardResponseContext> HighlightAvailableTargets(SelectedTargetsRequest request, Action<object> clickedAction)
        {
            //找出所有可选择的目标
            var targets = BindPlayer.GameLevel.GetAlivePlayers();
            foreach (var target in targets)
            {
                //获取两个player之间的距离，检查是否在攻击范围内
                var canBeSelected = await BindPlayer.IsAvailableForPlayer(target, request.SrcCards?.FirstOrDefault(), request.TargetType);
                //设置玩家选中状态
                target.PlayerUiState.SelectStatus = canBeSelected ? SelectStatusEnum.PendingSelected : SelectStatusEnum.IsNotAbleToSelected;
                //设置玩家被点击的事件,如果被点击则代表玩家被选中（多次则为取消）
                if (canBeSelected)
                {
                    target.PlayerUiState.OnPlayerClicked = async () =>
                        {
                            var selResults = GetSelectedTargets();
                            //切换状态
                            target.PlayerUiState.SelectStatus =
                                target.PlayerUiState.SelectStatus == SelectStatusEnum.IsSelected ? SelectStatusEnum.PendingSelected : SelectStatusEnum.IsSelected;
                            //检查当前选中的数目是否超过了最大值，如果超过，则将逆时针最近的一个选中的目标取消选中
                            if (selResults.Count >= request.MaxTargetCount - 1)
                            {
                                var last = selResults.LastOrDefault();
                                if (last != null)
                                {
                                    last.PlayerUiState.SelectStatus = SelectStatusEnum.PendingSelected;
                                }
                            }

                            //当选中n个目标后，弹窗提示确认来选择目标，如果确认，则返回此时选中的目标，如果取消则放弃选择目标。
                            var areReady = AreTargetsReady(request);
                            if (areReady)
                            {
                                BindPlayer.PlayerUiState.SetupOkCancelActionBar(request.RequestTaskCompletionSource, "", "确定", "取消");
                            }

                            return await Task.FromResult(false);
                        };
                }
                else
                {
                    target.PlayerUiState.OnPlayerClicked = null;
                }
            }


            var response = await request.RequestTaskCompletionSource.Task;
            return response;
        }

        public List<Player> GetSelectedTargets()
        {
            var results = BindPlayer.GameLevel.Players.Where(p =>
                p.PlayerUiState?.SelectStatus == SelectStatusEnum.IsSelected).ToList();
            return results;
        }

        private bool AreTargetsReady(SelectedTargetsRequest request)
        {
            var results = GetSelectedTargets();
            if (results.Count >= request.MinTargetCount && results.Count <= request.MaxTargetCount)
            {
                return true;
            }

            return false;
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

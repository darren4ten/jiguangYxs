using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Runtime.CompilerServices;
using System.Text;
using System.Threading.Tasks;
using Logic.Annotations;
using Logic.GameLevel;
using Logic.GameLevel.Panel;
using Logic.Model.Enums;
using Logic.Model.RequestResponse.Request;
using Logic.Model.RequestResponse.Response;

namespace Logic.Model.Player
{
    public delegate Task<bool?> BtnRoutedEventHandler();
    public delegate Task<bool?> CardEventHandler(object sender);
    public class PlayerUIState : INotifyPropertyChanged
    {
        /// <summary>
        /// 面板，如探囊取物、五谷丰登
        /// </summary>
        public PanelBase Panel { get; set; }

        private ActionBar _actionBar;
        public ActionBar ActionBar
        {
            get { return _actionBar; }
            set
            {
                _actionBar = value;
                OnPropertyChanged();
            }
        }

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
            ActionBar = ActionBar ?? new ActionBar();
            ActionBar.Visiable = true;
            ActionBar.DisplayMessage = new DisplayMessage()
            {
                Content = displayMessage,
                IsVisible = true
            };
            if (string.IsNullOrEmpty(okText))
            {
                ActionBar.BtnAction1 = ActionBar.BtnAction1 ?? new BtnAction()
                {
                    IsVisible = false
                };
                ActionBar.BtnAction1.IsVisible = false;
            }
            else
            {
                ActionBar.BtnAction1 = new BtnAction()
                {
                    BtnText = okText,
                    IsVisible = true,
                    BtnRoutedEventHandler = async () =>
                    {
                        HideActionBar();
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
                ActionBar.BtnAction2 = ActionBar.BtnAction2 ?? new BtnAction()
                {
                    IsVisible = false
                };
                ActionBar.BtnAction2.IsVisible = false;
            }
            else
            {
                ActionBar.BtnAction2 = new BtnAction()
                {
                    BtnText = cancelText,
                    IsVisible = true,
                    BtnRoutedEventHandler = async () =>
                    {
                        HideActionBar();
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

            ActionBar.BtnAction3 = ActionBar.BtnAction3 ?? new BtnAction()
            {
                IsVisible = false
            };
            ActionBar.BtnAction3.IsVisible = false;
        }


        /// <summary>
        /// 显示面板
        /// </summary>
        /// <param name="panel"></param>
        public void ShowPanel(PanelBase panel)
        {
            if (BindPlayer.IsAi())
            {
                return;
            }
            if (panel.IsGlobal)
            {
                BindPlayer.GameLevel.Panel = panel;
            }
            else
            {
                BindPlayer.PlayerUiState.Panel = panel;
            }
        }

        /// <summary>
        /// 关闭面板
        /// </summary>
        /// <param name="panel"></param>
        public void ClosePanel(PanelBase panel)
        {
            if (BindPlayer.IsAi())
            {
                return;
            }
            if (panel.IsGlobal)
            {
                BindPlayer.GameLevel.Panel = null;
            }
            else
            {
                BindPlayer.PlayerUiState.Panel = null;
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
        public async Task<SelectedTargetsResponse> HighlightAvailableTargets(SelectedTargetsRequest request, Action<object> clickedAction)
        {
            //找出所有可选择的目标
            var targets = BindPlayer.GameLevel.GetAlivePlayers();
            foreach (var target in targets)
            {
                if (target.PlayerId == BindPlayer.PlayerId)
                {
                    target.PlayerUiState.SelectStatus = SelectStatusEnum.Idle;
                    continue;
                }
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
                                //BindPlayer.PlayerUiState.SetupOkCancelActionBar(request.RequestTaskCompletionSource, "", "确定", "取消");
                                request.SelectTargetsRequestTaskCompletionSource.SetResult(new SelectedTargetsResponse()
                                {
                                    Status = ResponseResultEnum.Success,
                                    Targets = GetSelectedTargets()
                                });

                            }

                            return await Task.FromResult(true);
                        };
                }
                else
                {
                    target.PlayerUiState.OnPlayerClicked = null;
                }
            }


            var response = await request.SelectTargetsRequestTaskCompletionSource.Task;
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

        private void HideActionBar()
        {
            ActionBar.Visiable = false;
            ActionBar.DisplayMessage = new DisplayMessage()
            {
                Content = "",
                IsVisible = false
            };
            ActionBar.BtnAction1.IsVisible = false;
            ActionBar.BtnAction2.IsVisible = false;
            ActionBar.BtnAction3.IsVisible = false;
        }
    }

    public class ActionBar : INotifyPropertyChanged
    {
        public Guid ActionId { get; set; }

        private bool _Visiable;

        public bool Visiable
        {
            get
            {
                return _Visiable;
            }
            set
            {
                _Visiable = value;
                OnPropertyChanged();
            }
        }

        private ToastMessage _toastMessage;
        public ToastMessage ToastMessage
        {
            get
            {
                return _toastMessage;
            }
            set
            {
                _toastMessage = value;
                OnPropertyChanged();
            }
        }

        private DisplayMessage _displayMessage;
        public DisplayMessage DisplayMessage
        {
            get
            {
                return _displayMessage;
            }
            set
            {
                _displayMessage = value;
                OnPropertyChanged();
            }
        }

        private BtnAction _btnAction1;
        public BtnAction BtnAction1
        {
            get
            {
                return _btnAction1;
            }
            set
            {
                _btnAction1 = value;
                OnPropertyChanged();
            }
        }
        private BtnAction _btnAction2;
        public BtnAction BtnAction2
        {
            get
            {
                return _btnAction2;
            }
            set
            {
                _btnAction2 = value;
                OnPropertyChanged();
            }
        }
        private BtnAction _btnAction3;
        public BtnAction BtnAction3
        {
            get
            {
                return _btnAction3;
            }
            set
            {
                _btnAction3 = value;
                OnPropertyChanged();
            }
        }
        public event PropertyChangedEventHandler PropertyChanged;

        [NotifyPropertyChangedInvocator]
        protected virtual void OnPropertyChanged([CallerMemberName] string propertyName = null)
        {
            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propertyName));
        }
    }

    public class BtnAction : INotifyPropertyChanged
    {
        private string _btnText;
        public string BtnText
        {
            get
            {
                return _btnText;
            }
            set
            {
                _btnText = value;
                OnPropertyChanged();
            }
        }
        private bool _isVisible;
        public bool IsVisible
        {
            get
            {
                return _isVisible;
            }
            set
            {
                _isVisible = value;
                OnPropertyChanged();
            }
        }

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
        private bool _isVisible;
        public bool IsVisible
        {
            get
            {
                return _isVisible;
            }
            set
            {
                _isVisible = value;
                OnPropertyChanged();
            }
        }

        private string _content;
        public string Content
        {
            get
            {
                return _content;
            }
            set
            {
                _content = value;
                OnPropertyChanged();
            }
        }
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

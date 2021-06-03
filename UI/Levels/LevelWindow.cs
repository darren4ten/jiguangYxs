using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using JgYxs.UI.Model;
using JgYxs.UI.UserCtrl.Panel;
using Logic.GameLevel.Panel;
using Logic.Model.Cards.BaseCards;
using Logic.Model.Cards.EquipmentCards;
using Logic.Model.Cards.JinlangCards;
using Logic.Model.Enums;
using Logic.Model.Player;

namespace JgYxs.UI.Levels
{
    public class LevelWindow : Window
    {
        public GameDataContext GameDataContext { get; set; } = new GameDataContext();

        /// <summary>
        /// 临时弹窗，如：探囊取物弹窗
        /// </summary>
        protected Window PopupWindow { get; set; }

        protected override void OnInitialized(EventArgs e)
        {
            base.OnInitialized(e);
            GameDataContext.CurrentPlayer.PlayerUiState.UiAction = UiAction;
            GameDataContext.CurrentPlayer.PlayerUiState.PropertyChanged += (async (sender, args) =>
            {
                //显示弹窗
                if (args.PropertyName == nameof(GameDataContext.CurrentPlayer.PlayerUiState.Panel))
                {
                    var uiState = (PlayerUIState)sender;
                    if (uiState?.Panel != null)
                    {
                        await ShowPanel(uiState.Panel);
                    }
                    else
                    {
                        await ClosePanel();
                    }
                }
            });
        }

        private Task UiAction(Action act)
        {
            return Task.Run(() =>
            {
                //这个地方不能用await,否则线程会被block
                this.Dispatcher.BeginInvoke(new Action(async () =>
                {
                    act();
                    await Task.FromResult(0);
                }));
            });
        }

        public async Task ShowPanel(PanelBase panel)
        {
            ContentControl tn = null;
            if (panel.PanelType == PanelTypeEnum.Fudichouxin || panel.PanelType == PanelTypeEnum.Longlindao || panel.PanelType == PanelTypeEnum.Tannangquwu)
            {
                tn = new TannangFudiPanel();
                tn.DataContext = panel;
            }
            else
            {

            }

            var win = PopupWindow ?? new Window()
            {
                Title = panel.DisplayMessage,
                Width = tn.Width,
                Height = tn.Height,
                WindowStartupLocation = WindowStartupLocation.CenterScreen
            };
            win.Content = tn;
            PopupWindow = win;
            win.ShowDialog();
            await Task.FromResult(0);
        }

        public async Task ClosePanel()
        {
            PopupWindow.Hide();
            await Task.FromResult(0);
        }
    }
}

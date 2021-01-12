using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using JgYxs.UI.Model;
using Logic.Cards;
using Logic.Model.Cards.BaseCards;

namespace JgYxs.UI
{
    /// <summary>
    /// Interaction logic for MainWindow.xaml
    /// </summary>
    public partial class MainWindow : Window
    {
        public MainWindow()
        {
            InitializeComponent();
            Init();
        }

        private void Init()
        {
            var uiAction = new UIState()
            {
                Tip = "OK to add card, cancel to remove card",
                BtnAction1 = new BtnAction()
                {
                    BtnText = "OK",
                    IsVisible = Visibility.Visible,
                    BtnRoutedEventHandler = (obj, e) =>
                    {
                        if (obj is Button button)
                        {
                            var dataContext = button.DataContext;
                            if (dataContext is UIState)
                            {
                                var uiState = (UIState)dataContext;
                                Task.Run(() =>
                                {

                                });
                                uiState.PlayerHero.CardsInHand.Add(new Sha()
                                {
                                    Number = new Random().Next(1, 9)
                                });
                            }
                        }
                        //this.PlayerHero.CardsInHand.Add(new Sha()
                        //{
                        //    Number = new Random().Next(1, 9)
                        //});
                        //MessageBox.Show("Click btn1");
                    }
                },
                BtnAction2 = new BtnAction()
                {
                    BtnText = "Cancel",
                    IsVisible = Visibility.Visible,
                    BtnRoutedEventHandler = (obj, e) =>
                    {
                        if (obj is Button button)
                        {
                            var dataContext = button.DataContext;
                            if (dataContext is UIState)
                            {
                                var uiState = (UIState)dataContext;
                                var t = Task.Run(() =>
                                  {
                                      var index = uiState.PlayerHero.CardsInHand.Count - 1;
                                      if (index > 0)
                                      {
                                          Application.Current.Dispatcher.BeginInvoke((Action)delegate // <--- HERE
                                          {
                                              uiState.PlayerHero.CardsInHand.RemoveAt(index);
                                          });
                                      }
                                  });
                                Task.WaitAll(t);
                            }
                        }
                    }
                },
                PlayerHero = new PlayerHero
                {
                    MaxLife = 5,
                    CurrentLife = 4,
                    Cards = new List<CardBase>()
                    {
                        new Sha()
                        {
                            Number=2,
                        }
                    },
                    CardsInHand = new System.Collections.ObjectModel.ObservableCollection<CardBase>()
                    {
                    new Sha()
                    {
                    Number=2,
                 },
                    new Shan()
                    {
                        Number=9,
                    }
            },
                },
            };
            //PlayerPanel.DataContext = uiAction;
        }
    }
}

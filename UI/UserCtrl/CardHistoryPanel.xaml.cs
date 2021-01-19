using System;
using System.Collections.Generic;
using System.Text;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Navigation;
using System.Windows.Shapes;
using Logic.Log;

namespace JgYxs.UI.UserCtrl
{
    /// <summary>
    /// Interaction logic for CardHistoryPanel.xaml
    /// </summary>
    public partial class CardHistoryPanel : UserControl, ILogUpdate
    {
        private FlowDocLogMananger _logMananger;
        public FlowDocLogMananger LogManager
        {
            get { return _logMananger; }
            set
            {
                _logMananger = value;
            }
        }
        protected FlowDocument HistoryDoc { get; set; } = new FlowDocument();
        public CardHistoryPanel()
        {
            InitializeComponent();
        }

        protected override void OnInitialized(EventArgs e)
        {
            BRtxt.Document = HistoryDoc;
            base.OnInitialized(e);
        }
        #region Log

        public void LogUpdate(RichTextParagraph richPara)
        {
            Paragraph p = new Paragraph();
            foreach (var richTextWrapper in richPara.Wrappers)
            {
                var nameRun = new Run(richTextWrapper.Content);
                nameRun.Foreground = new SolidColorBrush(Color.FromRgb(richTextWrapper.Color[0], richTextWrapper.Color[1], richTextWrapper.Color[2]));
                nameRun.FontWeight = richTextWrapper.IsBold ? FontWeights.Bold : FontWeights.Normal;
                nameRun.FontSize = richTextWrapper.FontSize <= 0 ? 14 : richTextWrapper.FontSize;
                p.Inlines.Add(nameRun);
            }
            HistoryDoc.Blocks.Add(p);
            BRtxt.ScrollToEnd();
        }
        #endregion
    }
}

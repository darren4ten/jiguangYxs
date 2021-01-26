using System;
using System.Collections.Concurrent;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.IO;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Shapes;
using Logic.ActionManger;
using Logic.Cards;
using Logic.Enums;
using Logic.GameLevel;
using Logic.GameLevel.Levels;
using Logic.Log;
using Logic.Model.Cards.BaseCards;
using Logic.Model.Cards.EquipmentCards;
using Logic.Model.Cards.EquipmentCards.Defense;
using Logic.Model.Hero.Presizdent;
using Logic.Model.Mark;
using Logic.Model.Player;
using Logic.Model.Skill;
using Logic.Model.Skill.SubSkill;
using Logic.Util.Extension;

namespace JgYxs.UI
{
    /// <summary>
    /// Interaction logic for TestWindow.xaml
    /// </summary>
    public partial class TestWindow : Window, ILogUpdate
    {
        public ObservableCollection<CardBase> EquipmentSet { get; set; } = new ObservableCollection<CardBase>();
        public Paragraph CurP { get; set; }

        public Player player1 { get; set; }

        public FlowDocument FlowDocument { get; set; }
        public TestWindow()
        {
            this.DataContext = EquipmentSet;

            InitializeComponent();
        }

        protected override void OnInitialized(EventArgs e)
        {
            EquipmentSet.AddRange(new List<CardBase>(){
                new Langyabang() { Number = 1, FlowerKind = FlowerKindEnum.Hongtao },
                new Yuruyi() { Number = 8, FlowerKind = FlowerKindEnum.Fangkuai },
                new Jingongma() { Number = 13, FlowerKind = FlowerKindEnum.Meihua },
                new Fangyuma() { Number = 10, FlowerKind = FlowerKindEnum.Heitao }
            });
            LvEquipment.ItemsSource = EquipmentSet;
            TestParagraph();
            InitTestHero();
            base.OnInitialized(e);
        }

        private void InitTestHero()
        {
            var level1 = new GameLevel1();
            var star3Zhuyuanzhang1 = new PlayerHero(3, new Zhuyuanzhang(), null,
                new List<SkillBase>(){
                    new Xixue(5,50),
                });

            player1 = new Player(level1, new AiActionManager(), new List<PlayerHero>() { star3Zhuyuanzhang1 })
            {
                PlayerId = 1,
                GroupId = Guid.NewGuid(),
                RoundContext = new RoundContext()
                {
                    AttackDynamicFactor = AttackDynamicFactor.GetDefaultDeltaAttackFactor()
                }
            };
            level1.OnLoad(player1, new List<Player>() { });
            player1.Init();
            player1.AddCardInHand(new Sha()).GetAwaiter().GetResult();
            player1.AddMark(new ShoupengleiMark()).GetAwaiter().GetResult();
            player1.AddMark(new HuadiweilaoMark()).GetAwaiter().GetResult();
            TestHero.DataContext = player1;
        }

        private void TestParagraph()
        {

            FlowDocument = GetDocument();
            BRtxt.Document = FlowDocument;
            FlowDocLogMananger mananger = new FlowDocLogMananger(this);
            //mananger.LogAction(new RichText() { });
            this.Dispatcher.BeginInvoke(new Action(async () =>
            {
                //Paragraph p = new Paragraph();
                //var nameRun = new Run("Test....");
                //nameRun.Foreground = new SolidColorBrush(Colors.Red);
                //nameRun.FontWeight = FontWeights.Bold;
                //p.Inlines.Add(nameRun);
                //FlowDocument.Blocks.Add(p);
                //UpdateDocument(new List<RichTextWrapper>(){
                //    new RichTextWrapper()
                //    {
                //        Color = new byte[3]{234,67,53},
                //        Content="TestRichText afa",
                //        IsBold = true,
                //        FontSize=24
                //    }
                //});
                mananger.LogAction(new RichTextParagraph(new RichTextWrapper("TestRichText afa", new byte[3] { 234, 67, 53 })));
            }));
        }

        private void UpdateDocument(List<RichTextWrapper> records)
        {
            foreach (var richText in records)
            {
                Paragraph p = new Paragraph();
                var nameRun = new Run(richText.Content);
                nameRun.Foreground = new SolidColorBrush(Color.FromRgb(richText.Color[0], richText.Color[1], richText.Color[2]));
                nameRun.FontWeight = richText.IsBold ? FontWeights.Bold : FontWeights.Normal;
                nameRun.FontSize = richText.FontSize <= 0 ? 14 : richText.FontSize;
                p.Inlines.Add(nameRun);
                FlowDocument.Blocks.Add(p);
            }
        }

        public FlowDocument GetDocument()
        {
            FlowDocument doc = new FlowDocument();

            Paragraph p = new Paragraph();
            var nameRun = new Run("陈大雷");
            nameRun.Foreground = new SolidColorBrush(Colors.Red);
            nameRun.FontWeight = FontWeights.Bold;
            p.Inlines.Add(nameRun);

            var actionRun = new Run("说");
            actionRun.Foreground = new SolidColorBrush(Colors.Black);
            actionRun.FontWeight = FontWeights.Normal;
            p.Inlines.Add(actionRun);

            var contentRun = new Run("今天是个好天气");
            contentRun.Foreground = new SolidColorBrush(Colors.Green);

            contentRun.FontWeight = FontWeights.Normal;
            p.Inlines.Add(contentRun);

            doc.Blocks.Add(p);

            Paragraph p1 = new Paragraph();
            var nameRun1 = new Run("顺溜");
            nameRun1.Foreground = new SolidColorBrush(Colors.Red);
            nameRun1.FontWeight = FontWeights.Bold;
            p1.Inlines.Add(nameRun1);

            var actionRun1 = new Run("说");
            actionRun1.Foreground = new SolidColorBrush(Colors.Black);
            actionRun1.FontWeight = FontWeights.Normal;
            p1.Inlines.Add(actionRun1);

            var contentRun1 = new Run("我不能同意更多了！");
            contentRun1.Foreground = new SolidColorBrush(Colors.Green);

            contentRun1.FontWeight = FontWeights.Normal;
            p1.Inlines.Add(contentRun1);

            doc.Blocks.Add(p1);

            return doc;
        }

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
            FlowDocument.Blocks.Add(p);
        }

        private void TestHero_OnMouseDoubleClick(object sender, MouseButtonEventArgs e)
        {
            player1.MakeDie().GetAwaiter().GetResult();
        }
    }
}

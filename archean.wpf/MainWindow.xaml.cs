using System;
using System.Collections.Concurrent;
using archean.mysql;
using archean.core;
using log4net;
using System.Threading.Tasks;
using System.Windows;
using System.Collections.Generic;

namespace archean.wpf
{
    public partial class MainWindow : Window
    {
        private static readonly ILog log = LogManager.GetLogger(System.Reflection.MethodBase.GetCurrentMethod
            ().DeclaringType);

        public MainWindow()
        {
            InitializeComponent();
            int batchMult = 5000;


            for (var stageCount = 2; stageCount < 10; stageCount++)
            {
                var refSorter = SortersFromData.RefSorter.End16;
                var randSwitchFill = SortersFromData.RandSwitchFill.FullStage;
                var refSorterPrefixStages = new SortersFromData.RefSorterPrefixStages(refSorter, stageCount);
                var randGenerationMode = SortersFromData.RandGenerationMode.NewPrefixed(
                    refSorterPrefixStages, randSwitchFill);

                BatchArgsList.Add(new BatchArgs()
                {
                    Order = 16,
                    RandGenerationMode = randGenerationMode,
                    SorterCount = batchMult * 10,
                    SorterLen = 1200 - (stageCount * 50)
                });

                var refSorter2 = SortersFromData.RefSorter.Green16;
                var randSwitchFill2 = SortersFromData.RandSwitchFill.FullStage;
                var refSorterPrefixStages2 = new SortersFromData.RefSorterPrefixStages(refSorter2, stageCount);
                var randGenerationMode2 = SortersFromData.RandGenerationMode.NewPrefixed(
                    refSorterPrefixStages2, randSwitchFill2);

                BatchArgsList.Add(new BatchArgs()
                {
                    Order = 16,
                    RandGenerationMode = randGenerationMode2,
                    SorterCount = batchMult * 10,
                    SorterLen = 1200 - (stageCount * 50)
                });


            }
        }

        private List<BatchArgs> BatchArgsList = new List<BatchArgs>();

        private void Button_Click(object sender, RoutedEventArgs e)
        {
            Block.Dispatcher.Invoke(() => Block.Text = "Running");

            var task = Task.Factory.StartNew(() => {
                RunBatches();
            }).ContinueWith(t => {
                log.Info("Test Finished");
                Block.Dispatcher.Invoke(() => Block.Text = "Done");
            });
        }


        private void RunBatches()
        {
            try
            {
                log.Debug("Test Starting");
                Tuple<string, string[]> q;
                for (var i = 0; true; i++)
                {
                    var ba = BatchArgsList[i % BatchArgsList.Count];
                    q = SortingReports.MakeStageAndSwitchUseHistogram
                    (
                        sorterLen: ba.SorterLen,
                        randGenerationMode: ba.RandGenerationMode,
                        sorterCount: ba.SorterCount,
                        seed: Math.Abs((int)(DateTime.Now.Ticks))
                    );

                    log.Info(q.Item1);

                    var hist = q.Item2;
                    for (var j = 0; j < hist.Length; j++)
                    {
                        log.Info(hist[j]);
                    }
                }
            }
            catch (Exception e)
            {
                log.Info(e.Message);
                throw;
            }
        }

        public class BatchArgs
        {
            public int Order { get; set; }
            public int SorterLen { get; set; }
            public SortersFromData.RandGenerationMode RandGenerationMode { get; set; }
            public int SorterCount { get; set; }
        }

    }

}

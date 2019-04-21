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
            int batchMult = 50;
            int Order = 16;
            int totalStageCount = 150;

            for (var prefixStageCount = 2; prefixStageCount < 10; prefixStageCount++)
            {
                var refSorter = SortersFromData.RefSorter.End16;
                var randSwitchFill = SortersFromData.RandSwitchFill.FullStage;
                var randSorterStages = new SortersFromData.RandSorterStages(Order, totalStageCount - prefixStageCount * 2, randSwitchFill);
                var refSorterPrefixStages = new SortersFromData.RefSorterPrefixStages(refSorter, prefixStageCount);
                var randGenerationMode = SortersFromData.RandGenerationMode.NewPrefixed(
                    refSorterPrefixStages, randSorterStages);

                BatchArgsList.Add(new BatchArgs()
                {
                    RandGenerationMode = randGenerationMode,
                    SorterCount = batchMult * 10
                });

                var refSorter2 = SortersFromData.RefSorter.Green16;
                var randSwitchFill2 = SortersFromData.RandSwitchFill.FullStage;
                var randSorterStages2 = new SortersFromData.RandSorterStages(Order, totalStageCount - prefixStageCount * 2, randSwitchFill2);
                var refSorterPrefixStages2 = new SortersFromData.RefSorterPrefixStages(refSorter2, prefixStageCount);
                var randGenerationMode2 = SortersFromData.RandGenerationMode.NewPrefixed(
                    refSorterPrefixStages2, randSorterStages2);

                BatchArgsList.Add(new BatchArgs()
                {
                    RandGenerationMode = randGenerationMode2,
                    SorterCount = batchMult * 10
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
            public SortersFromData.RandGenerationMode RandGenerationMode { get; set; }
            public int SorterCount { get; set; }
        }

    }

}

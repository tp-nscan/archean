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
            int sc = 100;

            //BatchArgsList.Add(new BatchArgs() {
            //    Order = 11,
            //    RandGenerationMode = Sorting.SorterDefModule.RandGenerationMode.LooseSwitches,
            //    SwitchableType = SortingReports.SwitchableType.Generated,
            //    SorterCount = sc * 25,
            //    SorterLen = 340 });

            //BatchArgsList.Add(new BatchArgs()
            //{
            //    Order = 11,
            //    RandGenerationMode = Sorting.SorterDefModule.RandGenerationMode.FullStage,
            //    SwitchableType = SortingReports.SwitchableType.Generated,
            //    SorterCount = sc * 25,
            //    SorterLen = 340
            //});


            BatchArgsList.Add(new BatchArgs()
            {
                Order = 16,
                RandGenerationMode = Sorting.SorterDefModule.RandGenerationMode.LooseSwitches,
                SwitchableType = SortingReports.SwitchableType.IntArray,
                SorterCount = sc * 10,
                SorterLen = 1000
            });

            BatchArgsList.Add(new BatchArgs()
            {
                Order = 16,
                RandGenerationMode = Sorting.SorterDefModule.RandGenerationMode.FullStage,
                SwitchableType = SortingReports.SwitchableType.IntArray,
                SorterCount = sc * 10,
                SorterLen = 1000
            });

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
                        order: ba.Order,
                        sorterLen: ba.SorterLen,
                        randGenerationMode: ba.RandGenerationMode,
                        switchableType: ba.SwitchableType,
                        sorterCount: ba.SorterCount,
                        seed: (int)System.DateTime.Now.Ticks
                    );


                    log.Info(ba.SwitchableType);
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
            public Sorting.SorterDefModule.RandGenerationMode RandGenerationMode { get; set; }
            public SortingReports.SwitchableType SwitchableType { get; set; }
            public int SorterCount { get; set; }
        }

    }

}

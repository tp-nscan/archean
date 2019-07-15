using archean.ViewModel.Pages;
using System.Windows;
using System.Windows.Controls;

namespace archean.View.Pages
{
    public partial class MenuPage : Page
    {
        public MenuPage()
        {
            InitializeComponent();
        }

        private void BtnBenchmark_Click(object sender, RoutedEventArgs e)
        {
            var nextPage = new BenchmarkPage();
            nextPage.DataContext = new BenchmarkPageVm();
            NavigationService.Navigate(nextPage);
        }

        private void BtnSorterControl_Click(object sender, RoutedEventArgs e)
        {
            var nextPage = new SorterPage();
            nextPage.DataContext = new SorterPageVm();
            NavigationService.Navigate(nextPage);
        }

        private void BtnStageControl_Click(object sender, RoutedEventArgs e)
        {
            var nextPage = new StagePage();
            nextPage.DataContext = new StagePageVm();
            NavigationService.Navigate(nextPage);
        }

        private void BtnSorter2Control_Click(object sender, RoutedEventArgs e)
        {
            var nextPage = new SorterPage2();
            nextPage.DataContext = new SorterPageVm2();
            NavigationService.Navigate(nextPage);
        }
    }
}

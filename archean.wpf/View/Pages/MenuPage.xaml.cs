using archean.ViewModel.Pages;
using System;
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
            this.NavigationService.Navigate(nextPage);
        }

        private void BtnSorterControl_Click(object sender, RoutedEventArgs e)
        {
            var nextPage = new SorterPage();
            nextPage.DataContext = new SorterPageVm();
            this.NavigationService.Navigate(nextPage);
        }

        private void BtnStageControl_Click(object sender, RoutedEventArgs e)
        {
            var nextPage = new StagePage();
            nextPage.DataContext = new StagePageVm();
            this.NavigationService.Navigate(nextPage);
        }

        private void BtnSorter2Control_Click(object sender, RoutedEventArgs e)
        {
            var nextPage = new SorterPage2();
            nextPage.DataContext = new SorterPage2Vm();
            this.NavigationService.Navigate(nextPage);
        }
    }
}

using log4net;
using System.Windows;

namespace archean.wpf
{
    public partial class App : Application
    {

        private static readonly ILog log = LogManager.GetLogger(System.Reflection.MethodBase.GetCurrentMethod
            ().DeclaringType);

        ////private static readonly ILog log = LogManager.GetLogger(typeof (Program)) ;
        //static void Main(string[] args)
        //{
        //    log.Debug("Application Starting");
        //}

        private void App_Startup(object sender, StartupEventArgs e)
        {
            var mainWindow = new MainWindow();
            mainWindow.Show();
        }
    }
}

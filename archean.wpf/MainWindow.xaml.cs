using archean.mysql;
using System.Windows;

namespace archean.wpf
{
    public partial class MainWindow : Window
    {
        public MainWindow()
        {
            InitializeComponent();

            // mysql.SimBuilder.NoCnxn();

            //var DbContext = new ArcheanContext(ArcheanMySql.TestContextName);
            //DbContext.Database.CreateIfNotExists();
            //DbContext.Database.Initialize(force:true);

            //var res = ArcheanMySqlTest.TestAddAndRemoveSimGroup();

        }
       


    }

}

using System.Windows;

namespace FQ.BuildLauncher
{
    /// <summary>
    /// Interaction logic for MainWindow.xaml
    /// </summary>
    public partial class MainWindow
    {
        private DownloaderViewmodel vm;
        
        public MainWindow()
        {
            vm = new DownloaderViewmodel();
            DataContext = vm;
            InitializeComponent();
        }
    }
}
using System.Windows;

namespace WpfApp1
{
    public partial class MainWindow : Window
    {
        public MainWindow()
        {
            InitializeComponent();
            MainContent.Content = new LiveMonitor(); // Mặc định là tab Monitor
        }

        private void LiveMonitor_Click(object sender, RoutedEventArgs e)
        {
            MainContent.Content = new LiveMonitor();
        }

        private void StressAnalysis_Click(object sender, RoutedEventArgs e)
        {
            MainContent.Content = new StressAnalysis(); // Điều hướng đến StressAnalysis
        }

        private void RelaxZone_Click(object sender, RoutedEventArgs e)
        {
            MainContent.Content = new Relax();
        }
    }
}
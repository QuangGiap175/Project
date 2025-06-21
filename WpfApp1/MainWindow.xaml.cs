using System.Windows;

namespace WpfApp1
{
    public partial class MainWindow : Window
    {
        public MainWindow()
        {
            InitializeComponent();
        }
        private void LiveMonitor_Click(object sender, RoutedEventArgs e)
        {
            MainContent.Content = new Live_Monitor(); // Gọi UserControl tên Live_Monitor
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
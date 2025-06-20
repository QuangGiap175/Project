using System.Windows;

namespace WpfApp1
{
    public partial class MainWindow : Window
    {
        public MainWindow()
        {
            InitializeComponent();
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
using System.Text;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Navigation;
using System.Windows.Shapes;

namespace WpfApp1
{
    /// <summary>
    /// Interaction logic for MainWindow.xaml
    /// </summary>
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
            MainContent.Content = new StressAnalysis();
        }

        private void RelaxZone_Click(object sender, RoutedEventArgs e)
        {
            MainContent.Content = new Relax();
        }
    }
}
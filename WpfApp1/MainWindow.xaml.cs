using System.Windows;

namespace WpfApp1
{
    public partial class MainWindow : Window
    {
        private Live_Monitor _liveMonitor;
        private StressAnalysis _stressAnalysis;

        public MainWindow()
        {
            InitializeComponent();

            _liveMonitor = new Live_Monitor();
            _stressAnalysis = new StressAnalysis();

            // Lắng nghe sự kiện khi đủ 20 mẫu từ Live Monitor
            _liveMonitor.On20thReadingReady += LiveMonitor_On20thReadingReady;

            // Giao diện mặc định
            MainContent.Content = _liveMonitor;
        }

        private void LiveMonitor_Click(object sender, RoutedEventArgs e)
        {
            MainContent.Content = _liveMonitor;
        }

        private void StressAnalysis_Click(object sender, RoutedEventArgs e)
        {
            MainContent.Content = _stressAnalysis;
        }

        private void RelaxZone_Click(object sender, RoutedEventArgs e)
        {
            MainContent.Content = new Relax();
        }

        private void LiveMonitor_On20thReadingReady(object sender, Live_Monitor.SensorDataEventArgs e)
        {
            // Gán dữ liệu từ Live Monitor sang StressAnalysis
            _stressAnalysis.UpdateInputFields(e.HeartRate, e.SpO2, e.HRV);

            // Hiển thị luôn trang phân tích stress
            MainContent.Content = _stressAnalysis;
        }
    }
}
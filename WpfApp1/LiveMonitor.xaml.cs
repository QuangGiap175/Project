using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Navigation;
using System.Windows.Shapes;
using System.Windows.Threading;

namespace WpfApp1
{
    /// <summary>
    /// Interaction logic for LiveMonitor.xaml
    /// </summary>
    public partial class LiveMonitor : UserControl
    {
        private DispatcherTimer _timer;
        private Random _rand = new Random();

        public LiveMonitor()
        {
            InitializeComponent();
           // StartMockMonitoring();
        }

       /* private void StartMockMonitoring()
        {
            _timer = new DispatcherTimer();
            _timer.Interval = TimeSpan.FromSeconds(1); // cập nhật mỗi giây
            _timer.Tick += (s, e) =>
            {
                int heartRate = _rand.Next(65, 100);  // nhịp tim từ 65–99
                int spo2 = _rand.Next(95, 100);       // SpO2 từ 95–99

                HeartRateText.Text = $"{heartRate} bpm";
                SpO2Text.Text = $"{spo2}%";
            };
            _timer.Start();
        }*/
    }
}

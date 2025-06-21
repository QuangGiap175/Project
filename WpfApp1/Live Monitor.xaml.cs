using System;
using System.Collections.Generic;
using System.IO.Ports;
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

namespace WpfApp1
{
    /// <summary>
    /// Interaction logic for Live_Monitor.xaml
    /// </summary>
    public partial class Live_Monitor : UserControl
    {
        private SerialPort _serialPort;

        public Live_Monitor()
        {
            InitializeComponent();
        }

        private void Connect_Click(object sender, RoutedEventArgs e)
        {
            try
            {
                if (_serialPort == null || !_serialPort.IsOpen)
                {
                    _serialPort = new SerialPort("COM4", 115200); // 🔧 Thay COM4 bằng cổng của bạn
                    _serialPort.DataReceived += SerialPort_DataReceived;
                    _serialPort.Open();

                    MessageBox.Show("Đã kết nối với ESP32!", "Kết nối thành công", MessageBoxButton.OK, MessageBoxImage.Information);
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show("Không thể kết nối tới ESP32: " + ex.Message, "Lỗi", MessageBoxButton.OK, MessageBoxImage.Error);
            }
        }

        private void SerialPort_DataReceived(object sender, SerialDataReceivedEventArgs e)
        {
            try
            {
                string data = _serialPort.ReadLine()?.Trim();  // Dữ liệu kiểu: "78.2,97.5,65.3"
                if (string.IsNullOrWhiteSpace(data)) return;

                string[] parts = data.Split(',');
                if (parts.Length != 3) return;

                string heartRate = parts[0];
                string spo2 = parts[1];
                string hrv = parts[2];

                Dispatcher.Invoke(() =>
                {
                    HeartRateText.Text = $"{heartRate} bpm";
                    SpO2Text.Text = $"{spo2} %";
                    HRVText.Text = $"{hrv} ms";
                });
            }
            catch
            {
                // Bỏ qua lỗi format hoặc mất kết nối
            }
        }
    }
}

using System;
using System.IO.Ports;
using System.Windows;
using System.Windows.Controls;

namespace WpfApp1
{
    /// <summary>
    /// Interaction logic for Live_Monitor.xaml
    /// </summary>
    public partial class Live_Monitor : UserControl
    {
        private SerialPort _serialPort;
        private int _readCount = 0;

        public Live_Monitor()
        {
            InitializeComponent();
        }

        // Class chứa dữ liệu cảm biến
        public class SensorDataEventArgs : EventArgs
        {
            public float HeartRate { get; set; }
            public float SpO2 { get; set; }
            public float HRV { get; set; }
        }

        // Sự kiện để gửi dữ liệu lần thứ 20 sang StressAnalysis
        public event EventHandler<SensorDataEventArgs> On20thReadingReady;

        private void Connect_Click(object sender, RoutedEventArgs e)
        {
            try
            {
                if (_serialPort == null || !_serialPort.IsOpen)
                {
                    _serialPort = new SerialPort("COM6", 115200); // ✅ Thay COM4 bằng cổng phù hợp
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
                string data = _serialPort.ReadLine()?.Trim();  // Ví dụ: "78.2,97.5,65.3"
                if (string.IsNullOrWhiteSpace(data)) return;

                string[] parts = data.Split(',');
                if (parts.Length != 3) return;

                if (float.TryParse(parts[0], out float heartRate) &&
                    float.TryParse(parts[1], out float spo2) &&
                    float.TryParse(parts[2], out float hrv))
                {
                    Dispatcher.Invoke(() =>
                    {
                        HeartRateText.Text = $"{heartRate:F0} bpm";
                        SpO2Text.Text = $"{spo2:F0} %";
                        HRVText.Text = $"{hrv:F2} ms";

                        _readCount++;
                        if (_readCount == 20)
                        {
                            _readCount = 0;
                            On20thReadingReady?.Invoke(this, new SensorDataEventArgs
                            {
                                HeartRate = heartRate,
                                SpO2 = spo2,
                                HRV = hrv
                            });
                        }
                    });
                }
            }
            catch
            {
                // Bỏ qua lỗi nếu dữ liệu không hợp lệ hoặc bị mất kết nối
            }
        }
    }
}
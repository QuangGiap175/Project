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


namespace WpfApp1
{
    /// <summary>
    /// Interaction logic for StressAnalysis.xaml
    /// </summary>
    public partial class StressAnalysis : Page
    {
        private StressAnalyzer analyzer = new BasicStressAnalyzer();

        public StressAnalysis()
        {
            InitializeComponent();
        }

       private void AnalyzeButton_Click(object sender, RoutedEventArgs e)
        {
           /* try
            {
                int heartRate = int.Parse(HeartRateTextBox.Text);
                int spo2 = int.Parse(SpO2TextBox.Text);

                var data = new SensorData
                {
                    HeartRate = heartRate,
                    SpO2 = spo2,
                    Timestamp = DateTime.Now
                };

                StressLevel level = analyzer.Analyze(data);

                string result = level switch
                {
                    StressLevel.Low => "Mức độ stress thấp. Bạn đang thư giãn tốt!",
                    StressLevel.Medium => "Mức độ stress trung bình. Hãy nghỉ ngơi nhẹ nhàng.",
                    StressLevel.High => "Stress cao! Nên thực hiện bài thở hoặc chơi game thư giãn.",
                    _ => "Không xác định."
                };

                ResultTextBlock.Text = result;
            }
            catch (Exception ex)
            {
                MessageBox.Show("Dữ liệu không hợp lệ. Vui lòng nhập số nguyên.\n" + ex.Message,
                                "Lỗi", MessageBoxButton.OK, MessageBoxImage.Warning);
            }*/
        }
    }
}

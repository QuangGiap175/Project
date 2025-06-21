using System;
using System.Windows.Controls;

namespace WpfApp1
{
    /// <summary>
    /// Interaction logic for StressAnalysis.xaml
    /// </summary>
    public partial class StressAnalysis : UserControl
    {
        public StressAnalysis()
        {
            InitializeComponent();
        }

        // ✅ Phương thức để cập nhật dữ liệu từ Live Monitor
        public void UpdateInputFields(float heartRate, float spo2, float hrv)
        {
            BpmInput.Text = heartRate.ToString("F0");
            Spo2Input.Text = spo2.ToString("F0");
            RmssdInput.Text = hrv.ToString("F2");

            // Nếu muốn auto nhập tuổi (có thể tùy chỉnh)
            AgeInput.Text = "25";

            // Nếu muốn tự động phân tích ngay khi có dữ liệu đủ:
            // AnalyzeStress_Click(null, null);
        }

        private void AnalyzeStress_Click(object sender, System.Windows.RoutedEventArgs e)
        {
            try
            {
                // Lấy giá trị từ TextBox và chuyển thành số
                if (float.TryParse(AgeInput.Text, out float age) &&
                    float.TryParse(BpmInput.Text, out float bpm) &&
                    float.TryParse(Spo2Input.Text, out float spo2) &&
                    float.TryParse(RmssdInput.Text, out float rmssd))
                {
                    if (age < 0 || age > 120 || bpm < 40 || bpm > 200 || spo2 < 70 || spo2 > 100 || rmssd < 0 || rmssd > 200)
                    {
                        AnalysisResult.Text = "Dữ liệu không hợp lệ. Vui lòng nhập trong phạm vi: Tuổi (0-120), BPM (40-200), SpO2 (70-100%), RMSSD (0-200 ms).";
                        return;
                    }

                    string analysis = AnalyzeStressLevels(age, bpm, spo2, rmssd);
                    AnalysisResult.Text = analysis;
                }
                else
                {
                    AnalysisResult.Text = "Vui lòng nhập số hợp lệ cho Tuổi, BPM, SpO2 và RMSSD.";
                }
            }
            catch (Exception ex)
            {
                AnalysisResult.Text = $"Lỗi: {ex.Message}";
            }
        }

        private string AnalyzeStressLevels(float age, float bpm, float spo2, float rmssd)
        {
            (float minBpm, float maxBpm, string ageGroup) = GetHeartRateRange(age);

            bool isBpmNormal = bpm >= minBpm && bpm <= maxBpm;
            bool isBpmHigh = bpm > maxBpm;
            bool isBpmLow = bpm < minBpm;
            string bpmStatus = isBpmNormal ? "Bình thường" : isBpmHigh ? "Cao" : "Thấp";

            string spo2Status;
            int spo2Severity = 0;
            if (spo2 >= 95)
                spo2Status = "✅ Bình thường – Phổi hoạt động tốt";
            else if (spo2 >= 90)
            {
                spo2Status = "⚠️ Thấp nhẹ – Nên theo dõi thêm";
                spo2Severity = 1;
            }
            else if (spo2 >= 85)
            {
                spo2Status = "❗ Thiếu oxy (Hypoxemia) – Có thể nguy hiểm";
                spo2Severity = 2;
            }
            else
            {
                spo2Status = "🚨 Cảnh báo nghiêm trọng – Cần can thiệp y tế";
                spo2Severity = 3;
            }

            string hrvStatus;
            int hrvSeverity = 0;
            if (rmssd > 100)
                hrvStatus = "🟢 Tuyệt vời – Hệ thần kinh khỏe mạnh";
            else if (rmssd >= 70)
                hrvStatus = "✅ Tốt – Phản xạ tim mạch ổn";
            else if (rmssd >= 50)
            {
                hrvStatus = "⚠️ Trung bình – Có thể stress nhẹ";
                hrvSeverity = 1;
            }
            else if (rmssd >= 30)
            {
                hrvStatus = "❗ Thấp – Căng thẳng, mất ngủ, kém hồi phục";
                hrvSeverity = 2;
            }
            else
            {
                hrvStatus = "🚨 Rất thấp – Stress cao, lo âu, cần nghỉ";
                hrvSeverity = 3;
            }

            string analysis = $"Nhóm tuổi: {ageGroup}\n";
            analysis += $"Nhịp tim: {bpm} bpm ({bpmStatus} so với phạm vi {minBpm}-{maxBpm} bpm)\n";
            analysis += $"SpO2: {spo2}% ({spo2Status})\n";
            analysis += $"HRV: {rmssd} ms ({hrvStatus})\n\n";

            int totalSeverity = (isBpmNormal ? 0 : 1) + spo2Severity + hrvSeverity;

            if (totalSeverity >= 5 || spo2Severity == 3 || hrvSeverity == 3)
                analysis += "Nhận xét: Mức độ stress cao hoặc có dấu hiệu bất thường nghiêm trọng. Nhịp tim bất thường, SpO2 rất thấp, hoặc HRV rất thấp. Cần nghỉ ngơi hoặc tham khảo ý kiến bác sĩ ngay!";
            else if (totalSeverity >= 3 || spo2Severity == 2 || hrvSeverity == 2)
                analysis += "Nhận xét: Mức độ stress cao vừa phải. Có dấu hiệu căng thẳng hoặc thiếu oxy. Nên thư giãn, theo dõi thêm, và cân nhắc kiểm tra y tế nếu triệu chứng kéo dài.";
            else if (totalSeverity >= 1 || !isBpmNormal || spo2Severity == 1 || hrvSeverity == 1)
                analysis += "Nhận xét: Mức độ stress trung bình. Một số chỉ số chưa tối ưu, có thể do căng thẳng nhẹ. Hãy nghỉ ngơi và theo dõi thêm.";
            else
                analysis += "Nhận xét: Mức độ stress thấp. Các chỉ số cho thấy cơ thể đang ở trạng thái thư giãn và khỏe mạnh.";

            return analysis;
        }

        private (float minBpm, float maxBpm, string ageGroup) GetHeartRateRange(float age)
        {
            if (age <= 0.0833f) return (70, 190, "Trẻ sơ sinh (0-1 tháng)");
            else if (age <= 1) return (80, 160, "Trẻ sơ sinh (1-12 tháng)");
            else if (age <= 2) return (80, 130, "Trẻ nhỏ (1-2 tuổi)");
            else if (age <= 4) return (80, 120, "Trẻ em (3-4 tuổi)");
            else if (age <= 6) return (75, 115, "Trẻ em (5-6 tuổi)");
            else if (age <= 9) return (70, 110, "Trẻ em (7-9 tuổi)");
            else if (age <= 15) return (60, 100, "Thanh thiếu niên (10-15 tuổi)");
            else if (age <= 60) return (60, 100, "Người lớn (18-60 tuổi)");
            else if (age > 60) return (60, 100, "Người già (>60 tuổi)");
            else return (40, 60, "Vận động viên chuyên nghiệp");
        }
    }
}
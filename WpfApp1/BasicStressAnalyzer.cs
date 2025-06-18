using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace WpfApp1
{
   public class BasicStressAnalyzer : StressAnalyzer
{
    public override StressLevel Analyze(SensorData data)
    {
        if (data.HeartRate > 100 || data.SpO2 < 95) return StressLevel.High;
        if (data.HeartRate > 85) return StressLevel.Medium;
        return StressLevel.Low;
    }
}

}

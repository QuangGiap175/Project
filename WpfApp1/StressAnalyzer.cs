using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace WpfApp1
{
    public abstract class StressAnalyzer
    {
        public abstract StressLevel Analyze(SensorData data);
    }
}

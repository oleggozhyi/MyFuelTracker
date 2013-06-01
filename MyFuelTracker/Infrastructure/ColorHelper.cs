using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Media;

namespace MyFuelTracker.Infrastructure
{
    public static class ColorHelper
    {
        private const double MAX_HUE = 0.0;
        private const double MIN_HUE = 140.0;
        private const double AVG_HUE = 60.0;

        public static readonly Color MaxColor = new HslColor { A = 1, H = MAX_HUE, L = 0.5, S = 1 }.ToColor();
        public static readonly Color MinColor = new HslColor { A = 1, H = MIN_HUE, L = 0.5, S = 1 }.ToColor();
        public static readonly Color AvgColor = new HslColor { A = 1, H = AVG_HUE, L = 0.5, S = 1 }.ToColor();

        public static Color GetColor(double currentValue, double minValue, double avgValue, double maxValue, bool foreground = true)
        {
			if(Math.Abs(minValue - maxValue) < 0.001)
				return new HslColor { A = 1, H = MIN_HUE, L = foreground ? 0.5 : 0.35, S = 1 }.ToColor(); 
            double currentValueHue;
            if (currentValue >= minValue &&
                currentValue < avgValue)
            {
                double d = (currentValue - minValue) / (avgValue - minValue);
                currentValueHue = MIN_HUE - (MIN_HUE - AVG_HUE) * d;
            }
            else
            {
                double d = (currentValue - avgValue) / (maxValue - avgValue);
                currentValueHue = AVG_HUE - (AVG_HUE - MAX_HUE) * d;
            }

            var color = new HslColor { A = 1, H = currentValueHue, L = foreground ? 0.5 : 0.35, S = 1 }.ToColor();
            return color;
        }

    }
}

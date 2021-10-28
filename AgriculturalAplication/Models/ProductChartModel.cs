using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AgriculturalAplication.Models
{
    public class ProductChartModel
    {
        public class TemperatureModel
        {
            public DateTime DateTimePost { get; set; }

            public int Temperature { get; set; }
        }

        public class HumidityModel
        {
            public DateTime DateTimePost { get; set; }

            public int Humidity { get; set; }
        }

        public class SoilHumidityModel
        {
            public DateTime DateTimePost { get; set; }

            public int SoilHumidity { get; set; }
        }

        public class LuminosityModel
        {
            public DateTime DateTimePost { get; set; }

            public int Luminosity { get; set; }
        }
    }
}

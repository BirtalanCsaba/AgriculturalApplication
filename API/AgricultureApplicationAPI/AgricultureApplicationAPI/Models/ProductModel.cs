using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;

namespace AgricultureApplicationAPI.Models
{
    public class ProductModel
    {
        [Key]
        public Guid Id { get; set; }

        public string Key { get; set; }

        public string Desc { get; set; }

        //sensors
        public int Temperature { get; set; }

        public int Humidity { get; set; }

        public int SoilHumidity { get; set; }

        public int Luminosity { get; set; }

        public int DelayTime { get; set; }
    }
}

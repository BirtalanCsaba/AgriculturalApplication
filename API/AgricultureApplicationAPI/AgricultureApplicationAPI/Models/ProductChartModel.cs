using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Threading.Tasks;

namespace AgricultureApplicationAPI.Models
{
    public class ProductChartModel
    {
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        [Key]
        public int Id { get; set; }

        public Guid ProductId { get; set; }

        public DateTime DateTimePost { get; set; }

        //sensors
        public int Temperature { get; set; }

        public int Humidity { get; set; }

        public int SoilHumidity { get; set; }

        public int Luminosity { get; set; }
    }
}

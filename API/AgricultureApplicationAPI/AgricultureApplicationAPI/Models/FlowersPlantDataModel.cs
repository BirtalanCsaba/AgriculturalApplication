using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Threading.Tasks;

namespace AgricultureApplicationAPI.Models
{
    public class FlowersPlantDataModel
    {
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        [Key]
        public int Id { get; set; }

        public string Name { get; set; }

        public int Temperature { get; set; }

        public int Humidity { get; set; }

        public int Luminosity { get; set; }
    }
}

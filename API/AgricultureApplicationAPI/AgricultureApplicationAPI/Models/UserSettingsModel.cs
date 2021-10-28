using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;

namespace AgricultureApplicationAPI.Models
{
    public class UserSettingsModel
    {
        [Key]
        public Guid ProductId { get; set; }

        public Guid UserId { get; set; }

        [DefaultValue(-50)]
        public int TempMinValue { get; set; } = -50;

        [DefaultValue(50)]
        public int TempMaxValue { get; set; } = 50;

        [DefaultValue(0)]
        public int HumMinValue { get; set; } = 0;

        [DefaultValue(100)]
        public int HumMaxValue { get; set; } = 100;

        [DefaultValue(0)]
        public int LumMinValue { get; set; } = 0;

        [DefaultValue(100)]
        public int LumMaxValue { get; set; } = 100;

        [DefaultValue(0)]
        public int PlantType { get; set; } = 0;

        [DefaultValue(0)]
        public int PlantId { get; set; } = 0;
    }
}

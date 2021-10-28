using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AgriculturalAplication.Models
{
    public class UserSettingsModel
    {
        public Guid UserId { get; set; }

        public int TempMinValue { get; set; }

        public int TempMaxValue { get; set; }

        public int HumMinValue { get; set; }

        public int HumMaxValue { get; set; }

        public int LumMinValue { get; set; }

        public int LumMaxValue { get; set; }
    }
}

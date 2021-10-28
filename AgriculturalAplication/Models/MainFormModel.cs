using AgriculturalAplication.TemporaryUserData;
using Newtonsoft.Json;
using Newtonsoft.Json.Linq;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Http;
using System.Net.Http.Headers;
using System.Text;
using System.Text.RegularExpressions;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Media.Imaging;

namespace AgriculturalAplication.Models
{
    public class MainFormModel
    {
        public class Project
        {
            public string Name { get; set; }

            public string Description { get; set; }

            public byte[] Image { get; set; }

            public string ProductId { get; set; }

            public BitmapImage BMImage { get; set; }
        }

        public class ComboBoxItems
        {
            public string Name { get; set; }

            public int Value { get; set; }
        }
    }
}

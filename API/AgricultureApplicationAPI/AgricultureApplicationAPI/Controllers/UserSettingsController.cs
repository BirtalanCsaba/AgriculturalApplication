using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using AgricultureApplicationAPI.Models;
using Microsoft.AspNetCore.Mvc;

namespace AgricultureApplicationAPI.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class UserSettingsController : Controller
    {
        private readonly ApplicationDbContext _context;

        public UserSettingsController(ApplicationDbContext context)
        {
            _context = context;
        }

        [HttpPost("GetSettings")]
        public UserSettingsModel GetSettings([FromForm]string UserId)
        {
            UserSettingsModel settings = _context.Settings.Where(a => a.UserId.Equals(new Guid(UserId))).FirstOrDefault();

            return settings;
        }

        [HttpPost("CreateSettings")]
        public async Task CreateSettings([FromForm]string UserId,[FromForm]string ProductId)
        {
            UserSettingsModel settings = new UserSettingsModel()
            {
                UserId = new Guid(UserId),
                ProductId = new Guid(ProductId)
            };

            _context.Settings.Add(settings);
            await _context.SaveChangesAsync();
        }

        [HttpPost("DeleteSettings")]
        public async Task DeleteSettings([FromForm]string UserId, [FromForm]string ProductId)
        {
            var settings = _context.Settings.Where(a => a.UserId.Equals(new Guid(UserId)) && a.ProductId.Equals(new Guid(ProductId))).FirstOrDefault();

            if (settings != null)
            {
                _context.Settings.Remove(settings);
                await _context.SaveChangesAsync();
            }
        }

        [HttpPost("SaveSensorChanges")]
        public async Task SaveSensorChanges([FromForm]string UserId,[FromForm]string ProductId,[FromForm]int TempMinValue,
            [FromForm]int TempMaxValue,[FromForm]int HumMinValue,[FromForm]int HumMaxValue,[FromForm]int LumMinValue,
            [FromForm]int LumMaxValue)
        {
            var settings = _context.Settings.Where(a => a.UserId.Equals(new Guid(UserId)) && a.ProductId.Equals(new Guid(ProductId))).FirstOrDefault();

            if(settings != null)
            {
                settings.TempMinValue = TempMinValue;
                settings.TempMaxValue = TempMaxValue;
                settings.HumMinValue = HumMinValue;
                settings.HumMaxValue = HumMaxValue;
                settings.LumMinValue = LumMinValue;
                settings.LumMaxValue = LumMaxValue;

                _context.Settings.Update(settings);
                await _context.SaveChangesAsync();
            }
        }

        [HttpPost("AddPlant")]
        public async Task AddPlant([FromForm]string ProductId,[FromForm]int PlantType,[FromForm] int PlantId)
        {
            var setting = _context.Settings.Where(a => a.ProductId.Equals(new Guid(ProductId))).FirstOrDefault();

            if(setting != null)
            {
                setting.PlantType = PlantType;
                setting.PlantId = PlantId;

                _context.Settings.Update(setting);

                await _context.SaveChangesAsync();
            }
        }

        [HttpPost("GetPlantValuesById")]
        public object GetPlantValuesById([FromForm]int PlantType,[FromForm]int PlantId)
        {
            if (PlantType == 1)
            {
                var plant = _context.VegetablesPlants.Where(a => a.Id.Equals(PlantId)).FirstOrDefault();

                return plant;
            }
            else if(PlantType == 2)
            {
                var plant = _context.FlowersPlants.Where(a => a.Id.Equals(PlantId)).FirstOrDefault();

                return plant;
            }
            else
            {
                VegetablesPlantDataModel plant = new VegetablesPlantDataModel
                {
                    Name="Default",
                    Humidity=50,
                    Luminosity=80,
                    Temperature=30
                };

                return plant;
            }
        }

        [HttpPost("GetSelectedPlant")]
        public KeyValuePair<int,int> GetSelectedPlant([FromForm]string ProductId)
        {
            var setting = _context.Settings.Where(a => a.ProductId.Equals(new Guid(ProductId))).FirstOrDefault();

            return new KeyValuePair<int, int>(setting.PlantId,setting.PlantType);
        }

        [HttpPost("GetSelectedPlantName")]
        public string GetSelectedPlantName([FromForm]string ProductId)
        {
            var setting = _context.Settings.Where(a => a.ProductId.Equals(new Guid(ProductId))).FirstOrDefault();

            if(setting.PlantType == 0)
            {
                return "Default Plant";
            }
            else if(setting.PlantType == 2)
            {
                var plant = _context.FlowersPlants.Where(a => a.Id.Equals(setting.PlantId)).FirstOrDefault();

                return plant.Name;
            }
            else
            {
                var plant = _context.VegetablesPlants.Where(a => a.Id.Equals(setting.PlantId)).FirstOrDefault();

                return plant.Name;
            }
        }
    }
}
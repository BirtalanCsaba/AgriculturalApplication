using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using AgricultureApplicationAPI.Models;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

namespace AgricultureApplicationAPI.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class ProductController : ControllerBase
    {
        private readonly ApplicationDbContext _context;

        public ProductController(ApplicationDbContext context)
        {
            _context = context;
        }

        // POST: api/Product
        [HttpPost]
        public ActionResult<bool> Post([FromForm] string ProdKey)
        {
            var product = _context.Products.Where(a => a.Key.Equals(ProdKey)).FirstOrDefault();
            if (product == null) return false;
            else return true;
        }

        [HttpPost("CheckIfKeyIsAvailable")]
        public bool CheckIfKeyIsAvailable([FromForm] string ProdKey)
        {
            var prod = _context.Products.Where(a => a.Key.Equals(ProdKey)).FirstOrDefault();

            var proj = _context.Projects.Where(a => a.ProductId.Equals(prod.Id)).FirstOrDefault();

            if (proj == null) return false;
            else return true;
        }

        [HttpPost("GetDescription")]
        public string GetDescription([FromForm]string ProductId)
        {
            string desc = "";

            var product = _context.Products.Where(a => a.Id.Equals(new Guid(ProductId))).FirstOrDefault();
            if(product != null)
            {
                desc = product.Desc;
            }

            return desc;
        }

        [HttpPost("GetTemperature")]
        public int GetTemperature([FromForm]string ProductId)
        {
            int temp = 0;

            var product = _context.Products.Where(a => a.Id.Equals(new Guid(ProductId))).FirstOrDefault();
            if (product != null)
            {
                temp = product.Temperature;
            }

            return temp;
        }

        [HttpPost("GetHumidity")]
        public int GetHumidity([FromForm]string ProductId)
        {
            int temp = 0;

            var product = _context.Products.Where(a => a.Id.Equals(new Guid(ProductId))).FirstOrDefault();
            if (product != null)
            {
                temp = product.Humidity;
            }

            return temp;
        }

        [HttpPost("GetLuminosity")]
        public int GetLuminosity([FromForm]string ProductId)
        {
            int temp = 0;

            var product = _context.Products.Where(a => a.Id.Equals(new Guid(ProductId))).FirstOrDefault();
            if (product != null)
            {
                temp = product.Luminosity;
            }

            return temp;
        }

        [HttpPost("GetSoilHumidity")]
        public int GetSoilHumidity([FromForm]string ProductId)
        {
            int temp = 0;

            var product = _context.Products.Where(a => a.Id.Equals(new Guid(ProductId))).FirstOrDefault();
            if (product != null)
            {
                temp = product.SoilHumidity;
            }

            return temp;
        }

        [HttpGet("SendSensorValues")]
        public async Task<int> SendSensorValues([FromQuery]string ProdId,[FromQuery]string Temp,[FromQuery]string Hum, 
            [FromQuery]string SoilHum,[FromQuery]string Lum)
        {

            var prod = _context.Products.Where(a => a.Id.Equals(new Guid(ProdId))).FirstOrDefault();

            if(prod != null)
            {
                if(Convert.ToInt32(Temp) < 100 && Convert.ToInt32(Hum) <= 100)
                {
                    prod.Temperature = Convert.ToInt32(Temp);
                    prod.Humidity = Convert.ToInt32(Hum);
                    prod.SoilHumidity = Convert.ToInt32(SoilHum);
                    prod.Luminosity = Convert.ToInt32(Lum);

                    await _context.SaveChangesAsync();

                    //delete data after 6 values
                    var chartvalues = _context.Charts.Where(a => a.ProductId.Equals(new Guid(ProdId))).ToList();
                    if (chartvalues.Count >= 6)
                    {
                        chartvalues.ForEach(a => _context.Charts.Remove(a));
                        await _context.SaveChangesAsync();
                    }

                    //save data to chart table
                    var chart = new ProductChartModel()
                    {
                        ProductId = new Guid(ProdId),
                        DateTimePost = DateTime.Now,
                        Temperature = Convert.ToInt32(Temp),
                        Humidity = Convert.ToInt32(Hum),
                        SoilHumidity = Convert.ToInt32(SoilHum),
                        Luminosity = Convert.ToInt32(Lum)
                    };

                    _context.Charts.Add(chart);
                    await _context.SaveChangesAsync();

                    return prod.DelayTime;
                }
            }

            return 30000;
        }

        [HttpPost("ChangeDelayTime")]
        public async Task ChangeDelayTime([FromForm]string ProdId,[FromForm]int DelayTime)
        {
            var prod = _context.Products.Where(a => a.Id.Equals(new Guid(ProdId))).FirstOrDefault();

            prod.DelayTime = DelayTime;

            _context.Products.Update(prod);
            await _context.SaveChangesAsync();
        }

        [HttpPost("{id}")]
        public ActionResult<string> ProductsGet(int id,[FromForm] string ProdKey)
        {
            string ProdId = "";
            if(id == 1)
            {
                return ProdId = _context.Products.Where(a => a.Key.Equals(ProdKey)).FirstOrDefault().Id.ToString();
            }

            return ProdId;
        }
    }
}

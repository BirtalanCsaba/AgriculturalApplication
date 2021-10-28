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
    public class ProductChartController : ControllerBase
    {
        private readonly ApplicationDbContext _context;

        public ProductChartController(ApplicationDbContext context)
        {
            _context = context;
        }

        [HttpPost("GetChartValue")]
        public ActionResult<List<ProductChartModel>> GetChartValue([FromForm] string ProductId)
        {
            var chart = _context.Charts.Where(a => a.ProductId.Equals(new Guid(ProductId))).ToList();

            if (chart != null) return chart;
            else return new List<ProductChartModel>();
        }
    }
}

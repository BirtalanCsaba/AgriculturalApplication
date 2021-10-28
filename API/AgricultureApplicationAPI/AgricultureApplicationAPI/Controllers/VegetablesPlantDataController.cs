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
    public class VegetablesPlantDataController : ControllerBase
    {
        private readonly ApplicationDbContext _context;

        public VegetablesPlantDataController(ApplicationDbContext context)
        {
            _context = context;
        }

        [HttpGet("GetValues")]
        public List<VegetablesPlantDataModel> GetValues()
        {
            var plants = _context.VegetablesPlants.ToList();

            return plants;
        }
    }
}
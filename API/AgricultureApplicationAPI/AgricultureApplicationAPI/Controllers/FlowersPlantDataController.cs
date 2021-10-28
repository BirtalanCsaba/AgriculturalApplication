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
    public class FlowersPlantDataController : ControllerBase
    {
        private readonly ApplicationDbContext _context;

        public FlowersPlantDataController(ApplicationDbContext context)
        {
            _context = context;
        }

        [HttpGet("GetValues")]
        public List<FlowersPlantDataModel> GetValues()
        {
            var plants = _context.FlowersPlants.ToList();

            return plants;
        }
    }
}
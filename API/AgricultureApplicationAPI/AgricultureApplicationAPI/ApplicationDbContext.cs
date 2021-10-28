using AgricultureApplicationAPI.Models;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace AgricultureApplicationAPI
{
    public class ApplicationDbContext:DbContext
    {
        public DbSet<UserModel> Users { get; set; }
        public DbSet<UserProjectsModel> Projects { get; set; }
        public DbSet<ProductModel> Products { get; set; }
        public DbSet<ProductChartModel> Charts { get; set; }
        public DbSet<UserSettingsModel> Settings { get; set; }
        public DbSet<VegetablesPlantDataModel> VegetablesPlants { get; set; }
        public DbSet<FlowersPlantDataModel> FlowersPlants { get; set; }

        public ApplicationDbContext(DbContextOptions<ApplicationDbContext>options)
            :base(options)
        {

        }
    }
}

using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using AgricultureApplicationAPI.Models;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

namespace AgricultureApplicationAPI.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class ProjectController : ControllerBase
    {
        private readonly ApplicationDbContext _context;

        public ProjectController(ApplicationDbContext context)
        {
            _context = context;
        }

        // POST: api/Project
        [HttpPost("GetProjectsById")]
        public ICollection<UserProjectsModel> GetProjectsById([FromForm]string UserId)
        {
            var proj = _context.Projects.Where(a => a.UserId.Equals(new Guid(UserId))).ToList();

            return proj;

        }

        [HttpPost("{id}")]
        public async Task Project(int id, [FromForm]string ProjName, [FromForm]string UserId,
            [FromForm]string ProjDesc,[FromForm]string ProjImage,[FromForm]string ProdId)
        {
            //remove project
            if (id == 1)
            {
                var proj = _context.Projects.FirstOrDefault(a => a.Name.Equals(ProjName) && a.UserId.Equals(new Guid(UserId)));
                if(proj != null)
                {
                    _context.Projects.Remove(proj);
                    await _context.SaveChangesAsync();
                }
            }
            //create project
            else if(id == 2)
            {
                UserProjectsModel proj;

                if (ProjImage != null)
                {
                    proj = new UserProjectsModel
                    {
                        UserId = new Guid(UserId),
                        ProductId = new Guid(ProdId),
                        Name = ProjName,
                        Description = ProjDesc,
                        Image = Convert.FromBase64String(ProjImage)
                    };
                }
                else
                {
                    proj = new UserProjectsModel
                    {
                        UserId = new Guid(UserId),
                        ProductId = new Guid(ProdId),
                        Name = ProjName,
                        Description = ProjDesc
                    };
                }
                

                _context.Projects.Add(proj);
                _context.SaveChanges();
            }
        }

        [HttpPost("ValidateProject")]
        public bool ValidateProject(string ProdId)
        {
            var proj = _context.Projects.Where(a => a.ProductId.Equals(new Guid(ProdId))).FirstOrDefault();
            if (proj == null) return false;
            else return true;
        }

        [HttpPost("EditProject")]
        public async Task EditProject([FromForm]string ProdId,[FromForm]string ProjName,
            [FromForm]string ProjDesc,[FromForm]string ProjImage)
        {
            var proj = _context.Projects.Where(a => a.ProductId.Equals(new Guid(ProdId))).FirstOrDefault();

            if(proj != null)
            {
                if (!String.IsNullOrEmpty(ProjName)) proj.Name = ProjName;
                if (!String.IsNullOrEmpty(ProjDesc)) proj.Description = ProjDesc;
                if (ProjImage != null) proj.Image = Convert.FromBase64String(ProjImage);

                _context.Projects.Update(proj);
                await _context.SaveChangesAsync();
            }
        }
    }
}

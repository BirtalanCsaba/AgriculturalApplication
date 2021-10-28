using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using MySql.Data.MySqlClient;
using AgricultureApplicationAPI.Models;

namespace AgricultureApplicationAPI.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class UserController : ControllerBase
    {
        private readonly ApplicationDbContext _context;

        public UserController(ApplicationDbContext context)
        {
            _context = context;
        }

        [HttpPost]
        public ActionResult<bool> Authenticate([FromForm] string username, [FromForm] string password)
        {
            var userFound = _context.Users.FirstOrDefault(e => e.Username == username && e.Password == password);
            if (userFound == null) return false;
            else return true;
        }

        [HttpPost("{id}")]
        public ActionResult<Guid> GetUserId([FromForm] string username, [FromForm] string password)
        {
            var userFound = _context.Users.FirstOrDefault(e => e.Username == username && e.Password == password);
            return userFound.Id;
        }

        [HttpPost("CreateUser")]
        public async Task CreateUser([FromForm]string Username,[FromForm]string Email,
            [FromForm]string Password)
        {
            UserModel user = new UserModel
            {
                Id = Guid.NewGuid(),
                Username = Username,
                Email = Email,
                Password = Password
            };

            _context.Add(user);
            await _context.SaveChangesAsync();
        }

        [HttpPost("GetUserEmail")]
        public string GetUserEmail([FromForm]string UserId)
        {
            var user = _context.Users.Where(a => a.Id.Equals(new Guid(UserId))).FirstOrDefault();

            return user.Email;
        }

        [HttpPost("UpdateUsername")]
        public async Task UpdateUsername([FromForm]string UserId, [FromForm]string Username)
        {
            var user = _context.Users.Where(a => a.Id.Equals(new Guid(UserId))).FirstOrDefault();

            user.Username = Username;

            _context.Users.Update(user);
            await _context.SaveChangesAsync();
        }

        [HttpPost("UpdateEmail")]
        public async Task UpdateEmail([FromForm]string UserId,[FromForm]string Email)
        {
            var user = _context.Users.Where(a => a.Id.Equals(new Guid(UserId))).FirstOrDefault();

            user.Email = Email;

            _context.Users.Update(user);
            await _context.SaveChangesAsync();
        }


        [HttpPost("UpdatePassword")]
        public async Task UpdatePassword([FromForm]string UserId, [FromForm]string Password)
        {
            var user = _context.Users.Where(a => a.Id.Equals(new Guid(UserId))).FirstOrDefault();

            user.Password = Password;

            _context.Users.Update(user);
            await _context.SaveChangesAsync();
        }


    }
}

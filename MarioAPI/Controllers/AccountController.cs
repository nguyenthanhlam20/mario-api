using MarioAPI.Models;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace MarioAPI.Controllers
{
    [ApiController]
    [Route("[controller]/[action]")]
    public class AccountController : ControllerBase
    {
        private readonly MarioContext _context;
        public AccountController(MarioContext context)
        {
            _context = context;
        }

        [HttpPost]
        public async Task<IActionResult> Login(string username, string password)
        {
            var exist = await _context.Accounts.Where(x => x.Username == username && x.Password == password).AnyAsync();
            if (exist) return Ok();
            else return BadRequest();
        }
    }
}

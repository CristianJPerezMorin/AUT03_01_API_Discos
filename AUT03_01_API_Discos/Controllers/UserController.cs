using AUT03_01_API_Discos.Data;
using AUT03_01_API_Discos.Models;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace AUT03_01_API_Discos.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class UserController : ControllerBase
    {
        private readonly AppUserContext _context;
        public readonly UserManager<AppUser> _userManager;

        public UserController(AppUserContext context, UserManager<AppUser> userManager)
        {
            _context = context;
            _userManager = userManager;
        }

        // GET: api/Artists
        [HttpGet]
        public async Task<ActionResult<IEnumerable<AppUser>>> ListUser()
        {
            var users = await _context.Users.ToListAsync();

            foreach (var user in users)
            {
                user.roles = await _userManager.GetRolesAsync(user);
            }

            return Ok(users);
        }
    }
}

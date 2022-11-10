using AUT03_01_API_Discos.Data;
using AUT03_01_API_Discos.Models;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Microsoft.IdentityModel.JsonWebTokens;
using Microsoft.IdentityModel.Tokens;
using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Text;
using JwtRegisteredClaimNames = Microsoft.IdentityModel.JsonWebTokens.JwtRegisteredClaimNames;

namespace AUT03_01_API_Discos.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class AuthController : ControllerBase
    {
        public readonly AppUserContext _context;
        private readonly IConfiguration _configuration;
        public readonly UserManager<IdentityUser> _userManager;

        public AuthController(AppUserContext context, UserManager<IdentityUser> userManager, IConfiguration configuration)
        {
            _context = context;
            _userManager = userManager;
            _configuration = configuration;
        }

        [HttpPost("[Action]")]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        public async Task<IActionResult> Login(string email, string contraseña)
        {
            if (!string.IsNullOrEmpty(email) && !string.IsNullOrEmpty(contraseña))
            {
                var user = await _context.Users.FindAsync(email);

                if (user != null && await _userManager.CheckPasswordAsync(user, contraseña))
                {
                    var roles = await _userManager.GetRolesAsync(user);
                    //Añadir Claims
                    var claims = new List<Claim>
                    {
                        new Claim(JwtRegisteredClaimNames.Jti, Guid.NewGuid().ToString()),
                        new Claim(JwtRegisteredClaimNames.Iat, DateTime.UtcNow.ToString()),
                        new Claim("UserId", user.Id),
                        new Claim("UserName", user.UserName),
                        new Claim("Email", user.Email)
                    };
                    foreach (var role in roles)
                    {
                        claims.Add(new Claim(ClaimTypes.Role, role));
                    }

                    var key = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(_configuration["Jwt:Key"]));
                    var credentials = new SigningCredentials(key, SecurityAlgorithms.HmacSha256);
                    var token = new JwtSecurityToken(
                        _configuration["Jwt:Issuer"],
                        _configuration["Jwt:Audience"],
                        claims,
                        expires: DateTime.UtcNow.AddMinutes(10),
                        signingCredentials: credentials
                        );

                    return Ok(new JwtSecurityTokenHandler().WriteToken(token));
                }
                else
                {
                    return NotFound("Error: No se ha encontrado el registro.");
                }
            }
            else
            {
                return BadRequest("Error: No se ha completado de forma correcta el registro.");
            }
        }

        [HttpPost("[Action]")]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        public async Task<IActionResult> Register(string email, string contraseña, string nombre, string apellidos, int CodPostal)
        {
            if (!string.IsNullOrEmpty(email) && !string.IsNullOrEmpty(contraseña) && !string.IsNullOrEmpty(nombre) && !string.IsNullOrEmpty(apellidos) && CodPostal > 999)
            {
                var users = await _context.Users.ToListAsync();
                if (users != null)
                {
                    if (users.Find(x => x.Email == email) != null)
                    {
                        return BadRequest("Error: El usuario ya existe.");
                    }
                    else
                    {
                        var newUser = new AppUser
                        {
                            UserName = email,
                            Email = email,
                            NormalizedUserName = email.ToUpper(),
                            NormalizedEmail = email.ToUpper(),
                            Nombre = nombre,
                            Apellidos = apellidos,
                            CodPostal = CodPostal,
                            EmailConfirmed = true
                        };
                        var passwordHasher = new PasswordHasher<IdentityUser>();
                        newUser.PasswordHash = passwordHasher.HashPassword(newUser, contraseña);
                        var roles = await _context.Roles.ToListAsync();
                        IdentityUserRole<string> relacion = new IdentityUserRole<string>
                        {
                            RoleId = roles.Find(r => r.Name == "Default").Id,
                            UserId = newUser.Id
                        };

                        await _userManager.CreateAsync(newUser);
                        await _userManager.AddToRoleAsync(newUser, "Default");

                        return Ok("Registro realizado correctamente");
                    }

                }
                else
                {
                    return NotFound("Error: No se ha encontrado el registro.");
                }
            }
            else
            {
                return BadRequest("Error: No se ha completado de forma correcta el registro.");
            }
        }
    }
}


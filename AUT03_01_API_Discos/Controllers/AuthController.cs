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
        public readonly UserManager<AppUser> _userManager;

        public AuthController(AppUserContext context, UserManager<AppUser> userManager, IConfiguration configuration)
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
                var user = await _userManager.FindByEmailAsync(email);

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
        public async Task<IActionResult> Register(AppUser user, string contraseña)
        {
            if (!string.IsNullOrEmpty(user.Email) && !string.IsNullOrEmpty(contraseña) && !string.IsNullOrEmpty(user.Nombre) && !string.IsNullOrEmpty(user.Apellidos) && user.CodPostal > 999)
            {
                var users = await _context.Users.ToListAsync();
                if (users != null)
                {
                    var findUser = users.Find(x => x.Email == user.Email);
                    if (findUser != null)
                    {
                        return BadRequest("Error: El usuario ya existe.");
                    }
                    else
                    {
                        var newUser = new AppUser
                        {
                            UserName = user.Email,
                            Email = user.Email,
                            NormalizedUserName = user.Email.ToUpper(),
                            NormalizedEmail = user.Email.ToUpper(),
                            Nombre = user.Nombre,
                            Apellidos = user.Apellidos,
                            CodPostal = user.CodPostal,
                            EmailConfirmed = true
                        };
                        var passwordHasher = new PasswordHasher<AppUser>();
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


using Microsoft.AspNetCore.Identity;
using Microsoft.Build.Framework;

namespace AUT03_01_API_Discos.Models
{
    public class AppUser : IdentityUser
    {
        [Required]
        public string? Nombre { get; set; }
        [Required]
        public string? Apellidos { get; set; }
        [Required]
        public int? CodPostal { get; set; }
        public IList<string> roles { get; set; }
    }
}

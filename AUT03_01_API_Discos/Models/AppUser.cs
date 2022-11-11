using Microsoft.AspNetCore.DataProtection.KeyManagement;
using Microsoft.AspNetCore.Identity;
using Microsoft.Build.Framework;
using Swashbuckle.AspNetCore.Annotations;

namespace AUT03_01_API_Discos.Models
{
    public class AppUser : IdentityUser
    {
        [SwaggerSchema(ReadOnly = true)]
        public override string UserName { get; set; }
        [SwaggerSchema(ReadOnly = true)]
        public override string NormalizedUserName { get; set; }
        [SwaggerSchema(ReadOnly = true)]
        public override string NormalizedEmail { get; set; }
        [SwaggerSchema(ReadOnly = true)]
        public override bool EmailConfirmed { get; set; }
        [SwaggerSchema(ReadOnly = true)]
        public override string PasswordHash { get; set; }
        [SwaggerSchema(ReadOnly = true)]
        public override string SecurityStamp { get; set; }
        [SwaggerSchema(ReadOnly = true)]
        public override string ConcurrencyStamp { get; set; } = Guid.NewGuid().ToString();
        [SwaggerSchema(ReadOnly = true)]
        public override string PhoneNumber { get; set; }
        [SwaggerSchema(ReadOnly = true)]
        public override bool PhoneNumberConfirmed { get; set; }
        [SwaggerSchema(ReadOnly = true)]
        public override bool TwoFactorEnabled { get; set; }
        [SwaggerSchema(ReadOnly = true)]
        public override DateTimeOffset? LockoutEnd { get; set; }
        [SwaggerSchema(ReadOnly = true)]
        public override bool LockoutEnabled { get; set; }
        [SwaggerSchema(ReadOnly = true)]
        public override int AccessFailedCount { get; set; }

        [Required]
        public string? Nombre { get; set; }
        [Required]
        public string? Apellidos { get; set; }
        [Required]
        public int? CodPostal { get; set; }
        [SwaggerSchema(ReadOnly = true)]
        public IList<string> roles { get; set; }
    }
}

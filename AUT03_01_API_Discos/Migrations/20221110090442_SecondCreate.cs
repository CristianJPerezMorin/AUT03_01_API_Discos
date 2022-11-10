using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace AUT03_01_API_Discos.Migrations
{
    public partial class SecondCreate : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            

            migrationBuilder.InsertData(
                table: "AppUser",
                columns: new[] { "Id", "AccessFailedCount", "Apellidos", "CodPostal", "ConcurrencyStamp", "Email", "EmailConfirmed", "LockoutEnabled", "LockoutEnd", "Nombre", "NormalizedEmail", "NormalizedUserName", "PasswordHash", "PhoneNumber", "PhoneNumberConfirmed", "SecurityStamp", "TwoFactorEnabled", "UserName" },
                values: new object[,]
                {
                    { "8badc220-bb80-4ca3-8c4b-f2f08ac6c79d", 0, "Admin Admin", 12345, "d2b5aa37-935c-4da0-8072-d6fbc1eb9606", "Admin@api.com", true, false, null, "Cristian", "ADMIN@API.COM", "ADMIN@API.COM", "AQAAAAEAACcQAAAAEByv1+Hj1E7Q5fFTByVhWsvKn7uRJYatXlqnIoOZb0LR2PM3qZGMbvHWLzU6Iqwx3w==", null, false, "a5810fd0-ef86-40ca-857c-4706dc278da1", false, "Admin@api.com" },
                    { "cab14033-2380-44cd-8aa3-2fbf1aa7c411", 0, "Manager Manager", 54321, "020ff4ea-6c8d-435f-9637-1b3d8110875f", "Manager@api.com", true, false, null, "Manager", "MANAGER@API.COM", "MANAGER@API.COM", "AQAAAAEAACcQAAAAEPnmqRceDK4avBNmqQeAQsBomFQUpBbesKDtLa+PQ4xYPhYVZhDzB77hi1IPAF1BmA==", null, false, "30eb79d9-b800-4099-aa19-c0abbdd9a2a4", false, "Manager@api.com" }
                });

            migrationBuilder.InsertData(
                table: "AspNetRoles",
                columns: new[] { "Id", "ConcurrencyStamp", "Name", "NormalizedName" },
                values: new object[,]
                {
                    { "1cf4fe92-0c00-47a3-96c3-d9dea63a08e6", "420f0d29-4dec-4a2f-8fe5-66f219a76494", "Default", "DEFAULT" },
                    { "9c3d9fbb-2dc3-4b9a-a141-026384c1c099", "ce8bb24b-cf83-4c6b-b539-c5c977dc3caf", "Admin", "ADMIN" },
                    { "c9fd04e6-d480-42ec-ab07-3da7d1a9c054", "6b0628f8-bb17-41d5-81c4-4b7a7742397c", "Manager", "MANAGER" }
                });

            migrationBuilder.InsertData(
                table: "AspNetUserRoles",
                columns: new[] { "RoleId", "UserId" },
                values: new object[] { "9c3d9fbb-2dc3-4b9a-a141-026384c1c099", "8badc220-bb80-4ca3-8c4b-f2f08ac6c79d" });

            migrationBuilder.InsertData(
                table: "AspNetUserRoles",
                columns: new[] { "RoleId", "UserId" },
                values: new object[] { "c9fd04e6-d480-42ec-ab07-3da7d1a9c054", "cab14033-2380-44cd-8aa3-2fbf1aa7c411" });

        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "AspNetRoleClaims");

            migrationBuilder.DropTable(
                name: "AspNetUserClaims");

            migrationBuilder.DropTable(
                name: "AspNetUserLogins");

            migrationBuilder.DropTable(
                name: "AspNetUserRoles");

            migrationBuilder.DropTable(
                name: "AspNetUserTokens");

            migrationBuilder.DropTable(
                name: "AspNetRoles");

            migrationBuilder.DropTable(
                name: "AppUser");
        }
    }
}

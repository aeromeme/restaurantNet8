using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace restaurantAPI.Migrations
{
    /// <inheritdoc />
    public partial class Fans : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateTable(
                name: "Fans",
                columns: table => new
                {
                    id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    Name = table.Column<string>(type: "varchar(100)", unicode: false, maxLength: 100, nullable: false),
                    Email = table.Column<string>(type: "varchar(100)", unicode: false, maxLength: 100, nullable: false),
                    YearsAsFan = table.Column<int>(type: "int", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK__Fans__E5E8A1BFAF3C9B2B", x => x.id);
                    table.CheckConstraint("CK_Fan_YearsAsFan_Range", "[YearsAsFan] >= 0 AND [YearsAsFan] <= 100");
                });
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "Fans");
        }
    }
}

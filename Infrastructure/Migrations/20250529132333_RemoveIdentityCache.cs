using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace Infrastructure.Migrations
{
    /// <inheritdoc />
    public partial class RemoveIdentityCache : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.Sql(
                sql: "ALTER DATABASE SCOPED CONFIGURATION SET IDENTITY_CACHE = OFF;",
                suppressTransaction: true);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.Sql(
                sql: "ALTER DATABASE SCOPED CONFIGURATION SET IDENTITY_CACHE = ON;",
                suppressTransaction: true);
        }
    }
}

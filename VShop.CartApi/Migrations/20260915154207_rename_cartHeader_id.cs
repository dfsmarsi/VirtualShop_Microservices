using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace VShop.CartApi.Migrations
{
    /// <inheritdoc />
    public partial class rename_cartHeader_id : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.Sql(@"
DO $$
BEGIN
    IF EXISTS (SELECT 1 FROM information_schema.columns WHERE table_name = 'CartHeaders' AND column_name = 'id') THEN
        ALTER TABLE ""CartHeaders"" RENAME COLUMN id TO ""Id"";
    END IF;
END $$;");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.Sql(@"
DO $$
BEGIN
    IF EXISTS (SELECT 1 FROM information_schema.columns WHERE table_name = 'CartHeaders' AND column_name = 'Id') THEN
        ALTER TABLE ""CartHeaders"" RENAME COLUMN ""Id"" TO id;
    END IF;
END $$;");
        }
    }
}

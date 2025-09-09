using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace Postly.WebAPI.Migrations
{
    /// <inheritdoc />
    public partial class mg1 : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateTable(
                name: "users",
                columns: table => new
                {
                    id = table.Column<Guid>(type: "uuid", nullable: false),
                    image_url = table.Column<string>(type: "text", nullable: false),
                    ad = table.Column<string>(type: "text", nullable: false),
                    soyad = table.Column<string>(type: "text", nullable: false),
                    email = table.Column<string>(type: "text", nullable: false),
                    sifre_hash = table.Column<string>(type: "text", nullable: false),
                    kayit_tarihi = table.Column<DateTime>(type: "timestamp with time zone", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("pk_users", x => x.id);
                });

            migrationBuilder.CreateTable(
                name: "shares",
                columns: table => new
                {
                    id = table.Column<Guid>(type: "uuid", nullable: false),
                    icerik = table.Column<string>(type: "text", nullable: false),
                    icerik_resim_url = table.Column<string>(type: "text", nullable: true),
                    user_id = table.Column<Guid>(type: "uuid", nullable: false),
                    paylasim_tarihi = table.Column<DateTime>(type: "timestamp with time zone", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("pk_shares", x => x.id);
                    table.ForeignKey(
                        name: "fk_shares_users_user_id",
                        column: x => x.user_id,
                        principalTable: "users",
                        principalColumn: "id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateIndex(
                name: "ix_shares_user_id",
                table: "shares",
                column: "user_id");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "shares");

            migrationBuilder.DropTable(
                name: "users");
        }
    }
}

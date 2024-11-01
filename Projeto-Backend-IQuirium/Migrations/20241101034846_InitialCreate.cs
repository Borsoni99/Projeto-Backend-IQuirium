using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace Projeto_Backend_IQuirium.Migrations
{
    /// <inheritdoc />
    public partial class InitialCreate : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateTable(
                name: "usuario1",
                columns: table => new
                {
                    Id = table.Column<Guid>(type: "uuid", nullable: false),
                    Nome = table.Column<string>(type: "character varying(128)", maxLength: 128, nullable: false),
                    Email = table.Column<string>(type: "character varying(128)", maxLength: 128, nullable: false),
                    Criado_em = table.Column<DateTime>(type: "timestamp with time zone", nullable: false, defaultValueSql: "now()")
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_usuario1", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "feedback1",
                columns: table => new
                {
                    Id = table.Column<Guid>(type: "uuid", nullable: false),
                    Id_usuario = table.Column<Guid>(type: "uuid", nullable: false),
                    Tipo_feedback = table.Column<int>(type: "integer", nullable: false),
                    Id_destinatario = table.Column<Guid>(type: "uuid", nullable: false),
                    Conteudo = table.Column<string>(type: "character varying(2048)", maxLength: 2048, nullable: false),
                    Criado_em = table.Column<DateTime>(type: "timestamp with time zone", nullable: false, defaultValueSql: "now()")
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_feedback1", x => x.Id);
                    table.ForeignKey(
                        name: "FK_feedback1_usuario1_Id_destinatario",
                        column: x => x.Id_destinatario,
                        principalTable: "usuario1",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_feedback1_usuario1_Id_usuario",
                        column: x => x.Id_usuario,
                        principalTable: "usuario1",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateIndex(
                name: "IX_feedback1_Id_destinatario",
                table: "feedback1",
                column: "Id_destinatario");

            migrationBuilder.CreateIndex(
                name: "IX_feedback1_Id_usuario",
                table: "feedback1",
                column: "Id_usuario");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "feedback1");

            migrationBuilder.DropTable(
                name: "usuario1");
        }
    }
}

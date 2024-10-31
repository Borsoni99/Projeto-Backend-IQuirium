using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace Projeto_Backend_IQuirium.Migrations
{
    /// <inheritdoc />
    public partial class createDatabase : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateTable(
                name: "usuario",
                columns: table => new
                {
                    Id = table.Column<Guid>(type: "uuid", nullable: false),
                    Nome = table.Column<string>(type: "character varying(128)", maxLength: 128, nullable: false),
                    Email = table.Column<string>(type: "character varying(128)", maxLength: 128, nullable: false),
                    Criado_em = table.Column<string>(type: "text", nullable: false, defaultValue: "10/30/2024 22:23:54")
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_usuario", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "feedback",
                columns: table => new
                {
                    Id = table.Column<Guid>(type: "uuid", nullable: false),
                    Id_usuario = table.Column<Guid>(type: "uuid", nullable: false),
                    Tipo_feedback = table.Column<int>(type: "integer", nullable: false),
                    Id_destinatario = table.Column<Guid>(type: "uuid", nullable: false),
                    Conteudo = table.Column<string>(type: "character varying(2048)", maxLength: 2048, nullable: false),
                    Criado_em = table.Column<DateTime>(type: "timestamp with time zone", nullable: false, defaultValue: new DateTime(2024, 10, 30, 22, 23, 54, 545, DateTimeKind.Local).AddTicks(7580)),
                    DestinatarioId = table.Column<Guid>(type: "uuid", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_feedback", x => x.Id);
                    table.ForeignKey(
                        name: "FK_feedback_usuario_DestinatarioId",
                        column: x => x.DestinatarioId,
                        principalTable: "usuario",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_feedback_usuario_Id_usuario",
                        column: x => x.Id_usuario,
                        principalTable: "usuario",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "status_feedback",
                columns: table => new
                {
                    Id = table.Column<Guid>(type: "uuid", nullable: false),
                    Id_feedback = table.Column<Guid>(type: "uuid", nullable: false),
                    Status = table.Column<string>(type: "text", nullable: false),
                    Atualizado_em = table.Column<DateTime>(type: "timestamp with time zone", nullable: false, defaultValue: new DateTime(2024, 10, 30, 22, 23, 54, 545, DateTimeKind.Local).AddTicks(9511))
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_status_feedback", x => x.Id);
                    table.ForeignKey(
                        name: "FK_status_feedback_feedback_Id_feedback",
                        column: x => x.Id_feedback,
                        principalTable: "feedback",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateIndex(
                name: "IX_feedback_DestinatarioId",
                table: "feedback",
                column: "DestinatarioId");

            migrationBuilder.CreateIndex(
                name: "IX_feedback_Id_usuario",
                table: "feedback",
                column: "Id_usuario");

            migrationBuilder.CreateIndex(
                name: "IX_status_feedback_Id_feedback",
                table: "status_feedback",
                column: "Id_feedback");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "status_feedback");

            migrationBuilder.DropTable(
                name: "feedback");

            migrationBuilder.DropTable(
                name: "usuario");
        }
    }
}

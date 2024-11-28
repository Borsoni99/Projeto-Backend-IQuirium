using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace Projeto_Backend_IQuirium.Migrations
{
    /// <inheritdoc />
    public partial class createDatabase2 : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "feedback");

            migrationBuilder.AlterColumn<string>(
                name: "Nome",
                table: "usuario",
                type: "character varying(255)",
                maxLength: 255,
                nullable: false,
                oldClrType: typeof(string),
                oldType: "character varying(128)",
                oldMaxLength: 128);

            migrationBuilder.AlterColumn<string>(
                name: "Email",
                table: "usuario",
                type: "character varying(255)",
                maxLength: 255,
                nullable: false,
                oldClrType: typeof(string),
                oldType: "character varying(128)",
                oldMaxLength: 128);

            migrationBuilder.CreateTable(
                name: "feedback_produto",
                columns: table => new
                {
                    Id = table.Column<Guid>(type: "uuid", nullable: false),
                    Id_usuario = table.Column<Guid>(type: "uuid", nullable: false),
                    Tipo_feedback = table.Column<int>(type: "integer", maxLength: 100, nullable: false),
                    Conteudo = table.Column<string>(type: "character varying(2048)", maxLength: 2048, nullable: false),
                    Criado_em = table.Column<DateTime>(type: "timestamp with time zone", nullable: false, defaultValueSql: "now()")
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_feedback_produto", x => x.Id);
                    table.ForeignKey(
                        name: "FK_feedback_produto_usuario_Id_usuario",
                        column: x => x.Id_usuario,
                        principalTable: "usuario",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "feedback_usuario",
                columns: table => new
                {
                    Id = table.Column<Guid>(type: "uuid", nullable: false),
                    RemetenteId = table.Column<Guid>(type: "uuid", nullable: false),
                    DestinatarioId = table.Column<Guid>(type: "uuid", nullable: false),
                    Tipo = table.Column<int>(type: "integer", maxLength: 100, nullable: false),
                    ConteudoFeedback = table.Column<string>(type: "character varying(2048)", maxLength: 2048, nullable: false),
                    DataHoraEnvio = table.Column<DateTime>(type: "timestamp with time zone", nullable: false, defaultValueSql: "now()"),
                    Status = table.Column<int>(type: "integer", maxLength: 255, nullable: false),
                    Motivo = table.Column<string>(type: "character varying(1024)", maxLength: 1024, nullable: false),
                    ConteudoReport = table.Column<string>(type: "character varying(1024)", maxLength: 1024, nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_feedback_usuario", x => x.Id);
                    table.ForeignKey(
                        name: "FK_feedback_usuario_usuario_DestinatarioId",
                        column: x => x.DestinatarioId,
                        principalTable: "usuario",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_feedback_usuario_usuario_RemetenteId",
                        column: x => x.RemetenteId,
                        principalTable: "usuario",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateIndex(
                name: "IX_feedback_produto_Id_usuario",
                table: "feedback_produto",
                column: "Id_usuario");

            migrationBuilder.CreateIndex(
                name: "IX_feedback_usuario_DestinatarioId",
                table: "feedback_usuario",
                column: "DestinatarioId");

            migrationBuilder.CreateIndex(
                name: "IX_feedback_usuario_RemetenteId",
                table: "feedback_usuario",
                column: "RemetenteId");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "feedback_produto");

            migrationBuilder.DropTable(
                name: "feedback_usuario");

            migrationBuilder.AlterColumn<string>(
                name: "Nome",
                table: "usuario",
                type: "character varying(128)",
                maxLength: 128,
                nullable: false,
                oldClrType: typeof(string),
                oldType: "character varying(255)",
                oldMaxLength: 255);

            migrationBuilder.AlterColumn<string>(
                name: "Email",
                table: "usuario",
                type: "character varying(128)",
                maxLength: 128,
                nullable: false,
                oldClrType: typeof(string),
                oldType: "character varying(255)",
                oldMaxLength: 255);

            migrationBuilder.CreateTable(
                name: "feedback",
                columns: table => new
                {
                    Id = table.Column<Guid>(type: "uuid", nullable: false),
                    Id_usuario = table.Column<Guid>(type: "uuid", nullable: false),
                    Conteudo = table.Column<string>(type: "character varying(2048)", maxLength: 2048, nullable: false),
                    Criado_em = table.Column<DateTime>(type: "timestamp with time zone", nullable: false, defaultValueSql: "now()"),
                    Tipo_feedback = table.Column<int>(type: "integer", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_feedback", x => x.Id);
                    table.ForeignKey(
                        name: "FK_feedback_usuario_Id_usuario",
                        column: x => x.Id_usuario,
                        principalTable: "usuario",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateIndex(
                name: "IX_feedback_Id_usuario",
                table: "feedback",
                column: "Id_usuario");
        }
    }
}

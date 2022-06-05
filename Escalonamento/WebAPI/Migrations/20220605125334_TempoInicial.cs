using Microsoft.EntityFrameworkCore.Migrations;

namespace Escalonamento.Migrations
{
    public partial class TempoInicial : Migration
    {
        /*protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateTable(
                name: "conexao",
                columns: table => new
                {
                    IdUser = table.Column<int>(type: "int", nullable: false),
                    IdSim = table.Column<int>(type: "int", nullable: false),
                    IdJob = table.Column<int>(type: "int", nullable: false),
                    IdOp = table.Column<int>(type: "int", nullable: false),
                    IdMaq = table.Column<int>(type: "int", nullable: true),
                    Duracao = table.Column<int>(type: "int", nullable: true),
                    estado = table.Column<bool>(type: "bit", nullable: true),
                    Tempo_Inicial = table.Column<int>(type: "int", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("pk_conexao", x => new { x.IdUser, x.IdSim, x.IdJob, x.IdOp });
                });

            migrationBuilder.CreateTable(
                name: "maquina",
                columns: table => new
                {
                    id_maq = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    estado = table.Column<string>(type: "varchar(15)", unicode: false, maxLength: 15, nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("pk_id_maq", x => x.id_maq);
                });

            migrationBuilder.CreateTable(
                name: "utilizador",
                columns: table => new
                {
                    id_user = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    mail = table.Column<string>(type: "varchar(30)", unicode: false, maxLength: 30, nullable: false),
                    aut = table.Column<bool>(type: "bit", nullable: true),
                    pass_salt = table.Column<string>(type: "varchar(max)", unicode: false, nullable: false),
                    pass_hash = table.Column<string>(type: "varchar(max)", unicode: false, nullable: false),
                    estado = table.Column<string>(type: "varchar(15)", unicode: false, maxLength: 15, nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("pk_id_user", x => x.id_user);
                });

            migrationBuilder.CreateIndex(
                name: "uk_mail",
                table: "utilizador",
                column: "mail",
                unique: true);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "conexao");

            migrationBuilder.DropTable(
                name: "maquina");

            migrationBuilder.DropTable(
                name: "utilizador");
        }*/

        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<int>(
                name: "TempoInicial",
                table: "Conexao",
                nullable: true);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "TempoInicial",
                table: "Conexao");
        }
    }
}

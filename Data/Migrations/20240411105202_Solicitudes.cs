using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace SistemaChequesNuevo.Data.Migrations
{
    public partial class Solicitudes : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_SolicitudCheques_Proveedores_ProveedorId",
                table: "SolicitudCheques");

            migrationBuilder.AlterColumn<int>(
                name: "ProveedorId",
                table: "SolicitudCheques",
                type: "int",
                nullable: true,
                oldClrType: typeof(int),
                oldType: "int");

            migrationBuilder.CreateTable(
                name: "Solicitudes",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    Descripcion = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    Estado = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    NumeroCheque = table.Column<int>(type: "int", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Solicitudes", x => x.Id);
                });

            migrationBuilder.AddForeignKey(
                name: "FK_SolicitudCheques_Proveedores_ProveedorId",
                table: "SolicitudCheques",
                column: "ProveedorId",
                principalTable: "Proveedores",
                principalColumn: "Id");
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_SolicitudCheques_Proveedores_ProveedorId",
                table: "SolicitudCheques");

            migrationBuilder.DropTable(
                name: "Solicitudes");

            migrationBuilder.AlterColumn<int>(
                name: "ProveedorId",
                table: "SolicitudCheques",
                type: "int",
                nullable: false,
                defaultValue: 0,
                oldClrType: typeof(int),
                oldType: "int",
                oldNullable: true);

            migrationBuilder.AddForeignKey(
                name: "FK_SolicitudCheques_Proveedores_ProveedorId",
                table: "SolicitudCheques",
                column: "ProveedorId",
                principalTable: "Proveedores",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);
        }
    }
}

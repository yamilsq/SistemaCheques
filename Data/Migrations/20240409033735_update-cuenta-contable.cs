using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace SistemaChequesNuevo.Data.Migrations
{
    public partial class updatecuentacontable : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            //migrationBuilder.DropForeignKey(
            //    name: "FK_SolicitudCheques_Proveedores_ProveedorId",
            //    table: "SolicitudCheques");

            //migrationBuilder.AlterColumn<int>(
            //    name: "ProveedorId",
            //    table: "SolicitudCheques",
            //    type: "int",
            //    nullable: false,
            //    defaultValue: 0,
            //    oldClrType: typeof(int),
            //    oldType: "int",
            //    oldNullable: true);

            migrationBuilder.AlterColumn<int>(
                name: "CuentaContable",
                table: "SolicitudCheques",
                type: "int",
                nullable: true,
                oldClrType: typeof(string),
                oldType: "nvarchar(max)",
                oldNullable: true);

            //migrationBuilder.AddForeignKey(
            //    name: "FK_SolicitudCheques_Proveedores_ProveedorId",
            //    table: "SolicitudCheques",
            //    column: "ProveedorId",
            //    principalTable: "Proveedores",
            //    principalColumn: "Id",
            //    onDelete: ReferentialAction.Cascade);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
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

            migrationBuilder.AlterColumn<string>(
                name: "CuentaContable",
                table: "SolicitudCheques",
                type: "nvarchar(max)",
                nullable: true,
                oldClrType: typeof(int),
                oldType: "int",
                oldNullable: true);

            migrationBuilder.AddForeignKey(
                name: "FK_SolicitudCheques_Proveedores_ProveedorId",
                table: "SolicitudCheques",
                column: "ProveedorId",
                principalTable: "Proveedores",
                principalColumn: "Id");
        }
    }
}

using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace SistemaChequesNuevo.Data.Migrations
{
    public partial class updatingprovidercuentacontable : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            //    migrationBuilder.DropForeignKey(
            //        name: "FK_SolicitudCheques_Proveedores_ProveedorId",
            //        table: "SolicitudCheques");

            //    migrationBuilder.AlterColumn<int>(
            //        name: "ProveedorId",
            //        table: "SolicitudCheques",
            //        type: "int",
            //        nullable: true,
            //        oldClrType: typeof(int),
            //        oldType: "int");

            //    migrationBuilder.AlterColumn<string>(
            //        name: "Estado",
            //        table: "Proveedores",
            //        type: "nvarchar(max)",
            //        nullable: false,
            //        defaultValue: "",
            //        oldClrType: typeof(string),
            //        oldType: "nvarchar(max)",
            //        oldNullable: true);

            //    migrationBuilder.AlterColumn<string>(
            //        name: "DocumentoIdentificador",
            //        table: "Proveedores",
            //        type: "nvarchar(max)",
            //        nullable: false,
            //        defaultValue: "",
            //        oldClrType: typeof(string),
            //        oldType: "nvarchar(max)",
            //        oldNullable: true);

            migrationBuilder.AlterColumn<int>(
                name: "CuentaContable",
                table: "Proveedores",
                type: "int",
                nullable: false,
                defaultValue: 0,
                oldClrType: typeof(string),
                oldType: "nvarchar(max)",
                oldNullable: true);

            //migrationBuilder.AlterColumn<decimal>(
            //    name: "Balance",
            //    table: "Proveedores",
            //    type: "decimal(18,2)",
            //    nullable: false,
            //    defaultValue: 0m,
            //    oldClrType: typeof(decimal),
            //    oldType: "decimal(18,2)",
            //    oldNullable: true);

            //migrationBuilder.AddForeignKey(
            //    name: "FK_SolicitudCheques_Proveedores_ProveedorId",
            //    table: "SolicitudCheques",
            //    column: "ProveedorId",
            //    principalTable: "Proveedores",
            //    principalColumn: "Id");
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
                nullable: false,
                defaultValue: 0,
                oldClrType: typeof(int),
                oldType: "int",
                oldNullable: true);

            migrationBuilder.AlterColumn<string>(
                name: "Estado",
                table: "Proveedores",
                type: "nvarchar(max)",
                nullable: true,
                oldClrType: typeof(string),
                oldType: "nvarchar(max)");

            migrationBuilder.AlterColumn<string>(
                name: "DocumentoIdentificador",
                table: "Proveedores",
                type: "nvarchar(max)",
                nullable: true,
                oldClrType: typeof(string),
                oldType: "nvarchar(max)");

            migrationBuilder.AlterColumn<string>(
                name: "CuentaContable",
                table: "Proveedores",
                type: "nvarchar(max)",
                nullable: true,
                oldClrType: typeof(int),
                oldType: "int");

            migrationBuilder.AlterColumn<decimal>(
                name: "Balance",
                table: "Proveedores",
                type: "decimal(18,2)",
                nullable: true,
                oldClrType: typeof(decimal),
                oldType: "decimal(18,2)");

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
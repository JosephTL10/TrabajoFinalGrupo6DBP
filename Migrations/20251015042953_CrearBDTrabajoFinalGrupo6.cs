using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace TrabajoFinalGrupo6DBP.Migrations
{
    /// <inheritdoc />
    public partial class CrearBDTrabajoFinalGrupo6 : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateTable(
                name: "Pacientes",
                columns: table => new
                {
                    Id_Paciente = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    DNI_Paciente = table.Column<int>(type: "int", nullable: false),
                    Nombre_Completo_Paciente = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    Fecha_Nacimiento_Paciente = table.Column<DateTime>(type: "datetime2", nullable: false),
                    Telefono_Paciente = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    Direccion_Paciente = table.Column<string>(type: "nvarchar(max)", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Pacientes", x => x.Id_Paciente);
                });

            migrationBuilder.CreateTable(
                name: "Usuarios",
                columns: table => new
                {
                    Id_Usuario = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    DNI_Usuario = table.Column<int>(type: "int", nullable: false),
                    Nombres_Usuario = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    Apellidos_Usuario = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    Telefono_Usuario = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    Correo_Electronico_Usuario = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    Contrasenia_Usuario = table.Column<string>(type: "nvarchar(max)", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Usuarios", x => x.Id_Usuario);
                });

            migrationBuilder.CreateTable(
                name: "Citas_Medicas",
                columns: table => new
                {
                    Id_CitaMedica = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    PacienteId = table.Column<int>(type: "int", nullable: false),
                    Fecha_CitaMedica = table.Column<DateTime>(type: "datetime2", nullable: false),
                    Hora_CitaMedica = table.Column<TimeOnly>(type: "time", nullable: false),
                    Especialidad = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    Medico = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    Observaciones = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    Estado_CitaMedica = table.Column<string>(type: "nvarchar(max)", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Citas_Medicas", x => x.Id_CitaMedica);
                    table.ForeignKey(
                        name: "FK_Citas_Medicas_Pacientes_PacienteId",
                        column: x => x.PacienteId,
                        principalTable: "Pacientes",
                        principalColumn: "Id_Paciente",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateIndex(
                name: "IX_Citas_Medicas_PacienteId",
                table: "Citas_Medicas",
                column: "PacienteId");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "Citas_Medicas");

            migrationBuilder.DropTable(
                name: "Usuarios");

            migrationBuilder.DropTable(
                name: "Pacientes");
        }
    }
}

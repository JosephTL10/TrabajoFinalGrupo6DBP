using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace TrabajoFinalGrupo6DBP.Migrations
{
    /// <inheritdoc />
    public partial class CrearBDTrabajoFinalGrupo6DBP : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateTable(
                name: "Medicos",
                columns: table => new
                {
                    Id_Medico = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    DNI_Medico = table.Column<int>(type: "int", nullable: false),
                    Nombre_Completo_Medico = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    Especialidad = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    Telefono_Medico = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    Correo_Medico = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    Estado_Medico = table.Column<bool>(type: "bit", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Medicos", x => x.Id_Medico);
                });

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
                name: "Horarios_Medicos",
                columns: table => new
                {
                    Id_Horario = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    MedicoId = table.Column<int>(type: "int", nullable: false),
                    DiaSemana = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    Hora_Inicio = table.Column<TimeSpan>(type: "time", nullable: false),
                    Hora_Fin = table.Column<TimeSpan>(type: "time", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Horarios_Medicos", x => x.Id_Horario);
                    table.ForeignKey(
                        name: "FK_Horarios_Medicos_Medicos_MedicoId",
                        column: x => x.MedicoId,
                        principalTable: "Medicos",
                        principalColumn: "Id_Medico",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "Citas_Medicas",
                columns: table => new
                {
                    Id_CitaMedica = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    PacienteId = table.Column<int>(type: "int", nullable: false),
                    MedicoId = table.Column<int>(type: "int", nullable: false),
                    Fecha_CitaMedica = table.Column<DateTime>(type: "datetime2", nullable: false),
                    Hora_CitaMedica = table.Column<TimeSpan>(type: "time", nullable: false),
                    Especialidad = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    Observaciones = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    Estado_CitaMedica = table.Column<string>(type: "nvarchar(max)", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Citas_Medicas", x => x.Id_CitaMedica);
                    table.ForeignKey(
                        name: "FK_Citas_Medicas_Medicos_MedicoId",
                        column: x => x.MedicoId,
                        principalTable: "Medicos",
                        principalColumn: "Id_Medico",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_Citas_Medicas_Pacientes_PacienteId",
                        column: x => x.PacienteId,
                        principalTable: "Pacientes",
                        principalColumn: "Id_Paciente",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateIndex(
                name: "IX_Citas_Medicas_MedicoId",
                table: "Citas_Medicas",
                column: "MedicoId");

            migrationBuilder.CreateIndex(
                name: "IX_Citas_Medicas_PacienteId",
                table: "Citas_Medicas",
                column: "PacienteId");

            migrationBuilder.CreateIndex(
                name: "IX_Horarios_Medicos_MedicoId",
                table: "Horarios_Medicos",
                column: "MedicoId");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "Citas_Medicas");

            migrationBuilder.DropTable(
                name: "Horarios_Medicos");

            migrationBuilder.DropTable(
                name: "Usuarios");

            migrationBuilder.DropTable(
                name: "Pacientes");

            migrationBuilder.DropTable(
                name: "Medicos");
        }
    }
}

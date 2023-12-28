using System;
using Microsoft.EntityFrameworkCore.Migrations;
using Npgsql.EntityFrameworkCore.PostgreSQL.Metadata;

#nullable disable

namespace FlightGearApi.Migrations
{
    /// <inheritdoc />
    public partial class InitialCreate : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateTable(
                name: "flight_sessions",
                columns: table => new
                {
                    id = table.Column<int>(type: "integer", nullable: false)
                        .Annotation("Npgsql:ValueGenerationStrategy", NpgsqlValueGenerationStrategy.IdentityByDefaultColumn),
                    title = table.Column<string>(type: "text", nullable: false),
                    duration = table.Column<int>(type: "integer", nullable: false),
                    date = table.Column<DateTime>(type: "timestamp with time zone", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_flight_sessions", x => x.id);
                });

            migrationBuilder.CreateTable(
                name: "flight_properties_shots",
                columns: table => new
                {
                    id = table.Column<int>(type: "integer", nullable: false)
                        .Annotation("Npgsql:ValueGenerationStrategy", NpgsqlValueGenerationStrategy.IdentityByDefaultColumn),
                    flight_session_id = table.Column<int>(type: "integer", nullable: false),
                    order = table.Column<int>(type: "integer", nullable: false),
                    Longitude = table.Column<double>(type: "double precision", nullable: false),
                    Latitude = table.Column<double>(type: "double precision", nullable: false),
                    AltitudeAgl = table.Column<double>(type: "double precision", nullable: false),
                    Altitude = table.Column<double>(type: "double precision", nullable: false),
                    AltitudeIndicatedBaro = table.Column<double>(type: "double precision", nullable: false),
                    AltitudeAbsoluteBaro = table.Column<double>(type: "double precision", nullable: false),
                    Roll = table.Column<double>(type: "double precision", nullable: false),
                    Pitch = table.Column<double>(type: "double precision", nullable: false),
                    Heading = table.Column<double>(type: "double precision", nullable: false),
                    HeadingMagnetic = table.Column<double>(type: "double precision", nullable: false),
                    HeadingMagneticIndicated = table.Column<double>(type: "double precision", nullable: false),
                    IndicatedSpeed = table.Column<double>(type: "double precision", nullable: false),
                    Airspeed = table.Column<double>(type: "double precision", nullable: false),
                    VerticalBaroSpeed = table.Column<double>(type: "double precision", nullable: false),
                    Mach = table.Column<double>(type: "double precision", nullable: false),
                    UBodyMps = table.Column<double>(type: "double precision", nullable: false),
                    VBodyMps = table.Column<double>(type: "double precision", nullable: false),
                    WBodyMps = table.Column<double>(type: "double precision", nullable: false),
                    SideOverload = table.Column<double>(type: "double precision", nullable: false),
                    PilotOverload = table.Column<double>(type: "double precision", nullable: false),
                    AccelerationY = table.Column<double>(type: "double precision", nullable: false),
                    AccelerationX = table.Column<double>(type: "double precision", nullable: false),
                    AccelerationNormal = table.Column<double>(type: "double precision", nullable: false),
                    Temperature = table.Column<double>(type: "double precision", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_flight_properties_shots", x => x.id);
                    table.ForeignKey(
                        name: "FK_flight_properties_shots_flight_sessions_flight_session_id",
                        column: x => x.flight_session_id,
                        principalTable: "flight_sessions",
                        principalColumn: "id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateIndex(
                name: "IX_flight_properties_shots_flight_session_id",
                table: "flight_properties_shots",
                column: "flight_session_id");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "flight_properties_shots");

            migrationBuilder.DropTable(
                name: "flight_sessions");
        }
    }
}

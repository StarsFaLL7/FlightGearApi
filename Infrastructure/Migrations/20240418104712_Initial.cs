using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace Infrastructure.Migrations
{
    /// <inheritdoc />
    public partial class Initial : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateTable(
                name: "Airports",
                columns: table => new
                {
                    Id = table.Column<Guid>(type: "uuid", nullable: false),
                    Title = table.Column<string>(type: "text", nullable: false),
                    Code = table.Column<string>(type: "text", nullable: false),
                    City = table.Column<string>(type: "text", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Airports", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "FlightFunctions",
                columns: table => new
                {
                    Id = table.Column<Guid>(type: "uuid", nullable: false),
                    Description = table.Column<string>(type: "text", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_FlightFunctions", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "FlightSessions",
                columns: table => new
                {
                    Id = table.Column<Guid>(type: "uuid", nullable: false),
                    Title = table.Column<string>(type: "text", nullable: false),
                    PropertiesReadsPerSec = table.Column<int>(type: "integer", nullable: false),
                    DateTimeStart = table.Column<DateTime>(type: "timestamp with time zone", nullable: false),
                    DurationSec = table.Column<int>(type: "integer", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_FlightSessions", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "AirportRunways",
                columns: table => new
                {
                    Id = table.Column<Guid>(type: "uuid", nullable: false),
                    Title = table.Column<string>(type: "text", nullable: false),
                    AirportId = table.Column<Guid>(type: "uuid", nullable: false),
                    DepartureFunctionId = table.Column<Guid>(type: "uuid", nullable: false),
                    ArrivalFunctionId = table.Column<Guid>(type: "uuid", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_AirportRunways", x => x.Id);
                    table.ForeignKey(
                        name: "FK_AirportRunways_Airports_AirportId",
                        column: x => x.AirportId,
                        principalTable: "Airports",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_AirportRunways_FlightFunctions_ArrivalFunctionId",
                        column: x => x.ArrivalFunctionId,
                        principalTable: "FlightFunctions",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_AirportRunways_FlightFunctions_DepartureFunctionId",
                        column: x => x.DepartureFunctionId,
                        principalTable: "FlightFunctions",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "FunctionPoints",
                columns: table => new
                {
                    Id = table.Column<Guid>(type: "uuid", nullable: false),
                    Order = table.Column<int>(type: "integer", nullable: false),
                    Longitude = table.Column<double>(type: "double precision", nullable: false),
                    Latitude = table.Column<double>(type: "double precision", nullable: false),
                    Altitude = table.Column<double>(type: "double precision", nullable: false),
                    Speed = table.Column<double>(type: "double precision", nullable: false),
                    Remarks = table.Column<string>(type: "text", nullable: true),
                    FunctionId = table.Column<Guid>(type: "uuid", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_FunctionPoints", x => x.Id);
                    table.ForeignKey(
                        name: "FK_FunctionPoints_FlightFunctions_FunctionId",
                        column: x => x.FunctionId,
                        principalTable: "FlightFunctions",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "FlightPropertiesShots",
                columns: table => new
                {
                    Id = table.Column<Guid>(type: "uuid", nullable: false),
                    Order = table.Column<int>(type: "integer", nullable: false),
                    FlightSessionId = table.Column<Guid>(type: "uuid", nullable: false),
                    Longitude = table.Column<double>(type: "double precision", nullable: false),
                    Latitude = table.Column<double>(type: "double precision", nullable: false),
                    AltitudeAgl = table.Column<double>(type: "double precision", nullable: false),
                    Altitude = table.Column<double>(type: "double precision", nullable: false),
                    AltitudeIndicatedBaro = table.Column<double>(type: "double precision", nullable: false),
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
                    PilotOverload = table.Column<double>(type: "double precision", nullable: false),
                    AccelerationY = table.Column<double>(type: "double precision", nullable: false),
                    AccelerationX = table.Column<double>(type: "double precision", nullable: false),
                    AccelerationNormal = table.Column<double>(type: "double precision", nullable: false),
                    Temperature = table.Column<double>(type: "double precision", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_FlightPropertiesShots", x => x.Id);
                    table.ForeignKey(
                        name: "FK_FlightPropertiesShots_FlightSessions_FlightSessionId",
                        column: x => x.FlightSessionId,
                        principalTable: "FlightSessions",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "FlightPlans",
                columns: table => new
                {
                    Id = table.Column<Guid>(type: "uuid", nullable: false),
                    Title = table.Column<string>(type: "text", nullable: false),
                    Remarks = table.Column<string>(type: "text", nullable: false),
                    DepartureRunwayId = table.Column<Guid>(type: "uuid", nullable: true),
                    ArrivalRunwayId = table.Column<Guid>(type: "uuid", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_FlightPlans", x => x.Id);
                    table.ForeignKey(
                        name: "FK_FlightPlans_AirportRunways_ArrivalRunwayId",
                        column: x => x.ArrivalRunwayId,
                        principalTable: "AirportRunways",
                        principalColumn: "Id");
                    table.ForeignKey(
                        name: "FK_FlightPlans_AirportRunways_DepartureRunwayId",
                        column: x => x.DepartureRunwayId,
                        principalTable: "AirportRunways",
                        principalColumn: "Id");
                });

            migrationBuilder.CreateTable(
                name: "RoutePoints",
                columns: table => new
                {
                    Id = table.Column<Guid>(type: "uuid", nullable: false),
                    Order = table.Column<int>(type: "integer", nullable: false),
                    Longitude = table.Column<double>(type: "double precision", nullable: false),
                    Latitude = table.Column<double>(type: "double precision", nullable: false),
                    Altitude = table.Column<double>(type: "double precision", nullable: false),
                    Remarks = table.Column<string>(type: "text", nullable: true),
                    FlightPlanId = table.Column<Guid>(type: "uuid", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_RoutePoints", x => x.Id);
                    table.ForeignKey(
                        name: "FK_RoutePoints_FlightPlans_FlightPlanId",
                        column: x => x.FlightPlanId,
                        principalTable: "FlightPlans",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateIndex(
                name: "IX_AirportRunways_AirportId",
                table: "AirportRunways",
                column: "AirportId");

            migrationBuilder.CreateIndex(
                name: "IX_AirportRunways_ArrivalFunctionId",
                table: "AirportRunways",
                column: "ArrivalFunctionId");

            migrationBuilder.CreateIndex(
                name: "IX_AirportRunways_DepartureFunctionId",
                table: "AirportRunways",
                column: "DepartureFunctionId");

            migrationBuilder.CreateIndex(
                name: "IX_FlightPlans_ArrivalRunwayId",
                table: "FlightPlans",
                column: "ArrivalRunwayId");

            migrationBuilder.CreateIndex(
                name: "IX_FlightPlans_DepartureRunwayId",
                table: "FlightPlans",
                column: "DepartureRunwayId");

            migrationBuilder.CreateIndex(
                name: "IX_FlightPropertiesShots_FlightSessionId",
                table: "FlightPropertiesShots",
                column: "FlightSessionId");

            migrationBuilder.CreateIndex(
                name: "IX_FunctionPoints_FunctionId",
                table: "FunctionPoints",
                column: "FunctionId");

            migrationBuilder.CreateIndex(
                name: "IX_RoutePoints_FlightPlanId",
                table: "RoutePoints",
                column: "FlightPlanId");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "FlightPropertiesShots");

            migrationBuilder.DropTable(
                name: "FunctionPoints");

            migrationBuilder.DropTable(
                name: "RoutePoints");

            migrationBuilder.DropTable(
                name: "FlightSessions");

            migrationBuilder.DropTable(
                name: "FlightPlans");

            migrationBuilder.DropTable(
                name: "AirportRunways");

            migrationBuilder.DropTable(
                name: "Airports");

            migrationBuilder.DropTable(
                name: "FlightFunctions");
        }
    }
}

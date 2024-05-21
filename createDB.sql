CREATE TABLE IF NOT EXISTS "__EFMigrationsHistory" (
    "MigrationId" character varying(150) NOT NULL,
    "ProductVersion" character varying(32) NOT NULL,
    CONSTRAINT "PK___EFMigrationsHistory" PRIMARY KEY ("MigrationId")
);

START TRANSACTION;

CREATE TABLE "Airports" (
    "Id" uuid NOT NULL,
    "Title" text NOT NULL,
    "Code" text NOT NULL,
    "City" text NOT NULL,
    CONSTRAINT "PK_Airports" PRIMARY KEY ("Id")
);

CREATE TABLE "FlightFunctions" (
    "Id" uuid NOT NULL,
    "Description" text,
    CONSTRAINT "PK_FlightFunctions" PRIMARY KEY ("Id")
);

CREATE TABLE "FlightSessions" (
    "Id" uuid NOT NULL,
    "Title" text NOT NULL,
    "PropertiesReadsPerSec" integer NOT NULL,
    "DateTimeStart" timestamp with time zone NOT NULL,
    "DurationSec" integer NOT NULL,
    CONSTRAINT "PK_FlightSessions" PRIMARY KEY ("Id")
);

CREATE TABLE "AirportRunways" (
    "Id" uuid NOT NULL,
    "Title" text NOT NULL,
    "AirportId" uuid NOT NULL,
    "DepartureFunctionId" uuid,
    "ArrivalFunctionId" uuid,
    CONSTRAINT "PK_AirportRunways" PRIMARY KEY ("Id"),
    CONSTRAINT "FK_AirportRunways_Airports_AirportId" FOREIGN KEY ("AirportId") REFERENCES "Airports" ("Id") ON DELETE CASCADE,
    CONSTRAINT "FK_AirportRunways_FlightFunctions_ArrivalFunctionId" FOREIGN KEY ("ArrivalFunctionId") REFERENCES "FlightFunctions" ("Id") ON DELETE CASCADE,
    CONSTRAINT "FK_AirportRunways_FlightFunctions_DepartureFunctionId" FOREIGN KEY ("DepartureFunctionId") REFERENCES "FlightFunctions" ("Id") ON DELETE CASCADE
);

CREATE TABLE "FunctionPoints" (
    "Id" uuid NOT NULL,
    "Order" integer NOT NULL,
    "Longitude" double precision NOT NULL,
    "Latitude" double precision NOT NULL,
    "Altitude" double precision NOT NULL,
    "Speed" double precision NOT NULL,
    "Remarks" text,
    "FunctionId" uuid NOT NULL,
    CONSTRAINT "PK_FunctionPoints" PRIMARY KEY ("Id"),
    CONSTRAINT "FK_FunctionPoints_FlightFunctions_FunctionId" FOREIGN KEY ("FunctionId") REFERENCES "FlightFunctions" ("Id") ON DELETE CASCADE
);

CREATE TABLE "FlightPropertiesShots" (
    "Id" uuid NOT NULL,
    "Order" integer NOT NULL,
    "FlightSessionId" uuid NOT NULL,
    "Longitude" double precision NOT NULL,
    "Latitude" double precision NOT NULL,
    "AltitudeAgl" double precision NOT NULL,
    "Altitude" double precision NOT NULL,
    "AltitudeIndicatedBaro" double precision NOT NULL,
    "Roll" double precision NOT NULL,
    "Pitch" double precision NOT NULL,
    "Heading" double precision NOT NULL,
    "HeadingMagnetic" double precision NOT NULL,
    "HeadingMagneticIndicated" double precision NOT NULL,
    "IndicatedSpeed" double precision NOT NULL,
    "Airspeed" double precision NOT NULL,
    "VerticalBaroSpeed" double precision NOT NULL,
    "Mach" double precision NOT NULL,
    "UBodyMps" double precision NOT NULL,
    "VBodyMps" double precision NOT NULL,
    "WBodyMps" double precision NOT NULL,
    "PilotOverload" double precision NOT NULL,
    "AccelerationY" double precision NOT NULL,
    "AccelerationX" double precision NOT NULL,
    "AccelerationNormal" double precision NOT NULL,
    "Temperature" double precision NOT NULL,
    CONSTRAINT "PK_FlightPropertiesShots" PRIMARY KEY ("Id"),
    CONSTRAINT "FK_FlightPropertiesShots_FlightSessions_FlightSessionId" FOREIGN KEY ("FlightSessionId") REFERENCES "FlightSessions" ("Id") ON DELETE CASCADE
);

CREATE TABLE "FlightPlans" (
    "Id" uuid NOT NULL,
    "Title" text NOT NULL,
    "Remarks" text NOT NULL,
    "DepartureRunwayId" uuid,
    "ArrivalRunwayId" uuid,
    CONSTRAINT "PK_FlightPlans" PRIMARY KEY ("Id"),
    CONSTRAINT "FK_FlightPlans_AirportRunways_ArrivalRunwayId" FOREIGN KEY ("ArrivalRunwayId") REFERENCES "AirportRunways" ("Id"),
    CONSTRAINT "FK_FlightPlans_AirportRunways_DepartureRunwayId" FOREIGN KEY ("DepartureRunwayId") REFERENCES "AirportRunways" ("Id")
);

CREATE TABLE "RoutePoints" (
    "Id" uuid NOT NULL,
    "Order" integer NOT NULL,
    "Longitude" double precision NOT NULL,
    "Latitude" double precision NOT NULL,
    "Altitude" double precision NOT NULL,
    "Remarks" text,
    "FlightPlanId" uuid NOT NULL,
    CONSTRAINT "PK_RoutePoints" PRIMARY KEY ("Id"),
    CONSTRAINT "FK_RoutePoints_FlightPlans_FlightPlanId" FOREIGN KEY ("FlightPlanId") REFERENCES "FlightPlans" ("Id") ON DELETE CASCADE
);

CREATE INDEX "IX_AirportRunways_AirportId" ON "AirportRunways" ("AirportId");

CREATE INDEX "IX_AirportRunways_ArrivalFunctionId" ON "AirportRunways" ("ArrivalFunctionId");

CREATE INDEX "IX_AirportRunways_DepartureFunctionId" ON "AirportRunways" ("DepartureFunctionId");

CREATE INDEX "IX_FlightPlans_ArrivalRunwayId" ON "FlightPlans" ("ArrivalRunwayId");

CREATE INDEX "IX_FlightPlans_DepartureRunwayId" ON "FlightPlans" ("DepartureRunwayId");

CREATE INDEX "IX_FlightPropertiesShots_FlightSessionId" ON "FlightPropertiesShots" ("FlightSessionId");

CREATE INDEX "IX_FunctionPoints_FunctionId" ON "FunctionPoints" ("FunctionId");

CREATE INDEX "IX_RoutePoints_FlightPlanId" ON "RoutePoints" ("FlightPlanId");

INSERT INTO "__EFMigrationsHistory" ("MigrationId", "ProductVersion")
VALUES ('20240418104712_Initial', '8.0.4');

COMMIT;


create database flightgearapi

CREATE TABLE flight_sessions (
	"id" serial NOT NULL,
	"title" varchar(100) not null,
	"duration" integer not null,
	"date" timestamp not null,
	CONSTRAINT flight_sessions_pkey PRIMARY KEY ("id")
);

CREATE TABLE flight_properties_shots (
	"id" serial NOT NULL,
	"flight_session_id" integer not null,
	"order" integer not null,
	
	"Longitude" double precision not null,
	"Latitude" double precision not null,
	"AltitudeAgl" double precision not null,
	"Altitude" double precision not null,
	"AltitudeIndicatedBaro" double precision not null,
	"AltitudeAbsoluteBaro" double precision not null,
	"Roll" double precision not null,
	"Pitch" double precision not null,
	"Heading" double precision not null,
	"HeadingMagnetic" double precision not null,
	"HeadingMagneticIndicated" double precision not null,
	"IndicatedSpeed" double precision not null,
	"Airspeed" double precision not null,
	"VerticalBaroSpeed" double precision not null,
	"Mach" double precision not null,
	"UBodyMps" double precision not null,
	"VBodyMps" double precision not null,
	"WBodyMps" double precision not null,
	"SideOverload" double precision not null,
	"PilotOverload" double precision not null,
	"Temperature" double precision not null,
	"AccelerationY" double precision not null,
	"AccelerationX" double precision not null,
	"AccelerationNormal" double precision not null,
	
	CONSTRAINT flight_properties_shots_pkey PRIMARY KEY ("id"),
	foreign key("flight_session_id") references flight_sessions("id")
);


CREATE USER fgapi WITH PASSWORD 'superpassword123';
GRANT CONNECT ON DATABASE flightgearapi TO fgapi;
GRANT SELECT, INSERT, UPDATE, delete, references ON flight_sessions TO fgapi;
GRANT SELECT, INSERT, UPDATE, DELETE, references ON flight_properties_shots TO fgapi;

GRANT SELECT, USAGE ON SEQUENCE flight_sessions_id_seq TO fgapi;
GRANT SELECT, USAGE ON SEQUENCE flight_properties_shots_id_seq TO fgapi;
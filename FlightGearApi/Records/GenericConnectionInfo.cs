using FlightGearApi.Enums;

namespace FlightGearApi.records;

public record GenericConnectionInfo(IoType IoType, int Port, int RefreshesPerSecond, string ProtocolFileName, string Address="127.0.0.1");
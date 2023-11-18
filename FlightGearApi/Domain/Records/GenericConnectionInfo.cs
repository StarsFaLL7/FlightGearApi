using FlightGearApi.Domain.Enums;

namespace FlightGearApi.Domain.Records;

public record GenericConnectionInfo(IoType IoType, int Port, double RefreshesPerSecond, string ProtocolFileName, string Address="127.0.0.1");
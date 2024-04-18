namespace Domain.Enums.FlightUtilityProperty;

public enum FlightUtilityProperty
{
    Throttle = 0, // Мощность двигателя
    Aileron = 1, // Наклон штурвала
    Elevator = 2, // Положение штурвала (на/от себя)
    Rudder = 3, // Прокрутка самолёта для управления рысканием
    SpeedBrake = 4, // Воздушные тормозы
    ParkingBrake = 5,
    Longitude = 6,
    Latitude = 7,
    Altitude = 8, // Высота в метрах
    Roll = 9, // Крен
    Pitch = 10, // Тангаж
    Heading = 11, // Курс
    Flaps = 12, // Закрылки
    VerticalSpeed = 13,
    IndicatedSpeed = 14,
    ApPitchSwitch = 15,
    ApRollSwitch = 16,
    ApHeadingSwitch = 17,
    ApTargetVerticalPressureRate = 18,
    ApHeadingHeadingDeg = 19,
    
}
using GeoCoordinatePortable;

namespace Domain.Utility;

/// <summary>
/// Статический класс для вычислений, связанный с географическими точками
/// </summary>
public static class GeographyHelper
{
    /// <summary>
    /// Возвращает расстояние в метрах между двумя точками.
    /// </summary>
    /// <param name="lat1">Широта первой точки</param>
    /// <param name="lon1">Долгота первой точки</param>
    /// <param name="lat2">Широта второй точки</param>
    /// <param name="lon2">Долгота второй точки</param>
    /// <returns>Расстояние в метрах.</returns>
    public static double GetDistanceBetweenCoordsInMeters(double lat1, double lon1,
        double lat2, double lon2)
    {
        var p1 = new GeoCoordinate(lat1, lon1);
        var p2 = new GeoCoordinate(lat2, lon2);
        var res = p1.GetDistanceTo(p2);
        return res;
    }
    
    /// <summary>
    /// Возвращает курс в градусах, по которому находиться вторая точка относительно первой.
    /// </summary>
    /// <param name="lat1">Широта первой точки</param>
    /// <param name="lon1">Долгота первой точки</param>
    /// <param name="lat2">Широта второй точки</param>
    /// <param name="lon2">Долгота второй точки</param>
    /// <returns>Угол в градусах, где север = 0 или 360, восток = 90, юг = 180, запад = 270.</returns>
    public static double GetDirectionDeg(double lat1, double lon1, double lat2, double lon2)
    {
        var lat1Rad = ConvertToRadians(lat1);
        var lon1Rad = ConvertToRadians(lon1);
        var lat2Rad = ConvertToRadians(lat2);
        var lon2Rad = ConvertToRadians(lon2);

        var dLon = lon2Rad - lon1Rad;

        var y = Math.Sin(dLon) * Math.Cos(lat2Rad);
        var x = Math.Cos(lat1Rad) * Math.Sin(lat2Rad) - Math.Sin(lat1Rad) * Math.Cos(lat2Rad) * Math.Cos(dLon);

        var bearing = Math.Atan2(y, x);
        bearing = ConvertToDegrees(bearing);
        bearing = (bearing + 360) % 360;
        return bearing;
    }
    
    private static double ConvertToRadians(double angle)
    {
        return angle * (Math.PI / 180d);
    }
    
    private static double ConvertToDegrees(double angle)
    {
        return angle * (180d / Math.PI);
    }

    /// <summary>
    /// Метод создаёт новую географ. точку, сдвинутую от указанных координат на расстояние в метрах по заданному курсу.
    /// </summary>
    /// <param name="latitude">Широта начальной точки</param>
    /// <param name="longitude">Долгота начальной точки</param>
    /// <param name="distanceInMeters">Расстояние, на которое нужно сдвинуть координаты (в метрах)</param>
    /// <param name="bearing">Курс в сторону которого нужно сдвинуть координаты. (0/360 - Север, 90 - Восток, 180 - Юг, 270 - Запад)</param>
    /// <returns></returns>
    public static GeoCoordinate MoveGeoPoint(double latitude, double longitude, double distanceInMeters, double bearing)
    {
        const double radiusEarthKm = 6371.0; // Радиус Земли в километрах

        // Преобразование расстояния из метров в радианы
        double distanceRadians = distanceInMeters / (radiusEarthKm * 1000.0);

        // Преобразование курса из градусов в радианы
        double bearingRadians = ConvertToRadians(bearing);

        // Преобразование широты текущей точки в радианы
        double latRad = ConvertToRadians(latitude);
        double lonRad = ConvertToRadians(longitude);

        // Вычисление новой широты и долготы
        double newLatRad = Math.Asin(Math.Sin(latRad) * Math.Cos(distanceRadians) +
                                     Math.Cos(latRad) * Math.Sin(distanceRadians) * Math.Cos(bearingRadians));
        double newLonRad = lonRad + Math.Atan2(Math.Sin(bearingRadians) * Math.Sin(distanceRadians) * Math.Cos(latRad),
            Math.Cos(distanceRadians) - Math.Sin(latRad) * Math.Sin(newLatRad));

        // Преобразование новых координат из радианов в градусы
        var lat = Math.Round(ConvertToDegrees(newLatRad), 5);
        var lon = Math.Round(ConvertToDegrees(newLonRad), 5);
        return new GeoCoordinate(lat, lon);
    }
}
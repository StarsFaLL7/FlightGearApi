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
}
using System.Text;
using FlightGearApi.Application.DTO;
using FlightGearApi.Domain.Enums;
using FlightGearApi.Domain.Records;
using FlightGearApi.Domain.UtilityClasses;

namespace FlightGearApi.Domain.FlightGearCore;

/// <summary>
/// Управляет созданием XML-файлов для общения с Flight Gear по протоколу UDP после запуска.
/// Хранит параметры FlightProperties для ввода/вывода. 
/// </summary>
public class IoManager
{
    public const int InputPort = 6788;
    public const int TelnetPort = 5501;
    private IConfiguration Configuration { get; }
    private string PathToProtocolFolder { get; }
    
    public double ConnectionRefreshesPerSecond { get; set; } = 1;
    public List<FlightPropertyInfo> OutputPropertiesList { get; } = new ();
    
    
    public IoManager(IConfiguration configuration)
    {
        Configuration = configuration;
        PathToProtocolFolder = Path.Combine(
            Configuration.GetSection("FlightGear:Path").Value,
            Configuration.GetSection("FlightGear:ProtocolSubPath").Value);
    }

    public string GetUdpInputConnectionString()
    {
        var argument = " --generic=socket,in,";
        var filename = Configuration.GetSection("FlightGear:XmlInputFilename").Value;
        argument += $"{ConnectionRefreshesPerSecond},127.0.0.1,{InputPort},udp,{filename}";
        return argument;
    }
    
    public string ConvertGenericConnectionToArgument(GenericConnectionInfo connectionInfo)
    {
        var argument = " --generic=socket,";
        argument += connectionInfo.IoType == IoType.Input ? "in," : "out,";
        argument += $"{connectionInfo.RefreshesPerSecond},{connectionInfo.Address},{connectionInfo.Port},udp,{connectionInfo.ProtocolFileName}";
        return argument;
    }
    
    public void SaveXmlFile()
    {
        var files = new Dictionary<string, string>()
        {
            //{Configuration.GetSection("FlightGear:XmlOutputFilename").Value + ".xml", GenerateXmlOutputFileContent()},
            {Configuration.GetSection("FlightGear:XmlInputFilename").Value + ".xml", GenerateXmlInputFileContent()}
        };
        foreach (var fileInfoPair in files)
        {
            var path = Path.Combine(PathToProtocolFolder, fileInfoPair.Key);
            try
            {
                var f = File.Create(path);
                var content = fileInfoPair.Value;
                var bytes = Encoding.UTF8.GetBytes(content);
                f.Write(bytes);
                f.Close();
            }
            catch (Exception e)
            {
                Console.WriteLine(e);
                throw;
            }
        }
    }
    
    public string GenerateXmlInputFileContent()
    {
        var builder = new StringBuilder();
        builder.Append(@"<?xml version=""1.0""?>
<PropertyList>
<generic>
    <input>
        <binary_mode>true</binary_mode>
        <byte_order>network</byte_order>

");
        
        foreach (var inProperty in FlightPropertiesHelper.InputProperties.Select(p => p.Value.Property))
        {
            builder.Append($@"        <chunk>
            <name>{inProperty.Name}</name>
            <type>{inProperty.TypeName}</type>
            <node>{inProperty.Path}</node>
        </chunk>

");
        }
        
        builder.Append(@"    </input>
</generic>
</PropertyList>
");
        
        return builder.ToString();
    }


    private Type ParseType(string typeString)
    {
        return typeString.ToLower() switch
        {
            "double" => typeof(double),
            "float" => typeof(float),
            "string" => typeof(string),
            "int" or "integer" => typeof(int),
            "bool" or "boolean" => typeof(bool),
            "char" => typeof(char),
            _ => throw new Exception("Couldn't parse the type.")
        };
    }

    private string GenerateFormatValue(string name, string typeString)
    {
        return typeString.ToLower() switch
        {
            "double" or "float" => $"{name}=%.5f",
            "string" => $"{name}=%s",
            "int" or "integer" => $"{name}=%d",
            "bool" or "boolean" => $"{name}=%d",
            "char" => $"{name}=%c",
            _ => throw new Exception("Couldn't parse the type.")
        };
    }
}
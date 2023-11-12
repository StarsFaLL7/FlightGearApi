using System.Text;
using FlightGearApi.DTO;
using FlightGearApi.Enums;
using FlightGearApi.records;
using FlightGearApi.UtilityClasses;

namespace FlightGearApi.FlightGearCore;

/// <summary>
/// Управляет созданием XML-файлов для общения с Flight Gear по протоколу UDP после запуска.
/// Хранит параметры FlightProperties для ввода/вывода. 
/// </summary>
public class IoManager
{
    private IConfiguration _configuration { get; }
    
    private string _pathToProtocolFolder { get; }
    
    private Dictionary<IoType, string> XmlFilenames { get; } = new Dictionary<IoType, string>()
    {
        { IoType.Input, "fg-input" },
        { IoType.Output, "fg-output" }
    };

    private List<FlightPropertyInfo> InputPropertiesList { get; } = new ();
    private List<FlightPropertyInfo> OutputPropertiesList { get; } = new ();
    
    public IoManager(IConfiguration configuration)
    {
        _configuration = configuration;
        _pathToProtocolFolder = Path.Combine(
            _configuration.GetSection("FlightGear:Path").Value,
            _configuration.GetSection("FlightGear:ProtocolSubPath").Value);
    }

    private string GenerateXmlInputFileContent()
    {
        var builder = new StringBuilder();
        builder.Append(@"<?xml version=""1.0""?>
<PropertyList>
<generic>
    <input>
        <binary_mode>true</binary_mode>
        <byte_order>network</byte_order>

");
        
        foreach (var inProperty in InputPropertiesList)
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
    
    private string GenerateXmlOutputFileContent()
    {
        var builder = new StringBuilder();
        builder.Append(@"<?xml version=""1.0""?>
<PropertyList>
<generic>
    <output>
        <line_separator>newline</line_separator>
        <var_separator>newline</var_separator>
        <binary_mode>false</binary_mode>

");
        
        foreach (var outProperty in OutputPropertiesList)
        {
            builder.Append($@"        <chunk>
            <name>{outProperty.Name}</name>
            <type>{outProperty.TypeName}</type>
            <node>{outProperty.Path}</node>
            <format>{outProperty.FormatValue}</format>
        </chunk>

");
        }
        
        builder.Append(@"   </output>
</generic>
</PropertyList>
");
        
        return builder.ToString();
    }

    public bool AddProperty(IoType type, string path, string name, string typeName)
    {
        var list = type == IoType.Input ? InputPropertiesList : OutputPropertiesList;
        var newProperty = new FlightPropertyInfo(path, name, ParseType(typeName), typeName, GenerateFormatValue(name, typeName));
        if (!list.Contains(newProperty))
        {
            list.Add(newProperty);
            return true;
        }

        return false;
    }
    
    public bool TryRemoveProperty(IoType type, string name)
    {
        var list = type == IoType.Input ? InputPropertiesList : OutputPropertiesList;
        var selectedProperty = list.FirstOrDefault(p => p.Name == name);
        if (selectedProperty != default)
        {
            list.Remove(selectedProperty);
            return true;
        }

        return false;
    }

    public async Task<FlightPropertiesResponse> GetAllIoParametersAsync()
    {
        var result = new FlightPropertiesResponse()
        {
            InputProperties = InputPropertiesList.Select(p => p.Name).ToList(),
            OutputProperties = OutputPropertiesList.Select(p => p.Name).ToList()
        };
        
        return result;
    }
    
    public void SaveOutputXmlFile()
    {
        var path = Path.Combine(_pathToProtocolFolder, XmlFilenames[IoType.Output] + ".xml");
        try
        {
            var f = File.Create(path);
            var content = GenerateXmlOutputFileContent();
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
    
    public void SaveInputXmlFile()
    {
        var path = Path.Combine(_pathToProtocolFolder, XmlFilenames[IoType.Input] + ".xml");
        try
        {
            var f = File.Create(path);
            var content = GenerateXmlInputFileContent();
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
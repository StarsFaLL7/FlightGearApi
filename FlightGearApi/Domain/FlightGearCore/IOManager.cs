﻿using System.Text;
using FlightGearApi.Application.DTO;
using FlightGearApi.Domain.Enums;
using FlightGearApi.Domain.Records;

namespace FlightGearApi.Domain.FlightGearCore;

/// <summary>
/// Управляет созданием XML-файлов для общения с Flight Gear по протоколу UDP после запуска.
/// Хранит параметры FlightProperties для ввода/вывода. 
/// </summary>
public class IoManager
{
    public double ConnectionRefreshesPerSecond { get; private set; } = 1;
    public readonly int InputPort = 6788;
    public readonly int OutputPort = 6789;
    
    private IConfiguration Configuration { get; }
    
    private string PathToProtocolFolder { get; }

    private List<FlightPropertyInfo> InputPropertiesList { get; } = new ();
    private List<FlightPropertyInfo> OutputPropertiesList { get; } = new ();
    
    public IoManager(IConfiguration configuration)
    {
        Configuration = configuration;
        PathToProtocolFolder = Path.Combine(
            Configuration.GetSection("FlightGear:Path").Value,
            Configuration.GetSection("FlightGear:ProtocolSubPath").Value);
    }

    public FlightPropertiesResponse GetAllIoPropertiesAsync()
    {
        var result = new FlightPropertiesResponse()
        {
            // InputProperties = InputPropertiesList.Select(p => p.Name).ToList(),
            OutputProperties = OutputPropertiesList.Select(p => p.Name).ToList()
        };
        
        return result;
    }

    public GenericConnectionInfo GetConnectionInfo(IoType type)
    {
        if (type == IoType.Input)
        {
            return new GenericConnectionInfo(IoType.Input, InputPort, ConnectionRefreshesPerSecond, 
                Configuration.GetSection("FlightGear:XmlInputFilename").Value);
        }
        
        return new GenericConnectionInfo(IoType.Output, OutputPort, ConnectionRefreshesPerSecond, 
            Configuration.GetSection("FlightGear:XmlOutputFilename").Value);
    }
    
    public string GenerateParametersGenericConnection()
    {
        var resultString = "";
        var connectionsList = new List<GenericConnectionInfo>()
        {
            GetConnectionInfo(IoType.Input),
            GetConnectionInfo(IoType.Output)
        };
        
        foreach (var connectionInfo in connectionsList)
        {
            var argument = " --generic=socket,";
            argument += connectionInfo.IoType == IoType.Input ? "in," : "out,";
            argument += $"{connectionInfo.RefreshesPerSecond},{connectionInfo.Address},{connectionInfo.Port},udp,{connectionInfo.ProtocolFileName}";
            
            resultString += argument;
        }

        return resultString;
    }

    public bool TrySetRefreshesPerSecond(double value)
    {
        if (value <= 0)
        {
            return false;
        }
        
        ConnectionRefreshesPerSecond = value;
        return true;
    }
    
    public bool TryAddProperty(IoType type, string path, string name, string typeName)
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

    public void Reset()
    {
        OutputPropertiesList.Clear();
        ConnectionRefreshesPerSecond = 1;
    }
    
    public void SaveXmlFiles()
    {
        var files = new Dictionary<string, string>()
        {
            {Configuration.GetSection("FlightGear:XmlOutputFilename").Value + ".xml", GenerateXmlOutputFileContent()},
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
    
    public string GenerateXmlOutputFileContent()
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
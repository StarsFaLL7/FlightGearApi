using System.Text;
using FlightGearApi.Enums;
using FlightGearApi.UtilityClasses;

namespace FlightGearApi.FlightGearCore;

/// <summary>
/// Управляет созданием XML-файлов для общения с Flight Gear по протоколу UDP после запуска.
/// Хранит параметры FlightProperties для ввода/вывода. 
/// </summary>
public class IoManager
{
    public Dictionary<IoType, string> XmlFilenames { get; } = new Dictionary<IoType, string>()
    {
        { IoType.Input, "fg-input" },
        { IoType.Output, "fg-output" }
    };
    public List<FlightProperties> InputPropertiesList { get; } = new ();
    public List<FlightProperties> OutputPropertiesList { get; } = new ();

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
            var info = FlightPropertiesInfo.Properties[inProperty];
            builder.Append($@"      <chunk>
            <name>{info.Name}</name>
            <type>{info.TypeName}</type>
            <node>{info.Path}</node>
        </chunk>

");
        }
        
        builder.Append(@"
    </input>
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
        
        foreach (var inProperty in OutputPropertiesList)
        {
            var info = FlightPropertiesInfo.Properties[inProperty];
            builder.Append($@"      <chunk>
            <name>{info.Name}</name>
            <type>{info.TypeName}</type>
            <node>{info.Path}</node>
            <format>{info.Name}={info.FormatValue}</format>
        </chunk>

");
        }
        
        builder.Append(@"   </output>
</generic>
</PropertyList>
");
        
        return builder.ToString();
    }

    public void AddProperty(IoType type, FlightProperties property)
    {
        var list = type == IoType.Input ? InputPropertiesList : OutputPropertiesList;
        if (!list.Contains(property))
        {
            list.Add(property);
        }
    }

    public void AddProperty(IoType type, string path, string name, string typeName, string formatValue)
    {
        // TODO
    }
    
    public void TryRemoveProperty(IoType type, string name)
    {
        // TODO
    }

    public void TryRemoveProperty(IoType type, FlightProperties property)
    {
        var list = type == IoType.Input ? InputPropertiesList : OutputPropertiesList;
        if (list.Contains(property))
        {
            list.Remove(property);
        }
    }
    
    public string SaveOutputXmlFile()
    {
        // TODO
        return XmlFilenames[IoType.Output];
    }
    
    public string SaveInputXmlFile()
    {
        // TODO
        return XmlFilenames[IoType.Input];
    }
}
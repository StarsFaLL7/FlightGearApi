using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace FlightGearApi.Infrastructure.ModelsDal;

[Table("flight_sessions")]
public class FlightSessionDal
{
    [Key]
    [Column("id")]
    public int Id { get; set; }
    
    [Column("title")]
    public string Title { get; set; }
    
    [Column("duration")]
    public int DurationSec { get; set; }
    
    [Column("date")]
    public DateTime Date { get; set; }
    
    public ICollection<FlightPropertiesModel> PropertiesCollection { get; set; }
}
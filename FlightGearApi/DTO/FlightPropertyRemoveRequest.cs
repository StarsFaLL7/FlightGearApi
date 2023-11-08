using System.ComponentModel.DataAnnotations;
using FlightGearApi.Enums;

namespace FlightGearApi.DTO;

public class FlightPropertyRemoveRequest
{
    [Required]
    public IoType IoType { get; set; }
    [Required]
    public string Name { get; set; }
    
}
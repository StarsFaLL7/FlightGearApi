using System.ComponentModel.DataAnnotations;
using FlightGearApi.Enums;

namespace FlightGearApi.DTO;

public class FlightPropertyAddRequest
{
    [Required]
    public IoType IoType { get; set; }
    [Required]
    public string Path { get; set; } 
    [Required]
    public string Name { get; set; }
    [Required]
    public string TypeName { get; set; }
}
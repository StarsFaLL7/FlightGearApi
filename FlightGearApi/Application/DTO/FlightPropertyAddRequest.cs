using System.ComponentModel.DataAnnotations;
using FlightGearApi.Domain.Enums;

namespace FlightGearApi.Application.DTO;

public class FlightPropertyAddRequest
{
    [Required]
    public string Path { get; set; } 
    [Required]
    public string Name { get; set; }
    [Required]
    public string TypeName { get; set; }
}
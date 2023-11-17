using System.ComponentModel.DataAnnotations;
using FlightGearApi.Domain.Enums;

namespace FlightGearApi.Application.DTO;

public class FlightPropertyRemoveRequest
{
    [Required]
    public IoType IoType { get; set; }
    [Required]
    public string Name { get; set; }
    
}
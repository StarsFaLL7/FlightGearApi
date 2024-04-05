using System.ComponentModel.DataAnnotations;

namespace Domain.Base;

public class BaseEntityWithKey<T>
{
    [Key]
    public required T Id { get; init; }
}
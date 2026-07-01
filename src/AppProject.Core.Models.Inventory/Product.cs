using System;
using System.ComponentModel.DataAnnotations;
using AppProject.Models;

namespace AppProject.Core.Models.Inventory;

public class Product : IEntity
{
    public Guid? Id { get; set; }

    [Required]
    [MaxLength(200)]
    public string Name { get; set; } = default!;

    [MaxLength(200)]
    public string? Code { get; set; }

    [Range(0, double.MaxValue)]
    public decimal MinimumStockQuantity { get; set; }

    public byte[]? RowVersion { get; set; }
}

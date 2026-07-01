using System;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using Microsoft.EntityFrameworkCore;

namespace AppProject.Core.Infrastructure.Database.Entities.Inventory;

[Table("Products")]
public class TbProduct : BaseEntity
{
    [Key]
    public Guid Id { get; set; }

    [Required]
    [MaxLength(200)]
    public string Name { get; set; } = default!;

    [MaxLength(200)]
    public string? Code { get; set; }

    [Precision(18, 2)]
    public decimal MinimumStockQuantity { get; set; }

    public ICollection<TbStockMovement> StockMovements { get; set; } = new List<TbStockMovement>();
}

using System;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using Microsoft.EntityFrameworkCore;

namespace AppProject.Core.Infrastructure.Database.Entities.Inventory;

[Table("StockMovements")]
public class TbStockMovement : BaseEntity
{
    [Key]
    public Guid Id { get; set; }

    [Required]
    public Guid ProductId { get; set; }

    [Required]
    [Precision(18, 2)]
    public decimal Quantity { get; set; }

    [Required]
    public DateTime MovementDate { get; set; }

    [Required]
    public bool IsOutbound { get; set; }

    [ForeignKey(nameof(ProductId))]
    public TbProduct Product { get; set; } = default!;
}

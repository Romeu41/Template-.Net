using System;
using AppProject.Models;

namespace AppProject.Core.Models.Inventory;

public class StockMovementSummary : ISummary
{
    public Guid Id { get; set; }

    public Guid ProductId { get; set; }

    public string ProductName { get; set; } = default!;

    public decimal Quantity { get; set; }

    public DateTime MovementDate { get; set; }

    public bool IsOutbound { get; set; }
}

using System;
using AppProject.Models;

namespace AppProject.Core.Models.Inventory;

public class ProductSummary : ISummary
{
    public Guid Id { get; set; }

    public string Name { get; set; } = default!;

    public string? Code { get; set; }

    public decimal MinimumStockQuantity { get; set; }
}

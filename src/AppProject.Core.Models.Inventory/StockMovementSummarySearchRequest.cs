using System;
using AppProject.Models;

namespace AppProject.Core.Models.Inventory;

public class StockMovementSummarySearchRequest : SearchRequest
{
    public Guid? ProductId { get; set; }
}

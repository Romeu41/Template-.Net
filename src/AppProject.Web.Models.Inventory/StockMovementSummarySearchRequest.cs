using System;
using AppProject.Models;

namespace AppProject.Web.Models.Inventory;

public class StockMovementSummarySearchRequest : SearchRequest
{
    public Guid? ProductId { get; set; }
}

using System;
using AppProject.Models;

namespace AppProject.Web.Models.Inventory;

public class StockMovement : ObservableModel, IEntity
{
    private Guid? id;
    private Guid productId;
    private decimal quantity;
    private DateTime movementDate = DateTime.UtcNow;
    private bool isOutbound;
    private byte[]? rowVersion;

    public Guid? Id { get => this.id; set => this.Set(ref this.id, value); }

    public Guid ProductId { get => this.productId; set => this.Set(ref this.productId, value); }

    public decimal Quantity { get => this.quantity; set => this.Set(ref this.quantity, value); }

    public DateTime MovementDate { get => this.movementDate; set => this.Set(ref this.movementDate, value); }

    public bool IsOutbound { get => this.isOutbound; set => this.Set(ref this.isOutbound, value); }

    public byte[]? RowVersion { get => this.rowVersion; set => this.Set(ref this.rowVersion, value); }
}

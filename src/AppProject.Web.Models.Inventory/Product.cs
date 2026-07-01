using System;
using AppProject.Models;

namespace AppProject.Web.Models.Inventory;

public class Product : ObservableModel, IEntity
{
    private Guid? id;
    private string name = default!;
    private string? code;
    private decimal minimumStockQuantity;
    private byte[]? rowVersion;

    public Guid? Id { get => this.id; set => this.Set(ref this.id, value); }

    public string Name { get => this.name; set => this.Set(ref this.name, value); }

    public string? Code { get => this.code; set => this.Set(ref this.code, value); }

    public decimal MinimumStockQuantity { get => this.minimumStockQuantity; set => this.Set(ref this.minimumStockQuantity, value); }

    public byte[]? RowVersion { get => this.rowVersion; set => this.Set(ref this.rowVersion, value); }
}

namespace AppProject.Exceptions;

public enum ExceptionCode
{
    Generic,
    SecurityValidation,
    RequestValidation,
    Concurrency,
    EntityNotFound,

    // Pattern: ModuleName_EntityName_ValidationName
    General_Country_DuplicateName,
    General_State_DuplicateName,
    General_City_DuplicateName,
    General_City_Neighborhood_DuplicateName,
    Inventory_Product_DuplicateName,
    Inventory_Product_DuplicateCode,
    Inventory_Product_ContainsStockMovements,
    Inventory_StockMovement_InvalidQuantity,
    Inventory_StockMovement_ProductNotFound,
    Inventory_StockMovement_InsufficientStock,
}

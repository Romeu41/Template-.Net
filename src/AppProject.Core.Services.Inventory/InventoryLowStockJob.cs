using System;
using System.Globalization;
using System.Text;
using AppProject.Core.Contracts;
using AppProject.Core.Infrastructure.Database;
using AppProject.Core.Infrastructure.Database.Entities.Inventory;
using AppProject.Core.Infrastructure.Email;
using AppProject.Core.Infrastructure.Jobs;
using AppProject.Resources;
using Hangfire;
using Microsoft.Extensions.Logging;

namespace AppProject.Core.Services.Inventory;

[DisableConcurrentExecution(timeoutInSeconds: 60)]
public class InventoryLowStockJob(
    ILogger<InventoryLowStockJob> logger,
    IDatabaseRepository databaseRepository,
    IEmailSender emailSender,
    IUserContext userContext)
    : JobBase(logger)
{
    protected override async Task RunAsync(CancellationToken cancellationToken)
    {
        var products = await databaseRepository.GetAllAsync<TbProduct>(cancellationToken);

        if (products.Count == 0)
        {
            return;
        }

        var movements = await databaseRepository.GetAllAsync<TbStockMovement>(cancellationToken);

        var balanceByProduct = movements
            .GroupBy(x => x.ProductId)
            .ToDictionary(
                group => group.Key,
                group => group.Sum(movement => movement.IsOutbound ? -movement.Quantity : movement.Quantity));

        var lowStockProducts = products
            .Select(product => new
            {
                Product = product,
                Balance = balanceByProduct.TryGetValue(product.Id, out var balance) ? balance : 0,
            })
            .Where(x => x.Balance < x.Product.MinimumStockQuantity)
            .OrderBy(x => x.Product.Name)
            .ToList();

        if (lowStockProducts.Count == 0)
        {
            return;
        }

        var systemAdmin = await userContext.GetSystemAdminUserAsync(cancellationToken);

        if (string.IsNullOrWhiteSpace(systemAdmin.Email))
        {
            logger.LogError("System admin email is not configured. Low stock notification will not be sent.");
            return;
        }

        var subject = StringResource.GetStringByKey("Inventory_LowStockEmail_Subject");
        var intro = StringResource.GetStringByKey("Inventory_LowStockEmail_Intro");
        var closing = StringResource.GetStringByKey("Inventory_LowStockEmail_Closing");

        var bodyBuilder = new StringBuilder();
        bodyBuilder.AppendFormat("<p>{0}</p>", intro);
        bodyBuilder.Append("<ul>");

        foreach (var item in lowStockProducts)
        {
            var balanceText = item.Balance.ToString("N2", CultureInfo.InvariantCulture);
            var minimumText = item.Product.MinimumStockQuantity.ToString("N2", CultureInfo.InvariantCulture);
            var message = StringResource.GetStringByKey("Inventory_LowStockEmail_ItemFormat", item.Product.Name, balanceText, minimumText);
            bodyBuilder.AppendFormat("<li>{0}</li>", message);
        }

        bodyBuilder.Append("</ul>");
        bodyBuilder.AppendFormat("<p>{0}</p>", closing);

        var sent = await emailSender.SendEmailAsync(
            subject: subject,
            body: bodyBuilder.ToString(),
            to: new[] { systemAdmin.Email },
            cancellationToken: cancellationToken);

        if (!sent)
        {
            logger.LogError("Low stock notification email was not sent.");
        }
    }
}

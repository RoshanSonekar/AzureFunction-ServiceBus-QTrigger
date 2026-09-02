using AzureFunction.Services.ProductCategory;
using AzureFunction.Services.ProductCategory.Models;
using Microsoft.Azure.Functions.Worker;
using Microsoft.Extensions.Configuration;
using System.Text.Json;

namespace AzureFunction;

public class ServiceBusFunction
{
	private readonly IHttpClientFactory _clientFactory;

  public ServiceBusFunction(IHttpClientFactory clientFactory)
  {
    _clientFactory = clientFactory;
  }

	[Function("SB-ProductCategoryCreated")]
  public async Task Run([ServiceBusTrigger("product-category-created",
    Connection = "ServiceBusConnectionString")] string msg)
  {
    Console.WriteLine(msg);
    if (!string.IsNullOrEmpty(msg))
    {
      Category cat = JsonSerializer.Deserialize<Category>(msg);

			IProductCategoryCommandService productCategoryService = new ProductCategoryCommandService(_clientFactory);
      Guid g = await productCategoryService.PostCategory(cat, "product-category-created-output"); // get qname from config
		}
  }
}
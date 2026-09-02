using AzureFunction.Services.ProductCategory.Models;
using System.Net.Http.Json;
using AzureFunction.ExceptionsQueue;

namespace AzureFunction.Services.ProductCategory
{
	public class ProductCategoryCommandService:IProductCategoryCommandService
	{
		private readonly IHttpClientFactory _clientFactory;
		public ProductCategoryCommandService(IHttpClientFactory clientFactory)
		{
			_clientFactory = clientFactory;
		}

		public async Task<Guid> PostCategory(Category category, string queueNameForExceptions)
		{
			try
			{
				// 1. Create your named HttpClient instance
				var client = _clientFactory.CreateClient("ProductCategoryCommandApi");

				// 2. Send the POST request with the object payload
				HttpResponseMessage response = await client.PostAsJsonAsync("ap/Categories/SQLServer/Category", category);

				return response.Content.ReadFromJsonAsync<Guid>().Result;
			}
			// then send it to the failure queue for later processing
			catch (Exception ex)
			{
				ExceptionMessage exceptionMessage = new ExceptionMessage
				{
					CategoryId = category.CategoryId,
					CategoryName = category.CategoryName.Trim(),
					Description = category.Description,
					DisplayOrder = category.DisplayOrder,
					IsActive = category.IsActive,
					ExceptionMsg = new ExceptionServiceMsg
					{
						ExceptionType = ex.GetType().ToString(),
						ExceptionMessageText = ex.Message,
						StackTrace = ex.StackTrace,
						OccurredAt = DateTime.UtcNow
					} 
				};
				// get the queue name and connection string from the configuration or environment variables
				ExceptionSender exceptionSender = new ExceptionSender(queueNameForExceptions, "Endpoint=sb://productcatalog.servicebus.windows.net/;SharedAccessKeyName=RootManageSharedAccessKey;SharedAccessKey=sQ0Xlw/T8/vx7KrEJCF/M4KLfBTjn5tbn+ASbCrXebo=");
				await exceptionSender.SendExceptionMessage(exceptionMessage);

				throw ex;
			} 
		}
	}
}

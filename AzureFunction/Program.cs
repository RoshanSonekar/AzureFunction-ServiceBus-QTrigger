using Microsoft.Azure.Functions.Worker;
using Microsoft.Azure.Functions.Worker.Builder;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;

var builder = FunctionsApplication.CreateBuilder(args);

builder.ConfigureFunctionsWebApplication();

builder.Services
		.AddApplicationInsightsTelemetryWorkerService()
		.ConfigureFunctionsApplicationInsights();

//Register the HttpClient with the base domain
var externalServiceBaseUrl = "https://localhost:7085";//builder.Configuration.GetValue<string>("ProductCategoryAPI:BaseUrl");
builder.Services.AddHttpClient("ProductCategoryCommandApi", client =>
{
	client.BaseAddress = new Uri(externalServiceBaseUrl);
});

builder.Build().Run();

//var host = new HostBuilder()
//		.ConfigureFunctionsWebApplication()
//		.ConfigureServices(services =>
//		{
//			services.AddApplicationInsightsTelemetryWorkerService();
//			services.ConfigureFunctionsApplicationInsights();
//		})
//		.Build();


//host.Run();

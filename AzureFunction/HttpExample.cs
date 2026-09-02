using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Azure.Functions.Worker;
using Microsoft.Azure.Functions.Worker.Http;
using Microsoft.Extensions.Logging;
using System;
using System.Threading.Tasks;
using Azure.Messaging.ServiceBus;
using Azure.Identity;

namespace AzureFunction;

public class HttpExample
{
  private readonly ILogger<HttpExample> _logger;

  public HttpExample(ILogger<HttpExample> logger)
  {
    _logger = logger;
  }

  //[Function("HttpGet")]
  public IActionResult Run([HttpTrigger(AuthorizationLevel.Anonymous, "get", Route ="Test/HttpGet")] HttpRequest req, string name)
  {
    var returnValue = string.IsNullOrEmpty(name)
     ? "Hello, World."
     : $"Hello, {name}.";

    _logger.LogInformation($"C# HTTP trigger function processed a request for {returnValue}.");

    return new OkObjectResult(returnValue);
  }
}
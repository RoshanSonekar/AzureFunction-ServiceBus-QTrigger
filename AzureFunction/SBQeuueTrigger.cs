using System;
using System.Threading.Tasks;
using Azure.Messaging.ServiceBus;
using Microsoft.Azure.Functions.Worker;
using Microsoft.Extensions.Logging;
using Microsoft.Azure.Amqp;

namespace AzureFunction;

public class SBQeuueTrigger
{
    private readonly ILogger<SBQeuueTrigger> _logger;

    public SBQeuueTrigger(ILogger<SBQeuueTrigger> logger)
    {
        _logger = logger;
    }


	ServiceBusClientOptions options = new ServiceBusClientOptions
	{
		TransportType = ServiceBusTransportType.AmqpWebSockets
	};

	//[Function("Test")]
    public void Run(
        [ServiceBusTrigger("product-category-created", Connection = "ServiceBusConnectionString")]
        ServiceBusReceivedMessage message,
        ServiceBusMessageActions messageActions)
    {
		_logger.LogInformation("Message ID: {id}", message.MessageId);
        _logger.LogInformation("Message Body: {body}", message.Body);
        _logger.LogInformation("Message Content-Type: {contentType}", message.ContentType);

        // Complete the message 
    }
}
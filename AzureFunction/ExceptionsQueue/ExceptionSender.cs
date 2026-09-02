using Azure.Messaging.ServiceBus;
using System.Text.Json;

namespace AzureFunction.ExceptionsQueue;

public class ExceptionSender
{
	string _queueName=string.Empty;
	string _nameSpace = string.Empty;

	public ExceptionSender(string queueName, string nameSpace) 
	{
		_queueName = queueName;
		_nameSpace = nameSpace;	
	}

	public async Task SendExceptionMessage(ExceptionMessage exceptionMessage)
	{
		var options = new ServiceBusClientOptions
		{
			TransportType = ServiceBusTransportType.AmqpWebSockets
		};

		await using (var client = new ServiceBusClient(_nameSpace, options))
		{
			var sender = client.CreateSender(_queueName);
			var serviceBusMessage = new ServiceBusMessage(JsonSerializer.Serialize(exceptionMessage));
			await sender.SendMessageAsync(serviceBusMessage);
		}

		//var client = new Azure.Messaging.ServiceBus.ServiceBusClient(_nameSpace);
		//var sender = client.CreateSender(_queueName);
		//var message = new Azure.Messaging.ServiceBus.ServiceBusMessage(JsonSerializer.Serialize(exceptionMessage));
		//await sender.SendMessageAsync(message);
	}
}

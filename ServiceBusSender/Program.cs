// See https://aka.ms/new-console-template for more information
using Azure.Messaging.ServiceBus; 
using Newtonsoft.Json;
using System.Text;

Console.WriteLine("Hello, World!");

string connectionString = "Endpoint=sb://productcatalog.servicebus.windows.net/;SharedAccessKeyName=RootManageSharedAccessKey;SharedAccessKey=sQ0Xlw/T8/vx7KrEJCF/M4KLfBTjn5tbn+ASbCrXebo=";
string queueName = "product-category-created";


var options = new ServiceBusClientOptions
{
	TransportType = ServiceBusTransportType.AmqpWebSockets
};

// Create Service Bus Client
ServiceBusClient sbClient = new ServiceBusClient(connectionString, options);


#region --- Send message to queue ---
try
{
	employee e = new employee
	{
		name = "Roshan",
		dept = "EBS",
		age = 40
	};
	var jSonEmployee = JsonConvert.SerializeObject(e);

	// Create Service Bus Sender
	ServiceBusSender sbSender = sbClient.CreateSender(queueName);


	// Send message to the queue
	ServiceBusMessage sbMessage = new ServiceBusMessage(Encoding.UTF8.GetBytes(jSonEmployee));
	await sbSender.SendMessageAsync(sbMessage);
}
catch (Exception ex)
{
	throw ex;
}
#endregion

#region --- Send message to queue in a batch ---
try
{
	employee e = new employee
	{
		name = "Roshan",
		dept = "EBS",
		age = 40
	};
	var jSonEmployee = JsonConvert.SerializeObject(e);

	// Send message to the queue
	ServiceBusMessage sbMessage = new ServiceBusMessage(Encoding.UTF8.GetBytes(jSonEmployee));

	// Create Service Bus Sender
	ServiceBusSender sbSender = sbClient.CreateSender(queueName);

	var msgBatch = await sbSender.CreateMessageBatchAsync();
	//add msg
	msgBatch.TryAddMessage(new ServiceBusMessage("message -1"));
	msgBatch.TryAddMessage(new ServiceBusMessage("message -2"));
	msgBatch.TryAddMessage(new ServiceBusMessage("message -3"));
	msgBatch.TryAddMessage(new ServiceBusMessage("message -4"));

	await sbSender.SendMessagesAsync(msgBatch);
}
catch (Exception ex)
{
	throw ex;
}
#endregion

Console.WriteLine("Message Sent");

class employee
{
	public string name { get; set; }
	public int age{ get; set; }
	public string dept { get; set; }



}
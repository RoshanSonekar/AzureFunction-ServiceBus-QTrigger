// See https://aka.ms/new-console-template for more information
using Azure.Messaging.ServiceBus; 
using Azure.Messaging.ServiceBus.Administration;
using Microsoft.Azure.Amqp.Framing;
using Newtonsoft.Json;
using System.Collections.Generic;
using System.Linq;
using System.Text;

Console.WriteLine("Hello, World!");

ServiceBusClientOptions options = new ServiceBusClientOptions
{
	TransportType = ServiceBusTransportType.AmqpWebSockets
};

//ServiceBusAdministrationClientOptions optionAdmin = new ServiceBusAdministrationClientOptions
//{
//	TransportType = ServiceBusTransportType.AmqpWebSockets
//};


string connectionString = "Endpoint=sb://practicebusnamespace.servicebus.windows.net/;SharedAccessKeyName=policy_firsttopic;SharedAccessKey=W4VOzWL5BtTaK4gJLQUHlNMiCHBD0J2uv+ASbGbpIlw=;EntityPath=firsttopic";
string topicName = "firsttopic";

// Create Service Bus Client
ServiceBusClient sbClient = new ServiceBusClient(connectionString, options);

#region --- Sender --- 

try
{
	// Create Service Bus Sender
	ServiceBusSender sbSender = sbClient.CreateSender(topicName);

	employee e = new employee
	{
		name = "Roshan",
		dept = "EBS",
		age = 40
	};
	var jSonEmployee = JsonConvert.SerializeObject(e);

	// Send plain message to the subscriptions
	ServiceBusMessage sbMessage1 = new ServiceBusMessage(Encoding.UTF8.GetBytes(jSonEmployee));

	// Send filter message to the subscriptions
	ServiceBusMessage sbMessage2 = new ServiceBusMessage(Encoding.UTF8.GetBytes(jSonEmployee))
	{
		ApplicationProperties =
		{
			{
				"Department","EBS"
			}
		}
	};

	await sbSender.SendMessageAsync(sbMessage1);
	await sbSender.SendMessageAsync(sbMessage2);

	//Console.WriteLine("Message Sent");
}
catch (Exception ex)
{
	throw ex;
}


#endregion

#region --- Receiver ---
try
{
	//string subscriptionName = "Subscription1";

	//// Create Service Bus Receiver
	//ServiceBusReceiver sbReceiver = sbClient.CreateReceiver(topicName, subscriptionName);
	////sbReceiver.ReceiveMode=ServiceBusReceiveMode.PeekLock;

	//// Receive message from the subscriptions
	//await foreach (ServiceBusReceivedMessage message in sbReceiver.ReceiveMessagesAsync())
	//{
	//	// Process message here
	//	Console.WriteLine(message.Body.ToString());
	//}
	 
	 
}
catch (Exception ex)
{
	throw ex;
}
#endregion

#region --- Create Subscription ---
try
{
	// create SB admin client
	var adminClient = new ServiceBusAdministrationClient(connectionString);

	// create normal subscription 
	//await adminClient.CreateSubscriptionAsync(topicName, "Subscription3");

	// create subscription with filters
	var subsDetails = new CreateSubscriptionOptions(topicName, "Subscription4");

	//Correlation Filter
	//var subsRule = new CreateRuleOptions("CorrelationRule", new CorrelationRuleFilter() { ApplicationProperties = { { "Department", "EBS" } } });

	// SQL Filter
	var subsRule = new CreateRuleOptions("SQLRule", new SqlRuleFilter("Deptartment = 'EBS'"));

	// Boolean 
	var subsRuleTrue = new CreateRuleOptions("SQLRule", new TrueRuleFilter()); // 1-1 accept all messgages // this is default filter
	var subsRuleFalse = new CreateRuleOptions("SQLRule", new FalseRuleFilter()); // 1-0 accept no maessage at all

	await adminClient.CreateSubscriptionAsync(subsDetails, subsRule);

	Console.WriteLine("Subscription Created Succesfully");
}
catch (Exception ex)
{
	throw ex;
}

#endregion

class employee
{
	public string name { get; set; }
	public int age { get; set; }
	public string dept { get; set; }
}
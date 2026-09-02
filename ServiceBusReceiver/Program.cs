// See https://aka.ms/new-console-template for more information
using Azure.Messaging.ServiceBus;
using System.Transactions;

Console.WriteLine("Hello, World!");

try
{
	//https://www.youtube.com/watch?v=5AkYGUdfgQs&list=PLU1w_BFZFd2pW5mcM_aTKCWwJw2sGBA20&index=19
	//https://www.youtube.com/watch?v=d2z9Q5dyRlg&list=PLU1w_BFZFd2pW5mcM_aTKCWwJw2sGBA20&index=22


	string connectionString = "Endpoint=sb://practicebusnamespace.servicebus.windows.net/;SharedAccessKeyName=policyQueueHttpPost;SharedAccessKey=6zlaIh3/LTwA8h7xdQ0oVT8JxvnqKvgNd+ASbA0I3qQ=;EntityPath=httppost";
	string queueName = "httppost";


	var options = new ServiceBusClientOptions
	{
		TransportType = ServiceBusTransportType.AmqpWebSockets
	};

	// Create Service Bus Client
	ServiceBusClient sbClient = new ServiceBusClient(connectionString, options);

	// Create Service Bus Receiver
	ServiceBusReceiver sbReceiver = sbClient.CreateReceiver(queueName);

	// Receiver message to the queue
	ServiceBusReceivedMessage sbMessage = await sbReceiver.ReceiveMessageAsync();
	//await sbReceiver.CompleteMessageAsync(sbMessage);
	await sbReceiver.AbandonMessageAsync(sbMessage);

	Console.WriteLine("Message : " + sbMessage.Body);


	// coss enitry - to send msg to multiple queues and ensures all queue receives msg
	using (var tran = new TransactionScope(TransactionScopeAsyncFlowOption.Enabled))
	{
		//send msg to q2
		//send msg to q3

		//complete source q1 transaction

		tran.Complete();
	}

}
catch (Exception ex)
{

	throw ex;
}

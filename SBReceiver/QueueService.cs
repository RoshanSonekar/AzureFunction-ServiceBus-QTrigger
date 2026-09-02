using System.Collections;
using Azure.Messaging.ServiceBus;

namespace SBReceiver
{
	public class QueueService
	{
		public async Task SendMessgeAsync<T>(T serviceMessage, string queueName)
		{
			try
			{
				var queueClient = new ServiceBusClient("https://PracticeBusNamespace.servicebus.windows.net/httppost");
			}
			catch (Exception ex)
			{

				throw;
			}
		}
	}
}

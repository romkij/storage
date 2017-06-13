﻿using System;
using System.Collections.Generic;
using System.Linq;
using Storage.Net.Messaging;
using System.Threading.Tasks;
using Microsoft.Azure.ServiceBus;

namespace Storage.Net.Microsoft.Azure.ServiceBus
{
   /// <summary>
   /// Implements Azure Service Bus Queue
   /// </summary>
   public class AzureServiceBusQueuePublisher : AsyncMessagePublisher
   {
      private readonly string _queueName;
      private readonly QueueClient _client;

      /// <summary>
      /// Creates a new instance of Azure Service Bus Queue by connection string and queue name
      /// </summary>
      /// <param name="connectionString">Service Bus connection string</param>
      /// <param name="queueName">Queue name in Service Bus. If queue doesn't exist it will be created for you.</param>
      /// <param name="createIfNotExists">If true, will try to create the queue if it doesn't exist. You must have permissions to do so though.</param>
      public AzureServiceBusQueuePublisher(string connectionString, string queueName, bool createIfNotExists)
      {
         _client = new QueueClient(connectionString, queueName);
         _queueName = queueName;
      }

      /// <summary>
      /// Puts message to the queue with default options
      /// </summary>
      public override async Task PutMessagesAsync(IEnumerable<QueueMessage> messages)
      {
         if (messages == null) return;

         IList<Message> sbmsg = messages.Select(Converter.ToMessage).ToList();
         await _client.SendAsync(sbmsg);
      }

      /// <summary>
      /// Closes connection to the queue
      /// </summary>
      public override void Dispose()
      {
         _client.CloseAsync().Wait();

         base.Dispose();
      }
   }
}
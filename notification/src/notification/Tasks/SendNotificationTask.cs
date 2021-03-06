using System;
using System.Collections.Generic;
using CamundaClient.Dto;
using CamundaClient.Worker;
using Microsoft.AspNetCore.SignalR.Client;
using Newtonsoft.Json;

namespace notification.Tasks
{
    [ExternalTaskTopic("send-notification")]
    public class NotificationTask : IExternalTaskAdapter
    {
        public void Execute(ExternalTask externalTask, ref Dictionary<string, object> resultVariables)
        {
            HubConnection connection;
            Console.WriteLine("--- External Task Variables ---");

            var s = JsonConvert.SerializeObject(externalTask);
            Console.WriteLine(s);
            foreach (var item in externalTask.Variables)
            {
                Console.WriteLine(item.Key.ToString().PadRight(18) + ": " + item.Value.Value.ToString() );
            }            

            connection = new HubConnectionBuilder()
                .WithUrl("http://localhost:5051/eventhub")
                .Build();

            connection.StartAsync();
            connection.InvokeAsync("PublishMessage",externalTask.Variables["topicid"].Value,externalTask.Variables["notificationmessage"].Value,"",s);
            connection.DisposeAsync();
        }
    }
}
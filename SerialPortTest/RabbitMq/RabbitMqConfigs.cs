using System;
namespace SerialPortTest.RabbitMq
{
    public class RabbitMqConfigs
    {
        public int Port { get; private set; } = 5003;
        public string Hostname { get; private set; } = "148.233.85.168";
        public string Password { get; private set; } = "admin";
        public string QueueName { get; private set; } = "Esp8266Queue";
        public string UserName { get; private set; } = "admin";
        public string Exchange { get; private set; } = "amq.topic";
        public string RoutingKey { get; private set; } = "DataFromAspNetCore";



        //Uri = new Uri("amqp://guest:guest@localhost:5672")
        public Uri Uri => new Uri($"amqp://{UserName}:{Password}@{Hostname}:{Port}");


        private static RabbitMqConfigs instance;

        private RabbitMqConfigs()
        {
           
        }

        public static RabbitMqConfigs CreateObject() {
            if(instance == null)
                instance = new RabbitMqConfigs();
            return instance;
        }
            //(instance == null) re
         //new RabbitMqConfigs();
    }
}
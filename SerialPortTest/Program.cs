using System;
using System.IO.Ports;
using System.Text;
using System.Text.Json;
using RabbitMQ.Client;
using RabbitMQ.Client.Events;
using SerialPortTest.RabbitMq;
using SerialPortTest.SerialConnection;

namespace SerialPortTest
{
    class Program
    {
        static SerialPortArduinoConnection serialPort = new SerialPortArduinoConnection("COM10", 9600);
        private static RabbitMqConfigs rabbitMqConfigs = RabbitMqConfigs.CreateObject();
        static IConnection connection;
        static IModel channel;
        static void Main(string[] args)
        {
            serialPort.DataFromArduinoHandler += DataFromArduinoEventHandler;

            
            var factory = new ConnectionFactory
            {
                Uri = rabbitMqConfigs.Uri//new Uri("amqp://guest:guest@localhost:5672")
            };
            connection = factory.CreateConnection();
            channel = connection.CreateModel();
            QueueConsumer.Consume(channel, serialPort);
            Console.ReadLine();
        }

        //Acción que es invocada al entrar información por el puerto serial
        private static void DataFromArduinoEventHandler(string data)
        {
            Console.WriteLine("Data will send to rabbitMq:");
            Console.WriteLine(data);
            //Metodo que publica a RabbitMq
            //QueueProducer.Publish(channel, data);
        }





    }



}

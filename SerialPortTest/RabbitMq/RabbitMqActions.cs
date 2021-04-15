using System;
using System.Text;
using System.Text.Json;
using System.Threading;
using System.Threading.Tasks;
using RabbitMQ.Client;
using RabbitMQ.Client.Events;
using SerialPortTest.SerialConnection;

namespace SerialPortTest.RabbitMq
{
    public static class QueueConsumer
    {

        private static RabbitMqConfigs rabbitMqConfigs = RabbitMqConfigs.CreateObject();//Traemos la información base de las configuraciones de RabbitMQ

        
        public static void Consume(IModel channel, SerialPortArduinoConnection serialPort)
        {
            //##La información de las variables rabbitMqConfigs es información que no va a cambiar y sirven principalmente para la conexión y creación de colas y Exchanges

            //Declaramos el exchange al que enlazaremos la cola que crearemos o que ya este creada
            channel.ExchangeDeclare(rabbitMqConfigs.Exchange, ExchangeType.Topic, true, false, null);

            //Declaramos la cola que enlazaremos al exchange
            channel.QueueDeclare(rabbitMqConfigs.QueueName, durable: true, exclusive: false, autoDelete: true, arguments: null);

            //Enlazamos la cola y el exchange anteriormente creado
            channel.QueueBind(rabbitMqConfigs.QueueName, rabbitMqConfigs.Exchange, rabbitMqConfigs.RoutingKey, null);

            var consumer = new EventingBasicConsumer(channel);//Creamos el consumidor, el cual estará esperando que lleguen mensajes desde AspNetCore y en la linea siguiente se adjunta el evento a invocar
            consumer.Received += (sender, e) =>
            {
                //Las siguientes dos lineas obtienen el mensaje y lo convierten a texto,
                var body = e.Body.ToArray();
                var message = Encoding.UTF8.GetString(body);
                //de aquí en adelante se la información se envía al arduino por el puerto serial  
                Console.WriteLine("Data will send to Arduino");
                Console.WriteLine(message);
                //Se envía la información proveniente de AspNetCore a Arduino por el puerto serial
                serialPort.SendToArduino(message);
            };

            channel.BasicConsume(rabbitMqConfigs.QueueName, true, consumer);
            Console.WriteLine("Consumer started");
        }

    }

    public static class QueueProducer
    {
        private static RabbitMqConfigs rabbitMqConfigs = RabbitMqConfigs.CreateObject();//Traemos la información base de las configuraciones de RabbitMQ
        public static void Publish(IModel channel, string message)
        {
            var queueName = rabbitMqConfigs.QueueName;
            //Declaramos la cola a la que nos conectaremos para enviar la información al RoutingKey amq.topic
            //channel.QueueDeclare(queueName, durable: true, exclusive: false, autoDelete: true, arguments: null);

            var body = Encoding.UTF8.GetBytes(message);//Convertimos el mensaje en arreglo de bytes, solo así se puede enviar a RabbitMq
            channel.BasicPublish(exchange: "amq.topic", routingKey: "DataFromAspNetCore", basicProperties: null, body: body);//Publicamos en RabbirMq al exchange amq.topic, con el tema (routingKey) DataFromArduino

            //Thread.Sleep(500);

        }
    }
}
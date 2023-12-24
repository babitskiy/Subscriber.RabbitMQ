using System.Text;
using RabbitMQ.Client;
using RabbitMQ.Client.Events;

namespace Subscriber.RabbitMQ;

class Program
{
    static void Main(string[] args)
    {
        var factory = new ConnectionFactory { HostName = "localhost" };
        var connection = factory.CreateConnection();
        using var channel = connection.CreateModel();

        channel.QueueDeclare(queue: "orders",
                       durable: true,
                       exclusive: false,
                       autoDelete: false,
                       arguments: null);

        var consumer = new EventingBasicConsumer(channel);
        consumer.Received += (model, eventArgs) =>
        {
            var body = eventArgs.Body.ToArray();
            var message = Encoding.UTF8.GetString(body);

            Console.WriteLine(message);
        };

        channel.BasicConsume(queue: "orders", autoAck: true, consumer: consumer);
        Console.ReadKey();
    }
}


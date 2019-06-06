using RabbitMQ.Client;
using System;
using System.Text;

namespace rabbitmaProducer
{
    class Program
    {
        static void Main(string[] args)
        {
            Console.WriteLine("Start!");
            IConnectionFactory connFactory = new ConnectionFactory()
            {
                HostName = "127.0.0.1",
                Port = 5672,
                VirtualHost = "mq1",
                UserName = "admin",
                Password = "admin"
            };
            using (IConnection conn = connFactory.CreateConnection())
            {
                using (IModel channel = conn.CreateModel())
                {
                    String queueName = String.Empty;
                    if (args.Length > 0)
                        queueName = args[0];
                    else
                        queueName = "queue1";
                    channel.QueueDeclare(
                        queue: queueName,
                        durable: false,
                        exclusive: false,
                        autoDelete: false,
                        arguments: null
                        );
                    while (true)
                    {
                        Console.WriteLine("消息内容：");
                        String message = Console.ReadLine();
                        //消息内容
                        byte[] body = Encoding.UTF8.GetBytes(message);
                        //发送消息
                        channel.BasicPublish(exchange: "", routingKey: queueName, basicProperties: null, body: body);
                        var isok = channel.WaitForConfirms();
                        Console.WriteLine("成功发送消息11：" + message + ";22" + isok.ToString());
                    }
                }
            }
        }
    }
}

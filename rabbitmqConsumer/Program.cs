using RabbitMQ.Client;
using RabbitMQ.Client.Events;
using System;
using System.Text;
using System.Threading;

namespace rabbitmqConsumer
{
    class Program
    {
        static void Main(string[] args)
        {
           
            IConnectionFactory connFactory = new ConnectionFactory()
            {
                HostName = "127.0.0.1",
                Port = 5672,
                VirtualHost = "mq1",
                UserName = "admin",
                Password = "admin"
            };

            //rabbitmq(connFactory);//普通模式接受
            //rabbitmqExchangeFanout(connFactory);//交换机-发布订阅模式
            //rabbitmqExchangeDirect(connFactory);//交换机-路由模式
            rabbitmqExchangeTopic(connFactory);//交换机-通配符模式
        }

        public static void rabbitmq(IConnectionFactory connFactory)
        {
            Console.WriteLine("Start!");
            using (IConnection conn = connFactory.CreateConnection())
            {
                using (IModel channel = conn.CreateModel())
                {
                    //String queueName = String.Empty;
                   
                     string  queueName = "queue1";

                    //声明一个队列
                    channel.QueueDeclare(
                        queue: queueName,////消息队列名称
                        durable: false,//是否缓存
                        exclusive: false,
                        autoDelete: false,
                        arguments: null
                        );
                    //告诉Rabbit每次只能向消费者发送一条信息,再消费者未确认之前,不再向他发送信息
                    channel.BasicQos(0, 1, false);
                    //创建消费者对象
                    var consumer = new EventingBasicConsumer(channel);
                    consumer.Received += (model, ea) =>
                    {
                        Thread.Sleep(2000);
                        byte[] message = ea.Body;//接收的消息
                        Console.WriteLine("接收到的消息为：" + Encoding.UTF8.GetString(message));
                        //返回消息确认
                        channel.BasicAck(ea.DeliveryTag, true);
                    };
                    //消费者开启监听
                    //将autoAck设置false 关闭自动确认
                    channel.BasicConsume(queue: queueName, autoAck: false, consumer: consumer);
                    Console.ReadKey();
                }
            }
        }

        //Exchange（交换机）模式(发布订阅模式(fanout不建议使用), 路由模式(direct), 通配符模式(topic))
        /// <summary>
        /// 交换机exchange发布订阅模式fanout
        /// </summary>
        /// <param name="connFactory"></param>
        public static void rabbitmqExchangeFanout(IConnectionFactory connFactory)
        {
            Console.WriteLine("Start-exchange-fanout");
            using (IConnection conn = connFactory.CreateConnection())
            {
                using (IModel channel = conn.CreateModel())
                {
                    //交换机名称
                    string exchangeName = "exchange1";

                    //声明交换机
                    channel.ExchangeDeclare(exchange: exchangeName, type: "fanout");
                    //消息队列名称
                    string queueName = exchangeName + "_" + new Random().Next(1,1000).ToString();
                    //声明队列
                    //声明一个队列
                    channel.QueueDeclare(
                        queue: queueName,////消息队列名称
                        durable: false,//是否缓存
                        exclusive: false,
                        autoDelete: false,
                        arguments: null
                        );
                    //将队列与交换机进行绑定
                    channel.QueueBind(queue: queueName, exchange: exchangeName, routingKey: "");
                    //告诉Rabbit每次只能向消费者发送一条信息,再消费者未确认之前,不再向他发送信息
                    //声明为手动确认
                    channel.BasicQos(0, 1, false);
                    //创建消费者对象
                    var consumer = new EventingBasicConsumer(channel);
                    Console.WriteLine("开始接受：" + queueName);

                   consumer.Received += (model, ea) =>
                    {
                        //Thread.Sleep(2000);
                        byte[] message = ea.Body;//接收的消息
                        Console.WriteLine("接收到的消息为：" + Encoding.UTF8.GetString(message));
                        //返回消息确认
                        channel.BasicAck(ea.DeliveryTag, true);
                    };
                    //消费者开启监听
                    //将autoAck设置false 关闭自动确认
                    channel.BasicConsume(queue: queueName, autoAck: false, consumer: consumer);
                    Console.ReadKey();
                }
            }
        }



        //Exchange（交换机）模式(发布订阅模式(fanout不建议使用), 路由模式(direct), 通配符模式(topic))
        /// <summary>
        /// 交换机exchange路由模式direct
        /// </summary>
        /// <param name="connFactory"></param>
        public static void rabbitmqExchangeDirect(IConnectionFactory connFactory)
        {
            Console.WriteLine("Start-exchange-direct");
            using (IConnection conn = connFactory.CreateConnection())
            {
                using (IModel channel = conn.CreateModel())
                {
                    //交换机名称
                    string exchangeName = "exchange2";

                    //声明交换机
                    channel.ExchangeDeclare(exchange: exchangeName, type: "direct");
                    //消息队列名称
                    string queueName = exchangeName + "_" + new Random().Next(1, 1000).ToString();
                    //声明队列
                    //声明一个队列
                    channel.QueueDeclare(
                        queue: queueName,////消息队列名称
                        durable: false,//是否缓存
                        exclusive: false,
                        autoDelete: false,
                        arguments: null
                        );
                    foreach (string routeKey in new string[] {"4","5","6","7","3"})
                    {
                        //将队列与交换机进行绑定
                        //匹配多个路由
                        channel.QueueBind(queue: queueName, exchange: exchangeName, routingKey: routeKey);
                    }
                    //告诉Rabbit每次只能向消费者发送一条信息,再消费者未确认之前,不再向他发送信息
                    //声明为手动确认
                    channel.BasicQos(0, 1, false);
                    //创建消费者对象
                    var consumer = new EventingBasicConsumer(channel);
                    Console.WriteLine("开始接受：" + queueName);

                    consumer.Received += (model, ea) =>
                    {
                        //Thread.Sleep(2000);
                        byte[] message = ea.Body;//接收的消息
                        Console.WriteLine("接收到的消息为：" + Encoding.UTF8.GetString(message));
                        //返回消息确认
                        channel.BasicAck(ea.DeliveryTag, true);
                    };
                    //消费者开启监听
                    //将autoAck设置false 关闭自动确认
                    channel.BasicConsume(queue: queueName, autoAck: false, consumer: consumer);
                    Console.ReadKey();
                }
            }
        }

        //Exchange（交换机）模式(发布订阅模式(fanout不建议使用), 路由模式(direct), 通配符模式(topic))
        /// <summary>
        /// 交换机exchange通配符模式topic
        /// </summary>
        /// <param name="connFactory"></param>
        public static void rabbitmqExchangeTopic(IConnectionFactory connFactory)
        {
            Console.WriteLine("Start-exchange-topic");
            using (IConnection conn = connFactory.CreateConnection())
            {
                using (IModel channel = conn.CreateModel())
                {
                    //交换机名称
                    string exchangeName = "exchange3";

                    //声明交换机
                    channel.ExchangeDeclare(exchange: exchangeName, type: "topic");
                    //消息队列名称
                    string queueName = exchangeName + "_" + new Random().Next(1, 1000).ToString();
                    //声明队列
                    //声明一个队列
                    channel.QueueDeclare(
                        queue: queueName,////消息队列名称
                        durable: false,//是否缓存
                        exclusive: false,
                        autoDelete: false,
                        arguments: null
                        );
                    foreach (string routeKey in new string[] { "topic.*" })
                    {
                        //将队列与交换机进行绑定
                        //匹配多个路由
                        channel.QueueBind(queue: queueName, exchange: exchangeName, routingKey: routeKey);
                    }
                    //告诉Rabbit每次只能向消费者发送一条信息,再消费者未确认之前,不再向他发送信息
                    //声明为手动确认
                    channel.BasicQos(0, 1, false);
                    //创建消费者对象
                    var consumer = new EventingBasicConsumer(channel);
                    Console.WriteLine("开始接受：" + queueName);
                    //接收消息
                    consumer.Received += (model, ea) =>
                    {
                        //Thread.Sleep(2000);
                        byte[] message = ea.Body;//接收的消息
                        Console.WriteLine("接收到的消息为：" + Encoding.UTF8.GetString(message));
                        //返回消息确认
                        channel.BasicAck(ea.DeliveryTag, true);
                    };
                    //消费者开启监听
                    //将autoAck设置false 关闭自动确认
                    channel.BasicConsume(queue: queueName, autoAck: false, consumer: consumer);
                    Console.ReadKey();
                }
            }
        }
    }
}

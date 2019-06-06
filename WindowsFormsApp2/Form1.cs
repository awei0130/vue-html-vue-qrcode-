using RabbitMQ.Client;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Windows.Forms;

namespace WindowsFormsApp2
{
    public partial class Form1 : Form
    {
        public Form1()
        {
            InitializeComponent();
        }

        private void button1_Click(object sender, EventArgs e)
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
            // rabbitmq(connFactory);//普通模式
            //rabbitmqExchangeFanout(connFactory);//交换机-发布订阅模式
            //rabbitmqExchangeDirect(connFactory);//交换机-路由模式
            rabbitmqExchangeTopic(connFactory);//交换机-通配符模式
        }
        /// <summary>
        /// 普通模式
        /// </summary>
        /// <param name="connFactory"></param>
        public void rabbitmq(IConnectionFactory connFactory)
        {
            using (IConnection conn = connFactory.CreateConnection())
            {
                using (IModel channel = conn.CreateModel())
                {
                    String queueName = String.Empty;

                    queueName = "queue1";
                    channel.QueueDeclare(
                        queue: queueName,
                        durable: false,
                        exclusive: false,
                        autoDelete: false,
                        arguments: null
                        );
                    channel.ConfirmSelect();
                    //Console.WriteLine("消息内容：");
                    String message = this.textBox1.Text;
                    //消息内容
                    byte[] body = Encoding.UTF8.GetBytes(message);
                    //发送消息
                    channel.BasicPublish(exchange: "", routingKey: queueName, basicProperties: null, body: body);
                    var isok = channel.WaitForConfirms();
                    Console.WriteLine("成功发送消息11：" + message + ";22" + isok.ToString());

                }
            }
        }

        //交换机模式exchange（交换机）模式(发布订阅模式(fanout不建议使用), 路由模式(direct), 通配符模式(topic))
        /// <summary>
        /// 交换机exchange发布订阅模式fanout
        /// </summary>
        /// <param name="connFactory"></param>
        public void rabbitmqExchangeFanout(IConnectionFactory connFactory)
        {
            using (IConnection conn = connFactory.CreateConnection())
            {
                using (IModel channel = conn.CreateModel())
                { 
                    //交换机名称
                    string exchangeName = "exchange1";

                    channel.ExchangeDeclare(exchange:exchangeName,type:"fanout");
                   
                    //channel.ConfirmSelect();
                    //Console.WriteLine("消息内容：");
                    String message = this.textBox1.Text;
                    //消息内容
                    byte[] body = Encoding.UTF8.GetBytes(message);
                    //发送消息
                    channel.BasicPublish(exchange: exchangeName, routingKey: "", basicProperties: null, body: body);
                    //var isok = channel.WaitForConfirms();
                    Console.WriteLine("成功发送消息11：" + message + ";33") ;// + isok.ToString());

                }
            }
        }

        //交换机模式exchange（交换机）模式(发布订阅模式(fanout不建议使用), 路由模式(direct), 通配符模式(topic))
        /// <summary>
        /// 交换机exchange路由模式dirent
        /// </summary>
        /// <param name="connFactory"></param>
        public void rabbitmqExchangeDirect(IConnectionFactory connFactory)
        {
            using (IConnection conn = connFactory.CreateConnection())
            {
                using (IModel channel = conn.CreateModel())
                {
                    //交换机名称
                    string exchangeName = "exchange2";
                    //路由名称
                    string routeKey = this.textBox2.Text;

                    channel.ExchangeDeclare(exchange: exchangeName, type: "direct");

                    //channel.ConfirmSelect();
                    //Console.WriteLine("消息内容：");
                    String message = this.textBox1.Text;
                    //消息内容
                    byte[] body = Encoding.UTF8.GetBytes(message);
                    //发送消息
                    channel.BasicPublish(exchange: exchangeName, routingKey: routeKey, basicProperties: null, body: body);
                    //var isok = channel.WaitForConfirms();
                    Console.WriteLine("成功发送消息11：" + message + ";33");// + isok.ToString());

                }
            }
        }

        //交换机模式exchange（交换机）模式(发布订阅模式(fanout不建议使用), 路由模式(direct), 通配符模式(topic))
        /// <summary>
        /// 交换机exchange通配符模式topic
        /// </summary>
        /// <param name="connFactory"></param>
        public void rabbitmqExchangeTopic(IConnectionFactory connFactory)
        {
            using (IConnection conn = connFactory.CreateConnection())
            {
                using (IModel channel = conn.CreateModel())
                {
                    //交换机名称
                    string exchangeName = "exchange3";
                    //路由名称
                    string routeKey = this.textBox2.Text;

                    channel.ExchangeDeclare(exchange: exchangeName, type: "topic");

                    //channel.ConfirmSelect();
                    //Console.WriteLine("消息内容：");
                    String message = this.textBox1.Text;
                    //消息内容
                    byte[] body = Encoding.UTF8.GetBytes(message);
                    //发送消息
                    channel.BasicPublish(exchange: exchangeName, routingKey: routeKey, basicProperties: null, body: body);
                    //var isok = channel.WaitForConfirms();
                    Console.WriteLine("成功发送消息11：" + message + ";33");// + isok.ToString());

                }
            }
        }
    }
}

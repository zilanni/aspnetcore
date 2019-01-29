using System;
using System.Collections.Generic;
using System.Threading;
using ILanni.FarmMarket.Models;
using RabbitMQ.Client;
using Newtonsoft.Json;
using ILanni.Common.RabbitMQ;

namespace ILanni.FarmMarket.MQ
{
    public class ProductPublisher
    {
        private ConnectionFactory factory;
        private Settings settings;
        //private static ThreadLocal<IConnection> localConnection = new ThreadLocal<IConnection>();
        private ChannelPool mqChannelPool;

        public ProductPublisher(ChannelPool mqChannelPool  /*ConnectionFactory factory*/, Settings settings)
        {
            this.mqChannelPool = mqChannelPool;
            //this.factory = factory;
            this.settings = settings;
        }

        public void Publish(Product product)
        {
            /*using (var connection = factory.CreateConnection())
            {
                using (IModel channel = connection.CreateModel())
                {
                    Dictionary<string, object> dic = new Dictionary<string, object>();
                    channel.ExchangeDeclare(settings.ExchangeForProduct, ExchangeType.Topic, true, false);
                    //channel.QueueDeclare(settings.QueueForProduct, true, true, false);
                    //channel.QueueBind(settings.QueueForProduct, settings.ExchangeForProduct, settings.RoutingKeyForProduct);
                    var propertys = channel.CreateBasicProperties();
                    propertys.Persistent = true;
                    propertys.DeliveryMode = 2;
                    channel.ConfirmSelect();
                    var content = JsonConvert.SerializeObject(product);
                    var body = System.Text.UTF8Encoding.UTF8.GetBytes(content);
                    channel.BasicPublish(settings.ExchangeForProduct, settings.RoutingKeyForProduct, propertys, body);
                }
            }*/

            using (IModel channel = mqChannelPool.GetChannel())
            {
                Dictionary<string, object> dic = new Dictionary<string, object>();
                channel.ExchangeDeclare(settings.ExchangeForProduct, ExchangeType.Topic, true, false);
                //channel.QueueDeclare(settings.QueueForProduct, true, true, false);
                //channel.QueueBind(settings.QueueForProduct, settings.ExchangeForProduct, settings.RoutingKeyForProduct);
                var propertys = channel.CreateBasicProperties();
                propertys.Persistent = true;
                propertys.DeliveryMode = 2;
                channel.ConfirmSelect();
                var content = JsonConvert.SerializeObject(product);
                var body = System.Text.UTF8Encoding.UTF8.GetBytes(content);
                channel.BasicPublish(settings.ExchangeForProduct, settings.RoutingKeyForProduct, propertys, body);
            }

        }
    }
}

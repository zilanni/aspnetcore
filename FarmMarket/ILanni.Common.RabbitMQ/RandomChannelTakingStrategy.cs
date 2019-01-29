using System;
using System.Collections.Generic;
using System.Text;
using System.Threading;
using RabbitMQ.Client;

namespace ILanni.Common.RabbitMQ
{
    internal class RandomChannelTakingStrategy : IChannelTakingStrategy
    {
        public IModel GetChannel(ConnectionPooling pool, PoolingSettings settings, CancellationToken token = default(CancellationToken))
        {

            var random = new Random();
            int rIndex = random.Next(pool.TotalCount);
            int index = rIndex;
            IModel channel;
            do
            {
                var connection = pool.Get(index);
                if (connection.TryGetChannel(out channel))
                {
                    return channel;
                }
                index++;
            } while (index < pool.TotalCount);
            for (var i = 0; i < rIndex; i++)
            {
                var connection = pool.Get(i);
                if (connection.TryGetChannel(out channel))
                {
                    return channel;
                }
            }
            if (pool.TotalCount < settings.MaxConnections)
            {
                ChannelPooling channels;
                if (pool.TryCreate(out channels))
                {
                    return channels.GetChannel(token);
                }
            }
            return pool.Get(rIndex).GetChannel(token);
        }
    }
}

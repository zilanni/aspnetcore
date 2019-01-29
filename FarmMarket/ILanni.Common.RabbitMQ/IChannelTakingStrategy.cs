using System;
using System.Collections.Generic;
using System.Text;
using RabbitMQ.Client;
using System.Threading;

namespace ILanni.Common.RabbitMQ
{
    public interface IChannelTakingStrategy
    {
        IModel GetChannel(ConnectionPooling pool, PoolingSettings settings, CancellationToken token = default(CancellationToken));

    }
}

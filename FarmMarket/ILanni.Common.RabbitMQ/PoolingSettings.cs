using System;
using System.Collections.Generic;
using System.Text;

namespace ILanni.Common.RabbitMQ
{
    public class PoolingSettings
    {
        public int MaxConnections { get; set; }

        public int MinConnections { get; set; } = 1;

        public int MaxChannelPerConnection { get; set; }

    }
}

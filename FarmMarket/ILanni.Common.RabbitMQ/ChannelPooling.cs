using System;
using System.Collections.Generic;
using System.Collections.Concurrent;
using RabbitMQ.Client;
using System.Threading;

namespace ILanni.Common.RabbitMQ
{
    public class ChannelPooling:IDisposable
    {

        private volatile int channelCount;
        private volatile int freeCount;

        private volatile IConnection currentConnection;

        private IConnectionFactory factory;
        private PoolingSettings settings;
        private readonly object _addLockObj = new object();
        //private ConcurrentBag<IConnection> connections = new ConcurrentBag<IConnection>();
        private BlockingCollection<IModel> channelsQueue;
        private List<IModel> channels;


        public int TotalCount
        {
            get
            {
                return channelCount;
            }
        }

        public int FreeCount
        {
            get
            {
                return freeCount;
            }
        }

        public ChannelPooling(IConnection connection, PoolingSettings settings)
        {
            this.currentConnection = connection;
            this.settings = settings;
            this.channelsQueue = new BlockingCollection<IModel>(new ConcurrentBag<IModel>(), Math.Max(1, settings.MaxChannelPerConnection));
            channels = new List<IModel>(Math.Max(1, settings.MaxChannelPerConnection));
        }

        public IModel GetChannel(CancellationToken cancellation = default(CancellationToken))
        {
            IModel channel;
            if (channelsQueue.TryTake(out channel))
            {
                Interlocked.Decrement(ref freeCount);
                return new ChannelWrapper(this, channel);
            }
            channel = CreateChannel();
            if (null != channel)
            {
                return new ChannelWrapper(this, channel);
            }
            channel = channelsQueue.Take(cancellation);
            Interlocked.Decrement(ref freeCount);
            return new ChannelWrapper(this, channel);
        }

        private IModel CreateChannel()
        {
            if (channelCount < settings.MaxChannelPerConnection)
            {
                lock (_addLockObj)
                {
                    if (channelCount < settings.MaxChannelPerConnection)
                    {
                        var model = currentConnection.CreateModel();
                        channels.Add(model);
                        Interlocked.Increment(ref channelCount);
                        return model;
                    }
                }
            }
            return null;
        }

        public bool TryGetChannel(out IModel model)
        {
            model = null;
            IModel channel;
            if (channelsQueue.TryTake(out channel))
            {
                Interlocked.Decrement(ref freeCount);
                model = new ChannelWrapper(this, channel);
                return true;
            }
            channel = CreateChannel();
            if (null != channel)
            {
                model = new ChannelWrapper(this, channel);
                return true;
            }
            return false;
        }

        internal void CheckIn(IModel model)
        {
            if (!channelsQueue.TryAdd(model, 1))
            {
                model.Dispose();
            }
            Interlocked.Increment(ref this.freeCount);
        }

        public void Dispose()
        {
            channelsQueue.Dispose();
            foreach (var channel in channels)
            {
                channel.Dispose();
            }
            this.currentConnection.Dispose();
            this.currentConnection = null;
            this.channels = null;
        }
    }

    public class ConnectionPooling : IDisposable
    {
        private IConnectionFactory factory;
        private PoolingSettings settings;
        private volatile ChannelPooling[] connections;
        private volatile int totalCount;
        private readonly object _addLockObj = new object();

        public ConnectionPooling(IConnectionFactory factory, PoolingSettings settings)
        {
            this.factory = factory;
            this.settings = settings;
            this.connections = new ChannelPooling[Math.Max(1,settings.MinConnections)];
            for(int i = 0; i < connections.Length; i++)
            {
                connections[i] = new ChannelPooling(factory.CreateConnection(), settings);
            }
            totalCount = connections.Length;
        }

        public int TotalCount
        {
            get
            {
                return totalCount;
            }
        }

        public bool TryCreate(out ChannelPooling pooling)
        {
            if (totalCount < settings.MaxConnections)
            {
                lock (_addLockObj)
                {
                    if (totalCount < settings.MaxConnections)
                    {
                        ChannelPooling[] newPooling = new ChannelPooling[this.connections.Length + 1];
                        this.connections.CopyTo(newPooling, 0);
                        var connection = factory.CreateConnection();
                        pooling = new ChannelPooling(connection, settings);
                        newPooling[this.connections.Length] = pooling;
                        this.connections = newPooling;
                        totalCount++;
                        return true;
                    }
                }
            }
            pooling = null;
            return false;
        }

        public ChannelPooling Get(int index)
        {
            if (index >= totalCount)
            {
                throw new IndexOutOfRangeException();
            }
            return this.connections[index];
        }
                 
        public void Dispose()
        {
            var oldConnections = this.connections;
            connections = new ChannelPooling[0];
            foreach(var c in oldConnections)
            {
                c.Dispose();
            }
            this.connections = null;
        }
    }

    public class ChannelPool : IDisposable
    {

        private ConnectionPooling pooling;
        private IConnectionFactory factory;
        private PoolingSettings settings;
        private IChannelTakingStrategy strategy;

        public ChannelPool(IConnectionFactory factory, PoolingSettings settings, IChannelTakingStrategy takingStrategy = null)
        {
            this.factory = factory;
            this.settings = settings;
            this.strategy = takingStrategy ?? new RandomChannelTakingStrategy();
            this.pooling = new ConnectionPooling(factory, settings);
        }

        public IModel GetChannel(CancellationToken token = default(CancellationToken))
        {
            return strategy.GetChannel(this.pooling, settings, token);
        }


        #region IDisposable Support
        private bool disposedValue = false; // 要检测冗余调用

        protected virtual void Dispose(bool disposing)
        {
            if (!disposedValue)
            {
                if (disposing)
                {
                    // TODO: 释放托管状态(托管对象)。
                    this.pooling.Dispose();
                }

                // TODO: 释放未托管的资源(未托管的对象)并在以下内容中替代终结器。
                // TODO: 将大型字段设置为 null。

                disposedValue = true;
            }
        }

        // TODO: 仅当以上 Dispose(bool disposing) 拥有用于释放未托管资源的代码时才替代终结器。
        ~ChannelPool()
        {
            // 请勿更改此代码。将清理代码放入以上 Dispose(bool disposing) 中。
            Dispose(false);
        }

        // 添加此代码以正确实现可处置模式。
        public void Dispose()
        {
            // 请勿更改此代码。将清理代码放入以上 Dispose(bool disposing) 中。
            Dispose(true);
            //TODO: 如果在以上内容中替代了终结器，则取消注释以下行。
             GC.SuppressFinalize(this);
        }
        #endregion

    }

}

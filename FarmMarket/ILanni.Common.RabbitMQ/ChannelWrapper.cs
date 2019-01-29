using System;
using System.Collections.Generic;
using System.Text;
using RabbitMQ.Client;
using RabbitMQ.Client.Events;

namespace ILanni.Common.RabbitMQ
{
    public class ChannelWrapper : IModel
    {

        private ChannelPooling pooling;
        private IModel inner;

        public ChannelWrapper(ChannelPooling pooling, IModel inner)
        {
            this.inner = inner;
            this.pooling = pooling;
        }


        public int ChannelNumber => inner.ChannelNumber;

        public ShutdownEventArgs CloseReason => inner.CloseReason;

        public IBasicConsumer DefaultConsumer { get => inner.DefaultConsumer; set => inner.DefaultConsumer = value; }

        public bool IsClosed => inner.IsClosed;

        public bool IsOpen => inner.IsOpen;

        public ulong NextPublishSeqNo => inner.NextPublishSeqNo;

        public TimeSpan ContinuationTimeout { get => inner.ContinuationTimeout; set => inner.ContinuationTimeout = value; }

        public event EventHandler<BasicAckEventArgs> BasicAcks;
        public event EventHandler<BasicNackEventArgs> BasicNacks;
        public event EventHandler<EventArgs> BasicRecoverOk;
        public event EventHandler<BasicReturnEventArgs> BasicReturn;
        public event EventHandler<CallbackExceptionEventArgs> CallbackException;
        public event EventHandler<FlowControlEventArgs> FlowControl;
        public event EventHandler<ShutdownEventArgs> ModelShutdown;

        public void Abort()
        {
            inner.Abort();
        }

        public void Abort(ushort replyCode, string replyText)
        {
            inner.Abort(replyCode, replyText);
        }

        public void BasicAck(ulong deliveryTag, bool multiple)
        {
            inner.BasicAck(deliveryTag, multiple);
        }

        public void BasicCancel(string consumerTag)
        {
            inner.BasicCancel(consumerTag);
        }

        public string BasicConsume(string queue, bool autoAck, string consumerTag, bool noLocal, bool exclusive, IDictionary<string, object> arguments, IBasicConsumer consumer)
        {
            return inner.BasicConsume(queue, autoAck, consumerTag, noLocal, exclusive, arguments, consumer);
        }

        public BasicGetResult BasicGet(string queue, bool autoAck)
        {
            return inner.BasicGet(queue, autoAck);
        }

        public void BasicNack(ulong deliveryTag, bool multiple, bool requeue)
        {
            inner.BasicNack(deliveryTag, multiple, requeue);
        }

        public void BasicPublish(string exchange, string routingKey, bool mandatory, IBasicProperties basicProperties, byte[] body)
        {
            inner.BasicPublish(exchange, routingKey, mandatory, basicProperties, body);
        }

        public void BasicQos(uint prefetchSize, ushort prefetchCount, bool global)
        {
            inner.BasicQos(prefetchSize, prefetchCount, global);
        }

        public void BasicRecover(bool requeue)
        {
            inner.BasicRecover(requeue);
        }

        public void BasicRecoverAsync(bool requeue)
        {
            inner.BasicRecoverAsync(requeue);
        }

        public void BasicReject(ulong deliveryTag, bool requeue)
        {
            inner.BasicReject(deliveryTag, requeue);
        }

        public void Close()
        {
            inner.Close();
        }

        public void Close(ushort replyCode, string replyText)
        {
            inner.Close(replyCode, replyText);
        }

        public void ConfirmSelect()
        {
            inner.ConfirmSelect();
        }

        public uint ConsumerCount(string queue)
        {
            return inner.ConsumerCount(queue);
        }

        public IBasicProperties CreateBasicProperties()
        {
            return inner.CreateBasicProperties();
        }

        public void ExchangeBind(string destination, string source, string routingKey, IDictionary<string, object> arguments)
        {
            inner.ExchangeBind(destination, source, routingKey, arguments);
        }

        public void ExchangeBindNoWait(string destination, string source, string routingKey, IDictionary<string, object> arguments)
        {
            inner.ExchangeBindNoWait(destination, source, routingKey, arguments);
        }

        public void ExchangeDeclare(string exchange, string type, bool durable, bool autoDelete, IDictionary<string, object> arguments)
        {
            inner.ExchangeDeclare(exchange, type, durable, autoDelete, arguments);
        }

        public void ExchangeDeclareNoWait(string exchange, string type, bool durable, bool autoDelete, IDictionary<string, object> arguments)
        {
            inner.ExchangeDeclareNoWait(exchange, type, durable, autoDelete, arguments);
        }

        public void ExchangeDeclarePassive(string exchange)
        {
            inner.ExchangeDeclarePassive(exchange);
        }

        public void ExchangeDelete(string exchange, bool ifUnused)
        {
            throw new NotImplementedException();
        }

        public void ExchangeDeleteNoWait(string exchange, bool ifUnused)
        {
            throw new NotImplementedException();
        }

        public void ExchangeUnbind(string destination, string source, string routingKey, IDictionary<string, object> arguments)
        {
            throw new NotImplementedException();
        }

        public void ExchangeUnbindNoWait(string destination, string source, string routingKey, IDictionary<string, object> arguments)
        {
            throw new NotImplementedException();
        }

        public uint MessageCount(string queue)
        {
            throw new NotImplementedException();
        }

        public void QueueBind(string queue, string exchange, string routingKey, IDictionary<string, object> arguments)
        {
            inner.QueueBind(queue, exchange, routingKey, arguments);
        }

        public void QueueBindNoWait(string queue, string exchange, string routingKey, IDictionary<string, object> arguments)
        {
            inner.QueueBindNoWait(queue, exchange, routingKey, arguments);
        }

        public QueueDeclareOk QueueDeclare(string queue, bool durable, bool exclusive, bool autoDelete, IDictionary<string, object> arguments)
        {
            return inner.QueueDeclare(queue, durable, exclusive, autoDelete, arguments);
        }

        public void QueueDeclareNoWait(string queue, bool durable, bool exclusive, bool autoDelete, IDictionary<string, object> arguments)
        {
            inner.QueueDeclareNoWait(queue, durable, exclusive, autoDelete, arguments);
        }

        public QueueDeclareOk QueueDeclarePassive(string queue)
        {
            return inner.QueueDeclarePassive(queue);
        }

        public uint QueueDelete(string queue, bool ifUnused, bool ifEmpty)
        {
            throw new NotImplementedException();
        }

        public void QueueDeleteNoWait(string queue, bool ifUnused, bool ifEmpty)
        {
            throw new NotImplementedException();
        }

        public uint QueuePurge(string queue)
        {
            throw new NotImplementedException();
        }

        public void QueueUnbind(string queue, string exchange, string routingKey, IDictionary<string, object> arguments)
        {
            throw new NotImplementedException();
        }

        public void TxCommit()
        {
            inner.TxCommit();
        }

        public void TxRollback()
        {
            inner.TxRollback();
        }

        public void TxSelect()
        {
            inner.TxSelect();
        }

        public bool WaitForConfirms()
        {
           return inner.WaitForConfirms();
        }

        public bool WaitForConfirms(TimeSpan timeout)
        {
            return inner.WaitForConfirms(timeout);
        }

        public bool WaitForConfirms(TimeSpan timeout, out bool timedOut)
        {
            return inner.WaitForConfirms(timeout, out timedOut);
        }

        public void WaitForConfirmsOrDie()
        {
            inner.WaitForConfirmsOrDie();
        }

        public void WaitForConfirmsOrDie(TimeSpan timeout)
        {
            inner.WaitForConfirmsOrDie(timeout);
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
                }
                // TODO: 释放未托管的资源(未托管的对象)并在以下内容中替代终结器。
                // TODO: 将大型字段设置为 null。
                pooling.CheckIn(inner);
                this.pooling = null;
                this.inner = null;
                disposedValue = true;
            }
        }

        // TODO: 仅当以上 Dispose(bool disposing) 拥有用于释放未托管资源的代码时才替代终结器。
        ~ChannelWrapper()
        {
            // 请勿更改此代码。将清理代码放入以上 Dispose(bool disposing) 中。
            Dispose(false);
        }

        // 添加此代码以正确实现可处置模式。
        public void Dispose()
        {
            // 请勿更改此代码。将清理代码放入以上 Dispose(bool disposing) 中。
            Dispose(true);
            // TODO: 如果在以上内容中替代了终结器，则取消注释以下行。
             GC.SuppressFinalize(this);
        }
        #endregion

    }
}

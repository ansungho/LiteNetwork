﻿using LiteNetwork.Common.Internal;
using Microsoft.Extensions.ObjectPool;
using System.Net.Sockets;

namespace LiteNetwork.Server.Internal
{
    internal class LiteServerSender : LiteSender
    {
        private readonly ObjectPool<SocketAsyncEventArgs> _writePool;

        /// <summary>
        /// Creates a new <see cref="LiteServerSender"/> instance.
        /// </summary>
        public LiteServerSender()
        {
            _writePool = ObjectPool.Create<SocketAsyncEventArgs>();
        }

        protected override void ClearSocketEvent(SocketAsyncEventArgs socketAsyncEvent)
        {
            socketAsyncEvent.SetBuffer(null, 0, 0);
            socketAsyncEvent.Completed -= OnSendCompleted;

            _writePool.Return(socketAsyncEvent);
        }

        protected override SocketAsyncEventArgs GetSocketEvent()
        {
            SocketAsyncEventArgs socketAsyncEvent = _writePool.Get();
            socketAsyncEvent.Completed += OnSendCompleted;

            return socketAsyncEvent;
        }
    }
}

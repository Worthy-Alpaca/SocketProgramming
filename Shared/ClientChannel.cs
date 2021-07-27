using System;
using System.Collections.Generic;
using System.Text;
using System.Net;
using System.Threading.Tasks;
using System.Net.Sockets;

namespace Shared
{
    public class ClientChannel<TProtocol, TMessageType> : Channel<TProtocol, TMessageType>
        where TProtocol : Protocol<TMessageType>, new()
    {
        public async Task ConnectAsync(IPEndPoint endPoint)
        {
            var socket = new Socket(endPoint.AddressFamily, SocketType.Stream, ProtocolType.Tcp);

            await socket.ConnectAsync(endPoint).ConfigureAwait(false);

            Attach(socket);
        }
    }
}

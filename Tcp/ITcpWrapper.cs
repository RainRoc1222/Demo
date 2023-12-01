using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CommunicationProtocol.WpfApp.Tcp
{
    public interface ITcpWrapper
    {
        void Connect();
        void Disconnect();
        void SendMessage(string message);
        string ReadMessage();
        event EventHandler<bool> ConnectionChagned;
    }
}

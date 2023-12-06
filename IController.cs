using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CommunicationProtocol.WpfApp
{
    public interface IController
    {
        bool IsConnected { get;  set; }
        void Connect();
        void Disconnect();
        void SendMessage(string message);
    }
}

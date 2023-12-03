using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CommunicationProtocol.WpfApp
{
    public interface IController
    {
        void Connect();
        void Disconnect();
        void SendMessage(string message);
    }
}

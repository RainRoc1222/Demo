using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CommunicationProtocol.WpfApp
{
    public interface ITcpWrapper : INotifyPropertyChanged
    {
        void Connect();
        void Disconnect();
        void SendMessage(string message);
        string ReadMessage();
        event EventHandler<bool> ConnectionChagned;
    }
}

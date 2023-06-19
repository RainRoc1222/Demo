using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MyTcpServerAndClient
{
    public interface ITcpWrapper : INotifyPropertyChanged
    {
        bool IsConnected { get; set; }
        void Connect();
        void Disconnect();
        void SendMessage(string message);
        string ReadMessage();
        EventHandler<bool>ConnectionChagned { get; set; }
    }
}

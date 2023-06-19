using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Net.Sockets;
using System.Text;
using System.Threading.Tasks;

namespace MyTcpServerAndClient
{
    public class TcpManager : INotifyPropertyChanged
    {
        public ITcpWrapper myTcp;
        public bool IsConnected { get; set; }

        public event PropertyChangedEventHandler PropertyChanged;
        public TcpManager(ITcpWrapper tcp)
        {
            myTcp = tcp;
            myTcp.ConnectionChagned += ConnectionChagned;
        }

        private void ConnectionChagned(object sender, bool e)
        {
            IsConnected = e;
        }

        public void Connect()
        {
            myTcp.Connect();
        }
        public void Disconnect()
        {
            myTcp.Disconnect();
        }
        public string ReadMessage()
        {
            return myTcp.ReadMessage();
        }
        public void SendMessage(string message)
        {
            myTcp.SendMessage(message);
        }
    }
}

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
        private ITcpWrapper myTcp;
        public bool IsConnected { get; set; }
        public bool IsRunning { get; set; }

        public event PropertyChangedEventHandler PropertyChanged;
        public TcpManager(ITcpWrapper tcp)
        {
            myTcp = tcp;
            myTcp.ConnectionChagned += ConnectionChagned;
        }

        private void ConnectionChagned(object sender, bool e)
        {
            IsConnected = e;

            if (sender is MyTcpClient)
            {
                IsRunning = e;
            }
        }

        public void Connect()
        {
            myTcp.Connect();
            IsRunning = true;
        }
        public void Disconnect()
        {
            myTcp.Disconnect();
            IsRunning = false;
            if(myTcp is MyTcpClient)
            {
                IsConnected = false;
            }
        }
        public string ReadMessage()
        {
            return myTcp.ReadMessage();
        }
        public void SendMessage(string message)
        {
            if (!string.IsNullOrWhiteSpace(message))
            {
                myTcp.SendMessage(message);
            }
        }
    }
}

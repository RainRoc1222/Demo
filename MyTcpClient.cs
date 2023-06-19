using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Net.Http;
using System.Net.Sockets;
using System.Runtime.InteropServices;
using System.Text;
using System.Text.RegularExpressions;
using System.Threading.Tasks;
using System.Windows;

namespace MyTcpServerAndClient
{
    public class MyTcpClient : ITcpWrapper
    {
        public TcpClient myTcpClient;

        public event PropertyChangedEventHandler PropertyChanged;

        public string IpAddress { get; private set; }
        public int Port { get; private set; }
        public bool IsConnected { get; set; }
        public EventHandler<bool> ConnectionChagned{get; set;}  

        public MyTcpClient(string ip, int port)
        {
            IpAddress = ip;
            Port = port;
        }
        private void OnIsConnectedChanged()
        {
            ConnectionChagned.Invoke(this, IsConnected);
        }
        private void CheckConnection()
        {
            Task.Run(() =>
            {
                while (true)
                {
                    myTcpClient.Client.Poll(0, SelectMode.SelectRead);
                    byte[] testRecByte = new byte[1];
                    if (myTcpClient.Client.Receive(testRecByte, SocketFlags.Peek) == 0)
                    {
                        IsConnected = false;
                    }

                    Task.Delay(10);
                }
            });
        }

        public void Connect()
        {
            try
            {
                myTcpClient = new TcpClient();
                myTcpClient.Connect(IpAddress, Port);
                IsConnected = myTcpClient.Connected;
                CheckConnection();
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.ToString());
            }
        }

        public void Disconnect()
        {
            try
            {
                myTcpClient.GetStream().Close();
                IsConnected = myTcpClient.Connected;
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.ToString());
            }
        }

        public void SendMessage(string message)
        {
            try
            {
                var data = Encoding.ASCII.GetBytes(message);
                var stream = myTcpClient.GetStream();
                stream.Write(data, 0, data.Length);
            }
            catch (Exception ex)
            {
                IsConnected = false;
                MessageBox.Show(ex.ToString());
            }
        }

        public string ReadMessage()
        {
            byte[] receiveBytes = new byte[myTcpClient.ReceiveBufferSize];
            var stream = myTcpClient.GetStream();
            var message = stream.Read(receiveBytes, 0, receiveBytes.Length);
            return Encoding.ASCII.GetString(receiveBytes, 0, message) + Environment.NewLine;
        }
    }
}

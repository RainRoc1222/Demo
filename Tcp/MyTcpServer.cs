﻿using CommunicationProtocol.WpfApp.Properties;
using CommunicationProtocol.WpfApp.Tcp;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.IO;
using System.Linq;
using System.Net;
using System.Net.Sockets;
using System.Text;
using System.Threading.Tasks;
using System.Windows;

namespace CommunicationProtocol.WpfApp
{
    public class MyTcpServer :ITcpWrapper
    {
        private TcpListener myTcpListener;
        private TcpClient myTcpClient;
        public string IpAddress { get; private set; }
        public int Port { get; private set; }
        public event EventHandler<bool> ConnectionChagned;
        public event PropertyChangedEventHandler PropertyChanged;
        public MyTcpServer(TcpSettings settings)
        {
            IpAddress = settings.IPAddress;
            Port = settings.Port;
        }

        public void Connect()
        {
            try
            {
                var ip = IPAddress.Parse(IpAddress);
                myTcpListener = new TcpListener(ip, Port);
                myTcpListener.Start();
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
                myTcpListener.Stop();
                ConnectionChagned.Invoke(this, false);
                RemoveClient();
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.ToString());
            }
        }
        private void RemoveClient()
        {
            if (myTcpClient != null)
            {
                myTcpClient.GetStream().Close();
                myTcpClient.Close();
                myTcpClient = null;
            }
        }

        public string ReadMessage()
        {
            byte[] receiveBytes = new byte[myTcpClient.ReceiveBufferSize];
            var stream = myTcpClient.GetStream();
            var message = stream.Read(receiveBytes, 0, receiveBytes.Length);
            return Encoding.ASCII.GetString(receiveBytes, 0, message) + Environment.NewLine;
        }

        public void SendMessage(string message)
        {
            try
            {
                var stream = myTcpClient.GetStream();
                var data = Encoding.ASCII.GetBytes(message);
                stream.Write(data, 0, data.Length);
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.ToString());
            }
        }
        private void Check()
        {
            myTcpClient.Client.Poll(0, SelectMode.SelectRead);
            byte[] testRecByte = new byte[1];
            if (myTcpClient.Client.Receive(testRecByte, SocketFlags.Peek) == 0)
            {
                Disconnect();
            }
        }

        private void CheckConnection()
        {
            Task.Run(() =>
            {
                while (true)
                {
                    if (myTcpClient == null)
                    {
                        myTcpClient = myTcpListener.AcceptTcpClient();
                        if (myTcpClient.Connected)
                        {
                            ConnectionChagned?.Invoke(this, true);
                        }
                    }
                    else
                    {
                        try
                        {
                            Check();
                        }
                        catch (Exception)
                        {
                            ConnectionChagned?.Invoke(this, false);
                        }
                    }

                    Task.Delay(10);
                }
            });
        }

    }
}

using CommunicationProtocol.WpfApp.Tcp;
using System;
using System.ComponentModel;
using System.Net;
using System.Net.Sockets;
using System.Text;
using System.Threading.Tasks;
using static System.Net.Mime.MediaTypeNames;

namespace CommunicationProtocol.WpfApp
{
    public class MyTcpClient : ITcpWrapper, INotifyPropertyChanging
    {
        public TcpClient myTcpClient;
        private bool myIsConnected;
        public string IpAddress { get; private set; }
        public int Port { get; private set; }
        public event EventHandler<bool> ConnectionChagned;
        public event PropertyChangingEventHandler PropertyChanging;

        public MyTcpClient(TcpSettings settings)
        {
            IpAddress = settings.IPAddress;
            Port = settings.Port;
        }

        private void Check()
        {
            myIsConnected = true;
            ConnectionChagned.Invoke(this, myIsConnected);
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
                while (myIsConnected)
                {
                    Check();
                    Task.Delay(150);
                }
            });
        }


        public void Connect()
        {
            try
            {
                myTcpClient = new TcpClient();
                myTcpClient.Connect(IpAddress, Port);
                myIsConnected = true;
                CheckConnection();
            }
            catch (Exception ex)
            {
                myIsConnected = false;
                ConnectionChagned.Invoke(this, myIsConnected);
            }
        }

        public void Disconnect()
        {
            try
            {
                myTcpClient.GetStream().Close();
                myTcpClient.Close();
                myIsConnected = false;
            }
            catch (Exception ex)
            {
            }

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
                myIsConnected = false;
                ConnectionChagned.Invoke(this, myIsConnected);
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

using CommunicationProtocol.WpfApp.Tcp;
using System;
using System.ComponentModel;
using System.Net.Sockets;
using System.Text;
using System.Threading.Tasks;

namespace CommunicationProtocol.WpfApp
{
    public class MyTcpClient : ITcpWrapper,INotifyPropertyChanging
    {
        public TcpClient myTcpClient;
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
                    try
                    {
                        Check();
                    }
                    catch (Exception)
                    {
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
                ConnectionChagned.Invoke(this, true);
                CheckConnection();
            }
            catch (Exception ex)
            {
                ConnectionChagned.Invoke(this, false);
            }
        }

        public void Disconnect()
        {
            try
            {
                myTcpClient.GetStream().Close();
                myTcpClient.Close();
            }
            catch (Exception ex)
            {
            }
            ConnectionChagned.Invoke(this, false);
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
                ConnectionChagned.Invoke(this, false);
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

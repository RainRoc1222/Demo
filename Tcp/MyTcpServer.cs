using CommunicationProtocol.WpfApp.Tcp;
using System;
using System.ComponentModel;
using System.Net;
using System.Net.Sockets;
using System.Text;
using System.Threading.Tasks;

namespace CommunicationProtocol.WpfApp
{
    public class MyTcpServer : ITcpWrapper, INotifyPropertyChanged
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
                Console.WriteLine(ex.ToString());
            }
        }

        public void Disconnect()
        {
            try
            {
                myTcpListener.Stop();
                RemoveClient();
            }
            catch (Exception ex)
            {
                Console.WriteLine(ex.ToString());
            }
        }
        private void RemoveClient()
        {
            if (myTcpClient != null)
            {
                myTcpClient.GetStream().Close();
                myTcpClient.Close();
                myTcpClient = null;
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
                Console.WriteLine(ex.ToString());
            }
        }
        private void Check()
        {
            try
            {
                myTcpClient.Client.Poll(0, SelectMode.SelectRead);
                byte[] testRecByte = new byte[1];
                if (myTcpClient.Client.Receive(testRecByte, SocketFlags.Peek) == 0)
                {
                    RemoveClient();
                    ConnectionChagned.Invoke(this, false);
                }
            }
            catch (Exception ex)
            {
                ConnectionChagned?.Invoke(this, false);
                Console.WriteLine(ex.ToString());
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
                        Check();
                    }

                    Task.Delay(10);
                }
            });
        }

    }
}

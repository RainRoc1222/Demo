using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Net;
using System.Net.Sockets;
using System.Text;
using System.Text.RegularExpressions;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Navigation;
using System.Windows.Shapes;

namespace MyTcpServerAndClient
{
    /// <summary>
    /// MainWindow.xaml 的互動邏輯
    /// </summary>
    public partial class MainWindow : Window, INotifyPropertyChanged
    {
        public TcpManager TcpManager { get; set; }
        public string IpAddress { get; set; } = "127.0.0.1";
        public int Port { get; set; } = 6101;
        public string Message { get; set; }
        public string ReceiveMessage { get; set; }

        public event PropertyChangedEventHandler PropertyChanged;
        public MainWindow()
        {
            InitializeComponent();
        }
        private void Connect(object sender, RoutedEventArgs e)
        {
            TcpManager.Connect();
        }
        private void Disconnect(object sender, RoutedEventArgs e)
        {
            TcpManager.Disconnect();
        }

        private void SendMessage(object sender, RoutedEventArgs e)
        {
            TcpManager.SendMessage(Message);
        }

        private void Window_Loaded(object sender, RoutedEventArgs e)
        {
            Task.Run(() =>
            {
                while (true)
                {
                    if (TcpManager != null && TcpManager.IsConnected)
                    {
                        try
                        {
                            ReceiveMessage += TcpManager.ReadMessage();
                        }
                        catch (Exception ex)
                        {
                        }
                    }
                    Task.Delay(10);
                }
            });
        }


        private void RadioButton_Checked(object sender, RoutedEventArgs e)
        {
            ChangeTcpManager((RadioButton)sender);
        }

        private void ChangeTcpManager(RadioButton ratioButton)
        {
            if (TcpManager != null && TcpManager.IsConnected)
            {
                TcpManager.Disconnect();
            }
            switch (ratioButton.Content)
            {
                case "Server":
                    TcpManager = new TcpManager(new MyTcpServer(IpAddress, Port));
                    break;
                default:
                    TcpManager = new TcpManager(new MyTcpClient(IpAddress, Port));
                    break;
            }
        }

        private void ClearMessage(object sender, RoutedEventArgs e)
        {
            ReceiveMessage = string.Empty;
        }
    }
}

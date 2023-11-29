using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Net;
using System.Text;
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

namespace CommunicationProtocol.WpfApp
{
    /// <summary>
    /// TcpControl.xaml 的互動邏輯
    /// </summary>
    public partial class TcpControl : UserControl, INotifyPropertyChanged
    {
        public TcpManager TcpManager { get; set; }
        public string IpAddress { get; set; }
        public int Port { get; set; }
        public string Message { get; set; }
        public event PropertyChangedEventHandler PropertyChanged;

        public TcpControl()
        {
            InitializeComponent();
            StartReceiveMessage();
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
            var message = $"{DateTime.Now:yyyy/MM/dd HH:mm:ss}     OUT:    {Message}{Environment.NewLine}{Environment.NewLine}";
            TextLogs.AppendText(message);
            TextLogs.ScrollToEnd();
            TcpManager.SendMessage(Message);
            Message = string.Empty;
        }

        private void StartReceiveMessage()
        {
            Task.Run(() =>
            {
                while (true)
                {
                    if (TcpManager != null && TcpManager.IsConnected)
                    {
                        try
                        {
                            UpdateUI();
                        }
                        catch (Exception)
                        {
                        }
                    }
                    Task.Delay(10);
                }
            });
        }

        private void UpdateUI()
        {
            var message = $"{DateTime.Now:yyyy/MM/dd HH:mm:ss}     IN:        {TcpManager.ReadMessage()}{Environment.NewLine}";

            Dispatcher.InvokeAsync(new Action(() =>
            {
                if (TextLogs.Text.Length > 30000)
                {
                    TextLogs.Clear();
                }

                TextLogs.AppendText(message);
                TextLogs.ScrollToEnd();
            }));


        }
        private void RadioButton_Checked(object sender, RoutedEventArgs e)
        {
            ChangeTcpManager((RadioButton)sender);
        }

        private void ChangeTcpManager(RadioButton ratioButton)
        {
            if (TcpManager != null && TcpManager.IsConnected) TcpManager.Disconnect();

            InitializeIPAddress();

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
        private void InitializeIPAddress()
        {
            IpAddress = AppSettingsMgt.AppSettings.IPAddress;
            Port = AppSettingsMgt.AppSettings.Port;
        }
        private void ClearMessage(object sender, RoutedEventArgs e)
        {
            TextLogs.Clear();
        }

        private void UserControl_Loaded(object sender, RoutedEventArgs e)
        {
            InitializeIPAddress();
        }
    }
}

using CommunicationProtocol.WpfApp.Modbus;
using CommunicationProtocol.WpfApp.Tcp;
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
using static CommunicationProtocol.WpfApp.TcpControl;

namespace CommunicationProtocol.WpfApp
{
    /// <summary>
    /// TcpControl.xaml 的互動邏輯
    /// </summary>
    public partial class TcpControl : UserControl, INotifyPropertyChanged
    {
        public TcpController TcpController { get; set; }
        public TcpSettings TcpSettings { get; set; }
        private TcpType myTcpType;
        public string Message { get; set; }
        public event PropertyChangedEventHandler PropertyChanged;

        public TcpControl()
        {
            InitializeComponent();
            StartReceiveMessage();
            ButtonControl.Send += ButtonControl_Send;
            ButtonControl.Clear += ButtonControl_Clear;
        }

        private void ButtonControl_Clear(object sender, EventArgs e)
        {
            TextLogs.Clear();
        }

        private void ButtonControl_Send(object sender, string e)
        {
            TextLogs.AppendText(e);
            TextLogs.ScrollToEnd();
            Message = string.Empty;
        }

        private void StartReceiveMessage()
        {
            Task.Run(() =>
            {
                while (true)
                {
                    if (TcpController != null && TcpController.IsConnected)
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
            var receiveMessage = TcpController.ReadMessage();
            if (!string.IsNullOrWhiteSpace(receiveMessage))
            {
                var message = $"{DateTime.Now:yyyy/MM/dd HH:mm:ss}     IN:        {receiveMessage}{Environment.NewLine}";

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
        }
        private void RadioButton_Checked(object sender, RoutedEventArgs e)
        {
            ChangeTcpController((RadioButton)sender);
        }

        private void ChangeTcpController(RadioButton radioButton)
        {
            TcpController?.Disconnect();
            if (TcpSettings == null) TcpSettings = AppSettingsMgt.AppSettings.TcpSettings;
            myTcpType = radioButton.Content.ToString() == "Server" ? TcpType.Server : TcpType.Client;
            InitializeController();
        }

        private void UserControl_Loaded(object sender, RoutedEventArgs e)
        {
            TcpSettings = AppSettingsMgt.AppSettings.TcpSettings;
            TcpSettings.PropertyChanged -= TcpSettings_PropertyChanged;
            TcpSettings.PropertyChanged += TcpSettings_PropertyChanged;
        }

        private void TcpSettings_PropertyChanged(object sender, PropertyChangedEventArgs e)
        {
            InitializeController();
        }

        private void Save_Click(object sender, RoutedEventArgs e)
        {
            AppSettingsMgt.AppSettings.TcpSettings = TcpSettings;
            AppSettingsMgt.Save();
        }
        private void InitializeController()
        {
            switch (myTcpType)
            {
                case TcpType.Server:
                    TcpController = new TcpController(new MyTcpServer(TcpSettings));
                    break;
                default:
                    TcpController = new TcpController(new MyTcpClient(TcpSettings));
                    break;
            }
        }
    }
}

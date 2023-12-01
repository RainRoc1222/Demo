using CommunicationProtocol.WpfApp.Serail_Port;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.ComponentModel;
using System.IO.Ports;
using System.Linq;
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
    /// SerialPort_Control.xaml 的互動邏輯
    /// </summary>
    public partial class SerialPortControl : UserControl, INotifyPropertyChanged
    {
        public string Message { get; set; }
        public string ReceiveMessage { get; set; }
        public SerialPortController SerialPortController { get; set; }
        public event PropertyChangedEventHandler PropertyChanged;

        public SerialPortControl()
        {
            InitializeComponent();
            var settings = AppSettingsMgt.AppSettings.SerialPortSettings;
            SerialPortController = new SerialPortController(settings);
            SerialPortController.ReceiveData += SerialPortController_ReceiveData;
        }

        private void SerialPortController_ReceiveData(object sender, byte[] e)
        {
            ReceiveMessage = Encoding.ASCII.GetString(e);
            UpdateUI();
        }

        private void UpdateUI()
        {
            var message = $"{DateTime.Now:yyyy/MM/dd HH:mm:ss}     IN:        {ReceiveMessage}{Environment.NewLine}";

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
        private void SendMessage(object sender, RoutedEventArgs e)
        {
            if (!string.IsNullOrWhiteSpace(Message))
            {
                var message = $"{DateTime.Now:yyyy/MM/dd HH:mm:ss}     OUT:    {Message}{Environment.NewLine}{Environment.NewLine}";
                TextLogs.AppendText(message);
                TextLogs.ScrollToEnd();
                SerialPortController.SendMessage(Message);
                Message = string.Empty;
            }
        }

        private void Connect(object sender, RoutedEventArgs e)
        {
            SerialPortController.Connect();
        }

        private void Disconnect(object sender, RoutedEventArgs e)
        {
            SerialPortController.Disconnect();
        }

        private void ClearMessage(object sender, RoutedEventArgs e)
        {
            TextLogs.Clear();
        }
    }
}

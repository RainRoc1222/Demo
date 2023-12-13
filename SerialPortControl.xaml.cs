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
        public CollectionSettings CollectionSettings => CollectionSettings.Instance;
        public string Message { get; set; } 
        public string ReceiveMessage { get; set; }
        public SerialPortSettings SelectedSettings { get; set; }
        public SerialPortController SerialPortController { get; set; }
        public event PropertyChangedEventHandler PropertyChanged;

        public SerialPortControl()
        {
            InitializeComponent();
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



        private void UserControl_Loaded(object sender, RoutedEventArgs e)
        {
            SelectedSettings = AppSettingsMgt.AppSettings.SerialPortSettings;
            SerialPortController = new SerialPortController(SelectedSettings);
            SerialPortController.ReceiveData -= SerialPortController_ReceiveData;
            SerialPortController.ReceiveData += SerialPortController_ReceiveData;
        }

    }
}

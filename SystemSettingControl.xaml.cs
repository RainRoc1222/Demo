using System;
using System.Collections.ObjectModel;
using System.ComponentModel;
using System.IO.Ports;
using System.Windows;
using System.Windows.Controls;
using Parity = System.IO.Ports.Parity;
using StopBits = System.IO.Ports.StopBits;

namespace CommunicationProtocol.WpfApp
{
    /// <summary>
    /// SystemSettingControl.xaml 的互動邏輯
    /// </summary>
    public partial class SystemSettingControl : UserControl, INotifyPropertyChanged
    {
        public string IpAddress { get; set; }
        public int Port { get; set; }
        public SerialPortSettings SerialPortSettings { get; set; }
        public ObservableCollection<string> PortNameCollection { get; set; }
        public ObservableCollection<int> BaudRateCollection { get; set; }
        public ObservableCollection<Parity> ParityCollection { get; set; }
        public ObservableCollection<int> DataBitsCollection { get; set; }
        public ObservableCollection<StopBits> StopBitsCollection { get; set; }
        public event PropertyChangedEventHandler PropertyChanged;
        public SystemSettingControl()
        {
            InitializeComponent();
            InitializeSerialPortSetting();
            SerialPortSettings = new SerialPortSettings();
        }

        public void InitializeSerialPortSetting()
        {
            PortNameCollection = GetPortNameCollection();
            BaudRateCollection = new ObservableCollection<int> { 300, 1200, 2400, 9600, 19200, 38400, 115200 };
            ParityCollection = new ObservableCollection<Parity> { Parity.None, Parity.Odd, Parity.Even, Parity.Mark, Parity.Space };
            DataBitsCollection = new ObservableCollection<int> { 5, 6, 7, 8 };
            StopBitsCollection = new ObservableCollection<StopBits>() { StopBits.None, StopBits.One, StopBits.OnePointFive, StopBits.Two };
        }
        private void UserControl_Loaded(object sender, RoutedEventArgs e)
        {
            GetTcpSettings();
            GetSerialPortSettings();
        }

        private ObservableCollection<string> GetPortNameCollection()
        {
            var coms = SerialPort.GetPortNames();
            return new ObservableCollection<string>(coms);
        }
  
        private void GetTcpSettings()
        {
            IpAddress = AppSettingsMgt.AppSettings.TcpSettings.IPAddress;
            Port = AppSettingsMgt.AppSettings.TcpSettings.Port;
        }

        private void GetSerialPortSettings()
        {
            SerialPortSettings = AppSettingsMgt.AppSettings.SerialPortSettings;
        }

        private void TcpSave_Click(object sender, RoutedEventArgs e)
        {
            AppSettingsMgt.AppSettings.TcpSettings.IPAddress = IpAddress;
            AppSettingsMgt.AppSettings.TcpSettings.Port = Port;
            AppSettingsMgt.Save();
        }

        private void SerailPortSave_Click(object sender, RoutedEventArgs e)
        {
            AppSettingsMgt.AppSettings.SerialPortSettings = SerialPortSettings;
            AppSettingsMgt.Save();
        }
    }
}

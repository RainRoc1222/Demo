using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.IO.Ports;
using System.Linq;
using System.Runtime.CompilerServices;
using System.Text;
using System.Threading.Tasks;

namespace CommunicationProtocol.WpfApp
{
    public class AppSettings
    {
        public TcpSettings TcpSettings { get; set; }
        public SerialPortSettings SerialPortSettings { get; set; }
        public SerialPortSettings ModbusSettings { get; set; }
        public AppSettings()
        {
            TcpSettings = new TcpSettings();
            SerialPortSettings = new SerialPortSettings();
            ModbusSettings = new SerialPortSettings();
        }
    }

    public class TcpSettings : INotifyPropertyChanged
    {
        public string IPAddress { get; set; }
        public int Port { get; set; }
        protected void OnPropertyChanged([CallerMemberName] string name = null)
        {
            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(name));
        }

        public event PropertyChangedEventHandler PropertyChanged;
    }
    public class SerialPortSettings:INotifyPropertyChanged
    {
        public string PortName { get; set; }
        public int BaudRate { get; set; }
        public Parity Parity { get; set; }
        public int DataBits { get; set; }
        public StopBits StopBits { get; set; }
        protected void OnPropertyChanged([CallerMemberName] string name = null)
        {
            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(name));
        }

        public event PropertyChangedEventHandler PropertyChanged;
    }
}

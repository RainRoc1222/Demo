using System;
using System.Collections.Generic;
using System.IO.Ports;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CommunicationProtocol.WpfApp
{
    public class AppSettings
    {
        public TcpSettings TcpSettings { get; set; }
        public SerialPortSettings SerialPortSettings { get; set; }
        public AppSettings()
        {
            TcpSettings = new TcpSettings();
            SerialPortSettings = new SerialPortSettings();
        }
    }

    public class TcpSettings
    {
        public string IPAddress { get; set; }
        public int Port { get; set; }
    }
    public class SerialPortSettings
    {
        public string PortName { get; set; }
        public int BaudRate { get; set; }
        public Parity Parity { get; set; }
        public int DataBits { get; set; }
        public StopBits StopBits { get; set; }
        public int ReadTimeout { get; set; }
    }
}

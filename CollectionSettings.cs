using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.ComponentModel;
using System.IO.Ports;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CommunicationProtocol.WpfApp
{
    public class CollectionSettings : INotifyPropertyChanged
    {
        public static CollectionSettings Instance { get; } = new CollectionSettings();
        public SerialPortSettings SerialPortSettings { get; set; }
        public ObservableCollection<string> PortNameCollection { get; set; }
        public ObservableCollection<int> BaudRateCollection { get; set; }
        public ObservableCollection<Parity> ParityCollection { get; set; }
        public ObservableCollection<int> DataBitsCollection { get; set; }
        public ObservableCollection<StopBits> StopBitsCollection { get; set; }

        public event PropertyChangedEventHandler PropertyChanged;

        public CollectionSettings()
        {
            InitializeSerialPortSetting();
        }
        public void InitializeSerialPortSetting()
        {
            PortNameCollection = GetPortNameCollection();
            BaudRateCollection = new ObservableCollection<int> { 300, 1200, 2400, 9600, 19200, 38400, 115200 };
            ParityCollection = new ObservableCollection<Parity> { Parity.None, Parity.Odd, Parity.Even, Parity.Mark, Parity.Space };
            DataBitsCollection = new ObservableCollection<int> { 5, 6, 7, 8 };
            StopBitsCollection = new ObservableCollection<StopBits>() { StopBits.None, StopBits.One, StopBits.OnePointFive, StopBits.Two };
        }
        public ObservableCollection<string> GetPortNameCollection()
        {
            var coms = SerialPort.GetPortNames();
            return new ObservableCollection<string>(coms);
        }

    }
}

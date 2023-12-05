using CommunicationProtocol.WpfApp.Modbus;
using System;
using System.Collections.Generic;
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
    /// ModbusControl.xaml 的互動邏輯
    /// </summary>
    public partial class ModbusControl : UserControl, INotifyPropertyChanged
    {
        public int[] InputSignals { get; set; }
        public int SelectedInputIndex { get; set; }
        public ModbusController ModbusController { get; set; }
        public SerialPortSettings SelectedSettings { get; set; }
        public CollectionSettings CollectionSettings => CollectionSettings.Instance;
        public event PropertyChangedEventHandler PropertyChanged;

        public ModbusControl()
        {
            InitializeComponent();
            InputSignals = new int[64];
        }

        private void Save_Click(object sender, RoutedEventArgs e)
        {
            AppSettingsMgt.AppSettings.ModbusSettings = SelectedSettings;
            AppSettingsMgt.Save();
        }

        private void UserControl_Loaded(object sender, RoutedEventArgs e)
        {
            try
            {
                SelectedSettings = AppSettingsMgt.AppSettings.ModbusSettings;
                var serialPort = new SerialPort()
                {
                    PortName = SelectedSettings.PortName,
                    BaudRate = SelectedSettings.BaudRate,
                    Parity = SelectedSettings.Parity,
                    StopBits = SelectedSettings.StopBits,
                    DataBits = SelectedSettings.DataBits,
                };
                ModbusController = new ModbusController(serialPort, 1);
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.ToString());
            }
        }
    }
}

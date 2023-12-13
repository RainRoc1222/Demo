using CommunicationProtocol.WpfApp.Modbus;
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
    /// ModbusControl.xaml 的互動邏輯
    /// </summary>
    public partial class ModbusControl : UserControl, INotifyPropertyChanged
    {
        public ObservableCollection<Signal> Signals { get; set; }
        public int SelectedIndex { get; set; }
        public ModbusController ModbusController { get; set; }
        public SerialPortSettings SelectedSettings { get; set; }
        public CollectionSettings CollectionSettings => CollectionSettings.Instance;
        public event PropertyChangedEventHandler PropertyChanged;

        private void OnSelectedIndexChanged()
        {
            if (ModbusController != null)
            {
                ModbusController.SelectedIndex = SelectedIndex;
            }
        }

        public ModbusControl()
        {
            InitializeComponent();
            InitialzeSignals();
            ButtonControl.Clear += ButtonControl_Clear;
        }

        private void ButtonControl_Clear(object sender, EventArgs e)
        {
            InitialzeSignals();
        }

        public void InitialzeSignals()
        {
            Signals = new ObservableCollection<Signal>();
            for (ushort i = 0; i < 64; i++)
            {
                Signals.Add(new Signal() { Index = i, RegisterValue = 0 });
            }
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
                SelectedSettings.PropertyChanged -= SelectedSettings_PropertyChanged;
                SelectedSettings.PropertyChanged += SelectedSettings_PropertyChanged;
                InitializeController();
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.ToString());
            }
        }

        private void SelectedSettings_PropertyChanged(object sender, PropertyChangedEventArgs e)
        {
            InitializeController();
        }

        private void ModbusController_ReceiveData(object sender, ObservableCollection<Signal> e)
        {
            Signals = e;
        }

        private void InitializeController()
        {
            ModbusController?.Disconnect();
            ModbusController = new ModbusController(SelectedSettings, 1)
            {
                Signals = Signals
            };
            ModbusController.ReceiveData -= ModbusController_ReceiveData;
            ModbusController.ReceiveData += ModbusController_ReceiveData;
        }
    }
}

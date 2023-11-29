using System;
using System.Collections.Generic;
using System.ComponentModel;
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
    /// SystemSettingControl.xaml 的互動邏輯
    /// </summary>
    public partial class SystemSettingControl : UserControl, INotifyPropertyChanged
    {
        public string IpAddress { get; set; }
        public int Port { get; set; }
        public event PropertyChangedEventHandler PropertyChanged;
        public SystemSettingControl()
        {
            InitializeComponent();
        }


        private void InitializeIpAddress()
        {
            IpAddress = AppSettingsMgt.AppSettings.IPAddress;
            Port = AppSettingsMgt.AppSettings.Port;
        }

        private void Save(object sender, RoutedEventArgs e)
        {
            AppSettingsMgt.Save();
        }

        private void UserControl_Loaded(object sender, RoutedEventArgs e)
        {
            InitializeIpAddress();
        }
    }
}

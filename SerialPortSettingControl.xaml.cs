using System;
using System.Collections.Generic;
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
    /// SerialPortSettingControl.xaml 的互動邏輯
    /// </summary>
    public partial class SerialPortSettingControl : UserControl
    {


        public CollectionSettings CollectionSettings
        {
            get { return (CollectionSettings)GetValue(CollectionSettingsProperty); }
            set { SetValue(CollectionSettingsProperty, value); }
        }

        // Using a DependencyProperty as the backing store for CollectionSettings.  This enables animation, styling, binding, etc...
        public static readonly DependencyProperty CollectionSettingsProperty =
            DependencyProperty.Register("CollectionSettings", typeof(CollectionSettings), typeof(SerialPortSettingControl), new PropertyMetadata());



        public SerialPortSettings SelectedSettings
        {
            get { return (SerialPortSettings)GetValue(SelectedSettingsProperty); }
            set { SetValue(SelectedSettingsProperty, value); }
        }

        // Using a DependencyProperty as the backing store for SelectedSettings.  This enables animation, styling, binding, etc...
        public static readonly DependencyProperty SelectedSettingsProperty =
            DependencyProperty.Register("SelectedSettings", typeof(SerialPortSettings), typeof(SerialPortSettingControl), new PropertyMetadata());

        public SerialPortSettingControl()
        {
            InitializeComponent();
        }

        private void Save_Click(object sender, RoutedEventArgs e)
        {
            AppSettingsMgt.AppSettings.SerialPortSettings = SelectedSettings;
            AppSettingsMgt.Save();
        }
    }
}

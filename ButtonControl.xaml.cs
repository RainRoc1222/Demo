using CommunicationProtocol.WpfApp.Modbus;
using CommunicationProtocol.WpfApp.Serail_Port;
using CommunicationProtocol.WpfApp.Tcp;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.ComponentModel;
using System.Linq;
using System.Net.Sockets;
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
    /// ButtonControl.xaml 的互動邏輯
    /// </summary>
    public partial class ButtonControl : UserControl, INotifyPropertyChanged
    {

     public SerialPortSettings SelectedSettings
        {
            get { return (SerialPortSettings)GetValue(SelectedSettingsProperty); }
            set { SetValue(SelectedSettingsProperty, value); }
        }

        // Using a DependencyProperty as the backing store for SelectedSettings.  This enables animation, styling, binding, etc...
        public static readonly DependencyProperty SelectedSettingsProperty =
            DependencyProperty.Register("SelectedSettings", typeof(SerialPortSettings), typeof(ButtonControl), new PropertyMetadata());


        public int SelectedIndex
        {
            get { return (int)GetValue(SelectedIndexProperty); }
            set { SetValue(SelectedIndexProperty, value); }
        }

        // Using a DependencyProperty as the backing store for SelectedIndex.  This enables animation, styling, binding, etc...
        public static readonly DependencyProperty SelectedIndexProperty =
            DependencyProperty.Register("SelectedIndex", typeof(int), typeof(ButtonControl), new PropertyMetadata());


        public ObservableCollection<Signal> Signals
        {
            get { return (ObservableCollection<Signal>)GetValue(SignalsProperty); }
            set { SetValue(SignalsProperty, value); }
        }

        // Using a DependencyProperty as the backing store for Signals.  This enables animation, styling, binding, etc...
        public static readonly DependencyProperty SignalsProperty =
            DependencyProperty.Register("Signals", typeof(ObservableCollection<Signal>), typeof(ButtonControl), new PropertyMetadata());


        public string Message
        {
            get { return (string)GetValue(MessageProperty); }
            set { SetValue(MessageProperty, value); }
        }

        // Using a DependencyProperty as the backing store for Message.  This enables animation, styling, binding, etc...
        public static readonly DependencyProperty MessageProperty =
            DependencyProperty.Register("Message", typeof(string), typeof(ButtonControl), new PropertyMetadata());


        public IController Controller
        {
            get { return (IController)GetValue(ControllerProperty); }
            set { SetValue(ControllerProperty, value); }
        }

        // Using a DependencyProperty as the backing store for Controller.  This enables animation, styling, binding, etc...
        public static readonly DependencyProperty ControllerProperty =
            DependencyProperty.Register("Controller", typeof(IController), typeof(ButtonControl), new PropertyMetadata());


        public bool IsRunning
        {
            get { return (bool)GetValue(IsRunningProperty); }
            set { SetValue(IsRunningProperty, value); }
        }

        // Using a DependencyProperty as the backing store for IsConnected.  This enables animation, styling, binding, etc...
        public static readonly DependencyProperty IsRunningProperty =
            DependencyProperty.Register("IsRunning", typeof(bool), typeof(ButtonControl), new PropertyMetadata());


        public bool IsConnected
        {
            get { return (bool)GetValue(IsConnectedProperty); }
            set { SetValue(IsConnectedProperty, value); }
        }

        // Using a DependencyProperty as the backing store for IsConnected.  This enables animation, styling, binding, etc...
        public static readonly DependencyProperty IsConnectedProperty =
            DependencyProperty.Register("IsConnected", typeof(bool), typeof(ButtonControl), new PropertyMetadata());



        public string TextLogsText
        {
            get { return (string)GetValue(TextLogsTextProperty); }
            set { SetValue(TextLogsTextProperty, value); }
        }

        // Using a DependencyProperty as the backing store for TextLog.  This enables animation, styling, binding, etc...
        public static readonly DependencyProperty TextLogsTextProperty =
            DependencyProperty.Register("TextLogsText", typeof(string), typeof(ButtonControl), new PropertyMetadata());

        public event EventHandler<string> Send;
        public event EventHandler Clear;
        public event PropertyChangedEventHandler PropertyChanged;

        public ButtonControl()
        {
            InitializeComponent();
        }


        private void Connect_Click(object sender, RoutedEventArgs e)
        {
            Controller.Connect();
        }

        private void Disconnect_Click(object sender, RoutedEventArgs e)
        {
            Controller.Disconnect();
        }

        private void ClearMessage_Click(object sender, RoutedEventArgs e)
        {
            Clear?.Invoke(this, null);
        }

        private void SendMessage_Click(object sender, RoutedEventArgs e)
        {
            if (Controller is ModbusController controller)
            {
                switch (SelectedIndex)
                {
                    case 1:
                        foreach (var signal in Signals)
                        {
                            controller.SendMessage(signal.Index, signal.ColiValue);
                        }
                        break;
                    default:
                        foreach (var signal in Signals)
                        {
                            controller.SendMessage(signal.Index, signal.RegisterValue);
                        }
                        break;
                }
            }
            else
            {
                if (!string.IsNullOrWhiteSpace(Message))
                {
                    var message = $"{DateTime.Now:yyyy/MM/dd HH:mm:ss}     OUT:    {Message}{Environment.NewLine}{Environment.NewLine}";
                    Controller.SendMessage(Message);
                    Send?.Invoke(this, message);
                }
            }
        }

        private void UserControl_Unloaded(object sender, RoutedEventArgs e)
        {
            Controller?.Disconnect();
        }
    }
}

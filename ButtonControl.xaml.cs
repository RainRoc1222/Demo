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
    /// ButtonControl.xaml 的互動邏輯
    /// </summary>
    public partial class ButtonControl : UserControl
    {


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





        public ButtonControl()
        {
            InitializeComponent();
        }


        private void Connect(object sender, RoutedEventArgs e)
        {
            Controller.Connect();
        }

        private void Disconnect(object sender, RoutedEventArgs e)
        {
            Controller.Disconnect();
        }

        private void ClearMessage(object sender, RoutedEventArgs e)
        {
            //TextLogs.Clear();
        }

        private void SendMessage(object sender, RoutedEventArgs e)
        {
            if (!string.IsNullOrWhiteSpace(Message))
            {
                var message = $"{DateTime.Now:yyyy/MM/dd HH:mm:ss}     OUT:    {Message}{Environment.NewLine}{Environment.NewLine}";
                //TextLogs.AppendText(message);
                //TextLogs.ScrollToEnd();
                Controller.SendMessage(Message);
            }
        }
    }
}

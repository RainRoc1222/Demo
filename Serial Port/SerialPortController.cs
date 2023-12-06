using GodSharp.SerialPort;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Diagnostics;
using System.IO.Ports;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;

namespace CommunicationProtocol.WpfApp.Serail_Port
{
    public class SerialPortController : INotifyPropertyChanged,IController
    {
        private SerialConfigOptions mySerialConfigOptions;
        private List<byte> myTempData;
        public GodSerialPort SerialPort { get; set; }
        public string ReceiveMessage { get; set; }
        public bool IsConnected { get; set; }
        public event EventHandler<byte[]> ReceiveData;
        public event PropertyChangedEventHandler PropertyChanged;

        public SerialPortController(SerialPortSettings settings)
        {
            myTempData = new List<byte>();  
            mySerialConfigOptions = new SerialConfigOptions()
            {
                PortName = settings.PortName,
                BaudRate = settings.BaudRate,
                Parity = settings.Parity,
                StopBits = settings.StopBits,
                DataBits = settings.DataBits,
            };
        }


        public void Connect()
        {
            try
            {
                SerialPort = new GodSerialPort(mySerialConfigOptions);
                IsConnected = SerialPort.Open();
                Read();
            }
            catch (Exception ex)
            {
                IsConnected = false;
                MessageBox.Show(ex.ToString());
            }
        }

        public void Disconnect()
        {
            SerialPort.Close();
            IsConnected = false;
        }

        public void SendMessage(string message)
        {
            var data = Encoding.UTF8.GetBytes(message);
            SerialPort.Write(data, 0, data.Length);
        }


        private Task Read()
        {
            return Task.Run(() =>
            {
                while (true)
                {
                    var readBytes = SerialPort.Read();
                    if (readBytes != null)
                    {
                        myTempData.AddRange(readBytes);
                        CheckDataAsync();
                    }

                    Task.Delay(10).Wait();
                }
            });
        }
        private void CheckDataAsync()
        {
            if (myTempData.Count > 0)
            {
                var stxIndex = myTempData.IndexOf(2);
                int extIndex = myTempData.IndexOf(3);

                if (stxIndex != -1 && extIndex != -1)
                {
                    int count = extIndex - stxIndex - 1;
                    var data = new byte[count];
                    Array.Copy(myTempData.ToArray(), stxIndex + 1, data, 0, count);

                    ReceiveData?.Invoke(this, data);
                    myTempData.Clear();
                }
            }
        }
    }
}

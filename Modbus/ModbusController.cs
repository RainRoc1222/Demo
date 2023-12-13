using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.ComponentModel;
using System.IO.Ports;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Markup;
using NModbus;
using NModbus.Serial;
using static System.Net.Mime.MediaTypeNames;

namespace CommunicationProtocol.WpfApp.Modbus
{
    public class ModbusController : IController, INotifyPropertyChanged
    {
        public int SelectedIndex { get; set; }
        public ObservableCollection<Signal> Signals { get; set; }

        private int mySlaveId;

        private IModbusSerialMaster mySerialMaster;
        public SerialPort SerialPort { get; private set; }
        public bool IsConnected { get; set; }

        public event EventHandler<ObservableCollection<Signal>> ReceiveData;
        public event PropertyChangedEventHandler PropertyChanged;


        public ModbusController(SerialPortSettings settings,int slaveId)
        {
            try
            {
                mySlaveId = slaveId;
                SerialPort = new SerialPort()
                {
                    PortName = settings.PortName,
                    BaudRate = settings.BaudRate,
                    Parity = settings.Parity,
                    StopBits = settings.StopBits,
                    DataBits = settings.DataBits,
                };

                var factory = new ModbusFactory();
                mySerialMaster = factory.CreateRtuMaster(SerialPort);
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message);
            }
        }

        public void Connect()
        {
            try
            {
                SerialPort.Open();
                IsConnected = true;
                Read();
            }
            catch (Exception ex)
            {
                IsConnected = false;
                Console.WriteLine(ex.ToString());
            }
        }

        public void Disconnect()
        {
            SerialPort.Close();
            IsConnected = false;
        }
        private Task Read()
        {
            return Task.Run(() =>
            {
                while (IsConnected)
                {
                    GetInputSingals();
                    Task.Delay(10).Wait();
                }
            });
        }

        private void GetInputSingals()
        {

            for (int i = 0; i < 64; i++)
            {
                if (SelectedIndex == 1)
                {
                    Signals[i].ColiValue = GetSingleColiInput(i);
                }
                else
                {
                    Signals[i].RegisterValue = GetSingleRegisterInput(i);
                }

                ReceiveData?.Invoke(this, Signals);
            }
        }

        private ushort GetSingleRegisterInput(int number)
        {
            try
            {
                var resultData = mySerialMaster?.ReadHoldingRegisters((byte)mySlaveId, (ushort)number, 1);
                if (resultData != null && resultData.Any())
                {
                    return resultData.First();
                }
            }
            catch (Exception)
            {
                Console.WriteLine("Read timeout.");
            }

            return 0;
        }

        private bool GetSingleColiInput(int number)
        {
            try
            {
                var resultData = mySerialMaster?.ReadCoils((byte)mySlaveId, (ushort)number, 1);
                if (resultData != null && resultData.Any())
                {
                    return resultData.First();
                }
            }
            catch (Exception)
            {
                Console.WriteLine("Read timeout.");
            }

            return false;
        }

        public void SendMessage(string message)
        {
        }

        public void SendMessage(ushort number, ushort value)
        {
            mySerialMaster?.WriteSingleRegister((byte)mySlaveId, number, value);
        }

        public void SendMessage(ushort number, bool value)
        {
            mySerialMaster?.WriteSingleCoil((byte)mySlaveId, number, value);
        }
    }
}

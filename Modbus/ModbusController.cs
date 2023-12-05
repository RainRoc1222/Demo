using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.IO.Ports;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using NModbus;
using NModbus.Serial;

namespace CommunicationProtocol.WpfApp.Modbus
{
    public class ModbusController : IController,INotifyPropertyChanged
    {
        private List<ushort> myTempData;

        private int mySlaveId;

        private IModbusSerialMaster mySerialMaster;
        public SerialPort SerialPort { get; private set; }
        public bool IsConnected { get;  set; }   

        public event EventHandler<ushort> ReceiveData;
        public event PropertyChangedEventHandler PropertyChanged;

        public ModbusController(SerialPort serialPort, int slaveId)
        {
            try
            {
                myTempData = new List<ushort>();
                mySlaveId = slaveId;
                SerialPort = serialPort;

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
                MessageBox.Show(ex.ToString());
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
                while (true)
                {
                    var readBytes = GetSingleInput(1);
                    if (readBytes != 0)
                    {
                        myTempData.Add(readBytes);
                        CheckDataAsync();
                    }

                    Task.Delay(10).Wait();
                }
            });
        }

        private Task CheckDataAsync()
        {
            return Task.Run(() =>
            {
                if (myTempData.Count > 0)
                {
                    var data = myTempData.First();
                    ReceiveData?.Invoke(this, data);
                    myTempData.Clear();
                }
            });
        }

        private ushort GetSingleInput(int number)
        {
            try
            {
                var resultData = mySerialMaster?.ReadInputRegisters((byte)mySlaveId, (ushort)number, 1);
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

        public void SendMessage(string message)
        {
        }

        public void SendMessage(ushort number, ushort data)
        {
            mySerialMaster?.WriteSingleRegister((byte)mySlaveId,number,data);
        }
    }
}

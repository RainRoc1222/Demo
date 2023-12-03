﻿using CommunicationProtocol.WpfApp.Modbus;
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
            ModbusController = new ModbusController();
        }

        private void Save_Click(object sender, RoutedEventArgs e)
        {
            AppSettingsMgt.AppSettings.ModbusSettings = SelectedSettings;
            AppSettingsMgt.Save();
        }

        private void ClearMessage(object sender, RoutedEventArgs e)
        {

        }

        private void SendMessage(object sender, RoutedEventArgs e)
        {

        }

        private void Disconnect(object sender, RoutedEventArgs e)
        {

        }

        private void Connect(object sender, RoutedEventArgs e)
        {

        }

        private void UserControl_Loaded(object sender, RoutedEventArgs e)
        {
            SelectedSettings = AppSettingsMgt.AppSettings.ModbusSettings;
        }
    }
}
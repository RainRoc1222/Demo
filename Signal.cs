﻿using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CommunicationProtocol.WpfApp
{
    public class Signal : INotifyPropertyChanged
    {
        public ushort Index { get; set; }
        public ushort RegisterValue { get; set; }
        public bool ColiValue { get; set; }
        public event PropertyChangedEventHandler PropertyChanged;
    }
}

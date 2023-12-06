using System;
using System.Collections.ObjectModel;
using System.ComponentModel;
using System.Linq;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Input;

namespace CommunicationProtocol.WpfApp
{
    /// <summary>
    /// MainWindow.xaml 的互動邏輯
    /// </summary>
    public partial class MainWindow : Window, INotifyPropertyChanged
    {
        public ObservableCollection<PageItem> PageItems { get; set; }

        public PageItem SelectedPageItem { get; set; }

        public event PropertyChangedEventHandler PropertyChanged;
        public MainWindow()
        {
            InitializePageItems();
            StateChanged += MainWindow_StateChanged;
        }
        private void MainWindow_StateChanged(object sender, EventArgs e)
        {
            if (WindowState == WindowState.Minimized)
            {
                Hide();
            }
        }

        private void InitializePageItems()
        {
            PageItems = new ObservableCollection<PageItem>()
            {
                new PageItem
                {
                    Name = "TCP/IP",
                    Content = new TcpControl(),
                },
                new PageItem
                {
                    Name = "SerailPort",
                    Content = new SerialPortControl(),
                },
                new PageItem
                {
                    Name = "Modbus",
                    Content = new ModbusControl(),
                },
            };
        }

        private void MainWindow_OnLoaded(object sender, RoutedEventArgs e)
        {
            SelectedPageItem = PageItems.FirstOrDefault();
        }

        private void ListBox_PreviewMouseLeftButtonDown(object sender, MouseButtonEventArgs e)
        {
            var listBoxItem = ItemsControl.ContainerFromElement(sender as ListBox, (DependencyObject)e.OriginalSource) as ListBoxItem;
            var content = listBoxItem?.Content;
            if (content is PageItem pageItem && content != SelectedPageItem)
            {
                SelectedPageItem = pageItem;
            }

            MenuToggleButton.IsChecked = false;

        }

        private void Icon_TrayMouseDoubleClick(object sender, RoutedEventArgs e)
        {
            Show();
            WindowState = WindowState.Normal;
            Topmost = true;
        }

        private void Window_Closed(object sender, EventArgs e)
        {
            CommunicationProtocol.Dispose();
        }
    }
}

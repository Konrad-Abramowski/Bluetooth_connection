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


namespace Bluetooth
{
    /// <summary>
    /// Interaction logic for MainWindow.xaml
    /// </summary>
    public partial class MainWindow : Window
    {
        private BluetoothController bluetoothController;
        public MainWindow()
        {
            InitializeComponent();       
            bluetoothController = new BluetoothController();
            this.DataContext = bluetoothController;
        }

        private void UpdateAdaptersBtn_Click(object sender, RoutedEventArgs e)
        {
            bluetoothController.UpdateAdapters();
        }

        private void SearchForDevicesBtn_Click(object sender, RoutedEventArgs e)
        {
            bluetoothController.SearchForDevices();
        }

        private void PairDeviceBtn_Click(object sender, RoutedEventArgs e)
        {
            bluetoothController.PairDevice();
        }

    }
}




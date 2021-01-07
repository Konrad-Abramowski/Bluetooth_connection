using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.ComponentModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using InTheHand.Net;
using InTheHand.Net.Bluetooth;
using InTheHand.Net.Sockets;

// 32feet.NET

namespace Bluetooth
{
    class BluetoothController : INotifyPropertyChanged
    {
        public ObservableCollection<BluetoothRadio> BluetoothAdapters { get; set; }
        public BluetoothRadio CurrentAdapter
        {
            get { return currentAdapter; }
            set
            {
                currentAdapter = value;
                this.OnPropertyChanged("CurrentAdapter");
            }
        }

        // wybrany adapter
        private BluetoothRadio currentAdapter;

        private BluetoothEndPoint localEndPoint;
        
        private BluetoothClient client;

        // dostępne urządzenia bluetooth
        public ObservableCollection<BluetoothDeviceInfo> BluetoothDevices { get; set; }

        public BluetoothDeviceInfo ChosenDevice
        {
            get { return chosenDevice; }
            set
            {
                chosenDevice = value;
                this.OnPropertyChanged("ChosenDevice");
            }
        }
        private BluetoothDeviceInfo chosenDevice;


        public BluetoothController()
        {
            BluetoothAdapters = new ObservableCollection<BluetoothRadio>();
            BluetoothDevices = new ObservableCollection<BluetoothDeviceInfo>();
        }

        public void UpdateAdapters()
        {
            BluetoothAdapters.Clear();
            BluetoothRadio[] temp = BluetoothRadio.AllRadios;

            // przepisanie dostępnych adapterów do BluetoothAdapters
            foreach (BluetoothRadio adapter in temp)
            {
                BluetoothAdapters.Add(adapter);
            }

            if (BluetoothAdapters.Any())
            {
                CurrentAdapter = BluetoothAdapters[0];
            }
            else
            {
                MessageBox.Show("Nie znaleziono adapterów bluetooth", "Błąd", MessageBoxButton.OK, MessageBoxImage.Error);
            }


        }

        public void SearchForDevices()
        {
            // urządzenia znalezione w poprzednim wyszukiwaniu nie muszą być aktywne przy aktualnym wyszukaniu
            BluetoothDevices.Clear();

            // utworzenie endpointu (adres hosta i port usługi (emulacja portu szeregowego))
            localEndPoint = new BluetoothEndPoint(CurrentAdapter.LocalAddress, BluetoothService.SerialPort);
            client = new BluetoothClient(localEndPoint);

            // przepisanie znalezionych urządzeń za pomocą client.DiscoverDevices() do BluetoothDevice
            foreach (BluetoothDeviceInfo device in client.DiscoverDevices())
            {
                BluetoothDevices.Add(device);
            }

            if (BluetoothDevices.Any())
            {
                ChosenDevice = BluetoothDevices[0];
            }
            else
            {
                MessageBox.Show("Nie znaleziono urządzeń bluetooth", "Błąd", MessageBoxButton.OK, MessageBoxImage.Error);
            }
        }
        public void PairDevice()
        {
            try
            {
                BluetoothSecurity.PairRequest(ChosenDevice.DeviceAddress, "123456");
            }
            catch
            {
                Console.WriteLine("Nie udało się sparować z urządzeniem: " + ChosenDevice.DeviceName.ToString());
            }
        }       

        public event PropertyChangedEventHandler PropertyChanged;

        protected void OnPropertyChanged(string propertyName)
        {
            PropertyChangedEventHandler handler = this.PropertyChanged;
            if (handler != null)
            {
                var e = new PropertyChangedEventArgs(propertyName);
                handler(this, e);
            }
        }
    }

}
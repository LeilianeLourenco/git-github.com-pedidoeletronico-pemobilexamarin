using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Xamarin.HLP.Mobile.AppPE.Droid.Services;
using Xamarin.HLP.Mobile.AppPE.Services;

[assembly: Xamarin.Forms.Dependency(typeof(BluetoothLE))]
namespace Xamarin.HLP.Mobile.AppPE.Droid.Services
{
    public class BluetoothLE : IBluetoothLE
    {
        public static BluetoothManager bluetoothManager = new BluetoothManager();

        //public List<KeyValuePair<string, string>> getDevices()
        //{
        //    var devices = bluetoothManager.getAllPairedDevices();

        //    List<KeyValuePair<string, string>> lretorno = new List<KeyValuePair<string, string>>();

        //    foreach (var bluetoothDevice in devices)
        //    {
        //        lretorno.Add(new KeyValuePair<string, string>(bluetoothDevice.Name, bluetoothDevice.Type.ToString()));
        //    }

        //    return lretorno;
        //}

        public string GetNameDevice()
        {
            return bluetoothManager.currentDevice.Name;
        }

        public bool Connect()
        {
            try
            {
                var retorno = bluetoothManager.opneDeviceConnection();

                return retorno;
            }
            catch (Exception ex)
            {
                return false;
            }

        }

        public bool EnsureConnected()
        {
            return bluetoothManager.EnsureConnected();
        }

        public void Write(string valor, string position)
        {
            bluetoothManager.Write(valor, position);

            if (position.ToUpper() == "CENTER")
            {
                bluetoothManager.Write("", "left");
            }
        }

        public void Close()
        {
            bluetoothManager.closeAll();
        }
    }
}
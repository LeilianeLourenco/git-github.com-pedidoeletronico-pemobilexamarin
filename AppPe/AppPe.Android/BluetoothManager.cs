using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Text;
using System.Threading.Tasks;
using Android.App;
using Android.Bluetooth;
using Android.Content;
using Android.Graphics;
using Android.OS;
using Android.Runtime;
using Android.Util;
using Android.Views;
using Android.Widget;
using Java.IO;
using Java.Lang;
using Java.Util;
using Xamarin.Forms;
using Xamarin.HLP.Mobile.AppPE.Droid.Services;
using Exception = System.Exception;
using IOException = Java.IO.IOException;
using String = System.String;

namespace Xamarin.HLP.Mobile.AppPE.Droid
{
    public class BluetoothManager
    {
        // unique ID witch hel us connect to any device
        private const string UuidUniverseProfile = "0001101-0000-1000-8000-00805F9B34FB";
        // get input/output stream of this cominication
        private BluetoothSocket mSocket;
        // convert byte[] to readable strings
        private BufferedReader reader;
        private System.IO.Stream mStream;
        private InputStreamReader mReader;
        public List<BluetoothDevice> lBluetoothDevice { get; set; }

        public BluetoothManager()
        {
            reader = null;
        }


        private List<BluetoothDevice> getAllPairedDevices()
        {
            lBluetoothDevice = new List<BluetoothDevice>();
            BluetoothAdapter btAdapter = BluetoothAdapter.DefaultAdapter;
            var devices = btAdapter.BondedDevices;

            if (devices != null && devices.Count > 0)
            {

                foreach (BluetoothDevice mDevice in devices)
                {
                    lBluetoothDevice.Add(mDevice);
                }
            }
            return lBluetoothDevice;
        }

        public BluetoothDevice currentDevice { get; set; }

        public bool opneDeviceConnection()
        {
            try
            {
                BluetoothDevice objPrinter = null;

                var devices = getAllPairedDevices();

                var uuid = UUID.FromString(UuidUniverseProfile);

                if (devices.Any())
                {
                    foreach (var bluetoothDevice in devices)
                    {
                        foreach (var parcelUuid in bluetoothDevice.GetUuids())
                        {
                            if (parcelUuid.Uuid.ToString() == uuid.ToString())
                            {

                                objPrinter = bluetoothDevice;
                                try
                                {
                                    mSocket = currentDevice.CreateRfcommSocketToServiceRecord(uuid);
                                    mSocket.Connect();
                                    break;
                                }
                                catch
                                {
                                    continue;
                                }

                            }
                        }

                    }

                    if (objPrinter != null)
                    {
                        if (objPrinter.Name != currentDevice?.Name)
                        {
                            currentDevice = objPrinter;
                            if (currentDevice != null)
                            {
                                mSocket = currentDevice.CreateRfcommSocketToServiceRecord(uuid);
                                mSocket.Connect();
                                mStream = mSocket.InputStream;
                                mReader = new InputStreamReader(mStream);
                                reader = new BufferedReader(mReader);
                                return true;
                            }
                        }
                        return true;
                    }
                }
                closeAll();
                currentDevice = null;
                return false;
            }
            catch (IOException e)
            {
                close(mSocket);
                close(mStream);
                close(mReader);
                return false;
            }
        }

        public void close(IDisposable aConnectedObject)
        {
            if (aConnectedObject == null) return;

            try
            {
                aConnectedObject.Dispose();
            }
            catch (Exception)
            {
                throw;
            }
            aConnectedObject = null;
        }

        public void closeAll()
        {
            try
            {
                mReader?.Close();
                mStream?.Close();
                mSocket?.Close();
            }
            catch (Exception ex)
            {
            }
            finally
            {
                mReader = null;
                mStream = null;
                mSocket = null;
            }
        }
        public bool EnsureConnected()
        {
            if (mSocket == null || !mSocket.IsConnected)
            {
                return opneDeviceConnection(); // tenta abrir a conexão
            }
            return true;
        }

        public void Write(string xValor, string position)
        {
            try
            {
                if (!EnsureConnected())
                {
                    throw new IOException("Não foi possível conectar à impressora");
                }

                // alinhamento
                if (string.IsNullOrEmpty(position) || position.ToUpper() == "LEFT")
                {
                    mSocket.OutputStream.Write(new byte[] { 0x1b, 0x61, 0x00 }, 0, 3);
                }
                else if (position.ToUpper() == "RIGHT")
                {
                    mSocket.OutputStream.Write(new byte[] { 0x1b, 0x61, 0x02 }, 0, 3);
                }
                else if (position.ToUpper() == "CENTER")
                {
                    mSocket.OutputStream.Write(new byte[] { 0x1b, (byte)'a', 0x01 }, 0, 3);
                }

                var xprint = Encoding.ASCII.GetBytes(xValor);
                mSocket.OutputStream.Write(xprint, 0, xprint.Length);
            }
            catch (IOException ex)
            {
                // socket quebrado, fecha tudo para tentar reconectar na próxima impressão
                closeAll();
                throw;
            }
        }
    }
}
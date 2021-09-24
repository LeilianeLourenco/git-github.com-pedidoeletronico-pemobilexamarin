using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Xamarin.HLP.Mobile.AppPE.Services
{
    public interface IBluetoothLE
    {

        string GetNameDevice();
        bool Connect();

        /// Escrever texto
        /// </summary>
        /// <param name="valor">valor a ser escrito</param>
        /// <param name="position">Posição no papel left - right - center</param>
        void Write(string valor, string position);


        void Close();



    }
}

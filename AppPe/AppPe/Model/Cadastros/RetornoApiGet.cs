using System;
using System.Collections.Generic;

namespace Xamarin.HLP.Mobile.AppPE.Model.Cadastros
{
    public class RetornoApiGet<T> where T : class
    {
        public RetornoApiGet()
        {
            isValid = false;
            xMessage = "FALHOU";
        }
        public bool isValid { get; set; }
        public DateTime dtServer { get; set; }
        public string xMessage { get; set; }
        public T retorno { get; set; }
        public List<T> Lretorno { get; set; }
        public int IdNuvem { get; set; }
    }
}

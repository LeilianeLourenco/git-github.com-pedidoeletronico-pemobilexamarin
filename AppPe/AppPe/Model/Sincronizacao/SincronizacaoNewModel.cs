using System.Collections.Generic;
using Xamarin.HLP.Mobile.AppPE.Common;

namespace Xamarin.HLP.Mobile.AppPE.Model.Sincronizacao
{
    public class SincronizacaoNewModel : NotifyCommon
    {
        private string _Display;
        public string Display
        {
            get { return _Display; }
            set
            {
                _Display = value; NotifyPropertyChanged();
                xDetail = $"{(Display ?? "").ToLower().Replace("tb_", "").Replace("_", " ")}";
            }
        }



        private string _xDetail;

        public string xDetail
        {
            get { return _xDetail; }
            set { _xDetail = value; NotifyPropertyChanged(); }
        }



        private int _iCount;

        public int iCount
        {
            get { return _iCount; }
            set
            {
                _iCount = value; NotifyPropertyChanged();
            }
        }

        private List<AlertaSincronizacao> _LAlertaSincronizacao = new List<AlertaSincronizacao>();
        public List<AlertaSincronizacao> LAlertaSincronizacao
        {
            get { return _LAlertaSincronizacao; }
            set { _LAlertaSincronizacao = value; NotifyPropertyChanged(); }
        }


    }
}

using System;
using System.Collections.Generic;
using Xamarin.Forms;
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

        public event Action<string> OnMensagemChanged;

        private string _xDetail;

        public string xDetail
        {
            get { return _xDetail; }
            set
            {
                _xDetail = value;
                NotifyPropertyChanged();

                OnMensagemChanged?.Invoke(_xDetail.ToLower());
            }
        }

        public event Action<int> OnCountChanged;

        private int _iCount;

        public int iCount
        {
            get { return _iCount; }
            set
            {
                _iCount = value;
                NotifyPropertyChanged();

                OnCountChanged?.Invoke(_iCount);
            }
        }

        private List<AlertaSincronizacao> _LAlertaSincronizacao = new List<AlertaSincronizacao>();
        public List<AlertaSincronizacao> LAlertaSincronizacao
        {
            get { return _LAlertaSincronizacao; }
            set { _LAlertaSincronizacao = value; NotifyPropertyChanged(); }
        }

        private List<AlertaSincronizacao> _LAlertaBloqueio = new List<AlertaSincronizacao>();
        public List<AlertaSincronizacao> LAlertaBloqueio
        {
            get { return _LAlertaBloqueio; }
            set { _LAlertaBloqueio = value; NotifyPropertyChanged(); }
        }
    }
}

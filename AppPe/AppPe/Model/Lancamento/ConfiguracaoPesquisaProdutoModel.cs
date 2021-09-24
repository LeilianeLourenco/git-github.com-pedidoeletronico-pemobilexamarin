using Xamarin.HLP.Mobile.AppPE.Common;

namespace Xamarin.HLP.Mobile.AppPE.Model.Lancamento
{
    public class ConfiguracaoPesquisaProdutoModel : NotifyCommon
    {
        public ConfiguracaoPesquisaProdutoModel()
        {
            Ordenacao = 0;
        }
        private int _Ordenacao = 1;

        /// <summary>
        /// 0 - DESCRIÇÃO
        /// 1 - CODIGO ALTERNATIVO
        /// </summary>
        public int Ordenacao
        {
            get { return _Ordenacao; }
            set
            {
                _Ordenacao = value;
                xOrdenacao = value == 0 ? "DESCRIÇÃO" : "CÓDIGO ALTERNATIVO";
                NotifyPropertyChanged();
            }
        }

        private string _xOrdenacao;

        public string xOrdenacao
        {
            get { return _xOrdenacao; }
            set { _xOrdenacao = value; NotifyPropertyChanged(); }
        }



        private ListItemModel _paramRepresentacao = null;
        public ListItemModel paramRepresentacao
        {
            get { return _paramRepresentacao; }
            set
            {
                _paramRepresentacao = value;
                NotifyPropertyChanged();
            }
        }

        private ListItemModel _paramCategoria = null;

        public ListItemModel paramCategoria
        {
            get { return _paramCategoria; }
            set
            {
                _paramCategoria = value;
                NotifyPropertyChanged();
            }
        }

        private bool _bUltimasCompras = false;

        public bool bUltimasCompras
        {
            get { return _bUltimasCompras; }
            set
            {
                if (_bUltimasCompras != value)
                    canExecuteInicial = true;
                _bUltimasCompras = value;
                NotifyPropertyChanged();
            }
        }


        private bool _bUtilizaMinimoVendasProduto = false;

        public bool bUtilizaMinimoVendasProduto
        {
            get { return _bUtilizaMinimoVendasProduto; }
            set
            {
                _bUtilizaMinimoVendasProduto = value;
                NotifyPropertyChanged();
            }
        }
         
        
        private byte _stCalculoVendas;

        public byte stCalculoVendas
        {
            get { return _stCalculoVendas; }
            set
            {
                _stCalculoVendas = value;
                NotifyPropertyChanged();
            }
        }



        public bool bNeedRefresh { get; set; }
    }
}

using Xamarin.HLP.Mobile.AppPE.Common;

namespace Xamarin.HLP.Mobile.AppPE.Model.TabelaPreco
{
    public class DisplayListaModel: ModelComum
    {
        public int? idProduto { get; set; }

        private int? _idProdutoOffLine;
        public int? idProdutoOffLine
        {
            get { return _idProdutoOffLine; }
            set { _idProdutoOffLine = value; NotifyPropertyChanged(); }
        }

        private string _xDisplay;

        public string xDisplay
        {
            get { return _xDisplay; }
            set { _xDisplay = value; NotifyPropertyChanged(); }
        }
        private string _xDetail;

        public string xDetail
        {
            get { return _xDetail; }
            set { _xDetail = value; NotifyPropertyChanged(); }
        }

        private double _vVenda;
        public double vVenda
        {
            get { return _vVenda; }
            set { _vVenda = value; NotifyPropertyChanged(); }
        }


        private double? _pIpiVenda;
        public double? pIpiVenda
        {
            get { return _pIpiVenda; }
            set { _pIpiVenda = value; NotifyPropertyChanged(); }
        }

        private double? _pStVenda;
        public double? pStVenda
        {
            get { return _pStVenda; }
            set { _pStVenda = value; NotifyPropertyChanged(); }
        }


    }
}

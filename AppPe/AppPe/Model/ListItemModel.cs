using Xamarin.HLP.Mobile.AppPE.Common;

namespace Xamarin.HLP.Mobile.AppPE.Model
{
    public class ListItemModel : NotifyCommon
    {
        private string _Display;
        public string Display
        {
            get { return _Display; }
            set { _Display = value; NotifyPropertyChanged(); }
        }

        private string _Detail;
        public string Detail
        {
            get { return _Detail; }
            set { _Detail = value; NotifyPropertyChanged(); }
        }


        public int? IdOnline { get; set; }

        public int Id { get; set; }     

        //public string XId { get; set; }

        private string _xId;

        public string XId
        {
            get { return string.IsNullOrEmpty(_xId) ? Id.ToString() : _xId; ; }
            set { _xId = value; }
        }

        //OS 35400 - Jessica Barbieri
        public double? vDescCondicao { get; set; }
        public int? idTabelaPreco { get; set; }

        public bool stCampanhaCliente { get; set; }
        public bool stCampanhaClienteRamoAtividade { get; set; }
        public bool stCampanhaClienteUF { get; set; }
        public bool stCampanhaRepresentante { get; set; }
        public bool stTabelaPrecoRepresentacao { get; set; }

    }
}

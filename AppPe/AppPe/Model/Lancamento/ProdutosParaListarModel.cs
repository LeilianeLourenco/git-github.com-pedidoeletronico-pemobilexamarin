using Xamarin.Forms;
using Xamarin.HLP.Mobile.AppPE.Common;

namespace Xamarin.HLP.Mobile.AppPE.Model.Lancamento
{
    public class ProdutosParaListarModel : ModelComum
    {
        public int idProdutoOffLine { get; set; }
        public int idRepresentada { get; set; }
        public int idCategoria { get; set; }
        public string xFabricante { get; set; }
        public string filtro { get; set; }

        private Color _corSelected = ColorStaticModel.CinzaPrincipal;
        public Color corSelected
        {
            get { return _corSelected; }
            set { _corSelected = value; NotifyPropertyChanged();  }
        }

    }
}

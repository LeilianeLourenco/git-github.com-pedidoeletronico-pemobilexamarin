using Xamarin.HLP.Mobile.AppPE.Common;

namespace Xamarin.HLP.Mobile.AppPE.Model.Cadastros.Escalonada
{
    public class ModelEscalonadaDisplay : ModelComum
    {
        public int iFaixa { get; set; }
        public string xDescontoDe { get; set; }
        public string xDescontoAte { get; set; }
        public string pComissao { get; set; }
        public bool bMostraFaixaTabelaEscalonada { get; set; }

        public int order { get; set; }

        public string xFiltro
        {
            get { return $"{iFaixa}-{xDescontoDe}-{xDescontoAte}-{pComissao}-{order}"; }
        }
    }
}

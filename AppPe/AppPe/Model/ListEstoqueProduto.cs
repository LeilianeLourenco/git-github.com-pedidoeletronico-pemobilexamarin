using System;
using Xamarin.HLP.Mobile.AppPE.Common;

namespace Xamarin.HLP.Mobile.AppPE.Model
{
    public class ListEstoqueProduto : ModelComum
    {
        public int idMovimentoEstoqueMobile { get; set; }

        public double vEstoque { get; set; }

        public int idProduto { get; set; }        

        public DateTime dtUltimaAlteracao { get; set; }

        public string xNomeGrade { get; set; }
    }
}

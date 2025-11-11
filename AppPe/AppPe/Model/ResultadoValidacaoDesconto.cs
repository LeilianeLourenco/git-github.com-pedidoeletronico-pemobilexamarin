using System.Collections.Generic;
using Xamarin.HLP.Mobile.AppPE.Model.Cadastros;

namespace Xamarin.HLP.Mobile.AppPE.Model
{
    public class ResultadoValidacaoDesconto
    {
        public List<ProdutoModel> ProdutosErrados { get; set; } = new List<ProdutoModel>();
        public List<ProdutoModel> ProdutosAlterados { get; set; } = new List<ProdutoModel>();
    }
}

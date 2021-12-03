using System.Collections.Generic;

namespace Xamarin.HLP.Mobile.AppPE.Core.PedidoVenda.Interfaces
{
    public interface IBuscaProdutosParaVenda
    {
        List<int> BuscarProdutos(int idEmpresa, int? idCondicaoParaTabelaPreco = null);
        List<int> BuscarProdutosCampanha(int idEmpresa, int? idCondicaoParaTabelaPreco = null);
    }
}

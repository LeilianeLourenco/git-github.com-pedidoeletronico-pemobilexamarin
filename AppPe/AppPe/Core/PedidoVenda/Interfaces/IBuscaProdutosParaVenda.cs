using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Xamarin.HLP.Mobile.AppPE.Core.PedidoVenda.Interfaces
{
    public interface IBuscaProdutosParaVenda
    {
        List<int> BuscarProdutos(int idEmpresa, int? idCondicaoParaTabelaPreco = null);
    }
}

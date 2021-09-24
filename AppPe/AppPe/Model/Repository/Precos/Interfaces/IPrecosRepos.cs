using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Xamarin.HLP.Mobile.AppPE.Model.TabelaPreco;

namespace Xamarin.HLP.Mobile.AppPE.Model.Repository.Precos.Interfaces
{
    public interface ITabelaPrecoManItensRepos
    {
        List<DisplayListaModel> ObterProdutosTabela(int idTabelaPreco);
    }
}

using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Xamarin.HLP.Mobile.AppPE.Model.Cadastros;

namespace Xamarin.HLP.Mobile.AppPE.Model.Repository.Interfaces
{
    public interface IBuscaPrecoRepositorio
    {

        List<TabelaPrecoModel> RetornaPrecos(int idEmpresa, int id, TipoPrecoBusca stBusca, string filtro = null);

    }

    public enum TipoPrecoBusca
    {
        tbl = 0,
        cmp = 1,
        tud = 2
    }

    public enum TipoPreco
    {
        auto = 0,
        manu = 1
    }
}

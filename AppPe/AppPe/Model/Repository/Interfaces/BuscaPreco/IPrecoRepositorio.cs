using System.Collections.Generic;
using Xamarin.HLP.Mobile.AppPE.Model.Cadastros;

namespace Xamarin.HLP.Mobile.AppPE.Model.Repository.Interfaces.BuscaPreco
{
    public interface IPrecoRepositorio
    {
        List<TabelaPrecoModel> Buscar(int idEmpresa, int idCliente, int idClienteOffLine,
            int idRepresentacao, int idRepresentante, int idProduto, TipoPrecoBusca stBusca);


        List<TabelaPrecoModel> BuscarSemRepresentacao(int idEmpresa, int idCliente, int idClienteOffLine,
            int idRepresentacao, int idRepresentante, int idProduto, TipoPrecoBusca stBusca);
    }
}

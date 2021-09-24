using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Xamarin.HLP.Mobile.AppPE.Model.Lancamento;

namespace Xamarin.HLP.Mobile.AppPE.Model.Repository.Interfaces.PedidoVenda
{
    public interface IPedidoVendaBuscaRepositorio
    {

        PedidoVendaModel Obter(int id);

    }
}

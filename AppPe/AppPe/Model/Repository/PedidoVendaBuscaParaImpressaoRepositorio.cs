using System.Linq;
using Xamarin.HLP.Mobile.AppPE.Model.Lancamento;
using Xamarin.HLP.Mobile.AppPE.Model.Repository.Interfaces.PedidoVenda;

namespace Xamarin.HLP.Mobile.AppPE.Model.Repository
{
    public class PedidoVendaBuscaParaImpressaoRepositorio : IPedidoVendaBuscaRepositorio
    {
        public PedidoVendaModel Obter(int id)
        {
            var xQuery =  $"SELECT * FROM TB_PEDIDOVENDA WHERE  idPedidoVendaOffLine = {id} and idEmpresa = {App.CurrentAspnetUserModel.objEmpresaAspnetUsersModel.idEmpresa}";
            var _pedido = (App.Data.Connection.Query<PedidoVendaModel>(xQuery)).FirstOrDefault();
            
            xQuery =   $"select distinct * from TB_PEDIDOVENDAITENS where idPedidoVendaOffLine = {id} and idEmpresa = {App.CurrentAspnetUserModel.objEmpresaAspnetUsersModel.idEmpresa}";
            var _lItens = App.Data.Connection.Query<PedidoVendaItensModel>(xQuery).ToList();

            if (_lItens?.Count > 0)
            {            
                _pedido.lItens = new System.Collections.ObjectModel.ObservableCollection<PedidoVendaItensModel>(_lItens);
            }

            return _pedido;
        }
    }
}

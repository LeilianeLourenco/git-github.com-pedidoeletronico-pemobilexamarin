using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Xamarin.HLP.Mobile.AppPE.Common;
using Xamarin.HLP.Mobile.AppPE.Model.Cadastros;

namespace Xamarin.HLP.Mobile.AppPE.Model.Repository.BuscaPreco
{
    public class BuscaPrecoItemManualRepositorio
    {

        public static TabelaPrecoItemModel ObterItem(int idTabelaPreco, int idProduto)
        {
            //var _item = App.Data.Connection.Table<TabelaPrecoItemModel>(
            //    ).FirstOrDefault(ti => ti.idTabelaPreco == idTabelaPreco && ti.idProduto == idProduto);

            var _item = App.Data.Connection.Query<TabelaPrecoItemModel>(
                $@"select pIpiVenda, pStVenda, vVendaComImpostos, vVenda, idProduto, pDescontoMaximo, pComissao from {TableMobile.TB_TABELAPRECOITEM} 
                    where idTabelaPreco = {idTabelaPreco} and idProduto = {idProduto}"
                ).FirstOrDefault();

            return _item;
        }

        public static List<TabelaPrecoItemModel> ObterItems(int idTabelaPreco, int idEmpresa)
        {
            var _itens = App.Data.Connection.Query<TabelaPrecoItemModel>(
                $@"select pIpiVenda, pStVenda, vVendaComImpostos, vVenda, idProduto, pDescontoMaximo, pComissao from {TableMobile.TB_TABELAPRECOITEM} 
                    where idEmpresa = {idEmpresa} and idTabelaPreco = {idTabelaPreco}"
                );

            return _itens;
        }

    }
}

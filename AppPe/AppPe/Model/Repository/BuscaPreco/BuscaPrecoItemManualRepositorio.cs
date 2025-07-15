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

        public static TabelaPrecoItemModel ObterItem(int idTabelaPreco, int idProduto, string filtro = null)
        {
            //var _item = App.Data.Connection.Table<TabelaPrecoItemModel>(
            //    ).FirstOrDefault(ti => ti.idTabelaPreco == idTabelaPreco && ti.idProduto == idProduto);

            var query =
                $@"select pIpiVenda, pStVenda, vVendaComImpostos, vVenda, idProduto, pDescontoMaximo, pComissao from {TableMobile.TB_TABELAPRECOITEM} 
                    where idTabelaPreco = {idTabelaPreco} and idProduto = {idProduto}";

            //if (!string.IsNullOrWhiteSpace(filtro))
            //    query += $" AND xNome LIKE '%{filtro.Replace("'", "''")}%'";

            return App.Data.Connection.Query<TabelaPrecoItemModel>(query).FirstOrDefault();
        }

        public static List<TabelaPrecoItemModel> ObterItems(int idTabelaPreco, int idEmpresa, string filtro = null)
        {

            var query = $@"select pIpiVenda, pStVenda, vVendaComImpostos, vVenda, idProduto, pDescontoMaximo, pComissao from {TableMobile.TB_TABELAPRECOITEM} 
                    where idEmpresa = {idEmpresa} and idTabelaPreco = {idTabelaPreco}";

            //if (!string.IsNullOrWhiteSpace(filtro))
            //    query += $" AND xNome LIKE '%{filtro.Replace("'", "''")}%'";

            var _itens = App.Data.Connection.Query<TabelaPrecoItemModel>(query);

            return _itens;
        }

    }
}

using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Xamarin.HLP.Mobile.AppPE.Model.Cadastros;
using Xamarin.HLP.Mobile.AppPE.Model.Repository.Interfaces;

namespace Xamarin.HLP.Mobile.AppPE.Model.Repository.BuscaPreco
{
    public class BuscaPrecoTabelaDefClienteRepositorio : IBuscaPrecoRepositorio
    {
        //   public List<TabelaPrecoModel> RetornaPrecos(int idEmpresa, int id, TipoPrecoBusca stBusca)
        //   {

        //       var _idTabelaPreco = App.Data.Connection.Table<ClientesModel>().FirstOrDefault(cl =>
        //cl.idClientesOffLine == id)?.idTabelaPreco;

        //       if (_idTabelaPreco != null && _idTabelaPreco > 0)
        //       {
        //           var _tbl = App.Data.Connection.Table<TabelaPrecoModel>().FirstOrDefault(tb =>
        //           tb.idTabelaPreco == _idTabelaPreco);

        //           if (_tbl != null)
        //           {
        //               return new List<TabelaPrecoModel>() { _tbl };
        //           }
        //       }

        //       return new List<TabelaPrecoModel>();

        //   }


        public List<TabelaPrecoModel> RetornaPrecos(int idEmpresa, int id, TipoPrecoBusca stBusca, string filtro = null)
        {
            var xquery = $@"
                            SELECT idTabelaPreco 
                            FROM tb_clientes
                            WHERE idClientesOffLine = {id} AND idEmpresa = {idEmpresa}";

            var _idTabelaPreco = App.Data.Connection.ExecuteScalar<int?>(xquery);

            if (_idTabelaPreco != null && _idTabelaPreco > 0)
            {
                TabelaPrecoModel _tbl;

                var query = App.Data.Connection.Table<TabelaPrecoModel>()
                    .Where(tb => tb.idTabelaPreco == _idTabelaPreco);

                if (!string.IsNullOrWhiteSpace(filtro))
                    query = query.Where(tb => tb.xNome.ToLower().Contains(filtro.ToLower()));

                _tbl = query.FirstOrDefault();

                if (_tbl != null)
                {
                    return new List<TabelaPrecoModel> { _tbl };
                }
            }

            return new List<TabelaPrecoModel>();
        }
    }
}

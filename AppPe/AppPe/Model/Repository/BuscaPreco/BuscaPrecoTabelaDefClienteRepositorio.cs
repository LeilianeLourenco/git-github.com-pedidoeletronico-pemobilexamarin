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


        public List<TabelaPrecoModel> RetornaPrecos(int idEmpresa, int id, TipoPrecoBusca stBusca)
        { 
            var xquery =
                $@"select idTabelaPreco from tb_clientes
                        where idClientesOffLine = {id} and idEmpresa = {idEmpresa}";

            var _idTabelaPreco = App.Data.Connection.ExecuteScalar<int?>(xquery);

            if (_idTabelaPreco != null && _idTabelaPreco > 0)
            {
                var _tbl = App.Data.Connection.Table<TabelaPrecoModel>().FirstOrDefault(tb =>
                tb.idTabelaPreco == _idTabelaPreco);

                if (_tbl != null)
                {
                    return new List<TabelaPrecoModel>() { _tbl };
                }
            }

            return new List<TabelaPrecoModel>(); 
        }
    }
}

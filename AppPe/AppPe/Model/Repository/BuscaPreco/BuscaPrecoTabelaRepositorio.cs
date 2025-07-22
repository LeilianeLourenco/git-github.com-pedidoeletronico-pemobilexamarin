using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Xamarin.HLP.Mobile.AppPE.Common;
using Xamarin.HLP.Mobile.AppPE.Model.Cadastros;
using Xamarin.HLP.Mobile.AppPE.Model.Repository.Interfaces;

namespace Xamarin.HLP.Mobile.AppPE.Model.Repository.BuscaPreco
{
    public class BuscaPrecoTabelaRepositorio : IBuscaPrecoRepositorio
    {
        public List<TabelaPrecoModel> RetornaPrecos(int idEmpresa, int id, TipoPrecoBusca stBusca)
        {
            //return App.Data.Connection.Table<TabelaPrecoModel>()
            //    .Where(tbl => tbl.idTabelaPreco == id && tbl.idEmpresa == idEmpresa).ToList();
            string _xQry =
                $@"select idTabelaPreco, xNome, pIndiceTabela, idEmpresa, 
                   stDefault, stTabelaPreco, stValor, dInicial, dFinal,
                   stCampanhaRepresentante, stCampanhaCliente,
                   stTabelaPrecoRepresentacao, stCampanhaClienteRamoAtividade, stCampanhaClienteUF 
            from {TableMobile.TB_TABELAPRECO}
            where stAtivo = 1 and idEmpresa = {idEmpresa}"; 

            var _tbls = App.Data.Connection.Query<TabelaPrecoSimples>(_xQry);


            return _tbls.Select(l => new TabelaPrecoModel
            {
                dFinal = l.dFinal,
                dInicial = l.dInicial,
                idEmpresa = l.idEmpresa,
                idTabelaPreco = l.idTabelaPreco,
                pIndiceTabela = l.pIndiceTabela,
                stDefault = l.stDefault,
                stTabelaPreco = l.stTabelaPreco,
                stValor = l.stValor,
                xNome = l.xNome,
                stCampanhaCliente = l.stCampanhaCliente,
                stCampanhaClienteRamoAtividade = l.stCampanhaClienteRamoAtividade,
                stCampanhaClienteUF = l.stCampanhaClienteUF,
                stCampanhaRepresentante = l.stCampanhaRepresentante,
                stTabelaPrecoRepresentacao = l.stTabelaPrecoRepresentacao
            }).ToList();

        }
    }
}

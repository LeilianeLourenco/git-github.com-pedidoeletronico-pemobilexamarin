using System;
using System.Collections.Generic;
using System.Linq;
using Xamarin.HLP.Mobile.AppPE.Common;
using Xamarin.HLP.Mobile.AppPE.Model.Cadastros;
using Xamarin.HLP.Mobile.AppPE.Model.Repository.Interfaces;

namespace Xamarin.HLP.Mobile.AppPE.Model.Repository.BuscaPreco
{
    public class BuscaPrecoClienteRepositorio : IBuscaPrecoRepositorio
    {
        public List<TabelaPrecoModel> RetornaPrecos(int idEmpresa, int id, TipoPrecoBusca stBusca, string filtro = null)
        {
            string _xQry =
                $@"select t.idTabelaPreco, t.xNome, t.pIndiceTabela, t.idEmpresa, 
                   t.stDefault, t.stTabelaPreco, t.stValor, t.dInicial, t.dFinal,
                   t.stCampanhaRepresentante, t.stCampanhaCliente, t.pDescontoMaximo,
                   t.stTabelaPrecoRepresentacao, t.stCampanhaClienteRamoAtividade, t.stCampanhaClienteUF from {TableMobile.TB_TABELAPRECO} t
                   join {TableMobile.TB_TABELA_PRECO_CLIENTES} tc on t.idTabelaPreco = tc.idTabelaPreco
                   and t.stAtivo = 1
                   and t.stCampanhaCliente = 1
                   and t.stCampanhaClienteRamoAtividade = 0 and t.stCampanhaClienteUF = 0
                   and tc.idClientes = {id}
                   and t.idEmpresa = {idEmpresa}";

            //REMOVIDAS LINHAS DE QUERY por não conseguir trazer dados de sql lite com condição de data/hora

            //            and(t.dInicial is null or t.dInicial < '{DateTime.Now.ToString("dd/MM/yyyy HH:mm:ss")}')
            //and(t.dFinal is null or t.dFinal > '{DateTime.Now.ToString("dd/MM/yyyy HH:mm:ss")}')

            if (stBusca != TipoPrecoBusca.tud)
                _xQry = $"{_xQry} and t.stTabelaPreco = {(byte)stBusca}";

            if (!string.IsNullOrWhiteSpace(filtro))
                _xQry += $" AND t.xNome LIKE '%{filtro.Replace("'", "''")}%'";
            
            var _tbls = App.Data.Connection.Query<TabelaPrecoSimples>(query: _xQry);

            _tbls = _tbls?.Where(tb => (tb.dInicial == null || tb.dInicial <= DateTime.UtcNow)
            && (tb.dFinal == null || tb.dFinal >= DateTime.UtcNow)).ToList();

            if (_tbls == null || _tbls.Count == 0)
            {
                return new List<TabelaPrecoModel>();
            }

            return _tbls.Select(l => new TabelaPrecoModel
            {
                dFinal = l.dFinal,
                dInicial = l.dInicial,
                idEmpresa = l.idEmpresa,
                idTabelaPreco = l.idTabelaPreco,
                pIndiceTabela = l.pIndiceTabela,
                stDefault = l.stDefault,
                stTabelaPreco = l.stTabelaPreco,
                pDescontoMaximo = l.pDescontoMaximo,
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

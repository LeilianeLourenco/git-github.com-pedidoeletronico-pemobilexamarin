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
    public class BuscaPrecoRepresentanteRepositorio : IBuscaPrecoRepositorio
    {
        public List<TabelaPrecoModel> RetornaPrecos(int idEmpresa, int id, TipoPrecoBusca stBusca)
        {
            string _xQry =
                $@"select t.idTabelaPreco, t.xNome, t.pIndiceTabela, t.idEmpresa, 
                   t.stDefault, t.stTabelaPreco, t.stValor, t.dInicial, t.dFinal,
                   t.stCampanhaRepresentante, t.stCampanhaCliente, t.pDescontoMaximo,
                   t.stTabelaPrecoRepresentacao, t.stCampanhaClienteRamoAtividade, t.stCampanhaClienteUF
                   from  {TableMobile.TB_TABELAPRECO} t
join {TableMobile.TB_TABELA_PRECO_REPRESENTANTES} tr on t.idTabelaPreco = tr.idTabelaPreco
and t.stAtivo = 1
and t.stCampanhaRepresentante = 1
and tr.idEmpresa_aspnetUsers = {id}
and t.idEmpresa = {idEmpresa}";

            if (stBusca != TipoPrecoBusca.tud)
            {
                _xQry = $"{_xQry} and t.stTabelaPreco = {(byte)stBusca}";
            }

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

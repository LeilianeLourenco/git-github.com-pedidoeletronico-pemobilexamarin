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
    public class BuscaPrecoGeralRepositorio : IBuscaPrecoRepositorio
    {
        public List<TabelaPrecoModel> RetornaPrecos(int idEmpresa, int id, TipoPrecoBusca stBusca)
        {
            string _xQry =
                $@"select idTabelaPreco, xNome, pIndiceTabela, idEmpresa, 
                   stDefault, stTabelaPreco, stValor, dInicial, dFinal, 
                   stCampanhaRepresentante, stCampanhaCliente, pDescontoMaximo,
                   stTabelaPrecoRepresentacao, stCampanhaClienteRamoAtividade, stCampanhaClienteUF from {TableMobile.TB_TABELAPRECO}
            where stAtivo = 1
            and stCampanhaCliente = 0 and stCampanhaRepresentante = 0 and stTabelaPrecoRepresentacao = 0
            and stCampanhaClienteRamoAtividade = 0 and stCampanhaClienteUF = 0
            and idEmpresa = {idEmpresa}"; 

            if (stBusca != TipoPrecoBusca.tud)
            {
                _xQry = $"{_xQry} and stTabelaPreco = {(byte)stBusca}";
            } 
             
           var _tbls = App.Data.Connection.Query<TabelaPrecoSimples>(_xQry);
             
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
                pDescontoMaximo = l.pDescontoMaximo,
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

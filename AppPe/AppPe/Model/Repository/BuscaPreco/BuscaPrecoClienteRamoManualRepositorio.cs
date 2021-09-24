using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Xamarin.HLP.Mobile.AppPE.Common;
using Xamarin.HLP.Mobile.AppPE.Model.Cadastros;
using Xamarin.HLP.Mobile.AppPE.Model.Repository.ClienteRep;
using Xamarin.HLP.Mobile.AppPE.Model.Repository.Interfaces;

namespace Xamarin.HLP.Mobile.AppPE.Model.Repository.BuscaPreco
{
    public class BuscaPrecoClienteRamoManualRepositorio : IBuscaPrecoRepositorio
    {
        int _idProduto;
        public BuscaPrecoClienteRamoManualRepositorio(int idProduto)
        {
            this._idProduto = idProduto;
        }

        public List<TabelaPrecoModel> RetornaPrecos(int idEmpresa, int id, TipoPrecoBusca stBusca)
        {
            ClienteRamoAtividadeRepositorio _cliRamoRep = new ClienteRamoAtividadeRepositorio();
            int _idRamoAtividade = _cliRamoRep.ObterIdRamoAtividade(idEmpresa: idEmpresa, idCliente: id);

            string _xQry =
                $@"select t.idTabelaPreco, t.xNome, t.pIndiceTabela, t.idEmpresa, 
                   t.stDefault, t.stTabelaPreco, t.stValor, t.dInicial, t.dFinal, 
                   t.stCampanhaRepresentante, t.stCampanhaCliente,
                   t.stTabelaPrecoRepresentacao, t.stCampanhaClienteRamoAtividade, t.stCampanhaClienteUF from {TableMobile.TB_TABELAPRECO} t
join {TableMobile.TB_TABELAPRECOITEM} ti on t.idTabelaPreco = ti.idTabelaPreco
join {TableMobile.tb_tabelapreco_ramoatividade_cliente} tr on t.idTabelaPreco = tr.idTabelaPreco
and t.stValor == 2 and t.stAtivo = 1
and t.stCampanhaCliente = 0
and t.stCampanhaClienteRamoAtividade = 1 and t.stCampanhaClienteUF = 0
and t.idEmpresa = {idEmpresa}
and ti.idProduto = {_idProduto}
and tr.idRamoAtividade = {_idRamoAtividade}";

            if (stBusca != TipoPrecoBusca.tud)
            {
                _xQry = $"{_xQry} and t.stTabelaPreco = {(byte)stBusca}";
            }

            var _tbls = App.Data.Connection.Query<TabelaPrecoSimples>(query: _xQry);
            _tbls = _tbls?.Where(tb => (tb.dInicial == null || tb.dInicial <= DateTime.Now)
            && (tb.dFinal == null || tb.dFinal >= DateTime.Now)).ToList();

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

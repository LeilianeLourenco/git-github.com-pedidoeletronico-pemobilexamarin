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
    public class BuscaPrecoClienteUfJoinRamoManualRepositorio : IBuscaPrecoRepositorio
    {
        int _idProduto;
        public BuscaPrecoClienteUfJoinRamoManualRepositorio(int idProduto)
        {
            this._idProduto = idProduto;
        }

        public List<TabelaPrecoModel> RetornaPrecos(int idEmpresa, int id, TipoPrecoBusca stBusca, string filtro = null)
        {
            ClienteEnderecoRepositorio _cliEnderRepos = new ClienteEnderecoRepositorio();

            var _end = _cliEnderRepos.ObterEnderecoPrincipalCliente(idCliente: id);

            if (_end == null)
            {
                return new List<TabelaPrecoModel>();
            }

            ClienteRamoAtividadeRepositorio _cliRamoRep = new ClienteRamoAtividadeRepositorio();
            int _idRamoAtividade = _cliRamoRep.ObterIdRamoAtividade(idEmpresa: idEmpresa, idCliente: id);

            string _xUf = _end.xEstado.ToLower();

            string _xQry =
                $@"select t.idTabelaPreco, t.xNome, t.pIndiceTabela, t.idEmpresa, 
                   t.stDefault, t.stTabelaPreco, t.stValor, t.dInicial, t.dFinal, 
                   t.stCampanhaRepresentante, t.stCampanhaCliente,
                   t.stTabelaPrecoRepresentacao, t.stCampanhaClienteRamoAtividade, t.stCampanhaClienteUF  from {TableMobile.TB_TABELAPRECO} t
join {TableMobile.tb_tabelapreco_ramoatividade_cliente} tr on t.idTabelaPreco = tr.idTabelaPreco
join {TableMobile.tb_tabelapreco_uf_cliente} tuf on t.idTabelaPreco = tuf.idTabelaPreco
join {TableMobile.TB_TABELAPRECOITEM} ti on t.idTabelaPreco = ti.idTabelaPreco
and t.stValor == 2 and t.stAtivo = 1
and t.stCampanhaCliente = 0
and t.stCampanhaClienteRamoAtividade = 1 and t.stCampanhaClienteUF = 1
and (t.dInicial is null or t.dInicial < '{DateTime.UtcNow}')
and (t.dFinal is null or t.dFinal > '{DateTime.UtcNow}')
and t.idEmpresa = {idEmpresa}
and tr.idRamoAtividade = {_idRamoAtividade}
and ti.idProduto = {_idProduto}
and LOWER(tuf.xUF) = '{_xUf}'";

            if (stBusca != TipoPrecoBusca.tud)
            {
                _xQry = $"{_xQry} and t.stTabelaPreco = {(byte)stBusca}";
            }

            var _tbls = App.Data.Connection.Query<TabelaPrecoSimples>(query: _xQry);

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

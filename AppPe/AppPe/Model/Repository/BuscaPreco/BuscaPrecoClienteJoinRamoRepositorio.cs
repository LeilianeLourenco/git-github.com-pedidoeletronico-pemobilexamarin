using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Xamarin.HLP.Mobile.AppPE.Common;
using Xamarin.HLP.Mobile.AppPE.Model.Cadastros;
using Xamarin.HLP.Mobile.AppPE.Model.Repository.ClienteRep;
using Xamarin.HLP.Mobile.AppPE.Model.Repository.Interfaces;
using Xamarin.HLP.Mobile.AppPE.Model.Repository.Interfaces.ClienteRep;

namespace Xamarin.HLP.Mobile.AppPE.Model.Repository.BuscaPreco
{
    public class BuscaPrecoClienteJoinRamoRepositorio : IBuscaPrecoRepositorio
    {
        public List<TabelaPrecoModel> RetornaPrecos(int idEmpresa, int id, TipoPrecoBusca stBusca, string filtro = null)
        {
            ClienteRamoAtividadeRepositorio _cliRamoRep = new ClienteRamoAtividadeRepositorio();
            int _idRamoAtividade = _cliRamoRep.ObterIdRamoAtividade(idEmpresa: idEmpresa, idCliente: id);

            IClienteRamoAtividadeRepositorio _cliRamosRep = new ClienteRamosAtividadeRepositorio();

            List<int> _idsRamosClienteAux = new List<int>();

            _idsRamosClienteAux.Add(_idRamoAtividade);

            List<int> _idsRamosCliente = _cliRamosRep.ObterRamosCliente(idEmpresa: idEmpresa, idCliente: id);

            if (_idsRamosCliente?.Count > 0)
            {
                _idsRamosClienteAux.AddRange(_idsRamosCliente);
            }

            string _xInRamosCliente = string.Empty;

            foreach (var r in _idsRamosClienteAux)
            {
                if (string.IsNullOrEmpty(_xInRamosCliente))
                {
                    _xInRamosCliente = r.ToString();
                }
                else
                {
                    _xInRamosCliente = $"{_xInRamosCliente}, {r.ToString()}";
                }
            }

            string _xQry =
                $@"select t.idTabelaPreco, t.xNome, t.pIndiceTabela, t.idEmpresa, 
                   t.stDefault, t.stTabelaPreco, t.stValor, t.dInicial, t.dFinal,
                   t.stCampanhaRepresentante, t.stCampanhaCliente, pDescontoMaximo,
                   t.stTabelaPrecoRepresentacao, t.stCampanhaClienteRamoAtividade, t.stCampanhaClienteUF from {TableMobile.TB_TABELAPRECO} t
join {TableMobile.TB_TABELA_PRECO_CLIENTES} tc on t.idTabelaPreco = tc.idTabelaPreco
join  {TableMobile.tb_tabelapreco_ramoatividade_cliente} tr on t.idTabelaPreco = tr.idTabelaPreco
and t.stAtivo = 1
and t.stCampanhaCliente = 1
and t.stCampanhaClienteRamoAtividade = 1 and t.stCampanhaClienteUF = 0
and t.idEmpresa = {idEmpresa}
and tc.idClientes = {id}
and tr.idRamoAtividade in ({_xInRamosCliente})";

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

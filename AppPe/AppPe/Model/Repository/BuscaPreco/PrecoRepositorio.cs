using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Xamarin.HLP.Mobile.AppPE.Model.Cadastros;
using Xamarin.HLP.Mobile.AppPE.Model.Cadastros.Escalonada;
using Xamarin.HLP.Mobile.AppPE.Model.Repository.Interfaces;
using Xamarin.HLP.Mobile.AppPE.Model.Repository.Interfaces.BuscaPreco;

namespace Xamarin.HLP.Mobile.AppPE.Model.Repository.BuscaPreco
{
    public class PrecoRepositorio : IPrecoRepositorio
    {
        public List<TabelaPrecoModel> Buscar(int idEmpresa, int idCliente, int idClienteOffLine,
            int idRepresentacao, int idRepresentante, int idProduto, TipoPrecoBusca stBusca)
        {
            List<IBuscaPrecoRepositorio> _lReposisBusca = new List<IBuscaPrecoRepositorio>();
            List<TabelaPrecoModel> _lRetorno = new List<TabelaPrecoModel>();

            //TabelaEscalonadaModel _tbEscalonada = TabelaPrecoRepository.VerificaSeUtilizaEscalonada();
            List<TabelaEscalonadaModel> _lEscalonadasAtivas = TabelaPrecoRepository.VerificaSeUtilizaEscalonadas();
            TabelaEscalonadaModel _tbEscalonada = new TabelaEscalonadaModel();

            TipoPrecoBusca _stBusca = stBusca;
            if (_tbEscalonada != null && _tbEscalonada.idTabelaPrecoVinculo.HasValue == true)
            {
                _lReposisBusca.Add(new BuscaPrecoTabelaRepositorio());

                if (_tbEscalonada.stExibeCampanhas == true)
                {
                    _stBusca = TipoPrecoBusca.cmp;
                    _lReposisBusca.Add(new BuscaPrecoGeralRepositorio());
                    _lReposisBusca.Add(new BuscaPrecoRepresentacaoRepositorio());
                    _lReposisBusca.Add(new BuscaPrecoRepresentanteRepositorio());
                    _lReposisBusca.Add(new BuscaPrecoClienteRepositorio());
                    _lReposisBusca.Add(new BuscaPrecoClienteUfRepository());
                    _lReposisBusca.Add(new BuscaPrecoClienteRamoRepositorio());
                    _lReposisBusca.Add(new BuscaPrecoClienteJoinRamoRepositorio());
                    _lReposisBusca.Add(new BuscaPrecoClienteUfJoinRamoRepositorio());
                }
            }
            else
            {
                _lReposisBusca.Add(new BuscaPrecoGeralRepositorio());
                _lReposisBusca.Add(new BuscaPrecoClienteRepositorio());
                _lReposisBusca.Add(new BuscaPrecoRepresentacaoRepositorio());
                _lReposisBusca.Add(new BuscaPrecoRepresentanteRepositorio());
                _lReposisBusca.Add(new BuscaPrecoClienteUfRepository());
                _lReposisBusca.Add(new BuscaPrecoClienteRamoRepositorio());
                _lReposisBusca.Add(new BuscaPrecoClienteJoinRamoRepositorio());
                _lReposisBusca.Add(new BuscaPrecoClienteUfJoinRamoRepositorio());
            }

            List<TabelaPrecoModel> _lRetornoAux;
            int _id = 0;
            foreach (var rep in _lReposisBusca)
            {

                if (rep.GetType() == typeof(BuscaPrecoTabelaDefClienteRepositorio))
                {
                    _id = idClienteOffLine;
                }
                else if (rep.GetType() == typeof(BuscaPrecoClienteRepositorio) || rep.GetType() == typeof(BuscaPrecoClienteManualRepositorio)
                    || rep.GetType() == typeof(BuscaPrecoClienteUfRepository)
                    || rep.GetType() == typeof(BuscaPrecoClienteUfManualRepositorio)
                    || rep.GetType() == typeof(BuscaPrecoClienteRamoRepositorio)
                    || rep.GetType() == typeof(BuscaPrecoClienteRamoManualRepositorio)
                    || rep.GetType() == typeof(BuscaPrecoClienteJoinRamoRepositorio)
                    || rep.GetType() == typeof(BuscaPrecoClienteJoinRamoManualRepositorio)
                    || rep.GetType() == typeof(BuscaPrecoClienteUfJoinRamoRepositorio)
                    || rep.GetType() == typeof(BuscaPrecoClienteUfJoinRamoManualRepositorio))
                {
                    _id = idCliente;
                }
                else if (rep.GetType() == typeof(BuscaPrecoRepresentacaoRepositorio) || rep.GetType() == typeof(BuscaPrecoRepresentacaoManualRepositorio))
                {
                    _id = idRepresentacao;
                }
                else if (rep.GetType() == typeof(BuscaPrecoRepresentanteRepositorio) || rep.GetType() == typeof(BuscaPrecoRepresentanteManualRepository))
                {
                    _id = idRepresentante;
                }
                else if (rep.GetType() == typeof(BuscaPrecoTabelaRepositorio))
                {
                    _id = _tbEscalonada?.idTabelaPrecoVinculo ?? 0;
                }
                else
                {
                    _id = 0;
                }

                _lRetornoAux = rep.RetornaPrecos(idEmpresa: idEmpresa, id: _id, stBusca: _stBusca);

                if (_lRetornoAux != null && _lRetornoAux.Count > 0)
                {
                    _lRetorno.AddRange(_lRetornoAux);
                }
            }

            if (_lEscalonadasAtivas?.Count() > 0 && _lRetorno.Count > 0)
            {
                foreach (var tb in _lRetorno)
                {
                    _tbEscalonada = _lEscalonadasAtivas.Where(t => t.idTabelaPrecoVinculo == tb.idTabelaPreco).FirstOrDefault();

                    if (_tbEscalonada == null)
                        _tbEscalonada = _lEscalonadasAtivas.Where(t => t.idTabelaPrecoVinculo == null).FirstOrDefault();

                    if (_tbEscalonada != null)
                    {
                        List<double> _faixas = _tbEscalonada.lFaixaComissao.Select(c => c.pFimFaixa).ToList();
                        double _pDescontoMax = 0;

                        if (_faixas?.Count() > 0)
                            _pDescontoMax = _faixas.Max();

                        tb.pDescontoMaximo = _pDescontoMax;
                        tb.tbEscalonada = _tbEscalonada;
                    }
                }
            }
            
            // OS 35398 - Jessica Barbieri
            if (_lRetorno?.Count() > 0)
            {
                foreach (var tabela in _lRetorno)
                {
                    int filtros = 0;

                    if (tabela.stCampanhaCliente)
                        filtros++;

                    if (tabela.stCampanhaClienteRamoAtividade && !tabela.stCampanhaClienteUF)
                        filtros++;

                    if (tabela.stCampanhaClienteUF && !tabela.stCampanhaClienteRamoAtividade)
                        filtros++;

                    if (tabela.stCampanhaClienteUF && tabela.stCampanhaClienteRamoAtividade)
                        filtros++;

                    if (tabela.stCampanhaRepresentante)
                        filtros++;

                    if (tabela.stTabelaPrecoRepresentacao)
                        filtros++;

                    var _contagemDeTabelas = _lRetorno
                        .Where(t => t.idTabelaPreco == tabela.idTabelaPreco)
                        .GroupBy(t => t.idTabelaPreco)
                        .Count();
                   
                    if (filtros > 0 && _contagemDeTabelas != filtros)
                    {
                        tabela.bRemoveTabela = true;
                    }
                }

                _lRetorno.RemoveAll(t => t.bRemoveTabela == true);
            }


            return _lRetorno;
        }


        /// <summary>
        /// Esse método é utilizado para a filtragem inicial dos produtos no pedido de venda, pois ainda não existe uma
        /// representação definida
        /// </summary>
        /// <param name="idEmpresa"></param>
        /// <param name="idCliente"></param>
        /// <param name="idClienteOffLine"></param>
        /// <param name="idRepresentacao"></param>
        /// <param name="idRepresentante"></param>
        /// <param name="idProduto"></param>
        /// <param name="stBusca"></param>
        /// <returns></returns>
        public List<TabelaPrecoModel> BuscarSemRepresentacao(int idEmpresa, int idCliente, int idClienteOffLine,
    int idRepresentacao, int idRepresentante, int idProduto, TipoPrecoBusca stBusca)
        {
            List<IBuscaPrecoRepositorio> _lReposisBusca = new List<IBuscaPrecoRepositorio>();
            List<TabelaPrecoModel> _lRetorno = new List<TabelaPrecoModel>();

            TabelaEscalonadaModel _tbEscalonada = TabelaPrecoRepository.VerificaSeUtilizaEscalonada();

            TipoPrecoBusca _stBusca = stBusca;
            if (_tbEscalonada != null && _tbEscalonada.idTabelaPrecoVinculo.HasValue == true)
            {
                _lReposisBusca.Add(new BuscaPrecoTabelaRepositorio());

                if (_tbEscalonada.stExibeCampanhas == true)
                {
                    _stBusca = TipoPrecoBusca.cmp;
                    _lReposisBusca.Add(new BuscaPrecoGeralRepositorio());
                    _lReposisBusca.Add(new BuscaPrecoRepresentacaoRepositorio());
                    _lReposisBusca.Add(new BuscaPrecoRepresentanteRepositorio());
                    _lReposisBusca.Add(new BuscaPrecoClienteRepositorio());
                    _lReposisBusca.Add(new BuscaPrecoClienteUfRepository());
                    _lReposisBusca.Add(new BuscaPrecoClienteRamoRepositorio());
                    _lReposisBusca.Add(new BuscaPrecoClienteJoinRamoRepositorio());
                    _lReposisBusca.Add(new BuscaPrecoClienteUfJoinRamoRepositorio());
                }
            }
            else
            {
                _lReposisBusca.Add(new BuscaPrecoGeralRepositorio());
                _lReposisBusca.Add(new BuscaPrecoClienteRepositorio());
                _lReposisBusca.Add(new BuscaPrecoRepresentacaoRepositorio());
                _lReposisBusca.Add(new BuscaPrecoRepresentanteRepositorio());
                _lReposisBusca.Add(new BuscaPrecoClienteUfRepository());
                _lReposisBusca.Add(new BuscaPrecoClienteRamoRepositorio());
                _lReposisBusca.Add(new BuscaPrecoClienteJoinRamoRepositorio());
                _lReposisBusca.Add(new BuscaPrecoClienteUfJoinRamoRepositorio());
            }

            List<TabelaPrecoModel> _lRetornoAux;
            int _id = 0;
            foreach (var rep in _lReposisBusca)
            {

                if (rep.GetType() == typeof(BuscaPrecoTabelaDefClienteRepositorio)
                    )
                {
                    _id = idClienteOffLine;
                }
                else if (rep.GetType() == typeof(BuscaPrecoClienteRepositorio) || rep.GetType() == typeof(BuscaPrecoClienteManualRepositorio)
                    || rep.GetType() == typeof(BuscaPrecoClienteUfRepository)
                    || rep.GetType() == typeof(BuscaPrecoClienteUfManualRepositorio)
                    || rep.GetType() == typeof(BuscaPrecoClienteRamoRepositorio)
                    || rep.GetType() == typeof(BuscaPrecoClienteRamoManualRepositorio)
                    || rep.GetType() == typeof(BuscaPrecoClienteJoinRamoRepositorio)
                    || rep.GetType() == typeof(BuscaPrecoClienteJoinRamoManualRepositorio)
                    || rep.GetType() == typeof(BuscaPrecoClienteUfJoinRamoRepositorio)
                    || rep.GetType() == typeof(BuscaPrecoClienteUfJoinRamoManualRepositorio))
                {
                    _id = idCliente;
                }
                else if (rep.GetType() == typeof(BuscaPrecoRepresentacaoRepositorio) || rep.GetType() == typeof(BuscaPrecoRepresentacaoManualRepositorio))
                {
                    _id = idRepresentacao;
                }
                else if (rep.GetType() == typeof(BuscaPrecoRepresentanteRepositorio) || rep.GetType() == typeof(BuscaPrecoRepresentanteManualRepository))
                {
                    _id = idRepresentante;
                }
                else if (rep.GetType() == typeof(BuscaPrecoTabelaRepositorio))
                {
                    _id = _tbEscalonada?.idTabelaPrecoVinculo ?? 0;
                }
                else
                {
                    _id = 0;
                }

                _lRetornoAux = rep.RetornaPrecos(idEmpresa: idEmpresa, id: _id, stBusca: _stBusca);

                if (_lRetornoAux != null && _lRetornoAux.Count > 0)
                {
                    _lRetorno.AddRange(_lRetornoAux);
                }
            }

            if (_tbEscalonada != null && _lRetorno.Count > 0)
            {
                List<double> _faixas = _tbEscalonada.lFaixaComissao.Select(c => c.pFimFaixa).ToList();
                double _pDescontoMax = 0;

                if (_faixas?.Count() > 0)
                    _pDescontoMax = _faixas.Max();


                foreach (var tb in _lRetorno)
                {
                    tb.pDescontoMaximo = _pDescontoMax;
                    tb.tbEscalonada = _tbEscalonada;
                }
            }

            return _lRetorno;
        }
    }
}

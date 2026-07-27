using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Text;
using System.Threading.Tasks;
using Xamarin.HLP.Mobile.AppPE.Model.Cadastros;
using Xamarin.HLP.Mobile.AppPE.Model.Cadastros.Escalonada;
using Xamarin.HLP.Mobile.AppPE.Model.Lancamento;
using Xamarin.HLP.Mobile.AppPE.Model.Repository;
using Xamarin.HLP.Mobile.AppPE.Model.Repository.BuscaPreco;

namespace Xamarin.HLP.Mobile.AppPE.Core.PedidoVenda
{
    public partial class BuscaPreco
    {

        List<KeyValuePair<int, List<TabelaPrecoModel>>> _lFiltroCampCliente;
        List<KeyValuePair<int, List<TabelaPrecoModel>>> _lFiltroCampRepre;
        List<KeyValuePair<int, List<TabelaPrecoModel>>> _lFiltroCampRepra;

        BuscaPrecoRepositorio _buscaRep;
        public BuscaPreco()
        {
            _lFiltroCampCliente = new List<KeyValuePair<int, List<TabelaPrecoModel>>>();
            _lFiltroCampRepre = new List<KeyValuePair<int, List<TabelaPrecoModel>>>();
            _lFiltroCampRepra = new List<KeyValuePair<int, List<TabelaPrecoModel>>>();

            _buscaRep = new BuscaPrecoRepositorio();
            LimpaItensTabelaManual();
        }

        public void Buscar(List<PedidoVendaItensModel> itens, int idClienteOff, int idCliente, int idRepresentante, int idEmpresa, int? idTabelaPrecoCondicao = null)
        {
            List<TabelaEscalonadaModel> _lEscalonadasAtivas = TabelaPrecoRepository.VerificaSeUtilizaEscalonadas();
            //TabelaEscalonadaModel _tbEscalonada = TabelaPrecoRepository.VerificaSeUtilizaEscalonada();
            TabelaEscalonadaModel _tbEscalonada = new TabelaEscalonadaModel();

            BuscaPrecoTabelaDefClienteRepositorio _repCliente = new BuscaPrecoTabelaDefClienteRepositorio();

            TabelaPrecoModel _tblDef = null;

            bool _bBuscaCampanhas = true;

            //rotina comentada, aqui ele trabalha com o conceito de 1 tabela escalonada, agora estamos aplicando múltiplas tabelas de acordo com as definições
            //if (_tbEscalonada == null)
            //{
            //    _tblDef = _repCliente.RetornaPrecos(idEmpresa: idEmpresa,
            //    id: idClienteOff, stBusca: Model.Repository.Interfaces.TipoPrecoBusca.tbl)?.FirstOrDefault();
            //}
            //else
            //{
            //    _bBuscaCampanhas = _tbEscalonada.stExibeCampanhas;

            //    if (_tbEscalonada.idTabelaPrecoVinculo.HasValue == false)
            //    {
            //        _tblDef = _repCliente.RetornaPrecos(idEmpresa: idEmpresa,
            //        id: idClienteOff, stBusca: Model.Repository.Interfaces.TipoPrecoBusca.tbl)?.FirstOrDefault();
            //    }
            //    else
            //    {
            //        _tblDef = TabelaPrecoRepository.GetRetistro(idTabelaPreco: _tbEscalonada.idTabelaPrecoVinculo.Value);
            //    }
            //}


            //rotina nova
            _tblDef = _repCliente.RetornaPrecos(idEmpresa: idEmpresa,
            id: idClienteOff, stBusca: Model.Repository.Interfaces.TipoPrecoBusca.tbl)?.FirstOrDefault();

            // [#5831] A tabela DEFAULT do cliente só precifica se o VENDEDOR tiver acesso a ela,
            // respeitando TODOS os filtros da tabela (vendedor/cliente/UF/ramo — mesma composição de
            // escopos do PrecoRepositorio.Buscar, já com a poda OS-35398). Antes o pedido usava o
            // preço da default do cliente mesmo sem permissão do vendedor (ex.: cliente com default
            // 64443, vendedor com acesso só à 64514). Fora do escopo → não usa a default; o item cai
            // no SetTabelaPrecoByProduto, que precifica só pelas tabelas liberadas ao vendedor.
            if (_tblDef != null)
            {
                var _idsTabelasDisponiveis = new HashSet<int>(
                    new PrecoRepositorio().Buscar(idEmpresa: idEmpresa, idCliente: idCliente,
                        idClienteOffLine: idClienteOff, idRepresentacao: 0, idRepresentante: idRepresentante,
                        idProduto: 0, stBusca: Model.Repository.Interfaces.TipoPrecoBusca.tud)
                    .Select(t => t.idTabelaPreco));

                if (!_idsTabelasDisponiveis.Contains(_tblDef.idTabelaPreco))
                    _tblDef = null;
            }

            if (_lEscalonadasAtivas?.Count() > 0)
            {
                //primeiro busco se a tabela de preço que veio é especifica da tabela escalonada
                _tbEscalonada = _lEscalonadasAtivas.Where(t => t.idTabelaPrecoVinculo == _tblDef?.idTabelaPreco).FirstOrDefault();

                //se for nulo, tento buscar a tabela escalonada que não tem nenhuma tabela de preço definida
                if (_tbEscalonada == null || _tbEscalonada.idTabelaPrecoEscalonada == 0)
                    _tbEscalonada = _lEscalonadasAtivas.Where(t => t.idTabelaPrecoVinculo == null).FirstOrDefault();


                if (_tbEscalonada != null)
                {
                    _bBuscaCampanhas = _tbEscalonada.stExibeCampanhas;
                }
            }

            List<TabelaPrecoModel> _lCampanhasValidas;
            List<TabelaPrecoSimplificada> _lCampanhasValidasVenda;


            List<TabelaPrecoModel> _lCampanhasCliente = new List<TabelaPrecoModel>();
            List<TabelaPrecoModel> _lCampanhasRpres = new List<TabelaPrecoModel>();

            if (_bBuscaCampanhas == true)
            {
                var _filtroCliente = _lFiltroCampCliente.FirstOrDefault(f => f.Key == idCliente);
                if (_filtroCliente.Key == 0)
                {
                    var _cmpsCliente = _buscaRep.BuscarCampanhasCliente(idEmpresa: idEmpresa,
                        idCliente: idCliente);

                    _lCampanhasCliente = _cmpsCliente;
                    _lFiltroCampCliente.Add(new KeyValuePair<int, List<TabelaPrecoModel>>(key: idCliente,
                        value: _cmpsCliente));
                }
                else
                {
                    _lCampanhasCliente = _filtroCliente.Value;
                }
            }

            if (_bBuscaCampanhas == true)
            {
                var _filtroRepre = _lFiltroCampRepre.FirstOrDefault(re => re.Key == idRepresentante);
                if (_filtroRepre.Key == 0)
                {
                    _lCampanhasRpres = _buscaRep.BuscarCampanhasRepresentante(idEmpresa: idEmpresa,
                        idUsuario: idRepresentante);

                    _lFiltroCampRepre.Add(new KeyValuePair<int, List<TabelaPrecoModel>>(key: idRepresentante, value: _lCampanhasRpres));
                }
                else
                {
                    _lCampanhasRpres = _filtroRepre.Value;
                }
            }

            var _lCampanhasGerais = _buscaRep.BuscarCampanhasGerais(idEmpresa: idEmpresa);

            TabelaPrecoSimplificada _tblAux;
            TabelaPrecoModel _tblVendaAux;
            ProdutoModel _infProduto;
            KeyValuePair<int, List<TabelaPrecoModel>> _filtroCampRpra;
            List<TabelaPrecoModel> _lCampsRpra;
            int _idRpra;

            foreach (var gr in itens.GroupBy(i => i.idRepresentada)) // Agrupado por representação para diminuir número de buscas necessárias
                                                                     // de preços disponíveis
            {
                _idRpra = gr.FirstOrDefault().idRepresentada;
                _lCampsRpra = new List<TabelaPrecoModel>();

                if (_bBuscaCampanhas == true)
                {
                    _filtroCampRpra = _lFiltroCampRepra.FirstOrDefault(f => f.Key == _idRpra);
                    if (_filtroCampRpra.Key == 0)
                    {
                        _lCampsRpra = _buscaRep.BuscarCampanhasRepresentacao(idEmpresa: idEmpresa,
                            idRepresentacao: _idRpra);
                    }
                }
                
                foreach (var pr in gr)
                {
                    _infProduto = new ProdutoModel
                    {
                        vVenda = pr.vVenda,
                        pIpiVenda = pr.pIpiVenda,
                        pStVenda = pr.pStVenda
                    };
                    _lCampanhasValidas = new List<TabelaPrecoModel>()
                    {
                    };

                    if ((_lCampanhasGerais?.Count ?? 0) > 0)
                    {
                        _lCampanhasValidas.AddRange(_lCampanhasGerais);
                    }

                    if ((_lCampanhasCliente?.Count ?? 0) > 0)
                    {
                        _lCampanhasValidas.AddRange(_lCampanhasCliente);
                    }
                    if ((_lCampanhasRpres?.Count ?? 0) > 0)
                    {
                        _lCampanhasValidas.AddRange(_lCampanhasRpres);
                    }
                    if ((_lCampsRpra?.Count ?? 0) > 0)
                    {
                        _lCampanhasValidas.AddRange(_lCampsRpra);
                    }

                    //Incluir em listagem campanhas encontradas
                    _lCampanhasValidasVenda = new List<TabelaPrecoSimplificada>();
                    if (_lCampanhasValidas.Count > 0)
                    {
                        foreach (var cmp in _lCampanhasValidas)
                        {
                            _tblAux = new TabelaPrecoSimplificada
                            {
                                idTabelaPreco = cmp.idTabelaPreco,
                                xTabelaPreco = cmp.xNome,
                                bCampanha = cmp.stTabelaPreco == 1,
                                stDefault = cmp.stDefault,
                                pComissao = cmp.pComissao ?? 0,
                                stValor = cmp.stValor,
                                pDescontoMaximo = cmp.pDescontoMaximo ?? 100,
                                pIndice = cmp.pIndiceTabela ?? 0,
                                bEscalonada = false,
                                lFaixaComissao = null
                            };

                            if (cmp.stValor == 2)
                            {
                                try
                                {
                                    BuscaPrecoCalculoHelper.CalcularTabelaManual(idTabelaPreco: cmp.idTabelaPreco,
                                        idProduto: pr.idProduto ?? 0,
                                        tblAux: _tblAux);
                                    _lCampanhasValidasVenda.Add(item: _tblAux);
                                }
                                catch (NullReferenceException ex)
                                {

                                }
                            }
                            else
                            {
                                BuscaPrecoCalculoHelper.CalcularTabelaAutomatica(tblAux: _tblAux, infProd: _infProduto,
                                    stValor: cmp.stValor, pIndiceTabela: cmp.pIndiceTabela ?? 0);
                                _lCampanhasValidasVenda.Add(item: _tblAux);
                            }
                        }
                    }

                    //Validação necessária porque pode haver campanhas disponíveis para as entidades envolvidas,
                    // porém não diponível para o produto

                    pr.lTabelaPreco = new List<TabelaPrecoSimplificada>();
                    if (_tblDef != null && _tblDef.stAtivo == true
                        && (_tblDef.dInicial == null
                        || _tblDef.dInicial <= DateTime.UtcNow)
                        && (_tblDef.dFinal == null
                        || _tblDef.dFinal >= DateTime.UtcNow
                        ))
                    {
                        var _tblDefParaVenda = PreparaDefClienteParaVenda(_tblDefCliente: _tblDef,
                                idProduto: pr.idProduto ?? 0,
                                _infProduto: _infProduto,
                                pr: pr);

                        if (_tbEscalonada != null &&
                            _tblDefParaVenda != null &&
                            _tbEscalonada.stAtivo == true &&
                            _tbEscalonada.idTabelaPrecoVinculo.HasValue &&
                            _tbEscalonada.idTabelaPrecoVinculo == _tblDefParaVenda.idTabelaPreco)
                        //if (_tbEscalonada != null && _tbEscalonada.idTabelaPrecoVinculo.HasValue == true)
                        {
                            _tblDefParaVenda.bEscalonada = true;
                            _tblDefParaVenda.lFaixaComissao = _tbEscalonada.lFaixaComissao;

                            //pr.bTabelasCarregadas = true;
                        }
                        else if (_tbEscalonada != null &&
                            _tblDefParaVenda != null &&
                            _tbEscalonada.stAtivo == true &&
                            !_tbEscalonada.idTabelaPrecoVinculo.HasValue)
                        {
                            _tblDefParaVenda.bEscalonada = true;
                            _tblDefParaVenda.lFaixaComissao = _tbEscalonada.lFaixaComissao;
                        }

                        if (_tblDefParaVenda != null)
                        {
                            if (idTabelaPrecoCondicao.GetValueOrDefault() == 0 || idTabelaPrecoCondicao.GetValueOrDefault() == _tblDefParaVenda.idTabelaPreco)
                            {
                                pr.lTabelaPreco.Add(_tblDefParaVenda);
                                pr.currentTabelaPreco = _tblDefParaVenda;
                            }
                            else
                            {
                                TabelaPrecoRepository.SetTabelaPrecoByProduto(item: pr,
                            idClienteOffLine: idClienteOff, idCliente: idCliente, _idRepresentante: idRepresentante, idTabelaPrecoCondicao: idTabelaPrecoCondicao);
                            }
                        }
                        else
                        {
                            TabelaPrecoRepository.SetTabelaPrecoByProduto(item: pr,
                                idClienteOffLine: idClienteOff, idCliente: idCliente, _idRepresentante: idRepresentante, idTabelaPrecoCondicao: idTabelaPrecoCondicao);
                        }

                    }
                    else
                    {
                        TabelaPrecoRepository.SetTabelaPrecoByProduto(item: pr,
                                idClienteOffLine: idClienteOff, idCliente: idCliente, _idRepresentante: idRepresentante, idTabelaPrecoCondicao: idTabelaPrecoCondicao);
                    }

                    if (_lCampanhasValidasVenda.Count > 0)
                    {
                        pr.currentTabelaPreco = _lCampanhasValidasVenda.OrderBy(c => c.vVenda).FirstOrDefault();
                        pr.lTabelaPreco.AddRange(_lCampanhasValidasVenda);
                    }
                }

            }

        }

    }


    /// <summary>
    /// Métodos de suporte para busca de preço
    /// </summary>
    public partial class BuscaPreco
    {
        static List<TabelaPrecoItemModel> _itemsManual = null;

        public static void LimpaItensTabelaManual()
        {
            _itemsManual = null;
        }

        //Utiliza tabela default de cliente para venda
        TabelaPrecoSimplificada PreparaDefClienteParaVenda(TabelaPrecoModel _tblDefCliente, int idProduto, ProdutoModel _infProduto, PedidoVendaItensModel pr)
        {
            TabelaPrecoSimplificada _tblAux = new TabelaPrecoSimplificada
            {
                idTabelaPreco = _tblDefCliente.idTabelaPreco,
                xTabelaPreco = _tblDefCliente.xNome,
                bCampanha = _tblDefCliente.stTabelaPreco == 1,
                stDefault = _tblDefCliente.stDefault,
                pComissao = _tblDefCliente.pComissao ?? 0,
                stValor = _tblDefCliente.stValor,
                pDescontoMaximo = _tblDefCliente.pDescontoMaximo ?? 100,
                pIndice = _tblDefCliente.pIndiceTabela ?? 0,
                bEscalonada = false,
                lFaixaComissao = null
            };

            if (_tblDefCliente.stValor == 2)
            {
                try
                {
                    if (_itemsManual == null)
                    {
                        _itemsManual = BuscaPrecoItemManualRepositorio.ObterItems(idTabelaPreco: _tblDefCliente.idTabelaPreco,
                            idEmpresa: App.EnvironmentPE.idEmpresaLogada) ?? new List<TabelaPrecoItemModel>();
                    }

                    TabelaPrecoItemModel _itemAux = _itemsManual.FirstOrDefault(i => i.idProduto == idProduto);

                    if (_itemAux != null)
                    {
                        BuscaPrecoCalculoHelper.CalcularTabelaManual(tblAux: _tblAux, itemManual: _itemAux);
                    }
                    else
                    {
                        return null;
                    }

                }
                catch (NullReferenceException ex)
                {
                    return null;
                }
            }
            else
            {
                BuscaPrecoCalculoHelper.CalcularTabelaAutomatica(
                    tblAux: _tblAux, infProd: _infProduto, stValor: _tblDefCliente.stValor, pIndiceTabela: _tblDefCliente.pIndiceTabela ?? 0);
            }

            return _tblAux;
        }
    }
}

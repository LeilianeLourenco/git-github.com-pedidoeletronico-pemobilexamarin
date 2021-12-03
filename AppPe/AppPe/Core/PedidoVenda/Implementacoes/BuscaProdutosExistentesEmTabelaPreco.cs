using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Xamarin.HLP.Mobile.AppPE.Core.PedidoVenda.Interfaces;
using Xamarin.HLP.Mobile.AppPE.Model.Repository;
using Xamarin.HLP.Mobile.AppPE.Model.Repository.BuscaPreco;
using Xamarin.HLP.Mobile.AppPE.Model.Repository.Interfaces;
using Xamarin.HLP.Mobile.AppPE.Model.Repository.Interfaces.BuscaPreco;
using Xamarin.HLP.Mobile.AppPE.Model.Repository.Interfaces.Representacao;
using Xamarin.HLP.Mobile.AppPE.Model.Repository.Representacao;

namespace Xamarin.HLP.Mobile.AppPE.Core.PedidoVenda.Implementacoes
{
    /// <summary>
    /// Classe responsável por trazer apenas produtos que possuam no mínimo uma tabela disponivel para a venda.
    /// 
    /// Funcionalidade solicitada por Paulo Rogério.
    /// 
    /// </summary>
    public class BuscaProdutosExistentesEmTabelaPreco : IBuscaProdutosParaVenda
    {
        IPrecoRepositorio _precoReposit;
        readonly int idCliente;
        readonly int idClienteOffLine;
        readonly int idRepresentante;
        readonly int idRepresentacao;
        public BuscaProdutosExistentesEmTabelaPreco(int idCliente, int idClienteOffLine, int idRepresentante, int idRepresentacao)
        {
            this._precoReposit = new PrecoRepositorio();
            this.idCliente = idCliente;
            this.idClienteOffLine = idClienteOffLine;
            this.idRepresentante = idRepresentante;
            this.idRepresentacao = idRepresentacao;
        }

        public List<int> BuscarProdutos(int idEmpresa, int? idCondicaoParaTabelaPreco = null)
        {
            var _tbls = this._precoReposit.BuscarSemRepresentacao(idEmpresa: idEmpresa, idProduto: 0,
                stBusca: Model.Repository.Interfaces.TipoPrecoBusca.tud,
                idCliente: this.idCliente, idClienteOffLine: this.idClienteOffLine,
                idRepresentante: this.idRepresentante, idRepresentacao: this.idRepresentacao
                ); 

            //processo de trazer apenas a tabela de preço vinculada na condição
            int? _idTabelaPrecoNaCondicao = null;
            if (idCondicaoParaTabelaPreco.GetValueOrDefault() > 0)
            {
                _idTabelaPrecoNaCondicao = CondicaoPagamentoRepository.GetIdTabelaPrecoCondicao(idCondicaoParaTabelaPreco.GetValueOrDefault());
                if (_idTabelaPrecoNaCondicao.GetValueOrDefault() > 0)
                {
                    _tbls = _tbls.Where(t => t.idTabelaPreco == _idTabelaPrecoNaCondicao).ToList();
                }
            }

            if (_tbls.Count(t => t.stValor != 2) > 0) //Em tabelas disponíveis existe tabela automática
            {

                if (_tbls.Count(t => t.stTabelaPrecoRepresentacao == false) > 0)
                {
                    return null; //Não há tabela manual ou configurada para uma representação em específica,
                    //portanto ao menos uma tabela preço estará disponível aos produtos.
                }
            }



            List<int> _prodsRetorno = new List<int>();

            IPrecoProdutoRepositorio _precoProdRep;
            IPrecoProdutoRepositorio _precoProdRpraRep;
            IRepresentacaoRepresentanteRepositorio _rprasRep = new RepresentacaoRepresentanteRepositorio();

            var _tblsManual = _tbls.Where(tb => tb.stValor == 2).ToList();

            if ((_tblsManual?.Count ?? 0) > 0)
            {
                _precoProdRep = new PrecoProdutoManuaisRepositorio(
                    idTabelasManuais: _tblsManual.Select(t => t.idTabelaPreco).ToList());

                var _idsProdutosManual = _precoProdRep.BuscarProdutosDisponiveis(idEmpresa: idEmpresa);

                if (_idsProdutosManual.Count > 0)
                {
                    _prodsRetorno.AddRange(_idsProdutosManual);
                }
            }
             

            //Obter representações do representante ativo
            var _idsRpras = _rprasRep.ObterRepresentadas(idRepresentante: idRepresentante);

            if((_idsRpras?.Count ?? 0) > 0)
            {
                IBuscaPrecoRepositorio _buscaTblsRpras = new BuscaPrecoRepresentacoesRepositorio(lIdRpras: _idsRpras);

                var _tblsPorRepresentacao = _buscaTblsRpras.RetornaPrecos(idEmpresa: idEmpresa, stBusca: TipoPrecoBusca.tud, id: 0);

                if ((_tblsPorRepresentacao?.Count ?? 0) > 0)
                {
                    //Das representações do passo anterior, verificar quais possuem tabela ativa
                    _precoProdRpraRep = new PrecoProdutoPorRepresentacaoRepositorio(
                        lIdTabelas: _tblsPorRepresentacao.Select(r => r.idTabelaPreco).ToList());

                    var _idsProdutosRpras = _precoProdRpraRep.BuscarProdutosDisponiveis(idEmpresa: idEmpresa);

                    if (_idsProdutosRpras.Count > 0)
                    {
                        _prodsRetorno.AddRange(_idsProdutosRpras);
                    }
                }
            }            

            return _prodsRetorno.Distinct().ToList();
        }



        public List<int> BuscarProdutosCampanha(int idEmpresa, int? idCondicaoParaTabelaPreco = null)
        {
            var _tbls = this._precoReposit.Buscar(idEmpresa: idEmpresa, idProduto: 0,
                stBusca: Model.Repository.Interfaces.TipoPrecoBusca.cmp,
                idCliente: this.idCliente, idClienteOffLine: this.idClienteOffLine,
                idRepresentante: this.idRepresentante, idRepresentacao: this.idRepresentacao
                );

            //processo de trazer apenas a tabela de preço vinculada na condição
            int? _idTabelaPrecoNaCondicao = null;
            if (idCondicaoParaTabelaPreco.GetValueOrDefault() > 0)
            {
                _idTabelaPrecoNaCondicao = CondicaoPagamentoRepository.GetIdTabelaPrecoCondicao(idCondicaoParaTabelaPreco.GetValueOrDefault());
                if (_idTabelaPrecoNaCondicao.GetValueOrDefault() > 0)
                {
                    _tbls = _tbls.Where(t => t.idTabelaPreco == _idTabelaPrecoNaCondicao).ToList();
                }
            }

            if (_tbls.Count(t => t.stValor != 2) > 0) //Em tabelas disponíveis existe tabela automática
            {

                if (_tbls.Count(t => t.stTabelaPrecoRepresentacao == false) > 0)
                {
                    return null; //Não há tabela manual ou configurada para uma representação em específica,
                    //portanto ao menos uma tabela preço estará disponível aos produtos.
                }
            }

            List<int> _prodsRetorno = new List<int>();

            IPrecoProdutoRepositorio _precoProdRep;
            IPrecoProdutoRepositorio _precoProdRpraRep;
            IRepresentacaoRepresentanteRepositorio _rprasRep = new RepresentacaoRepresentanteRepositorio();

            var _tblsManual = _tbls.Where(tb => tb.stValor == 2).ToList();

            if ((_tblsManual?.Count ?? 0) > 0)
            {
                _precoProdRep = new PrecoProdutoManuaisRepositorio(
                    idTabelasManuais: _tblsManual.Select(t => t.idTabelaPreco).ToList());

                var _idsProdutosManual = _precoProdRep.BuscarProdutosDisponiveis(idEmpresa: idEmpresa);

                if (_idsProdutosManual.Count > 0)
                {
                    _prodsRetorno.AddRange(_idsProdutosManual);
                }
            }

            return _prodsRetorno.Distinct().ToList();
        }
    }
}

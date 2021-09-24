using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Xamarin.HLP.Mobile.AppPE.Core.PedidoVenda.Interfaces;
using Xamarin.HLP.Mobile.AppPE.Model.Cadastros;
using Xamarin.HLP.Mobile.AppPE.Model.Lancamento;
using Xamarin.HLP.Mobile.AppPE.Model.Repository;
using Xamarin.HLP.Mobile.AppPE.Model.Repository.Interfaces.PedidoVenda;
using Xamarin.HLP.Mobile.AppPE.Model.Repository.Interfaces.ProdutoRep;

namespace Xamarin.HLP.Mobile.AppPE.Core.PedidoVenda.Implementacoes
{
    public class ItensImpressaoPedido : IItensImpressaoPedido
    {
        public PedidoVendaModel RetornarItensParaImpressao(int id)
        {
            IPedidoVendaBuscaRepositorio _buscaPedidoRep = new PedidoVendaBuscaParaImpressaoRepositorio();
            IProdutoBuscaRepositorio _prodRep = new ProdutoBuscaParaImpressaoPedidoRepositorio();

            var _obj = _buscaPedidoRep.Obter(id: id);

            var _itens = _obj.lItens;

            if(_itens?.Count == 0)
            {
                return _obj;
            }

            ProdutoModel _infoProd;
            var _itensAgrupados = new List<PedidoVendaItensModel>();
            PedidoVendaItensModel _itemAux;

            //foreach (var gr in _itens.GroupBy(gr => gr.idItemAgrupamento))
            //{
            //    _infoProd = _prodRep.Obter(id: gr.FirstOrDefault().idProdutoOffLine);
            //    _itemAux = new PedidoVendaItensModel
            //    {
            //        cAlternativo = _infoProd.cAlternativo, //33967
            //        idProdutoOffLine = gr.FirstOrDefault().idProdutoOffLine,
            //        vQtdItem = gr.Sum(g => g.vQtdItem),
            //        vUnitarioVendaComImpostos = gr.FirstOrDefault().vUnitarioVendaComImpostos,
            //        vSubTotal = gr.Sum(g => g.vSubTotal),
            //        xDescricao = _infoProd.xNome,
            //        idGradeCor = gr.FirstOrDefault().idGradeCor,
            //        idGradeTamanho = gr.FirstOrDefault().idGradeTamanho
            //    };

            //    _itemAux.SetDetalheItem();
            //    _itensAgrupados.Add(item: _itemAux);
            //}

            //OS 35384 - Jessica Barbieri
            //Trecho refeito, pois o anterior não recebia a listagem completa dos itens, apenas trazia o primeiro item
            foreach (var gr in _itens)
            {
                _infoProd = _prodRep.Obter(id: gr.idProdutoOffLine);
                _itemAux = new PedidoVendaItensModel
                {
                    cAlternativo = _infoProd.cAlternativo, //33967
                    idProdutoOffLine = gr.idProdutoOffLine,
                    vQtdItem = gr.vQtdItem,
                    vUnitarioVendaComImpostos = gr.vUnitarioVendaComImpostos,
                    vSubTotal = gr.vSubTotal,
                    xDescricao = _infoProd.xNome,
                    idGradeCor = gr.idGradeCor,
                    idGradeTamanho = gr.idGradeTamanho,
                    vDesconto = gr.vDesconto
                };

                _itemAux.SetDetalheItem();
                _itensAgrupados.Add(item: _itemAux);
            }

            _obj.lItens = new System.Collections.ObjectModel.ObservableCollection<PedidoVendaItensModel>(_itensAgrupados);
            return _obj;
        }
    }
}

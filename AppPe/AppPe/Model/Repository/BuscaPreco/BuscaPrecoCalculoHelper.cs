using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Xamarin.HLP.Mobile.AppPE.Model.Cadastros;

namespace Xamarin.HLP.Mobile.AppPE.Model.Repository.BuscaPreco
{
    public class BuscaPrecoCalculoHelper
    {

        public static void CalcularTabelaManual(int idTabelaPreco, int idProduto, TabelaPrecoSimplificada tblAux)
        {
            TabelaPrecoItemModel _itemManualAux = BuscaPrecoItemManualRepositorio.ObterItem(idTabelaPreco: idTabelaPreco,
                                idProduto: idProduto);

            AplicarCalculoTabelaManual(tblAux: tblAux, _itemManualAux: _itemManualAux);
        }

        public static void CalcularTabelaManual(TabelaPrecoSimplificada tblAux, TabelaPrecoItemModel itemManual)
        {
            AplicarCalculoTabelaManual(tblAux: tblAux, _itemManualAux: itemManual);
        }

        static void AplicarCalculoTabelaManual(TabelaPrecoSimplificada tblAux, TabelaPrecoItemModel _itemManualAux)
        {
            if (_itemManualAux == null)
            {
                throw new NullReferenceException();
            }

            tblAux.pDescontoMaximo = (double)(_itemManualAux.pDescontoMaximo ?? 0);
            tblAux.pIpiVenda = (double)_itemManualAux.pIpiVenda;
            tblAux.pStVenda = (double)_itemManualAux.pStVenda;
            tblAux.vUnitario = _itemManualAux.vVenda.GetValueOrDefault();

            if ((double)_itemManualAux.pIpiVenda > 0 || (double)_itemManualAux.pStVenda > 0)
            {
                var ipi = (double)_itemManualAux.vVenda * ((double)_itemManualAux.pIpiVenda / 100);
                var st = (double)_itemManualAux.vVenda * ((double)_itemManualAux.pStVenda / 100);

                tblAux.vVendaSemArredondamento = (double)_itemManualAux.vVenda + ipi + st;
                tblAux.vVenda = Extensions.ArredondarValorDecimal(((double)_itemManualAux.vVenda + ipi + st));  
            }
            else
            {
                tblAux.vUnitario = (double)_itemManualAux.vVendaComImpostos;
                tblAux.vVenda = (double)_itemManualAux.vVendaComImpostos; 
            }
            
            if(tblAux.bEscalonada == true)
            {
                tblAux.pComissao = tblAux.lFaixaComissao.FirstOrDefault()?.pComissao ?? 0;
            }
            else
            {
                tblAux.pComissao = (double)(_itemManualAux.pComissao ?? 0);
            }
        }

        public static void CalcularTabelaAutomatica(TabelaPrecoSimplificada tblAux, ProdutoModel infProd, byte stValor, double pIndiceTabela)
        {
            var _vTabelaAux = infProd.vVenda + (infProd.vVenda * (stValor == 0 ? -1 : 1) * (pIndiceTabela / 100));

            var _vTabela = Math.Round(value: _vTabelaAux, digits: 2, mode: MidpointRounding.ToEven);

            tblAux.vVenda = tblAux.vUnitario = _vTabela;
            tblAux.pIpiVenda = infProd.pIpiVenda ?? (double)0;
            tblAux.pStVenda = infProd.pStVenda ?? (double)0;

            if (infProd.pIpiVenda.HasValue)
            {
                tblAux.vVenda = tblAux.vVenda + (_vTabela * (infProd.pIpiVenda.Value / 100));
            }

            if (infProd.pStVenda.HasValue)
            {
                tblAux.vVenda = tblAux.vVenda + (_vTabela * (infProd.pStVenda.Value / 100));
            }
            

            if(tblAux.bEscalonada == true)
            {
                tblAux.pComissao = tblAux.lFaixaComissao.FirstOrDefault()?.pComissao ?? 0;
            }
        }
    }
}

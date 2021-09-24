using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Xamarin.HLP.Mobile.AppPE.Core.PedidoVenda.Interfaces;

namespace Xamarin.HLP.Mobile.AppPE.Core.PedidoVenda.Implementacoes
{
    public class Comissao : IComissao
    {
        public decimal CalcularPorcComissao(decimal vComissao, decimal pComissao, decimal pIpi, decimal pSt, bool bDescIpi, bool bDescSt)
        {
            throw new NotImplementedException();
        }

        public decimal CalcularValorComissao(decimal vUnitario, decimal pComissao, decimal pIpi, decimal pSt, bool bDescIpi, bool bDescSt)
        {
            var _vVenda = vUnitario;

            var _bDescontaIpi = bDescIpi;
            var _bDescontaSt = bDescSt;


            var _pIpi = pIpi;
            var _pSt = pSt;

            var _dIpi = _bDescontaIpi == false ? _pIpi / 100 : 0;
            var _dSt = _bDescontaSt == false ? _pSt / 100 : 0;

            var _vBaseComissao = _vVenda / (_dIpi + _dSt + 1);
            var _pComissao = pComissao / 100;

            return (_vBaseComissao * _pComissao);
        }
    }
}

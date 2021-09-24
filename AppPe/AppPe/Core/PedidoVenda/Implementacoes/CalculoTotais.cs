using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Xamarin.HLP.Mobile.AppPE.Core.PedidoVenda.Interfaces;

namespace Xamarin.HLP.Mobile.AppPE.Core.PedidoVenda.Implementacoes
{
    public class CalculoTotais : ICalculoTotais
    {
        public double CalcularTotalSemImpostos(double vUnitario, double vQtd, double pIpiVenda, double pStVenda)
        {
            var _pIpi = pIpiVenda;
            var _pSt = pStVenda;

            var _vIpi = (_pIpi / 100);
            var _vSt = (_pSt / 100);

            var _totalImpostos = _vIpi + _vSt;

            var _vUnitario = vUnitario;

            double _vTabelaSemImpostos = 0;

            if (_totalImpostos > 0)
            {
                _vTabelaSemImpostos = _vUnitario - (_vUnitario * _totalImpostos);
            }
            else
            {
                _vTabelaSemImpostos = _vUnitario;
            }

            return _vTabelaSemImpostos;
        }
    }
}

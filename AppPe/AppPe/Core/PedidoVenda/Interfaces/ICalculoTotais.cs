using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Xamarin.HLP.Mobile.AppPE.Core.PedidoVenda.Interfaces
{
    public interface ICalculoTotais
    {
        double CalcularTotalSemImpostos(double vUnitario, double vQtd, double pIpiVenda, double pStVenda);
    }
}

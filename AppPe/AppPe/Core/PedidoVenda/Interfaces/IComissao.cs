using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Xamarin.HLP.Mobile.AppPE.Core.PedidoVenda.Interfaces
{
    public interface IComissao
    {
        decimal CalcularValorComissao(decimal vUnitario, decimal pComissao, decimal pIpi, decimal pSt, bool bDescIpi, bool bDescSt);

        decimal CalcularPorcComissao(decimal vComissao, decimal pComissao, decimal pIpi, decimal pSt, bool bDescIpi, bool bDescSt);
    }
}

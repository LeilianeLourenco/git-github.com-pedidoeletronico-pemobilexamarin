using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Xamarin.HLP.Mobile.AppPE.Core.PedidoVenda.Interfaces;

namespace Xamarin.HLP.Mobile.AppPE.Core.PedidoVenda.Implementacoes
{
    public class DescontoValido : IDescontoValido
    {
        readonly double _pDescMaximo;
        public DescontoValido(double pDescMaximo)
        {
            this._pDescMaximo = pDescMaximo;
        }

        public bool ValidarDesconto(double pDesconto)
        {
            //adicionado para validar se o desconto máximo esta 0.
            bool _descontoValido = false;
            if(_pDescMaximo > 0)
            {
                _descontoValido = this._pDescMaximo >= pDesconto;
            }
            else
            {
                _descontoValido = true;
            }

            return _descontoValido;
        }
    }
}

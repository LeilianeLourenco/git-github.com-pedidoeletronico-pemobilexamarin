using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Xamarin.HLP.Mobile.AppPE.Model.Cadastros;
using Xamarin.HLP.Mobile.AppPE.Model.Repository.Interfaces;

namespace Xamarin.HLP.Mobile.AppPE.Model.Repository.BuscaPreco
{
    public class BuscaPrecoHelper
    {

        /// <summary>
        /// Query base para Tabelas/Campanhas automáticas
        /// </summary>
        /// <returns></returns>
        public static System.Linq.Expressions.Expression<Func<TabelaPrecoModel, bool>> BuildQueryPreco(int idEmpresa,
            TipoPrecoBusca stBusca, TipoPreco stPreco)
        {
            byte _stBusca = (byte)stBusca;
            byte _stPreco = (byte)stPreco;

            System.Linq.Expressions.Expression<Func<TabelaPrecoModel, bool>> _qry = t =>
                t.idEmpresa == idEmpresa
                            && (_stPreco == 0 ? t.stValor != 2 : t.stValor == 2)
                             && t.stAtivo == true
                             && (_stBusca == 2 ? t.idTabelaPreco > 0 : t.stTabelaPreco == _stBusca)
                             && (t.dInicial.HasValue == false || t.dInicial.Value < DateTime.UtcNow)
                             && (t.dFinal.HasValue == false || t.dFinal.Value > DateTime.UtcNow);
            return _qry;
        }

    }
}

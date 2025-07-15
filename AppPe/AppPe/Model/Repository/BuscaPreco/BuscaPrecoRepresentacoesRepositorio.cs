using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Xamarin.HLP.Mobile.AppPE.Common;
using Xamarin.HLP.Mobile.AppPE.Model.Cadastros;
using Xamarin.HLP.Mobile.AppPE.Model.Repository.Interfaces;

namespace Xamarin.HLP.Mobile.AppPE.Model.Repository.BuscaPreco
{
    public class BuscaPrecoRepresentacoesRepositorio : IBuscaPrecoRepositorio
    {
        readonly List<int> _lIdRpras;
        public BuscaPrecoRepresentacoesRepositorio(List<int> lIdRpras)
        {
            this._lIdRpras = lIdRpras;
        }

        public List<TabelaPrecoModel> RetornaPrecos(int idEmpresa, int id, TipoPrecoBusca stBusca, string filtro = null)
        {
            var _idsClausulaIn = _lIdRpras.Select(pr => pr)
                .ToList().Aggregate("", (current, item) => current + (current == "" ? "" : " , ") + item);

            string _xQry = $@"select t.idTabelaPreco, t.dInicial, t.dFinal from {TableMobile.TB_TABELAPRECO_REPRESENTACOES} tra 
                            join {TableMobile.TB_TABELAPRECO} t on tra.idTabelaPreco = t.idTabelaPreco                            
                            where t.stAtivo = 1 and t.stValor != 2 and tra.idRepresentada in ({_idsClausulaIn})";            

            var _tbls = App.Data.Connection.Query<TabelaPrecoModel>(_xQry);

            _tbls = _tbls?.Where(tb => (tb.dInicial == null || tb.dInicial <= DateTime.UtcNow)
            && (tb.dFinal == null || tb.dFinal >= DateTime.UtcNow)).ToList();

            if (_tbls == null || _tbls.Count == 0)
            {
                return new List<TabelaPrecoModel>();
            }

            return _tbls;
        }
    }
}

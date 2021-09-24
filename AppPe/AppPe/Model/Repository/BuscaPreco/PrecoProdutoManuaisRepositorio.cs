using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Xamarin.HLP.Mobile.AppPE.Common;
using Xamarin.HLP.Mobile.AppPE.Model.Cadastros;
using Xamarin.HLP.Mobile.AppPE.Model.Repository.Interfaces.BuscaPreco;

namespace Xamarin.HLP.Mobile.AppPE.Model.Repository.BuscaPreco
{
    public class PrecoProdutoManuaisRepositorio : IPrecoProdutoRepositorio
    {
        readonly List<int> _idTabelasManuais;
        public PrecoProdutoManuaisRepositorio()
        {

        }
        public PrecoProdutoManuaisRepositorio(List<int> idTabelasManuais)
        {
            this._idTabelasManuais = idTabelasManuais;
        }

        public List<int> BuscarProdutosDisponiveis(int idEmpresa)
        {
            var _lIdsTabelasManuais = _idTabelasManuais ?? new List<int>();

            if(_idTabelasManuais.Count == 0)
            {
                return new List<int>();
            }

            var _idsClausulaIn = _idTabelasManuais.Select(pr => pr)
                .ToList().Aggregate("", (current, item) => current + (current == "" ? "" : " , ") + item);

            string _xQry = $@"select ti.idProduto from {TableMobile.TB_TABELAPRECOITEM} ti 
                            join {TableMobile.TB_TABELAPRECO} t on ti.idTabelaPreco = t.idTabelaPreco                            
                            where ti.idTabelaPreco in ({_idsClausulaIn})";

            return App.Data.Connection.Query<TabelaPrecoItemModel>(_xQry).ToList()?.Select(i => i.idProduto).ToList();
        }
    }
}

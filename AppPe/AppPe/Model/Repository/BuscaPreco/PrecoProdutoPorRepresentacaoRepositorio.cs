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
    public class PrecoProdutoPorRepresentacaoRepositorio : IPrecoProdutoRepositorio
    {
        readonly List<int> _lIdTabelas;
        public PrecoProdutoPorRepresentacaoRepositorio(List<int> lIdTabelas)
        {
            this._lIdTabelas = lIdTabelas;
        }

        public List<int> BuscarProdutosDisponiveis(int idEmpresa)
        {
            var _lTabelas = this._lIdTabelas ?? new List<int>();

            if (_lTabelas.Count == 0)
            {
                return new List<int>();
            }

            var _idsClausulaIn = _lTabelas.Select(pr => pr)
                .ToList().Aggregate("", (current, item) => current + (current == "" ? "" : " , ") + item);

            string _xQry = $@"select tra.idRepresentada from {TableMobile.TB_TABELAPRECO_REPRESENTACOES} tra 
                            join {TableMobile.TB_TABELAPRECO} t on tra.idTabelaPreco = t.idTabelaPreco                            
                            where t.idTabelaPreco in ({_idsClausulaIn})";

            var _rpas = App.Data.Connection.Query<TabelaPrecoRepresentacoesModel>(_xQry)?.Select(r => r.idRepresentada).ToList();

            if((_rpas?.Count ?? 0) == 0)
            {
                return new List<int>();
            }

            var _idsClausulaInProdutos = _rpas.Select(r => r)
                .ToList().Aggregate("", (current, item) => current + (current == "" ? "" : " , ") + item);

            string _xQryProds = $@"select p.idProduto from tb_produto p where p.idRepresentada in ({_idsClausulaInProdutos})";

            return App.Data.Connection.Query<ProdutoModel>(_xQryProds)?.Select(r => r.idProduto ?? 0).ToList();
        }
    }
}

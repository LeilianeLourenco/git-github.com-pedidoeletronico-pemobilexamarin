using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Xamarin.HLP.Mobile.AppPE.Model.Cadastros;
using Xamarin.HLP.Mobile.AppPE.Model.Repository.Interfaces.ProdutoRep;

namespace Xamarin.HLP.Mobile.AppPE.Model.Repository
{
    public class ProdutoBuscaParaImpressaoPedidoRepositorio : IProdutoBuscaRepositorio
    {
        public ProdutoModel Obter(int id)
        {
            string _xQuery = $"select xNome, cAlternativo from tb_produto where idProdutoOffLine = {id}"; //33967

            var _obj = App.Data.Connection.Query<ProdutoModel>
                (_xQuery).FirstOrDefault();

            return _obj;

        }
    }
}

using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Xamarin.HLP.Mobile.AppPE.Model.Cadastros;

namespace Xamarin.HLP.Mobile.AppPE.Model.Repository.Interfaces.ProdutoRep
{
    public interface IProdutoBuscaRepositorio
    {

        ProdutoModel Obter(int id);

    }
}

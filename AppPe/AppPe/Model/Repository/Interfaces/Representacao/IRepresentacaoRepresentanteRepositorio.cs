using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Xamarin.HLP.Mobile.AppPE.Model.Repository.Interfaces.Representacao
{
    public interface IRepresentacaoRepresentanteRepositorio
    {
        List<int> ObterRepresentadas(int idRepresentante);
    }
}

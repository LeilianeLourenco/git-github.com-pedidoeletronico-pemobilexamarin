using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Xamarin.HLP.Mobile.AppPE.Model.Cadastros;

namespace Xamarin.HLP.Mobile.AppPE.Model.Repository.ClienteRep
{
    public class ClienteEnderecoRepositorio
    {
        /// <summary>
        /// Para garantir q será retornado um endereço do cliente, caso o mesmo não possua um principal será retornado o primeiro endereço encontrado
        /// </summary>
        /// <returns></returns>
        public EnderecoModel ObterEnderecoPrincipalCliente(int idCliente)
        {
            return App.Data.Connection.Table<EnderecoModel>()
                .Where(e => e.idClientes == idCliente)
                .OrderBy(cl => cl.stPrincipal)
                .FirstOrDefault();
        }
    }
}

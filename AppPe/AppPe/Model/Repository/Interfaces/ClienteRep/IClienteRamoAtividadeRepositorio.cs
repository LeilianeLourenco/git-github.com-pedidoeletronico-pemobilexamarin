using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Xamarin.HLP.Mobile.AppPE.Model.Repository.Interfaces.ClienteRep
{
    public interface IClienteRamoAtividadeRepositorio
    {
        List<int> ObterRamosCliente(int idEmpresa, int idCliente);
    }
}

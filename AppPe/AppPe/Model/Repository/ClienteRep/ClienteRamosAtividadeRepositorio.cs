using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Xamarin.HLP.Mobile.AppPE.Common;
using Xamarin.HLP.Mobile.AppPE.Model.Cadastros;
using Xamarin.HLP.Mobile.AppPE.Model.Repository.Interfaces.ClienteRep;

namespace Xamarin.HLP.Mobile.AppPE.Model.Repository.ClienteRep
{
    public class ClienteRamosAtividadeRepositorio : IClienteRamoAtividadeRepositorio
    {
        public List<int> ObterRamosCliente(int idEmpresa, int idCliente)
        {  
            var _ramos = App.Data.Connection.Query<ClienteRamosAtividade>($"select * from {TableMobile.tb_clienteramosatividade} where idEmpresa = {idEmpresa} and idCliente = {idCliente}").ToList();

            if (_ramos?.Count > 0)
            {
                return _ramos.Select(r => r.idRamoAtividade).ToList();
            }

            return new List<int>();
        }
    }
}

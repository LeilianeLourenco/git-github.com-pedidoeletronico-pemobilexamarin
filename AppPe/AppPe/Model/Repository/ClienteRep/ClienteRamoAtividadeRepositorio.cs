using System;

namespace Xamarin.HLP.Mobile.AppPE.Model.Repository.ClienteRep
{
    public class ClienteRamoAtividadeRepositorio
    {
        public int ObterIdRamoAtividade(int idEmpresa, int idCliente)
        {
            var _xQuery = $@"
select idRamoAtividade from tb_clientes where idEmpresa = {idEmpresa} and idClientes = {idCliente}
";

            try
            {
                return App.Data.Connection.ExecuteScalar<int>(_xQuery);
            }
            catch (Exception ex)
            {
                return 0;
            }
            
        }
    }
}

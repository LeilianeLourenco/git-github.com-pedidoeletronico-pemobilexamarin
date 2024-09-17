using System;
using System.Linq;
using Xamarin.HLP.Mobile.AppPE.Common;

namespace Xamarin.HLP.Mobile.AppPE.Model.Repository
{
    public class ExtensaoEmpresaRepository
    {
        public static bool GetbGeraOrcamento(int idEmpresa)
        {
            var xQuery =
               $@"SELECT bGeraOrcamento FROM {TableMobile.TB_EXTENSAO} WHERE 
                                    idEmpresa = {idEmpresa}";
            var resultado = App.Data.Connection.ExecuteScalar<bool>(xQuery);
            return resultado;
        }      
    }
}

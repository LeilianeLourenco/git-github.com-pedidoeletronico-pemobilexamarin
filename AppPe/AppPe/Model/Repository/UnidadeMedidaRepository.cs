using System.Collections.Generic;
using System.Linq;
using Xamarin.HLP.Mobile.AppPE.Common;

namespace Xamarin.HLP.Mobile.AppPE.Model.Repository
{
    public class UnidadeMedidaRepository
    {

        public static List<BasicPickerModel> GetListToBasicPickerModel()
        {
            var xQuery = string.Format(@"select idUnidadeMedida Id , xUnidadeMedida Display from {0}
                                                    where stAtivo = 1 and idEmpresa = {1}", TableMobile.TB_UNIDADEMEDIDA, App.CurrentAspnetUserModel.objEmpresaAspnetUsersModel.idEmpresa);
            var dados = App.Data.Connection.Query<BasicPickerModel>(xQuery);
            return dados == null ? new List<BasicPickerModel>() : dados.OrderBy(c => c.Display).ToList();
        }

        public static string GetNomeUN(int idUnidadeMedida)
        {

            var xQuery = $@"SELECT xUnidadeMedida FROM {TableMobile.TB_UNIDADEMEDIDA} WHERE idUnidadeMedida = {idUnidadeMedida}";

            var retorno = App.Data.Connection.ExecuteScalar<string>(xQuery);

            return retorno;


            //return
            //    App.Data.Connection.Table<UnidadeMedidaModel>()
            //        .Where(c => c.idUnidadeMedida == idUnidadeMedida)
            //        .Select(c => c.xUnidadeMedida)
            //        .FirstOrDefault();
        }

    }
}

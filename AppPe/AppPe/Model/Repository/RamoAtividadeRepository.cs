using System;
using System.Collections.Generic;
using System.Linq;
using Xamarin.HLP.Mobile.AppPE.Common;

namespace Xamarin.HLP.Mobile.AppPE.Model.Repository
{
    public class RamoAtividadeRepository
    {

        public static List<BasicPickerModel> GetListToBasicPickerModel()
        {
            var xQuery = string.Format(@"select idRamoAtividade Id , xRamoAtividade Display from tb_ramoatividade
                                                    where idEmpresa = {0}", App.CurrentAspnetUserModel.objEmpresaAspnetUsersModel.idEmpresa);
            var dados = App.Data.Connection.Query<BasicPickerModel>(xQuery);
            if (dados == null)
                return new List<BasicPickerModel>();
            return dados.OrderBy(c => c.Display).ToList();

        }

        public static List<ListItemModel> Get(int skip, int take, string xFiltro)
        {
            try
            {
                var idEmpresa = App.CurrentAspnetUserModel.objEmpresaAspnetUsersModel.idEmpresa;
                var xQuery = "";
                const string xFields =
                    "xRamoAtividade Display, idRamoAtividade Id";
                xQuery = $"select {xFields} from {TableMobile.TB_RAMOATIVIDADE} where idEmpresa = {idEmpresa}";

                if (!string.IsNullOrEmpty(xFiltro))
                {
                    xFiltro = xFiltro.RemoverAcentos().ToUpper();
                    xQuery += $" and (UPPER(xRamoAtividade) like('%{xFiltro}%') or UPPER(coalesce(xDisplaySemCaracter,'')) like('%{xFiltro}%'))";
                    //xQuery += $" and UPPER(xRamoAtividade) like('%{xFiltro.ToUpper()}%') ";
                }


                xQuery += $@" order by UPPER(xRamoAtividade)
                                            LIMIT {take} OFFSET {skip}";

                var resultado = App.Data.Connection.Query<ListItemModel>(xQuery);
                return resultado;
            }
            catch (Exception ex)
            {
                App.Messages.ShowAsync(ex.Message);
                return new List<ListItemModel>();
            }
        }


        public static ListItemModel GetItem(int idRamoAtividade)
        {
            var xQuery = $@"select idRamoAtividade Id , xRamoAtividade Display from tb_ramoatividade
                                                    where idEmpresa = {App.CurrentAspnetUserModel
                .objEmpresaAspnetUsersModel.idEmpresa} and idRamoAtividade = {idRamoAtividade}";
            var dados = App.Data.Connection.Query<ListItemModel>(xQuery);
            if (dados == null)
                return new ListItemModel();
            return dados.FirstOrDefault();
        }

        public static ListItemModel GetFirstItem()
        {
            var xQuery = $@"select idRamoAtividade Id , xRamoAtividade Display from tb_ramoatividade
                                                    where idEmpresa = {App.CurrentAspnetUserModel
                .objEmpresaAspnetUsersModel.idEmpresa} limit 1";
            var dados = App.Data.Connection.Query<ListItemModel>(xQuery);
            if (dados == null)
                return new ListItemModel();
            return dados.FirstOrDefault();
        }


        [Obsolete]
        public static BasicPickerModel GetBasicPickerModel(int idRamoAtividade)
        {
            var xQuery = string.Format(@"select idRamoAtividade Id , xRamoAtividade Display from tb_ramoatividade
                                                    where idEmpresa = {0} and idRamoAtividade = {1}", App.CurrentAspnetUserModel.objEmpresaAspnetUsersModel.idEmpresa, idRamoAtividade);
            var dados = App.Data.Connection.Query<BasicPickerModel>(xQuery);
            if (dados == null)
                return new BasicPickerModel();
            return dados.FirstOrDefault();
        }


    }
}

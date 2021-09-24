using System;
using System.Collections.Generic;
using System.Linq;
using Xamarin.HLP.Mobile.AppPE.Common;

namespace Xamarin.HLP.Mobile.AppPE.Model.Repository
{
    public class TransportadoraRepository
    {

        public static List<BasicPickerModel> GetListToBasicPickerModel()
        {
            var xQuery =
                $@"select idTransportadora Id , coalesce(idTransportadora,0) IdOnline, xRazaoSocial Display, xFantasia Detail, 'False' bTrazerImagem  from {TableMobile.TB_TRANSPORTADORAS}
                                                    where stAtivo = 1 and idEmpresa = {App.CurrentAspnetUserModel.objEmpresaAspnetUsersModel.idEmpresa}";

            var dados = App.Data.Connection.Query<BasicPickerModel>(xQuery);
            return dados == null ? new List<BasicPickerModel>() : dados.OrderBy(c => c.Display).ToList();
        }

        public static List<ListItemModel> Get(int skip, int take, string xFiltro)
        {
            try
            {
                var idEmpresa = App.CurrentAspnetUserModel.objEmpresaAspnetUsersModel.idEmpresa;
                var xQuery = "";
                const string xFields =
                    "xRazaoSocial Display, xFantasia Detail, idTransportadora Id";
                xQuery = $"select {xFields} from {TableMobile.TB_TRANSPORTADORAS} where idEmpresa = {idEmpresa} and stAtivo = 1";

                if (!string.IsNullOrEmpty(xFiltro))
                {
                    xFiltro = xFiltro.RemoverAcentos().ToUpper();
                    xQuery += $" and (UPPER(xRazaoSocial) like('%{xFiltro}%') or UPPER(coalesce(xDisplaySemCaracter,'')) like('%{xFiltro}%'))";

                    //xQuery += $" and UPPER(xRazaoSocial) like('%{xFiltro.ToUpper()}%') ";
                }


                xQuery += $@" order by UPPER(xRazaoSocial)
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


        public static ListItemModel GetItem(int idTransportadora)
        {
            try
            {
                var idEmpresa = App.CurrentAspnetUserModel.objEmpresaAspnetUsersModel.idEmpresa;
                var xQuery = "";
                const string xFields =
                    "xRazaoSocial Display, xFantasia Detail, idTransportadora Id";
                xQuery = $"select {xFields} from {TableMobile.TB_TRANSPORTADORAS} where idEmpresa = {idEmpresa} and idTransportadora = {idTransportadora} and stAtivo = 1";

                var resultado = App.Data.Connection.Query<ListItemModel>(xQuery);

                if (resultado?.Count() > 0)
                {
                    return resultado.FirstOrDefault();
                }
                else
                {
                    xQuery = $"select {xFields} from {TableMobile.TB_TRANSPORTADORAS} where idEmpresa = {idEmpresa} and stAtivo = 1";

                    resultado = App.Data.Connection.Query<ListItemModel>(xQuery);
                    if (resultado?.Count() > 0)
                    {
                        return resultado.FirstOrDefault();
                    } 
                }
                return new ListItemModel();
            }
            catch (Exception ex)
            {
                App.Messages.ShowAsync(ex.Message);
                return new ListItemModel();
            }
        }

        public static ListItemModel GetFirstItem()
        {
            try
            {
                var idEmpresa = App.CurrentAspnetUserModel.objEmpresaAspnetUsersModel.idEmpresa;
                var xQuery = "";
                const string xFields =
                    "xRazaoSocial Display, xFantasia Detail, idTransportadora Id";
                xQuery = $"select {xFields} from {TableMobile.TB_TRANSPORTADORAS} where stAtivo = 1 and idEmpresa = {idEmpresa} limit 1";

                var resultado = App.Data.Connection.Query<ListItemModel>(xQuery);

                if (resultado != null)
                {
                    return resultado.FirstOrDefault();
                }
                return new ListItemModel();
            }
            catch (Exception ex)
            {
                App.Messages.ShowAsync(ex.Message);
                return new ListItemModel();
            }
        }


        public static string GetDisplay(int idTransportadora)
        {
            try
            {
                var xquery = $@"select xRazaoSocial from TB_TRANSPORTADORAS
                        where idTransportadora  = {idTransportadora} and idEmpresa = {App.CurrentAspnetUserModel
                    .objEmpresaAspnetUsersModel.idEmpresa}";
                var result = App.Data.Connection.ExecuteScalar<string>(xquery);
                return result;
            }
            catch (Exception ex)
            {
                return "-";
            }
        }
    }
}

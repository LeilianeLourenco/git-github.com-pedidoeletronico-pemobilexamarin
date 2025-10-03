using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using Xamarin.HLP.Mobile.AppPE.Common;
using Xamarin.HLP.Mobile.AppPE.Model.Cadastros;

namespace Xamarin.HLP.Mobile.AppPE.Model.Repository.Anexos
{
    public class AnexosRepository
    {
        public static void SaveAnexos(List<AnexosModel> anexos)
        {
            try
            {
                foreach (var obj in anexos)
                {
                    if (obj.idAnexo == 0)
                        App.Data.Connection.Insert(obj);
                    else
                        App.Data.Connection.Update(obj);
                }
            }
            catch (Exception ex)
            {
                ex.TrakException();
            }
        }

        public static List<AnexosModel> GetAnexosParaUploadModel()
        {
            var dataSync = App.CurrentAspnetUserModel.objEmpresaAspnetUsersModel.UltimaSyncDateTime.AddHours(-3);

            var lUpload =
               App.Data.Connection.Table<AnexosModel>()
                   .Where(
                       c =>
                           (c.dtUltimaAlteracao > dataSync && c.bSincronizado == false
                           && c.idEmpresa == App.CurrentAspnetUserModel.objEmpresaAspnetUsersModel.idEmpresa));

            return lUpload.ToList();
        }

        public static List<AnexosModel> GetAnexosAtividade(int idAtividadeOffline)
        {
            var idEmpresa = App.CurrentAspnetUserModel.objEmpresaAspnetUsersModel.idEmpresa;

            var xQueryAnexo =
               $"SELECT * FROM {TableMobile.TB_ANEXOS} WHERE idAtividade = {idAtividadeOffline} and idEmpresa = {idEmpresa}";

            var anexos = (App.Data.Connection.Query<AnexosModel>(xQueryAnexo)).ToList();
            return anexos;
        }

        public static void DeleteAnexos(int idAtividadeOffLine)
        {
            App.Data.Connection.Table<AnexosModel>().Delete(c => c.idAtividade == idAtividadeOffLine
               && c.idEmpresa == App.CurrentAspnetUserModel.objEmpresaAspnetUsersModel.idEmpresa);
        }

        public static void Delete(int idAtividadeOffLine)
        {
            App.Data.Connection.Table<AnexosModel>().Delete(c => c.idEmpresa == App.CurrentAspnetUserModel.objEmpresaAspnetUsersModel.idEmpresa);
        }

        public static void AnexoSincronizado(int idAnexo)
        {
            App.Data.Connection.Execute($"UPDATE tb_anexos SET bSincronizado = 1 WHERE idAnexo = {idAnexo}");
        }
    }
}

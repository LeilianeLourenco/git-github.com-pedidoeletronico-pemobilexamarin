using System;
using System.Collections.Generic;
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
            var lUpload =
               App.Data.Connection.Table<AnexosModel>()
                   .Where(
                       c =>
                           (c.dtUltimaAlteracao >
                           App.CurrentAspnetUserModel.objEmpresaAspnetUsersModel.UltimaSyncDateTime)
                           && c.idEmpresa == App.CurrentAspnetUserModel.objEmpresaAspnetUsersModel.idEmpresa);

            return lUpload.ToList();
        }
    }
}

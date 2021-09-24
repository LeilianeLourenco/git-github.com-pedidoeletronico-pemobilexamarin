namespace Xamarin.HLP.Mobile.AppPE.Model
{
    //[Table(TableMobile.TB_LOGSYNC)]
    //public class LogSyncModel
    //{
    //    [PrimaryKey, AutoIncrement]
    //    public int Id { get; set; }
    //    [NotNull]
    //    public int idEmpresa { get; set; }
    //    [NotNull]
    //    public string idAspnetUser { get; set; }
    //    [NotNull]
    //    public string xTable { get; set; }

    //    [NotNull]
    //    public DateTime dtUltimaSync { get; set; }


    //    /// <summary>
    //    /// U -> UPLOAD
    //    /// D -> DOWNLOAD
    //    /// </summary>
    //    [NotNull]
    //    public string StSync { get; set; }
       
    //    [Ignore]
    //    public enumTipoSync TipoSync
    //    {
    //        get { return this.StSync == "U" ? enumTipoSync.Upload : enumTipoSync.Download; }
    //        set
    //        {
    //            if (value == enumTipoSync.Upload)
    //                this.StSync = "U";
    //            else
    //                this.StSync = "D";
    //        }
    //    }

    //    public enum enumTipoSync { Upload, Download }

    //    public static List<LogSyncModel> GetDadosSyncInicial(enumTipoSync tipoSync)
    //    {
    //        return (typeof(TableMobile)).GetRuntimeFields().Where
    //            (c => c.Name.ToUpper().StartsWith("TB_")).Select(tb =>
    //                new LogSyncModel
    //                {
    //                    xTable = tb.GetValue(tb).ToString(),
    //                    dtUltimaSync = DateTime.MinValue,
    //                    idEmpresa = App.CurrentAspnetUserModel.objEmpresaAspnetUsersModel.idEmpresa,
    //                    idAspnetUser = App.CurrentAspnetUserModel.Id,
    //                    TipoSync = tipoSync
    //                }).ToList();
    //    }


    //}
}

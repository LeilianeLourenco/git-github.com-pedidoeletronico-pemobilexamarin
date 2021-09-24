using System;
using SQLite;
using Xamarin.HLP.Mobile.AppPE.Common;

namespace Xamarin.HLP.Mobile.AppPE.Model.Cadastros
{
    [Table(TableMobile.TB_LOGEXCLUSAO)]
    public class LogExclusaoModel
    {
        [PrimaryKey, AutoIncrement]
        public int idLogExclusao { get; set; }
        [NotNull]
        public string xTable { get; set; }
        [NotNull()]
        public int idPK { get; set; }
        [NotNull]
        public int idEmpresa { get; set; }

        public DateTime dtExclusao { get; set; }
    }
}

using SQLite;
using System;
using Xamarin.HLP.Mobile.AppPE.Common;

namespace Xamarin.HLP.Mobile.AppPE.Model.Cadastros
{
    [Table(TableMobile.TB_ANEXOS)]
    public class AnexosModel : ModelComum
    {
        [PrimaryKey, AutoIncrement]
        public int idAnexo { get; set; }
        public int idEmpresa { get; set; }
        public int? idAtividade { get; set; }
        public bool bSincronizado { get; set; }
        public string base64Image { get; set; }
        public string xNomeArquivo { get; set; }
        public string xCaminhoArquivo { get; set; }
        public string xPathArquivo { get; set; }
        public string xCaminhoServidor { get; set; }
        public DateTime? dtUltimaAlteracao { get; set; }
    }
}

using SQLite;
using Xamarin.HLP.Mobile.AppPE.Common;

namespace Xamarin.HLP.Mobile.AppPE.Model.Cadastros
{
    [Table(TableMobile.TB_RAMOATIVIDADE)]
    public class RamoAtividadeModel
    {
        [PrimaryKey()]
        public int? idRamoAtividade { get; set; }
        
        private string _xRamoAtividade;
        [NotNull]
        public string xRamoAtividade
        {
            get { return _xRamoAtividade; }
            set
            {
                _xRamoAtividade = value;
                xDisplaySemCaracter = value.RemoverAcentos();
            }
        }

        public string cRamoAtividade { get; set; }
        [NotNull]
        public int idEmpresa { get; set; }
        [NotNull]
        public bool stAtivo { get; set; }

        public string xDisplaySemCaracter { get; set; }
    }
}

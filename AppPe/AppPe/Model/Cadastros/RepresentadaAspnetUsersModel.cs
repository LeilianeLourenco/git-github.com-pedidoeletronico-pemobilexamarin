using SQLite;
using Xamarin.HLP.Mobile.AppPE.Common;

namespace Xamarin.HLP.Mobile.AppPE.Model.Cadastros
{
    [Table(TableMobile.TB_REPRESENTADA_ASPNETUSERS)]
    public class RepresentadaAspnetUsersModel : ModelComum
    {
        [PrimaryKey]
        public int? idRepresentada_aspnetUsers { get; set; }

        public int idRepresentada { get; set; }

        public string xRazaoSocial { get; set; }

        public string xFantasia { get; set; }

        public double? pComissao { get; set; }

        private int _idEmpresa_aspnetUsers;
        public int idEmpresa_aspnetUsers
        {
            get { return _idEmpresa_aspnetUsers; }
            set { _idEmpresa_aspnetUsers = value; }
        }
    }
}

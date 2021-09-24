using SQLite;
using Xamarin.HLP.Mobile.AppPE.Common;

namespace Xamarin.HLP.Mobile.AppPE.Model.Cadastros
{
    [Table(TableMobile.TB_TRANSPORTADORAS)]
    public class TransportadorasModel
    {
        [PrimaryKey()]
        public int? idTransportadora { get; set; }
        
        private string _xRazaoSocial;
        [NotNull]
        public string xRazaoSocial
        {
            get { return _xRazaoSocial; }
            set
            {
                _xRazaoSocial = value;
                xDisplaySemCaracter = value.RemoverAcentos();
            }
        }


        public string xDisplaySemCaracter { get; set; }


        private string _xFantasia;
        [NotNull]
        public string xFantasia
        {
            get { return _xFantasia == null ? "TRANSPORTADORA" : _xFantasia; }
            set { _xFantasia = value; }
        }

        public string xCnpj { get; set; }
        public string xIe { get; set; }
        public string xAnotacao { get; set; }
        public string xEmails { get; set; }
        public string xTelefones { get; set; }
        [NotNull]
        public int? idEmpresa { get; set; }
        public bool stAtivo { get; set; }

        [Ignore]
        public string xAbreviacao => this.xRazaoSocial[0].ToString().ToUpper() + this.xRazaoSocial[1].ToString().ToUpper();

        [Ignore]
        public string xDisplayTelefone
        {
            get
            {
                if (string.IsNullOrEmpty(xTelefones))
                {
                    return "nenhum telefone registrado";
                }
                else
                {
                    return xTelefones.Replace(",", " - ");
                }
            }
        }


        [Ignore]
        public string xDisplayRazaoSocial
        {
            get
            {
                if (string.IsNullOrEmpty(xRazaoSocial))
                {
                    return "PESQUISE UMA TRANSPORTADORA";
                }
                else
                {
                    return xRazaoSocial.ToUpper();
                }
            }
        }

    }
}

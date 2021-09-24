namespace Xamarin.HLP.Mobile.AppPE.Model
{
    public class EnderecoFoneClienteContatoModel
    {

        public string xDescricao { get; set; }

        public bool hasEmail => !string.IsNullOrEmpty(xEmail);

        public bool hasFone => !string.IsNullOrEmpty(xFone);

        public string xFone { get; set; }

        public string xEmail { get; set; }

        public string Agrupamento { get; set; }
    }
}

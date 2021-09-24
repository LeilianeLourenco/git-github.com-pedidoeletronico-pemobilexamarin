using System;
using SQLite;
using Xamarin.HLP.Mobile.AppPE.Common;

namespace Xamarin.HLP.Mobile.AppPE.Model.Cadastros
{
    [Table(TableMobile.TB_CONTATOS)]
    public class ContatoModel : ModelComum
    {
        public ContatoModel()
        {
            stDepartamento = "AD";
            Guid g;
            g = Guid.NewGuid();
            idGuid = g.ToString();
        }

        [Ignore]
        public string idGuid { get; set; }

        public int? idContatos { get; set; }

        [PrimaryKey(), AutoIncrement()]
        public int? idContatoOffLine { get; set; }

        private string _xNome;
        [NotNull]
        public string xNome
        {
            get { return _xNome; }
            set
            {
                _xNome = value; NotifyPropertyChanged();
            }
        }

        private string _stDepartamento = "AD";
        public string stDepartamento
        {
            get { return _stDepartamento; }
            set
            {
                _stDepartamento = value; NotifyPropertyChanged();
            }
        }

        private string _xCargo;
        public string xCargo
        {
            get { return _xCargo; }
            set
            {
                _xCargo = value; NotifyPropertyChanged();
            }
        }

        private string _xAnotacao;
        public string xAnotacao
        {
            get { return _xAnotacao; }
            set
            {
                _xAnotacao = value; NotifyPropertyChanged();
            }
        }

        public int? idClientes { get; set; }

        public int? idClientesOffLine { get; set; }
        public int? idRepresentada { get; set; }

        public DateTime? dtUltimaAlteracao { get; set; }

        [NotNull]
        public int idEmpresa { get; set; }

        /// <summary>
        /// Propriedade usada para salvar o id do usuario que cadastrou o cliente
        /// </summary>
        public string idAspnetUsers { get; set; }

        public DateTime? dtCadastro { get; set; }

        private string _xEmail;

        public string xEmail
        {
            get { return _xEmail; }
            set { _xEmail = value; NotifyPropertyChanged(); NotifyPropertyChanged("xDisplay"); }
        }

        private string _xTelefone;

        public string xTelefone
        {
            get { return _xTelefone; }
            set { _xTelefone = value; NotifyPropertyChanged(); }
        }



        private bool _stUsaCatalogo;

        public bool stUsaCatalogo
        {
            get
            {
                return _stUsaCatalogo;
            }
            set { _stUsaCatalogo = value; NotifyPropertyChanged(); }
        }

        [Ignore]
        public string xDisplay
        {
            get
            {
                var sReturn = "sem email";

                if (!string.IsNullOrEmpty(xEmail))
                    sReturn = xEmail;
                return sReturn;
            }
        }

       
    }
}

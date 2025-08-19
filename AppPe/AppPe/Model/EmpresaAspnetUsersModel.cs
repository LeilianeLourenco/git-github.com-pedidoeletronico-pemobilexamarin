using System;
using System.Collections.Generic;
using SQLite;
using Xamarin.HLP.Mobile.AppPE.Common;
using Xamarin.HLP.Mobile.AppPE.Model.Empresa;

namespace Xamarin.HLP.Mobile.AppPE.Model
{
    [Table(TableMobile.TB_EMPRESA_ASPNETUSERS)]
    public class EmpresaAspnetUsersModel : ModelComum
    {
        
        [PrimaryKey]
        public int? idEmpresa_aspnetUsers { get; set; }

        private string _xNome;
        public string xNome
        {
            get { return _xNome ?? ""; }
            set
            {
                _xNome = value;
                xDisplaySemCaracter = value.RemoverAcentos();
            }
        }
        public string xDisplaySemCaracter { get; set; }

        private string _xApelido;
        public string xApelido
        {
            get { return _xApelido; }
            set { _xApelido = string.IsNullOrEmpty(value) ? "Apelido" : value; }
        }

        private string _xEmail;
        public string xEmail
        {
            get { return _xEmail ?? ""; }
            set
            {
                _xEmail = value;
            }

        }

        private string _imUsuario;
        public string imUsuario
        {
            get
            {
                return _imUsuario;
            }
            set { _imUsuario = value; NotifyPropertyChanged(); }
        }

        public string imUsuarioCropped { get; set; }

        public bool? bProibidoAlterarCadastroCliente { get; set; }

        public int? qVisitaNovosCliente { get; set; } = 0;

        public byte? stPeriodoNovosClientes { get; set; }
         
        public int? idJornada { get; set; }
         
        public int? qVisitaClientesAtivos { get; set; } = 0;

        public byte? stPeriodoClientesAtivos { get; set; }

        public bool _stAdministrador = false;
        public bool stAdministrador
        {
            get { return _stAdministrador; }
            set
            {
                _stAdministrador = value;
            }
        }

        public byte stAcessoTodosClientes { get; set; } = 0;
        
        public int idEmpresa { get; set; }

        private bool _stAtivo;

        public bool stAtivo
        {
            get { return _stAtivo; }
            set { _stAtivo = value; NotifyPropertyChanged(); }
        }

        private bool _isAtiva;
        /// <summary>
        /// Status para saber se essa empresa está ativa no momento.  current empresa
        /// </summary>
        public bool isAtiva
        {
            get { return _isAtiva; }
            set { _isAtiva = value; }
        }

        private DateTime _ultimaSyncDateTime = DateTime.Today.AddYears(-50);
        public DateTime UltimaSyncDateTime
        {
            get { return _ultimaSyncDateTime; }
            set { _ultimaSyncDateTime = value; base.NotifyPropertyChanged(); }
        }

        private DateTime? _ultimaSyncServerDateTime;
        public DateTime? ultimaSyncServerDateTime
        {
            get { return _ultimaSyncServerDateTime; }
            set { _ultimaSyncServerDateTime = value; NotifyPropertyChanged(); }
        }

        private DateTime? _ultimaSyncServerDateTimeCliente;
        public DateTime? ultimaSyncServerDateTimeCliente
        {
            get { return _ultimaSyncServerDateTimeCliente; }
            set { _ultimaSyncServerDateTimeCliente = value; NotifyPropertyChanged(); }
        }


        [Ignore]
        public EmpresaModel objEmpresaModel { get; set; }

        [Ignore]
        public PermissoesRepresentantesModel permissoesRepresentantesModel { get; set; }

        public bool bEmpresaCorrente { get; set; }

        public double vMetaCorrente { get; set; }
        public bool? bGravaLocRepresentante { get; set; }

    }
}

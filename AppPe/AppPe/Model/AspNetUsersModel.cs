using System;
using System.Collections.Generic;
using System.Linq;
using SQLite;
using Xamarin.HLP.Mobile.AppPE.Common;

namespace Xamarin.HLP.Mobile.AppPE.Model
{
    /// <summary>
    /// ESSA CLASSE É REF. A CLASSE DE USUARIO/REPRESENTANTE.
    /// </summary>
    [Table(TableMobile.AspNetUsers)]
    public class AspNetUsersModel
    {
     
        [PrimaryKey]
        public string Id { get; set; }
        public string Email { get; set; }
        public bool EmailConfirmed { get; set; }
        public string PasswordHash { get; set; }
        public string SecurityStamp { get; set; }
        public string PhoneNumber { get; set; }
        public bool PhoneNumberConfirmed { get; set; }
        public bool TwoFactorEnabled { get; set; }
        public DateTime? LockoutEndDateUtc { get; set; }
        public bool LockoutEnabled { get; set; }
        public int AccessFailedCount { get; set; }
        public string UserName { get; set; }
        /// <summary>
        /// indica se o usuário tem sua própria empresa cadastrada
        /// </summary>
        public bool hasEmpresa { get; set; }

        [Ignore]
        public EmpresaModel EmpresaModelTeste { get; set; }

        [Ignore]
        public EmpresaAspnetUsersModel objEmpresaAspnetUsersModel
        {
            get
            {
                var obj = (lEpresaAspnetUsersModel.FirstOrDefault(c => c.isAtiva) ??
                           lEpresaAspnetUsersModel.FirstOrDefault(c => c.stAtivo)) ??
                           lEpresaAspnetUsersModel.FirstOrDefault();
                return obj;
            }
        }

        private List<EmpresaAspnetUsersModel> _lEpresaAspnetUsersModel = new List<EmpresaAspnetUsersModel>();
        /// <summary>
        /// Lista de todas as empresas que o usuário tem acesso.
        /// </summary>
        [Ignore]
        public List<EmpresaAspnetUsersModel> lEpresaAspnetUsersModel
        {
            get { return _lEpresaAspnetUsersModel; }
            set { _lEpresaAspnetUsersModel = value; }
        }
        private List<PermissoesRepresentantesModel> _lPermissoesRepresentantesModel = new List<PermissoesRepresentantesModel>();
        /// <summary>
        /// Lista de todas permissoes dos representantes.
        /// </summary>
        [Ignore]
        public List<PermissoesRepresentantesModel> lPermissoesRepresentantesModel
        {
            get { return _lPermissoesRepresentantesModel; }
            set { _lPermissoesRepresentantesModel = value; }
        }
    }


    public class LoginMobile
    {
        public SignInStatus status { get; set; }
        public string xMessage { get; set; }
        public AspNetUsersModel objModel { get; set; }
    }

    public enum SignInStatus
    {
        // Summary:
        //     Sign in was successful
        Success = 0,
        //
        // Summary:
        //     User is locked out
        LockedOut = 1,
        //
        // Summary:
        //     Sign in requires addition verification (i.e. two factor)
        RequiresVerification = 2,
        //
        // Summary:
        //     Sign in failed
        Failure = 3,
    }
}

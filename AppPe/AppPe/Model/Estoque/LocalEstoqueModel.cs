using SQLite;
using System;
using System.Collections.ObjectModel;
using Xamarin.HLP.Mobile.AppPE.Common;

namespace Xamarin.HLP.Mobile.AppPE.Model.Estoque
{
    [Table(TableMobile.TB_LOCAL_ESTOQUE)]
    public class LocalEstoqueModel : ModelComum
    {
        [PrimaryKey(), AutoIncrement()]
        public int? idLocalEstoqueOffline { get; set; }

        public int idLocalEstoque { get; set; }
        /// <summary>
        /// data de alteração ( usado mais pro mobile )
        /// </summary>
        public DateTime? dtUltimaAlteracao { get; set; }

        public string xNomeLocal { get; set; }


        public string xCodigoLocal { get; set; }

        public string xMeuID { get; set; }
        /// <summary>
        /// campo definindo se está excluído
        /// </summary>
        public bool bExcluido { get; set; }

        /// <summary>
        /// tipo de estoque a ser usado
        /// 0 - Estoque próprio
        /// 1 - Estoque próprio em poder de terceiros
        /// 2 - Estoque de terceiros em poder da empresa
        /// </summary>
        public byte stTipoEstoque { get; set; }


        private ObservableCollection<LocalEstoqueClientesModel> _lClientesAtrelados = new ObservableCollection<LocalEstoqueClientesModel>(); 
        [Ignore]
        public ObservableCollection<LocalEstoqueClientesModel> lClientesAtrelados
        {
            get
            {
                return _lClientesAtrelados;
            }
            set
            {
                _lClientesAtrelados = value;
                NotifyPropertyChanged();
            }
        } 



        private ObservableCollection<LocalEstoqueUfModel> _lUfAtrelados = new ObservableCollection<LocalEstoqueUfModel>();
        [Ignore]
        public ObservableCollection<LocalEstoqueUfModel> lUfAtrelados
        {
            get
            {
                return _lUfAtrelados;
            }
            set
            {
                _lUfAtrelados = value;
                NotifyPropertyChanged();
            }
        }



        private ObservableCollection<LocalEstoqueRepresentantesModel> _lRepresentantesAtrelados = new ObservableCollection<LocalEstoqueRepresentantesModel>(); 
        [Ignore]
        public ObservableCollection<LocalEstoqueRepresentantesModel> lRepresentantesAtrelados
        {
            get
            {
                return _lRepresentantesAtrelados;
            }
            set
            {
                _lRepresentantesAtrelados = value;
                NotifyPropertyChanged();
            }
        }

        private ObservableCollection<LocalEstoqueClienteRamoAtividadesDataModel> _lRamoAtividades = new ObservableCollection<LocalEstoqueClienteRamoAtividadesDataModel>();
        [Ignore]
        public ObservableCollection<LocalEstoqueClienteRamoAtividadesDataModel> lRamoAtividades
        {
            get
            {
                return _lRamoAtividades;
            }
            set
            {
                _lRamoAtividades = value;
                NotifyPropertyChanged();
            }
        }

        public int idEmpresa { get; set; }
    }


    [Table(TableMobile.TB_LOCAL_ESTOQUE_CLIENTES)]
    public class LocalEstoqueClientesModel : ModelComum
    {
        [PrimaryKey, AutoIncrement]
        public int idLocalEstoqueClienteOffline { get; set; }
        public int idLocalEstoqueCliente { get; set; }
        public DateTime? dtUltimaAlteracao { get; set; }

        [NotNull()]
        public int idClientes { get; set; }

        [NotNull()]
        public int idLocalEstoque { get; set; }
    }


    [Table(TableMobile.TB_LOCAL_ESTOQUE_REPRESENTANTES)]
    public class LocalEstoqueRepresentantesModel : ModelComum
    {
        [PrimaryKey, AutoIncrement] 
        public int idLocalEstoqueRepresentanteOffline { get; set; }

        public int idLocalEstoqueRepresentante { get; set; }

        /// <summary>
        /// data de alteração ( usado mais pro mobile )
        /// </summary>
        public DateTime? dtUltimaAlteracao { get; set; }

        [NotNull()]
        public int idEmpresa_aspnetUsers { get; set; }

        [NotNull()]
        public int idLocalEstoque { get; set; }
    }



    [Table(TableMobile.TB_LOCAL_ESTOQUE_UF)]
    public class LocalEstoqueUfModel : ModelComum
    {
        [PrimaryKey, AutoIncrement]
        public int idLocalEstoqueUfOffline { get; set; }

        public int idLocalEstoqueUf { get; set; }

        /// <summary>
        /// data de alteração ( usado mais pro mobile )
        /// </summary>
        public DateTime? dtUltimaAlteracao { get; set; } 

        public string xUf { get; set; }

        [NotNull()]
        public int idLocalEstoque { get; set; }
    }


    [Table(TableMobile.TB_LOCAL_ESTOQUE_RAMOSATIVIDADES)]
    public class LocalEstoqueClienteRamoAtividadesDataModel
    {
        public int idLocalEstoqueRamoAtividade { get; set; }

        /// <summary>
        /// data de alteração ( usado mais pro mobile )
        /// </summary>
        public DateTime? dtUltimaAlteracao { get; set; }

        public int idRamoAtividade { get; set; }

        public int idLocalEstoque { get; set; }
    }



    public class LocalEstoqueSimplificado
    {
 
        public int idLocalEstoque { get; set; } 

        public string xNomeLocal { get; set; } 

        public int idEmpresa { get; set; }
    }

}

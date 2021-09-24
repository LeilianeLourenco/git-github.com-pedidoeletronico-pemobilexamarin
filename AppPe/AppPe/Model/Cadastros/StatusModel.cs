using System.Runtime.Serialization;
using SQLite;
using Xamarin.HLP.Mobile.AppPE.Common;

namespace Xamarin.HLP.Mobile.AppPE.Model.Cadastros
{
    [Table(TableMobile.TB_STATUS)]
    public class StatusModel
    {
        [PrimaryKey]
        public int idStatus { get; set; }


        /// <summary>
        /// 0 - Aberto
        /// 1 - Cancelado
        /// 2 - Vendido
        /// </summary>
        public int stVenda { get; set; }

        [Ignore]
        [IgnoreDataMember]
        public string xStatusBase
        {
            get
            {
                switch (stVenda)
                {
                    case 2:
                        return "Vendido";
                    case 0:
                        return "Aberto";
                    case 1:
                        return "Cancelado";
                    default:
                        return "Vendido";
                }
            }
        }
        private string _xNome;

        public string xNome
        {
            get { return _xNome; }
            set { _xNome = value;
                xDisplaySemCaracter = value.RemoverAcentos();
            }
        }



        public string xDisplaySemCaracter { get; set; }
        public int idEmpresa { get; set; }
        public bool bStatusDefault { get; set; }

        /// <summary>
        /// 0-ORÇAMENTO
        /// 1-PEDIDO
        /// 2-AMBOS
        /// </summary>
        public int stAparecerStatus { get; set; }

        public bool? stAtivo { get; set; }

        public string xSigla { get; set; }

        public string xCor { get; set; } = "2B3D8C";
        
    }
}

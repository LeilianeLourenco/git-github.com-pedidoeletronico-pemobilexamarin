using SQLite;
using System;
using System.Collections.Generic;
using System.Text;
using Xamarin.HLP.Mobile.AppPE.Common;

namespace Xamarin.HLP.Mobile.AppPE.Model
{
    [Table(TableMobile.TB_PERMISSOES_REPRESENTANTES)]
    public class PermissoesRepresentantesModel : ModelComum
    {
        [PrimaryKey()]
        public int? idPermissaoUsuario { get; set; }
        public int idEmpresa { get; set; }
        public int idEmpresa_aspnetusers { get; set; }
        public byte stPermissaoClientes { get; set; }

        private byte? _stPermissaoPedidoVenda;

        public byte? stPermissaoPedidoVenda
        {
            get
            {
                // Sem linha gravada em tb_permissoes_representantes = Restrito (só os próprios).
                // Admin é tratado em PedidoRepository.GetInfinit com bypass explícito (não depende
                // deste default). A web defaulta `?? 3` (Geral), mas no mobile a regra de negócio
                // é "não-admin sem permissão registrada vê só os próprios".
                return _stPermissaoPedidoVenda ?? 1;
            }
            set
            {
                _stPermissaoPedidoVenda = value;
            }
        }

        public byte stPermissaoRelatorios { get; set; }
        public byte stPermissaoFinanceiro { get; set; }
        public DateTime? dtUltimaAlteracao { get; set; }

        private DateTime _ultimaSyncDateTime = DateTime.Today.AddYears(-50);
        public DateTime UltimaSyncDateTime
        {
            get { return _ultimaSyncDateTime; }
            set { _ultimaSyncDateTime = value; base.NotifyPropertyChanged(); }
        }
    }
}

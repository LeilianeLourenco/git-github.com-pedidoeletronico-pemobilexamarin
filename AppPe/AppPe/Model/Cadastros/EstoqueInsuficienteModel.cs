using SQLite;

namespace Xamarin.HLP.Mobile.AppPE.Model.Cadastros
{
    [Table("TB_ESTOQUE_INSUFICIENTE")]
    public class EstoqueInsuficienteModel
    {
        [PrimaryKey, AutoIncrement]
        public int idEstoqueInsuficiente { get; set; }
        public int idPedidoVendaOffLine { get; set; }
        public int idProduto { get; set; }
        public int idEmpresa { get; set; }
        public int? idGradeCor { get; set; }
        public int? idGradeTamanho { get; set; }
        public double dEstoqueAtual { get; set; }

    }
}

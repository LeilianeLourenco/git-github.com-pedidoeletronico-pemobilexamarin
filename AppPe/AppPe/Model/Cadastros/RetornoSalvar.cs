namespace Xamarin.HLP.Mobile.AppPE.Model.Cadastros
{
    /// <summary>
    /// Estrutura que será utilizada para retornar status de método salvar de repositórios.
    /// </summary>
    public struct RetornoRepositoriosSalvar
    {
        public RetornoSalvar stRetorno { get; set; }
        public string erroIntegracao { get; set; }
        public object retorno { get; set; }
        public string msgErroApi { get; set; }
    }

    public enum StatusCrud
    {
        insert,
        update,
        delete,
        none
    }

    /// <summary>
    /// Enum que será utilizado para retorno de método salvar de repositórios
    /// </summary>
    public enum RetornoSalvar
    {
        Sucesso = 0,
        EstoqueInsuficiente = 1,
        DescontoInvalido = 2,
        Excecao = 3,
        SucessoMasAtencao = 4,
        StatusBaseSendoUsado = 5,
        NaoPodeMudarTabela = 6,
        CondicaoVazia = 7,
        PedidoJaSincronizado = 8,
    }

    public class RetornoSalvar<T> where T : class
    {
        public RetornoRepositoriosSalvar resulStruct { get; set; }

        public T objModel { get; set; }

    }
}

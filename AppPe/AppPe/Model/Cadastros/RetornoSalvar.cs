namespace Xamarin.HLP.Mobile.AppPE.Model.Cadastros
{
    /// <summary>
    /// Estrutura que será utilizada para retornar status de método salvar de repositórios.
    /// </summary>
    public struct RetornoRepositoriosSalvar
    {
        public RetornoSalvar stRetorno { get; set; }
        public object retorno { get; set; }
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
        Sucesso,
        EstoqueInsuficiente,
        DescontoInvalido,
        Excecao,
        SucessoMasAtencao,
        StatusBaseSendoUsado,
    }

    public class RetornoSalvar<T> where T : class
    {
        public RetornoRepositoriosSalvar resulStruct { get; set; }

        public T objModel { get; set; }

    }
}

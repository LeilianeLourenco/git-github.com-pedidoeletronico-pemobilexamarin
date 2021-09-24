using System;
using System.Diagnostics;
using Xamarin.Forms;

namespace Xamarin.HLP.Mobile.AppPE.Model
{
    public class GoogleInsightsReportingConstants
    {

        public enum InPage
        {
            PAGE_LOGIN,
            PAGE_REGISTER,
            PAGE_HOME,
            PAGE_SINCRONIZACAO,
            PAGE_LISTAR_PEDIDO,
            PAGE_LISTAR_CLIENTES,
            PAGE_LISTAR_PRODUTOS,
            PAGE_LISTA_PRECO,
            PAGE_INICIO,
            POPUP_COMPLEMENTO_PEDIDO,
            PAGE_APRESENTACAO_PRODUTO,
            PAGE_APRESENTACAO_CLIENTE,
            PAGE_APRESENTACAO_PEDIDO,
            PAGE_PEDIDO,
            PAGE_LISTAR_PRODUTO_PEDIDO,
            PAGE_EDITAR_PRODUTO,
            PAGE_CLIENTE,
            PAGE_PRODUTO,
            PAGE_ENDERECO,
            PAGE_CONTATO,
            PAGE_TROCAR_EMPRESA
        }

        public static void TrakException(string display, string message, bool isFatalError = false)
        {

#if DEBUG
            //App.Messages.ShowAsync($"{display}{Environment.NewLine}{message}");
            //Debug.WriteLine();
            Debug.WriteLine(@"ERROR HLP_MOBILE {0},{1}- {2}", display, ((object)message ?? ""), "");
#else
            if (Device.OS != TargetPlatform.Windows)
                GASConnect.Track_App_Exception($"{display}{Environment.NewLine}{message}", isFatalError);
#endif


        }

        public static void TrakPage(InPage page)
        {
            if (Device.OS != TargetPlatform.Windows)
            {
                switch (page)
                {
                    case InPage.PAGE_LOGIN:
                        GASConnect.Track_App_Page(PAGE_LOGIN);
                        break;
                    case InPage.PAGE_HOME:
                        GASConnect.Track_App_Page(PAGE_HOME);
                        break;
                    case InPage.PAGE_SINCRONIZACAO:
                        GASConnect.Track_App_Page(PAGE_SINCRONIZACAO);
                        break;
                    case InPage.PAGE_LISTAR_PEDIDO:
                        GASConnect.Track_App_Page(PAGE_LISTAR_PEDIDO);
                        break;
                    case InPage.PAGE_LISTAR_CLIENTES:
                        GASConnect.Track_App_Page(PAGE_LISTAR_CLIENTES);
                        break;
                    case InPage.PAGE_LISTAR_PRODUTOS:
                        GASConnect.Track_App_Page(PAGE_LISTAR_PRODUTOS);
                        break;
                    case InPage.PAGE_APRESENTACAO_PRODUTO:
                        GASConnect.Track_App_Page(PAGE_APRESENTACAO_PRODUTO);
                        break;
                    case InPage.PAGE_APRESENTACAO_CLIENTE:
                        GASConnect.Track_App_Page(PAGE_APRESENTACAO_CLIENTE);
                        break;
                    case InPage.PAGE_APRESENTACAO_PEDIDO:
                        GASConnect.Track_App_Page(PAGE_APRESENTACAO_PEDIDO);
                        break;
                    case InPage.PAGE_PEDIDO:
                        GASConnect.Track_App_Page(PAGE_PEDIDO);
                        break;
                    case InPage.PAGE_LISTAR_PRODUTO_PEDIDO:
                        GASConnect.Track_App_Page(PAGE_LISTAR_PRODUTO_PEDIDO);
                        break;
                    case InPage.PAGE_EDITAR_PRODUTO:
                        GASConnect.Track_App_Page(PAGE_EDITAR_PRODUTO);
                        break;
                    case InPage.PAGE_CLIENTE:
                        GASConnect.Track_App_Page(PAGE_CLIENTE);
                        break;
                    case InPage.PAGE_PRODUTO:
                        GASConnect.Track_App_Page(PAGE_PRODUTO);
                        break;
                    case InPage.PAGE_ENDERECO:
                        GASConnect.Track_App_Page(PAGE_ENDERECO);
                        break;
                    case InPage.PAGE_CONTATO:
                        GASConnect.Track_App_Page(PAGE_CONTATO);
                        break;
                    case InPage.PAGE_TROCAR_EMPRESA:
                        GASConnect.Track_App_Page(PAGE_TROCAR_EMPRESA);
                        break;
                }
            }
        }

        private const string PAGE_LOGIN = "PAGE DE LOGIN";
        private const string PAGE_HOME = "PAGE INICIAL - (HOME)";
        private const string PAGE_SINCRONIZACAO = "PAGE DE SINCRONIZAÇÃO";
        private const string PAGE_LISTAR_PEDIDO = "PAGE LISTAGEM DE PEDIDO";
        private const string PAGE_LISTAR_CLIENTES = "PAGE LISTAGEM DE CLIENTES";
        private const string PAGE_LISTAR_PRODUTOS = "PAGE LISTAGEM DE PRODUTOS";
        private const string PAGE_APRESENTACAO_PRODUTO = "PAGE APRESENTAÇÃO DO PRODUTO";
        private const string PAGE_APRESENTACAO_CLIENTE = "PAGE APRESENTAÇÃO DO CLIENTE";
        private const string PAGE_APRESENTACAO_PEDIDO = "PAGE APRESENTAÇÃO DO PEDIDO";
        private const string PAGE_PEDIDO = "PAGE LANÇAMENTO PEDIDO/ORÇAMENTO";
        private const string PAGE_LISTAR_PRODUTO_PEDIDO = "PAGE LISTAGEM DE PRODUTO PARA O PEDIDO";
        private const string PAGE_EDITAR_PRODUTO = "PAGE CADASTRO/EDIÇÃO DO PRODUTO";
        private const string PAGE_CLIENTE = "PAGE CADASTRO/EDIÇÃO DE CLIENTE";
        private const string PAGE_PRODUTO = "PAGE CADASTRO/EDIÇÃO DE PRODUTO";
        private const string PAGE_ENDERECO = "PAGE CADASTRO/EDIÇÃO DE ENDEREÇO";
        private const string PAGE_CONTATO = "PAGE CADASTRO/EDIÇÃO DE ENDEREÇO";
        private const string PAGE_TROCAR_EMPRESA = "PAGE TROCAR DE EMPRESA";
    }
}

using System;
using System.Globalization;
using System.Reflection;
using System.Text.RegularExpressions;
using System.Windows.Input;
using Xamarin.Forms;
using Xamarin.HLP.Mobile.AppPE.Model.Lancamento;

namespace Xamarin.HLP.Mobile.AppPE
{
    public static class Extensions
    {

        public static string ValidaMaxLength(this string valor, int maxlength)
        {
            if (valor.Length > maxlength)
            {
                return valor.Substring(0, maxlength - 1);
            }
            return valor;
        }

        public static string PathToNameImage(this string valor)
        {

            var lPath = (valor ?? "").Split('/');

            var retorno = lPath[lPath.Length - 1];

            return retorno.ToUpper().Replace(".PNG", "").Replace(".JPG", "").Replace(".BITMAP", "").Replace(".JPEG", "").Replace(".GIF", "");
        }


        public static string PathToNameImageWithExtension(this string valor)
        {

            var lPath = (valor ?? "").Split('/');

            var retorno = lPath[lPath.Length - 1];

            return retorno.ToUpper();
        }

        public static DateTime SqlMinDateTime(this DateTime date)
        {
            return new DateTime(1753, 1, 1, 0, 0, 0, DateTimeKind.Local);
        }

        public static double Clamp(this double self, double min, double max)
        {
            return Math.Min(max, Math.Max(self, min));
        }

        public static decimal? ObjectToDecimalNullable(this object obj)
        {
            if (obj == null)
            {
                return null;
            }
            else
            {
                decimal vParsed = 0;

                decimal.TryParse(s: obj.ToString(), result: out vParsed);

                return vParsed;
            }
        }

        /// <summary>
        /// RS 1.000,00
        /// </summary>
        /// <param name="value"></param>
        /// <returns></returns>
        public static string ToCurrencyStringPtBr(this object value)
        {
            var culturaBrasileira = new CultureInfo("pt-BR");
            double dValor;
            double.TryParse((value ?? "").ToString(), out dValor);
            return dValor.ToString("C", culturaBrasileira);
        }

        /// <summary>
        /// 1.000,00
        /// </summary>
        /// <param name="value"></param>
        /// <returns></returns>
        public static string ToCurrencyStringSimplesPtBr(this object value, CultureInfo cultrure = null)
        {
            var culturaBrasileira = new CultureInfo("pt-BR");
            if (cultrure != null)
            {
                culturaBrasileira = cultrure;
            }
            double dValor;
            double.TryParse((value ?? "").ToString(), out dValor);
            return dValor.ToString("N2", culturaBrasileira);
        }

        public static string ToCurrencyStringSimplesPlacesPtBr(this object value, CultureInfo cultrure = null)
        {
            var culturaBrasileira = new CultureInfo("pt-BR");
            if (cultrure != null)
            {
                culturaBrasileira = cultrure;
            }
            double dValor;
            double.TryParse((value ?? "").ToString(), out dValor);
            return dValor.ToString("N4", culturaBrasileira);
        }

        /// <summary>
        /// 1000,00
        /// </summary>
        /// <param name="value"></param>
        /// <returns></returns>
        public static double ToDoublePtBr(this string value, CultureInfo cultrure = null)
        {
            try
            {
                var culturaBrasileira = new CultureInfo("pt-BR");
                if (cultrure != null)
                {
                    culturaBrasileira = cultrure;
                }
                var valorDecimal = double.Parse(value ?? "", NumberStyles.Currency, culturaBrasileira);
                return valorDecimal;
            }
            catch (Exception)
            {
                return "0".ToDoublePtBr();
            }

        }

        public static decimal ToDecimalPtBr(this double? value)
        {
            return Convert.ToDecimal(value ?? 0);
        }

        public static string ToPercentage()
        {
            return Convert.ToString(" %");
        }

        public static decimal ToDecimalPtBr(this double value)
        {
            return Math.Round(Convert.ToDecimal(value), 2);
        }

        public static int TryToInt(this object valor)
        {
            if (valor == null) return 0;

            int result;

            int.TryParse(valor.ToString(), out result);

            return result;
        }

        public static Int64 TryToInt64(this object valor)
        {
            if (valor == null) return 0;

            Int64 result;

            Int64.TryParse(valor.ToString(), out result);

            return result;
        }

        private static string FormatarValorTelefone(string valor)
        {
            var ddd = valor.Substring(0, 2);
            ddd = string.Format("({0}) ", ddd);

            var number = valor.Substring(2, valor.Length - 2);
            return ddd + string.Format("{0:####-####}", number.TryToInt64());
        }

        private static string FormatarValorCelular(string valor)
        {
            var ddd = valor.Substring(0, 2);
            ddd = string.Format("({0}) ", ddd);

            var number = valor.Substring(2, valor.Length - 2);
            return ddd + string.Format("{0:#####-####}", number.TryToInt64());
        }

        public static string ToDisplayMonth(this object value)
        {
            var date = new DateTime(DateTime.Today.Year, Convert.ToInt32(value), 1);
            return $"{date.ToString("MMMM")}/{date.ToString("yyyy")}";
        }

        public static string ToDisplayMonthNew(this object value)
        {
            var month = value.ToString().Split('/')[0];
            var year = value.ToString().Split('/')[1];
            var date = new DateTime(Convert.ToInt32(year), Convert.ToInt32(month), 1);
            return $"{date.ToString("MMMM")}/{date.ToString("yyyy")}";
        }

        public static string ToCNPJFormat(this object value)
        {
            //   37.610.411/0001-09
            var valor = System.Convert.ToString((value ?? "")).RetiraCaracterEspecial();

            if (valor.Length > 14)
            {
                valor = valor.Substring(0, 14);
            }

            var iCount = 1;
            var resultado = "";

            if (valor.Length == 14)
            {
                foreach (var numero in valor.ToCharArray(0, valor.Length))
                {
                    resultado += numero;
                    if (iCount == 2 || iCount == 5)
                        resultado += ".";

                    if (iCount == 8)
                        resultado += "/";

                    if (iCount == 12)
                        resultado += "-";

                    iCount++;
                }
            }
            else
            {
                resultado = valor;
            }
            return resultado;
        }


        public static bool IsDigitsOnly(string str)
        {
            foreach (char c in str)
            {
                if (c < '0' || c > '9')
                    return false;
            }

            return true;
        }

        public static string ToCpfFormat(this object value)
        {
            var valor = System.Convert.ToString((value ?? "")).RetiraCaracterEspecial();

            if (valor.Length > 11)
            {
                valor = valor.Substring(0, 11);
            }

            var iCount = 1;
            var resultado = "";

            if (valor.Length == 11)
            {
                foreach (var numero in valor.ToCharArray(0, valor.Length))
                {
                    resultado += numero;
                    if (iCount == 3 || iCount == 6)
                        resultado += ".";

                    if (iCount == 9)
                        resultado += "-";

                    iCount++;
                }
            }
            else
            {
                resultado = valor;
            }
            return resultado;
        }

        public static string RetiraCaracterEspecial(this object value)
        {
            return
                System.Convert.ToString((value ?? ""))
                    .Replace("(", "")
                    .Replace(",", "")
                    .Replace(")", "")
                    .Replace("-", "")
                    .Replace(" ", "")
                    .Replace("-", "")
                    .Replace("/", "")
                    .Replace(".", "")
                    .Replace("*", "");
        }

        public static string RemoverAcentos(this string texto)
        {
            const string comAcentos = "ÄÅÁÂÀÃäáâàãÉÊËÈéêëèÍÎÏÌíîïìÖÓÔÒÕöóôòõÜÚÛüúûùÇç";
            const string semAcentos = "AAAAAAaaaaaEEEEeeeeIIIIiiiiOOOOOoooooUUUuuuuCc";

            for (var i = 0; i < comAcentos.Length; i++)
            {
                texto = texto.Replace(comAcentos[i].ToString(), semAcentos[i].ToString());
            }

            //texto = texto
            //        .Replace("(", "")
            //        .Replace(",", "")
            //        .Replace(")", "")
            //        .Replace("-", "")
            //        .Replace("  ", " ")
            //        .Replace("-", "")
            //        .Replace("/", "")
            //        .Replace(".", "")
            //        .Replace("*", "");

            return texto;
        }



        public static string ToPhoneFormat(this object value)
        {
            var valor = System.Convert.ToString((value ?? "")).RetiraCaracterEspecial();

            if (valor.Length > 0)
            {

                var valorInt = valor.TryToInt64().ToString();
                valor = valorInt;

                if (valor.Length > 11)
                {
                    valorInt = valor = valor.Substring(0, 11);
                }

                var resultado = "";
                if (valorInt.Length == 8 && valor.Length == 8)
                {
                    resultado = string.Format("{0:####-####}", valor.TryToInt64());
                }
                else if (valorInt.Length == 10 && valor.Length == 11)
                {
                    resultado = FormatarValorTelefone(valor);
                }
                else if (valorInt.Length == 11 && valor.Length == 12)
                {
                    resultado = FormatarValorCelular(valor);
                }
                else if (valorInt.Length == 10 && valor.Length == 10)
                {
                    //valor = "0" + valor;
                    //resultado = FormatarValorTelefone(valor);
                    resultado = FormatarValorCelular(valor);
                }
                else if (valorInt.Length == 11 && valor.Length == 11)
                {
                    //valor = "0" + valor;
                    resultado = FormatarValorCelular(valor);
                }

                return resultado != "" ? resultado : valor;
            }

            return "";
        }

        public static string ToPathSvg(this string svg)
        {
            if (svg != null)
                return "Xamarin.HLP.Mobile.AppPE.Images." + svg + (svg.Contains(".svg") ? "" : ".svg");
            return null;
        }

        public static string ToPathSvgMenu(this string svg)
        {
            if (svg != null)
                return "Xamarin.HLP.Mobile.AppPE.Images.MenuIcon." + svg + (svg.Contains(".svg") ? "" : ".svg");
            return null;
        }

        public static string ToPathSvgPage(this string svg)
        {
            if (svg != null)
                return "Xamarin.HLP.Mobile.AppPE.Images.PagesIcon." + svg + (svg.Contains(".svg") ? "" : ".svg");
            return null;
        }

        public static bool IsNumber(this string s)
        {
            const string verifica = "^[0-9]";
            return Regex.IsMatch(s, verifica);
        }

        public static DateTime ToDateTimeSync(this DateTime value)
        {
            return new DateTime(value.Year, value.Month, value.Day, value.Hour, value.Minute, value.Second,
                DateTimeKind.Utc);
        }

        public static DateTime ToDateTimeSync(this DateTime? value)
        {
            var _value = value ?? DateTime.Now;
            return new DateTime(_value.Year, _value.Month, _value.Day, _value.Hour, _value.Minute, _value.Second,
                DateTimeKind.Utc);
        }

        public static ImageSource ToImagemPNG(this object img)
        {
            if (img != null)
                return
                    ImageSource.FromResource("Xamarin.HLP.Mobile.AppPE.Images.ImagesPNG." + img +
                                             (img.ToString().Contains(".png") ? "" : ".png"));
            return null;
        }

        public static ImageSource ToImagemJPG(this object img)
        {
            ImageSource source;

            if (img != null)
            {
                source = ImageSource.FromResource("Xamarin.HLP.Mobile.AppPE.Images.ImagesPNG." + img +
                                             (img.ToString().Contains(".jpg") ? "" : ".jpg"));

                return source;
            }

            return null;
        }

        /// <summary>
        /// Metodo para carregar imagem
        /// Passe o caminho completo a partir da pasta Images ex: 'ImagesPNG.Logo.png'
        /// </summary>
        /// <param name="img"></param>
        /// <returns>Retorno será a imagem, Passe o caminho completo a partir da pasta Images ex: 'ImagesPNG.Logo.png'</returns>
        public static ImageSource ToImagem(this object img)
        {
            if (img != null)
                return
                    ImageSource.FromResource("Xamarin.HLP.Mobile.AppPE.Images." + img +
                                             (img.ToString().Contains(".png") ? "" : ".png"));
            return null;
        }

        public static double ArredondarValorDecimal(this double valor, int? nCasasDecimais = null)
        {
            return ArredondarValorDecimalInternal(valor: valor, nCasasDecimais: nCasasDecimais);
        }

        internal static double ArredondarValorDecimalInternal(this double valor, int? nCasasDecimais = null)
        {
            return Math.Round(valor, nCasasDecimais ?? NCasasDecimaisDefautlMoeda, MidpointRounding.ToEven);
        }

        public const int NCasasDecimaisDefautlMoeda = 2;

        public static void TrakException(this Exception ex, string detail = null, bool bShowMessage = true)
        {

            if (bShowMessage)
                App.Messages.ShowAsync($"Exception: {detail ?? ""} - EXCEPTION: {ex.Message} | {ex.StackTrace}");

            //GoogleInsightsReportingConstants.TrakException($"Exception: {detail ?? ""}",
            //    $"EXCEPTION: {ex.Message} | {ex.StackTrace}");
        }

        public static bool IsValidEmailAddress(this string s)
        {
            if (!string.IsNullOrEmpty(s))
            {
                var regex = new Regex(@"^[\w-\.]+@([\w-]+\.)+[\w-]{2,4}$");
                return regex.IsMatch(s);
            }
            else
            {
                return false;
            }
        }

        public static object GetPropValue(this object src, string propName)
        {
            return src.GetType().GetRuntimeProperty(propName).GetValue(src, null);
        }

        public static int ToInt(this DateTime value)
        {
            return Convert.ToInt32(value.ToString("yyyyMMdd"));
        }
        public static int ToIntFull(this DateTime value)
        {
            return Convert.ToInt32(value.ToString("yyyyMMddHH"));
        }

        public static void SetPropValue(this object src, string propName, object value)
        {
            src.GetType().GetRuntimeProperty(propName).SetValue(src, value);
        }

        public static PedidoVendaItensModel CloneItem(this PedidoVendaItensModel produto)
        {
            var item = new PedidoVendaItensModel
            {
                idProduto = produto.idProduto,
                idProdutoOffLine = produto.idProdutoOffLine,
                idEmpresa = App.CurrentAspnetUserModel.objEmpresaAspnetUsersModel.idEmpresa,
                idTabelaPreco = produto.idTabelaPreco,
                lTabelaPreco = produto.lTabelaPreco,
                currentTabelaPreco = produto.currentTabelaPreco,
                idRepresentada = produto.idRepresentada,
                nCasasDecimais = produto.nCasasDecimais,
                vUnitarioVenda = produto.vUnitarioVenda,
                vVenda = produto.vVenda,
                vVendaDef = produto.vVendaDef,
                stComissao = produto.stComissao,
                pComissaoOriginal = produto.pComissaoOriginal,
                cAlternativo = produto.cAlternativo,
                vCusto = produto.vCusto,
                stDescontaIpiComissao = produto.stDescontaIpiComissao,
                stDescontaStComissao = produto.stDescontaStComissao,
                pIpiVenda = produto.pIpiVenda,
                pStVenda = produto.pStVenda,
                idClientesOffLine = produto.idClientesOffLine,
                vDesconto = produto.vDesconto,
                vComissao = produto.vComissao,
                pComissao = produto.pComissao,
                pDesconto = produto.pDesconto,
                idPedidoVenda = produto.idPedidoVenda,
                xFileImagePrincipal = produto.xFileImagePrincipal,
                QtdeGrade = produto.QtdeGrade,
                ItensGrade = produto.ItensGrade,
                bPrecoAtualizado = produto.bPrecoAtualizado,
                dEstoque = produto.dEstoque,
                idGradeCor = produto.idGradeCor,
                idGradeTamanho = produto.idGradeTamanho,

            };
            return item;
        }

        public static void ChangeCanExecute(this ICommand command)
        {
            ((Command)command)?.ChangeCanExecute();
        }


        public static bool IsValidCpf(this string cpf)
        {
            cpf = cpf ?? "";

            var multiplicador1 = new int[9] { 10, 9, 8, 7, 6, 5, 4, 3, 2 };
            var multiplicador2 = new int[10] { 11, 10, 9, 8, 7, 6, 5, 4, 3, 2 };
            string tempCpf;
            string digito;
            int soma;
            int resto;
            cpf = cpf.Trim();
            cpf = cpf.Replace(".", "").Replace("-", "");
            if (cpf.Length != 11)
                return false;
            tempCpf = cpf.Substring(0, 9);
            soma = 0;

            for (var i = 0; i < 9; i++)
                soma += int.Parse(tempCpf[i].ToString()) * multiplicador1[i];
            resto = soma % 11;
            if (resto < 2)
                resto = 0;
            else
                resto = 11 - resto;
            digito = resto.ToString();
            tempCpf = tempCpf + digito;
            soma = 0;
            for (var i = 0; i < 10; i++)
                soma += int.Parse(tempCpf[i].ToString()) * multiplicador2[i];
            resto = soma % 11;
            if (resto < 2)
                resto = 0;
            else
                resto = 11 - resto;
            digito = digito + resto.ToString();
            return cpf.EndsWith(digito);
        }


        public static bool isValidCNPJ(this string cnpj)
        {
            cnpj = cnpj ?? "";
            var multiplicador1 = new int[12] { 5, 4, 3, 2, 9, 8, 7, 6, 5, 4, 3, 2 };
            var multiplicador2 = new int[13] { 6, 5, 4, 3, 2, 9, 8, 7, 6, 5, 4, 3, 2 };
            int soma;
            int resto;
            string digito;
            string tempCnpj;
            cnpj = cnpj.Trim();
            cnpj = cnpj.Replace(".", "").Replace("-", "").Replace("/", "");
            if (cnpj.Length != 14)
                return false;
            tempCnpj = cnpj.Substring(0, 12);
            soma = 0;
            for (var i = 0; i < 12; i++)
                soma += int.Parse(tempCnpj[i].ToString()) * multiplicador1[i];
            resto = (soma % 11);
            if (resto < 2)
                resto = 0;
            else
                resto = 11 - resto;
            digito = resto.ToString();
            tempCnpj = tempCnpj + digito;
            soma = 0;
            for (var i = 0; i < 13; i++)
                soma += int.Parse(tempCnpj[i].ToString()) * multiplicador2[i];
            resto = (soma % 11);
            if (resto < 2)
                resto = 0;
            else
                resto = 11 - resto;
            digito = digito + resto.ToString();
            return cnpj.EndsWith(digito);
        }


        public static string ReplaceToSqlite(this string value)
        {
            return $@"replace(replace(replace(replace(replace(replace(replace(replace(replace(replace(replace( UPPER({value}), 'á','a'), 'ã','a'), 'â','a'), 'é','e'), 'ê','e'), 'í','i'),'ó','o') ,'õ','o') ,'ô','o'),'ú','u'), 'ç','c')";
        }
    }
}

using System;
using System.Collections.ObjectModel;
using System.Threading.Tasks;
using Xamarin.Forms;
using Xamarin.HLP.Mobile.AppPE.Common;
using Xamarin.HLP.Mobile.AppPE.Controls.custom;
using Xamarin.HLP.Mobile.AppPE.Model;
using Xamarin.HLP.Mobile.AppPE.Model.Cadastros;
using Xamarin.HLP.Mobile.AppPE.Model.Repository;
using Xamarin.HLP.Mobile.AppPE.ViewModel.Cadastro;

namespace Xamarin.HLP.Mobile.AppPE.View.Cliente
{
    public partial class PageCliente : TabbedPage
    {
        public static ClienteViewModel StaticViewModel { get; set; }

        public PageCliente(ClientesModel objClientesModel)
        {
            try
            {
                try
                {
                    //StaticModel.StaticClientesModel = objClientesModel;
                    InitializeComponent();
                    NavigationPage.SetHasBackButton(this, false);

                    if (objClientesModel.dEfetivacao == null)
                        objClientesModel.dEfetivacao = DateTime.Now.SqlMinDateTime();

                    StaticViewModel = ViewModel;
                    ViewModel.currentModel = objClientesModel;



                    ToolbarItemSalvar.Command = new Command(AfterSave);
                    EntryNome.Completed += (sender, e) => { EntryFantasia.Focus(); };
                    EntryFantasia.Completed += (sender, e) =>
                    {
                        EntryAlternativo.Focus();
                        SetNameFantasia();
                    };

                    btnExecutePesquisa.Command = new Command(async () =>
                    {
                        if (await App.IsConected())
                        {
                            if (ViewModel.currentModel.idClientesOffLine == null || ViewModel.currentModel.idClientesOffLine == 0)
                                UtilNavidate.ShowPopupNew(new PageBuscaPorCnpj(forcarPesquisa:true));
                            else
                                await App.Messages.ShowAsync("Função habilitada apenas para cadastros novos.");
                        }
                        else
                        {
                            await App.Messages.ShowAsync("É necessário uma conexão com internet");
                        }
                    });
                }
                catch (Exception ex)
                {
                    ex.TrakException("PageCliente.Construtor");
                }
            }
            catch (Exception ex)
            {
                App.Messages.ShowAsync(ex.Message);
            }
        }


        private void SetNameFantasia()
        {
            if (string.IsNullOrEmpty(ViewModel.currentModel.xFantasia))
            {
                var xFant = ViewModel.currentModel.xRazaoSocial;
                if (xFant.Length >= 200)
                {
                    xFant = xFant.Substring(0, 197);
                }
                ViewModel.currentModel.xFantasia = xFant;
            }
        }


        public ClienteViewModel ViewModel => BindingContext as ClienteViewModel;

        private bool _canClose = true;

        protected override bool OnBackButtonPressed()
        {
            if (_canClose)
            {
                QuestionToBack();
            }
            return _canClose;
        }

        private async void QuestionToBack()
        {
            var answer = await App.Messages.ShowConfirmAsync("Deseja realmente sair do lançamento ?");
            if (answer)
            {
                ViewModel.needRestore = true;
                _canClose = false;
                UtilNavidate.PopAsync();
            }
        }



        protected override void OnAppearing()
        {
            base.OnAppearing();
            ViewModel.currentModel.lContato = new ObservableCollection<ContatoModel>(ViewModel.currentModel.lContato);
            ViewModel.currentModel.lEndereco = new ObservableCollection<EnderecoModel>(ViewModel.currentModel.lEndereco);

            if (ViewModel.canExecuteInicial && (ViewModel.currentModel.idClientesOffLine ?? 0) == 0)
                Device.StartTimer(UtilMethods.GetStartTime, AnaliseBuscaCnpj);

            Device.StartTimer(UtilMethods.GetStartTime, ViewModel.Initialize);
            GoogleInsightsReportingConstants.TrakPage(GoogleInsightsReportingConstants.InPage.PAGE_CLIENTE);
        }


        public bool bBuscaInfoCnpj { get; set; } = true;

        public bool AnaliseBuscaCnpj()
        {
            if (bBuscaInfoCnpj)
            {
                bBuscaInfoCnpj = false;
                BuscaInfoByCnpj();
            }
            return bBuscaInfoCnpj;
        }

        private async void BuscaInfoByCnpj()
        {
            if (await App.IsConected())
            {
                ViewModel.currentModel.stJuridico = 1;
                UtilNavidate.ShowPopupNew(new PageBuscaPorCnpj());
            }
        }


        private void EntryCpfCnpj_OnTextChanged(object sender, TextChangedEventArgs e)
        {
            try
            {
                var entry = sender as ExtendedEntry;
                if (entry != null && entry.IsFocused)
                {
                    if (ViewModel.currentModel.stJuridico == 0)
                    {
                        entry.TextColor = !entry.Text.IsValidCpf()
                            ? ColorStaticModel.VermelhoPrincipal
                            : ColorStaticModel.Preto;
                    }
                    else
                    {
                        entry.TextColor = !entry.Text.isValidCNPJ()
                            ? ColorStaticModel.VermelhoPrincipal
                            : ColorStaticModel.Preto;
                    }
                }
            }
            catch (Exception ex)
            {
                App.Messages.ShowAsync(ex.Message);
            }
        }

        private async void AfterSave()
        {

            ViewModel.currentModel.xTelefones = FoneControl.InputField;
            ViewModel.currentModel.xEmails = EmailControl.InputField;

            if (string.IsNullOrEmpty(ViewModel.currentModel.xRazaoSocial))
            {
                await App.Messages.ShowAsync("Nome/Razão social do cliente é obrigatório");
                CurrentPage = PagePrincipal;
                EntryNome.Focus();
                return;
            }

            if (App.tipouser == App.TipoUser.BLING)
            {
                if (string.IsNullOrEmpty(ViewModel.currentModel.xCpfCnpj))
                {
                    await App.Messages.ShowAsync("CNPJ/CPF é um campo obrigatório");
                    return;
                }
            }

            if (!string.IsNullOrEmpty(ViewModel.currentModel.xCpfCnpj))
            {
                if (ViewModel.currentModel.stJuridico == 0)
                {
                    if (ViewModel.currentModel.xCpfCnpj.IsValidCpf() == false)
                    {
                        await App.Messages.ShowAsync("CPF é inválido.");
                        return;
                    }
                }
                else
                {
                    if (ViewModel.currentModel.xCpfCnpj.isValidCNPJ() == false)
                    {
                        await App.Messages.ShowAsync("CNPJ é inválido.");
                        return;
                    }
                }
            }

            if (ClienteRepository.ClienteJaExiste(ViewModel.currentModel.xRazaoSocial,
                ViewModel.currentModel.idClientesOffLine))
            {
                await App.Messages.ShowAsync(
                    $"Cliente {ViewModel.currentModel.xRazaoSocial}, já existe em sua base de dados");
                CurrentPage = PagePrincipal;
                EntryNome.Focus();
                return;
            }

            if (!string.IsNullOrEmpty(ViewModel.currentModel.xCpfCnpj) && ClienteRepository.CpfCnpjClienteJaExiste(ViewModel.currentModel.xCpfCnpj,
                ViewModel.currentModel.idClientesOffLine))
            {
                if (await
                        App.Messages.ShowConfirmAsync(
                            $"{(ViewModel.currentModel.stJuridico == 0 ? "CPF" : "CNPJ")} {ViewModel.currentModel.xCpfCnpj}, já existe em sua base de dados. {Environment.NewLine}Deseja continuar?") ==
                    false)
                {
                    CurrentPage = PagePrincipal;
                    if (ViewModel.currentModel.stJuridico == 0)
                        EntryCpf.Focus();
                    else EntryCnpj.Focus();
                    return;
                }
            }

            if (ClienteRepository.RgIeClienteJaExiste(ViewModel.currentModel.xRgIe,
                ViewModel.currentModel.idClientesOffLine))
            {
                if (await
                        App.Messages.ShowConfirmAsync(
                            $"{(ViewModel.currentModel.stJuridico == 0 ? "RG" : "IE")} {ViewModel.currentModel.xRgIe}, já existe em sua base de dados. {Environment.NewLine}Deseja continuar?") ==
                    false)
                {
                    CurrentPage = PagePrincipal;
                    CurrentPage = PagePrincipal;
                    if (ViewModel.currentModel.stJuridico == 0)
                        EntryRG.Focus();
                    else EntryIE.Focus();
                    return;
                }
            }

            SetNameFantasia();
            if (string.IsNullOrEmpty(ViewModel.currentModel.xFantasia))
            {
                await App.Messages.ShowAsync("Apelido/Fantasia do cliente é obrigatório");
                EntryFantasia.Focus();
                return;
            }


            if (!string.IsNullOrEmpty(ViewModel.currentModel.xCpfCnpj))
            {
                var validCpfCnpj = true;
                if (ViewModel.currentModel.stJuridico == 0) // físico
                    validCpfCnpj = ViewModel.currentModel.xCpfCnpj.IsValidCpf();
                else
                    validCpfCnpj = ViewModel.currentModel.xCpfCnpj.isValidCNPJ();

                if (!validCpfCnpj)
                {
                    CurrentPage = PagePrincipal;
                    await
                        App.Messages.ShowAsync(
                            $"{(ViewModel.currentModel.stJuridico == 0 ? "CPF" : "CNPJ")} é inválido.");
                    if (ViewModel.currentModel.stJuridico == 0)
                        EntryCpf.Focus();
                    else EntryCnpj.Focus();
                    return;
                }

            }

            if (ViewModel.tabelaPreco.Id == 0)
            {
                CurrentPage = PageAdicionais;
                await App.Messages.ShowAsync("Tabela de preço é obrigatório");
                return;
            }
            if (ViewModel.Transportadora.Id == 0)
            {
                CurrentPage = PageAdicionais;
                await App.Messages.ShowAsync("Transportadora é obrigatório");
                return;
            }
            if (ViewModel.CondicaoPgto.Id == 0)
            {
                CurrentPage = PageAdicionais;
                await App.Messages.ShowAsync("Condição de pagamento é obrigatório");
                return;
            }
            if (ViewModel.ramoAtividade.Id == 0)
            {
                CurrentPage = PageAdicionais;
                await App.Messages.ShowAsync("Ramo de atividade é obrigatório");
                return;
            }

            ViewModel.Save();
        }

        private void BindableTipoPessoa_OnSelectedIndexChanged(object sender, EventArgs e)
        {
            try
            {
                var picker = sender as Picker;

                if (picker != null && picker.IsFocused)
                {
                    ViewModel.currentModel.xCpfCnpj = "";
                }

            }
            catch (Exception ex)
            {
                ex.TrakException();
            }
        }
    }


}

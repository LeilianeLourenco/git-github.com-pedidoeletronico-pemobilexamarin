using Plugin.BLE;
using Plugin.BLE.Abstractions.Contracts;
using Plugin.BLE.Abstractions.EventArgs;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Input;
using Xamarin.Forms;
using Xamarin.HLP.Mobile.AppPE.Common;
using Xamarin.HLP.Mobile.AppPE.Core.PedidoVenda.Implementacoes;
using Xamarin.HLP.Mobile.AppPE.Core.PedidoVenda.Interfaces;
using Xamarin.HLP.Mobile.AppPE.Model.Cadastros;
using Xamarin.HLP.Mobile.AppPE.Model.Lancamento;
using Xamarin.HLP.Mobile.AppPE.Model.Repository;
using Xamarin.HLP.Mobile.AppPE.Model.Repository.Interfaces.PedidoVenda;
using System.Reflection;
using Xamarin.Essentials;
using System.IO;
using PdfSharpCore.Drawing;
using PdfSharpCore.Pdf;
using PdfSharpCore.Fonts;
//using Syncfusion.Pdf;
//using Syncfusion.Pdf.Graphics;

namespace Xamarin.HLP.Mobile.AppPE.ViewModel.Pedido
{
    public class PedidoToPrintViewModel : NotifyCommon
    {
        public bool bCanPrint { get; set; } = Device.RuntimePlatform == Device.Android;

        private double vDescontoTotal { get; set; }
        private double vSubTotal { get; set; }
        private double vTotalComissao { get; set; }

        public int idPedidoVendaOffLine { get; set; }

        public string xFullText { get; set; }

        private string _xTitle;
        public string xTitle
        {
            get { return _xTitle; }
            set { _xTitle = value; NotifyPropertyChanged(); }
        }


        private string _xEmpresa;
        public string xEmpresa
        {
            get { return _xEmpresa; }
            set { _xEmpresa = (value ?? "").ToUpper(); NotifyPropertyChanged(); }
        }

        private string _xEnderecoEmpresa;
        public string xEnderecoEmpresa
        {
            get { return _xEnderecoEmpresa; }
            set { _xEnderecoEmpresa = (value ?? "").ToUpper(); NotifyPropertyChanged(); }
        }


        private string _xCliente;
        public string xCliente
        {
            get { return _xCliente; }
            set { _xCliente = (value ?? "").ToUpper(); NotifyPropertyChanged(); }
        }
        private string _xheader_item;

        private string _xRazaoSocial;
        public string xRazaoSocial
        {
            get { return _xRazaoSocial; }
            set { _xRazaoSocial = (value ?? "").ToUpper(); NotifyPropertyChanged(); }
        }

        private string _xTextoPrint;
        public string xTextoPrint
        {
            get { return _xTextoPrint; }
            set { _xTextoPrint = (value ?? "").ToUpper(); NotifyPropertyChanged(); }
        }
        public string xheader_item
        {
            get { return _xheader_item; }
            set { _xheader_item = value; NotifyPropertyChanged(); }
        }


        private string _itens;
        public string itens
        {
            get { return _itens; }
            set { _itens = (value ?? "").ToUpper(); NotifyPropertyChanged(); }
        }

        private string _totais;

        public string totais
        {
            get { return _totais; }
            set { _totais = (value ?? "").ToUpper(); NotifyPropertyChanged(); }
        }

        private string _xFileImgAss;
        public string xFileImgAss
        {
            get { return _xFileImgAss; }
            set { _xFileImgAss = value; NotifyPropertyChanged(); }
        }

        private ImageSource _imgAssinaturaPedido;
        public ImageSource imgAssinaturaPedido
        {
            get { return _imgAssinaturaPedido; }
            set { _imgAssinaturaPedido = value; NotifyPropertyChanged(); }
        }

        private string _agradecimento;

        public string agradecimento
        {
            get { return _agradecimento; }
            set { _agradecimento = value; NotifyPropertyChanged(); }
        }

        private string _Separador1;

        public string Separador1
        {
            get { return _Separador1; }
            set { _Separador1 = value; NotifyPropertyChanged(); }
        }

        public ICommand PrintCommand { get; set; }

        public ICommand CloseCommand { get; set; }

        IDisposable _connectedDisposable;

        public PedidoToPrintViewModel()
        {
            xTextoPrint = "IMPRIMIR VIA BLUETOOTH";
            PrintCommand = new Command(() =>
            {
                if (Device.RuntimePlatform == Device.iOS)
                {
                    //SendToPrintIos();
                }
                else
                {
                    SendToPrint();
                }
            });

            CloseCommand = new Command(() =>
            {
                UtilNavidate.PopPopupNew();
            });

            InitializeFontResolver();

        }

        public ICommand CompartilharPdfCommand
        {
            get
            {
                return new Command<StackLayout>(async (stackLayout) =>
                {
                    await CompartilharPdf(stackLayout);
                });
            }
        }

        public bool Initialize()
        {
            if (canExecuteInicial)
            {
                canExecuteInicial = false;

                //Refatorado em O.S. 33645
                //var pedido = PedidoRepository.GetPedidoVendaModel(idPedidoVendaOffLine);
                var xNumPedido = "";
                IItensImpressaoPedido _buscaItensImpressao = new ItensImpressaoPedido();
                var pedido = _buscaItensImpressao.RetornarItensParaImpressao(id: idPedidoVendaOffLine);

                AtualizaTotalizadores(pedido);

                xTitle = "";

                if (pedido.stPedidoVenda == 1)
                    xTitle += "CANCELADO" + Environment.NewLine;
                var empresa = EmpresaRepository.GetEmpresa();
                xEmpresa = empresa.xRazaoSocial + Environment.NewLine;

                xEnderecoEmpresa =
                    ($@"CNPJ: {empresa.xCnpj}, End: {empresa.xEndereco}, Numero: {empresa.cNumero}, Bairro: {empresa.xBairro}, Fones: {empresa
                        .xTelefones} - Email: {(empresa.xEmails ?? "").Replace(',', ' ')}{Environment.NewLine}").ToUpper();

                if (!string.IsNullOrEmpty(pedido.xDisplayIntegracao) || App.tipouser == App.TipoUser.OMIE || App.tipouser == App.TipoUser.BLING)
                {
                    xNumPedido = pedido.xDisplayIntegracao != null ? pedido.xDisplayIntegracao.ToString().PadLeft(4, '0') : "-----";
                }
                else
                {
                    xNumPedido = pedido.idPedidoDisplay != null ? pedido.idPedidoDisplay.ToString().PadLeft(4, '0') : "-----";
                }

                xCliente += $@"{(pedido.stLancamento == 0 ? "Orçamento" : "Pedido")}: {xNumPedido} - ";
                xCliente += $@"{(pedido.idPedidoVenda > 0 ? pedido.dEmissao.AddHours(-3).ToString("dd/MM/yyyy HH:mm") : pedido.dEmissao.ToString("dd/MM/yyyy HH:mm"))}{Environment.NewLine}";
                if (pedido.stLancamento == 0)
                {
                    xCliente += $@"Valido até: {(pedido.dtValidadeOrcamento ?? DateTime.UtcNow).ToString("dd/MM/yyyy HH:mm")}{Environment.NewLine}";
                }
                xCliente += $@"Prazo: {CondicaoPagamentoRepository.GetDisplay(pedido.idCondicaoPagamento ?? 0)}{Environment.NewLine}";
                xCliente += $@"Vendedor: {EmpresaAspnetUsersRepository.GetDisplay(pedido.idRepresentantePedido ?? 0)}{Environment.NewLine}";
                xCliente += $@"________________________________{Environment.NewLine}";
                var cliente = ClienteRepository.GetClienteModel(pedido.idClientesOffLine);
                xCliente += $@"Fantasia: {cliente.xFantasia}{Environment.NewLine}";
                xCliente += $@"Razao: {cliente.xRazaoSocial}{Environment.NewLine}";
                xCliente += $@"Fone: {cliente.xTelefones}{Environment.NewLine}";
                xCliente += $@"Email: {cliente.xEmails}{Environment.NewLine}";
                EnderecoModel ender = null;
                if (cliente.lEndereco.Any())
                {
                    ender = cliente.lEndereco.Any(c => c.stPrincipal) ? cliente.lEndereco.FirstOrDefault(c => c.stPrincipal) : cliente.lEndereco.FirstOrDefault();

                }
                if (ender != null)
                {
                    xCliente += $@"Ender: {ender.xEndereco}{Environment.NewLine}";
                    xCliente += $@"Bairro: {ender.xBairro}{Environment.NewLine}";
                    xCliente += $@"Numero: {ender.cNumero}{Environment.NewLine}";
                    xCliente += $@"Cidade/UF: {ender.xCidade}/{ender.xEstado}{Environment.NewLine}";
                    xCliente += $@"CEP: {ender.xCep}{Environment.NewLine}";
                }

                //xCliente += $@"Obs: {cliente.xAnotacao}{Environment.NewLine}";
                xCliente += $@"CNPJ/CPF: {cliente.xCpfCnpj}{Environment.NewLine}";
                xCliente += $@"IE/RG: {cliente.xRgIe}{Environment.NewLine}";

                // sep1
                xheader_item += $"CÓD. | DESCRIÇÃO";
                // sep1

                int contador = 0;
                foreach (var item in pedido.lItens)
                {
                    var cAlternativo = item.cAlternativo;
                    if ((item.idProduto ?? 0) > 0)
                    {
                        var resultado = ProdutoRepository.GetNomeByIdCliente(pedido.idClientesOffLine, item.idProduto ?? 0);
                        if (resultado != "")
                        {
                            cAlternativo = resultado;
                        }
                    }

                    //OS 35384 - Jessica Barbieri
                    var xItem = "";

                    var produto = ProdutoRepository.GetProduto(item.idProdutoOffLine);

                    //por opção do paulo e gustavo, o desconto é aplicado no valor unitario de venda com impostos
                    //para o cliente poder ver o valor cheio do produto e depois seu desconto
                    var unitarioCheio = item.vUnitarioVendaComImpostos + Math.Round(item.vDesconto, 2, MidpointRounding.ToEven);
                    item.vSubTotal = unitarioCheio * item.vQtdItem;
                    if ((item.idGradeCor == 0 || item.idGradeCor == null) && (item.idGradeTamanho == 0 || item.idGradeTamanho == null))
                    {
                        if (contador > 0)
                            xItem = $"\n{Environment.NewLine}";

                        if (item.vDesconto > 0)
                        {
                            xItem += $"{cAlternativo.ToUpper()} | {item.xDescricao}{Environment.NewLine} S/ Desc:  {item.xQtde}  {unitarioCheio.ToCurrencyStringSimplesPtBr()} = {item.xValorSubTotal}";
                            xItem += $"\n C/ desc: {item.xQtde} {item.vUnitarioVendaComImpostos.ToCurrencyStringSimplesPtBr()} = {(item.vUnitarioVendaComImpostos * item.vQtdItem).ToCurrencyStringSimplesPtBr()}";
                        }
                        else
                        {
                            xItem += $"{cAlternativo.ToUpper()} | {item.xDescricao}{Environment.NewLine} {item.xQtde}  {unitarioCheio.ToCurrencyStringSimplesPtBr()} = {item.xValorSubTotal}";
                        }

                        itens += xItem;
                    }

                    else if ((item.idGradeCor == 0 || item.idGradeCor == null) && (item.idGradeTamanho != 0 || item.idGradeTamanho != null))
                    {
                        var gradeTamanho = ProdutoRepository.GetGradeTamahoProduto(produto.idProduto ?? 0);
                        foreach (var tam in gradeTamanho)
                        {
                            if (tam.idGradeTamanho == item.idGradeTamanho)
                            {
                                if (contador > 0)
                                    xItem = $"\n{Environment.NewLine}";

                                if (item.vDesconto > 0)
                                {
                                    xItem += $"{cAlternativo.ToUpper()} | {item.xDescricao} | {tam.xNome}{Environment.NewLine} S/ Desc: {item.xQtde}  {unitarioCheio.ToCurrencyStringSimplesPtBr()} = {item.xValorSubTotal}";
                                    xItem += $"\n C/ Desc: {item.xQtde}  {item.vUnitarioVendaComImpostos.ToCurrencyStringSimplesPtBr()} = {(item.vUnitarioVendaComImpostos * item.vQtdItem).ToCurrencyStringSimplesPtBr()}";
                                }
                                else
                                {
                                    xItem += $"{cAlternativo.ToUpper()} | {item.xDescricao}{Environment.NewLine} {item.xQtde}  {unitarioCheio.ToCurrencyStringSimplesPtBr()} = {item.xValorSubTotal}";
                                }

                                itens += xItem;
                            }
                        }
                    }

                    else if ((item.idGradeCor != 0 || item.idGradeCor != null) && (item.idGradeTamanho == 0 || item.idGradeTamanho == null))
                    {
                        var gradeCor = ProdutoRepository.GetGradeCorProduto(produto.idProduto ?? 0);
                        foreach (var cor in gradeCor)
                        {
                            if (cor.idGradeCor == item.idGradeCor)
                            {
                                if (contador > 0)
                                    xItem = $"\n{Environment.NewLine}";

                                if (item.vDesconto > 0)
                                {
                                    xItem += $"\n{Environment.NewLine}{cAlternativo.ToUpper()} | {item.xDescricao} | {cor.xNome}{Environment.NewLine} S/ Desc:  {item.xQtde}  {unitarioCheio.ToCurrencyStringSimplesPtBr()} = {item.xValorSubTotal}";
                                    xItem += $"\n C/ desc: {item.xQtde} {item.vUnitarioVendaComImpostos.ToCurrencyStringSimplesPtBr()} = R$ {(item.vUnitarioVendaComImpostos * item.vQtdItem).ToCurrencyStringSimplesPtBr()}";
                                }
                                else
                                {
                                    xItem += $"{cAlternativo.ToUpper()} | {item.xDescricao} | {cor.xNome}{Environment.NewLine} {item.xQtde}  {unitarioCheio.ToCurrencyStringSimplesPtBr()} = {item.xValorSubTotal}";
                                }


                                itens += xItem;
                            }
                        }
                    }

                    else if ((item.idGradeCor != 0 || item.idGradeCor != null) && (item.idGradeTamanho != 0 || item.idGradeTamanho != null))
                    {
                        var gradeCor = ProdutoRepository.GetGradeCorProduto(produto.idProduto ?? 0);
                        var gradeTamanho = ProdutoRepository.GetGradeTamahoProduto(produto.idProduto ?? 0);

                        var nomeCor = "";
                        var nomeTam = "";

                        foreach (var cor in gradeCor)
                        {
                            if (cor.idGradeCor == item.idGradeCor)
                            {
                                nomeCor = cor.xNome;
                            }
                        }

                        foreach (var tam in gradeTamanho)
                        {
                            if (tam.idGradeTamanho == item.idGradeTamanho)
                            {
                                nomeTam = tam.xNome;
                            }
                        }

                        if (contador > 0)
                            xItem = $"\n{Environment.NewLine}";

                        if (item.vDesconto > 0)
                        {
                            xItem += $"{cAlternativo.ToUpper()} | {item.xDescricao} | {nomeCor} | {nomeTam}{Environment.NewLine} S/ Desc: {item.xQtde}  {unitarioCheio.ToCurrencyStringSimplesPtBr()} = {item.xValorSubTotal}";
                            xItem += $"\n C/ Desc: {item.xQtde} {item.vUnitarioVendaComImpostos.ToCurrencyStringSimplesPtBr()} = R$ {(item.vUnitarioVendaComImpostos * item.vQtdItem).ToCurrencyStringSimplesPtBr()}";
                        }
                        else
                        {
                            xItem += $"{cAlternativo.ToUpper()} | {item.xDescricao} | {nomeCor} | {nomeTam}{Environment.NewLine} {item.xQtde}  {unitarioCheio.ToCurrencyStringSimplesPtBr()} = {item.xValorSubTotal}";
                        }



                        itens += xItem;
                    }

                    contador++;
                    //var xItem = $"{Environment.NewLine}{cAlternativo.ToUpper()} | {item.xDescricao}{Environment.NewLine} Qtde: {item.xQtde}  {item.vUnitarioVendaComImpostos.ToCurrencyStringSimplesPtBr()} = {item.xValorSubTotal}";
                    //itens += xItem;
                }

                //totais += Separador3;

                totais = $"{Environment.NewLine}";
                //totais += $@"==================================={Environment.NewLine}";
                totais += $@"________________________________{Environment.NewLine}";
                totais += $@"{Environment.NewLine}";


                //10/08/17 comentado por requisição em 33610
                //totais += $"DESCONTO: {vDescontoTotal.ToCurrencyStringPtBr()}{Environment.NewLine}";

                // OS 35414 - Jessica Barbieri
                //totais += $"TOTAL: {pedido.VTotal.ToCurrencyStringPtBr()}{Environment.NewLine}";  

                //existe este tratamento pra desconto e somas apeans aqui nesse print na bluetooth
                double totalItens = pedido.lItens.Sum(p => p.vSubTotal);
                double descontoPedido = totalItens - pedido.VTotal;

                totais += $"TOTAL DOS ITENS ({pedido.lItens.Count}): {totalItens.ToCurrencyStringPtBr()}{Environment.NewLine}";
                if (descontoPedido > 0)
                    totais += $"TOTAL DE DESCONTO: {descontoPedido.ToCurrencyStringPtBr()}{Environment.NewLine}";
                if (pedido.vFretePed > 0)
                    totais += $"TOTAL DE FRETE: {pedido.vFretePed.ToCurrencyStringPtBr()}{Environment.NewLine}";
                if (pedido.vSeguroPed > 0)
                    totais += $"TOTAL DE SEGURO: {pedido.vSeguroPed.ToCurrencyStringPtBr()}{Environment.NewLine}";
                if (pedido.vOutrasPed > 0)
                    totais += $"TOTAL DE OUTROS: {pedido.vOutrasPed.ToCurrencyStringPtBr()}{Environment.NewLine}";
                totais += $"TOTAL DO PEDIDO: {pedido.VTotal.ToCurrencyStringPtBr()}{Environment.NewLine}";
                totais += $"\n";

                if (!string.IsNullOrEmpty(pedido.xInfAdicional))
                    totais += $"INF. ADICIONAL: {pedido.xInfAdicional}{Environment.NewLine}";

                if (!string.IsNullOrEmpty(pedido.xMotivoCancelamento))
                    totais += $"MOTIVO DE CANCELAMENTO: {pedido.xMotivoCancelamento.ToUpper()}{Environment.NewLine}";

                xFileImgAss = PedidoRepository.BuscarAssAtualizada(pedido.idPedidoVendaOffLine ?? 0);
                imgAssinaturaPedido = _fileService?.GetImage(xFileImgAss).Result;

                if (imgAssinaturaPedido?.ToString()?.Length > 0)
                    totais += $"\n";

                xRazaoSocial = $"{cliente.xRazaoSocial}";

                agradecimento += $"Obrigado pela preferência{Environment.NewLine}";
                //totais += Separador2;
                agradecimento += $"Gerado por pedidoeletronico.com{Environment.NewLine}{Environment.NewLine}{Environment.NewLine}";

                Separador1 = "=========================================================";
            }
            return canExecuteInicial;
        }

        public Xamarin.HLP.Mobile.AppPE.Services.IFileService _fileService => DependencyService.Get<Xamarin.HLP.Mobile.AppPE.Services.IFileService>();

        public void AtualizaTotalizadores(PedidoVendaModel currentModel)
        {
            try
            {
                if (currentModel.lItens == null) return;
                vSubTotal = currentModel.lItens.Sum(c => (c.ItensGrade != null ? c.ItensGrade.Sum(o => o.vSubTotal) : c.vSubTotal));
                vDescontoTotal = currentModel.lItens.Sum(c => (c.ItensGrade != null ? c.ItensGrade.Sum(o => o.vDesconto * o.vQtdItem)
                : (c.vDesconto * c.vQtdItem)));
                vTotalComissao = currentModel.lItens.Sum(c => (c.ItensGrade != null ? c.ItensGrade.Sum(o => o.vComissao) : c.vComissao));
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

        private bool _isConnected = false;

        public bool isConnected
        {
            get { return _isConnected; }
            set { _isConnected = value; NotifyPropertyChanged(); }
        }
        public int iColunas { get; set; } = 48;
        public int iColunasPrinterMenor { get; set; } = 32;

        public async void SendToPrintIos()
        {
            try
            {
                //busca no banco se existe algum dispositivo já atrelado

                xTextoPrint = $"AGUARDE ENQUANTO CONECTAMOS...";
                bool bOutraImpressora = false;
                var separador = string.Empty.PadLeft(33, '=');

                IAdapter adapter;
                IBluetoothLE bluetoothBLE;

                bluetoothBLE = CrossBluetoothLE.Current;
                adapter = CrossBluetoothLE.Current.Adapter;
                if (bluetoothBLE.State == BluetoothState.Off || bluetoothBLE.State == BluetoothState.Unknown)
                {
                    await App.Messages.ShowAsync("Bluetooth desabilitado");
                }
                else
                {
                    adapter.ScanTimeout = 2000;
                    adapter.ScanMode = ScanMode.LowLatency;
                    ObservableCollection<IDevice> lDevices = new ObservableCollection<IDevice>();

                    lDevices.Clear();
                    adapter.DeviceDiscovered += (obj, a) =>
                    {
                        if (!lDevices.Contains(a.Device))
                            lDevices.Add(a.Device);
                    };

                    adapter.DeviceConnected += delegate (object obj, DeviceEventArgs args)
                    {

                    };

                    if (!adapter.IsScanning)
                    {
                        await adapter.StartScanningForDevicesAsync();
                    }

                    if (lDevices?.Count() > 0)
                    {
                        IDevice device = lDevices.Where(t => t.Name == "MPT-II").FirstOrDefault();
                        if (device == null)
                        {
                            device = lDevices.Where(t => t.Name == "MPT-III" || t.Name == "MPT-3").FirstOrDefault();
                            if (device != null)
                                separador = "=".PadLeft(iColunas, '=');
                            else
                            {
                                device = lDevices.FirstOrDefault();
                                bOutraImpressora = true;
                            }
                        }
                        else
                            separador = "=".PadLeft(iColunasPrinterMenor, '=');


                        await adapter.ConnectToDeviceAsync(device);
                        var _services = await device.GetServiceAsync(device.Id);
                        var _caracteristicas = await _services.GetCharacteristicAsync(_services.Id);


                        await _caracteristicas.WriteAsync(WriteBytesPosition("center"));
                        await _caracteristicas.WriteAsync(WriteBytes((xTitle + Environment.NewLine).RemoverAcentos()));


                        await _caracteristicas.WriteAsync(WriteBytesPosition("center"));
                        await _caracteristicas.WriteAsync(WriteBytes(xEmpresa.RemoverAcentos()));


                        await _caracteristicas.WriteAsync(WriteBytesPosition("left"));
                        await _caracteristicas.WriteAsync(WriteBytes(xEnderecoEmpresa.RemoverAcentos()));


                        if (bOutraImpressora)
                        {
                            await _caracteristicas.WriteAsync(WriteBytesPosition("center"));
                            await _caracteristicas.WriteAsync(WriteBytes(separador + Environment.NewLine));
                        }
                        else
                        {
                            await _caracteristicas.WriteAsync(WriteBytesPosition("left"));
                            await _caracteristicas.WriteAsync(WriteBytes(separador + Environment.NewLine));
                        }

                        await _caracteristicas.WriteAsync(WriteBytesPosition("left"));
                        await _caracteristicas.WriteAsync(WriteBytes(xCliente.RemoverAcentos()));

                        if (bOutraImpressora)
                        {
                            await _caracteristicas.WriteAsync(WriteBytesPosition("center"));
                            await _caracteristicas.WriteAsync(WriteBytes(separador + Environment.NewLine));
                        }
                        else
                        {
                            await _caracteristicas.WriteAsync(WriteBytesPosition("left"));
                            await _caracteristicas.WriteAsync(WriteBytes(separador + Environment.NewLine));
                        }


                        if (bOutraImpressora)
                        {
                            await _caracteristicas.WriteAsync(WriteBytesPosition("center"));
                            await _caracteristicas.WriteAsync(WriteBytes(xheader_item.RemoverAcentos() + Environment.NewLine));
                        }
                        else
                        {
                            await _caracteristicas.WriteAsync(WriteBytesPosition("left"));
                            await _caracteristicas.WriteAsync(WriteBytes(xheader_item.RemoverAcentos()));
                        }

                        if (bOutraImpressora)
                        {
                            await _caracteristicas.WriteAsync(WriteBytesPosition("center"));
                            await _caracteristicas.WriteAsync(WriteBytes(separador + Environment.NewLine));
                        }
                        else
                        {
                            await _caracteristicas.WriteAsync(WriteBytesPosition("left"));
                            await _caracteristicas.WriteAsync(WriteBytes(separador));
                        }


                        await _caracteristicas.WriteAsync(WriteBytesPosition("left"));
                        await _caracteristicas.WriteAsync(WriteBytes(itens.RemoverAcentos()));


                        await _caracteristicas.WriteAsync(WriteBytesPosition("left"));
                        await _caracteristicas.WriteAsync(WriteBytes((totais + Environment.NewLine).RemoverAcentos()));

                        await _caracteristicas.WriteAsync(WriteBytesPosition("right"));
                        await _caracteristicas.WriteAsync(WriteBytes((agradecimento.RemoverAcentos() + Environment.NewLine + Environment.NewLine)));
                        UtilNavidate.PopPopupNew();
                    }
                    else
                    {
                        await App.Messages.ShowAsync("Nenhum dispositivo foi encontrado");
                    }
                }
            }
            catch (Exception ex)
            {

            }

        }

        public byte[] WriteBytes(string xValor)
        {
            return System.Text.Encoding.GetEncoding(Encoding.ASCII.CodePage).GetBytes(xValor);
        }

        public byte[] WriteBytesPosition(string position)
        {

            if (string.IsNullOrEmpty(position) || position.ToUpper().Equals("LEFT"))
            {
                byte[] left = { 0x1b, 0x61, 0x00 }; // left-aligned 
                return left;
            }
            else if (position.ToUpper().Equals("RIGHT"))
            {
                byte[] right = { 0x1b, 0x61, 0x02 }; // right-aligned 
                return right;
            }
            else if (position.ToUpper().Equals("CENTER"))
            {
                byte[] center = { 0x1b, (byte)'a', 0x01 }; // center alignment 
                return center;
            }

            return System.Text.Encoding.GetEncoding(Encoding.ASCII.CodePage).GetBytes("");
        }

        private static bool fontResolverInitialized = false;

        public void InitializeFontResolver()
        {
            if (!fontResolverInitialized)
            {
                var assembly = typeof(PedidoToPrintViewModel).GetTypeInfo().Assembly;
                var fontResourceName = "Xamarin.HLP.Mobile.AppPE.ViewModel.Pedido.Belfast Regular.ttf";

                if (assembly.GetManifestResourceNames().Contains(fontResourceName))
                {
                    using (Stream fontStream = assembly.GetManifestResourceStream(fontResourceName))
                    {
                        using (MemoryStream memoryStream = new MemoryStream())
                        {
                            fontStream.CopyTo(memoryStream);
                            memoryStream.Seek(0, SeekOrigin.Begin);

                            var fontResolver = new CustomFontResolver(memoryStream.ToArray());

                            GlobalFontSettings.FontResolver = fontResolver;

                        }
                    }
                }

                fontResolverInitialized = true;
            }
        }


        public async Task CompartilharPdf(StackLayout stackLayout)
        {
            try
            {
                IItensImpressaoPedido _buscaItensImpressao = new ItensImpressaoPedido();
                var pedido = _buscaItensImpressao.RetornarItensParaImpressao(id: idPedidoVendaOffLine);
                var cliente = ClienteRepository.GetClienteModel(pedido.idClientesOffLine);
                var empresa = EmpresaRepository.GetEmpresa();

                string fileName = $"{pedido.TipoLancamento.ToLower()}_{cliente.xFantasia}-{empresa.xRazaoSocial}.pdf";
                string filePath = Path.Combine(FileSystem.CacheDirectory, fileName);

                double pdfWidth = stackLayout.Width;
                double pdfHeight = stackLayout.Height;

                PdfDocument document = new PdfDocument();
                PdfPage page = document.AddPage();
                page.Width = XUnit.FromPoint(pdfWidth);
                page.Height = XUnit.FromPoint(pdfHeight);

                XGraphics gfx = XGraphics.FromPdfPage(page);

                var font1 = new XFont("Belfast Regular", 13);

                double y = 0;

                foreach (var child in stackLayout.Children)
                {
                    if (child is Label label)
                    {
                        string text = label.Text;
                        string[] lines = text.Split('\n');

                        (double x, double yOffset) = GetElementPosition(stackLayout, label);
                        y = yOffset;

                        foreach (string line in lines)
                        {
                            if (line.Length > 47 && !line.Contains("=========="))
                            {
                                string txt = line;
                                txt = txt.Insert(47, "\n");
                                string[] linesQuebrar = txt.Split('\n');

                                foreach (string lineQuebrar in linesQuebrar)
                                {
                                    gfx.DrawString(lineQuebrar, font1, XBrushes.Black, new XPoint(x, y));
                                    y += gfx.MeasureString(lineQuebrar, font1).Height;
                                }
                            }
                            else
                            {
                                if (line.ToLower().Contains("obrigado pela preferência"))
                                {
                                    var imageSource = (stackLayout.Children.FirstOrDefault(c => c is Xamarin.Forms.Image img) as Xamarin.Forms.Image)?.Source as FileImageSource;
                                    double newImageWidth = 80;
                                    double newImageHeight = 80;
                                    double imageX = (page.Width - newImageWidth) / 2;
                                    double imageY = y - 155;

                                    if (imageSource != null)
                                    {
                                        var filePathImage = imageSource.File;
                                        if (File.Exists(filePathImage))
                                        {
                                            var tempImagePath = Path.Combine(Path.GetTempPath(), Path.GetFileName(filePathImage));
                                            File.Copy(filePathImage, tempImagePath, overwrite: true);

                                            using (var pdfImage = XImage.FromFile(tempImagePath))
                                            {
                                                gfx.DrawImage(pdfImage, imageX, imageY, newImageWidth, newImageHeight);
                                            }

                                            File.Delete(tempImagePath);
                                        }
                                    }

                                    // Desenha a linha independentemente da imagem
                                    double extraLineWidth = 200;
                                    double lineStartX = imageX - (extraLineWidth / 2);
                                    double lineEndX = imageX + newImageWidth + (extraLineWidth / 2);
                                    double lineY = imageY + newImageHeight + 10;

                                    gfx.DrawLine(XPens.Black, lineStartX, lineY, lineEndX, lineY);

                                    // Ajusta a posição de `y` para continuar com o conteúdo
                                    y = lineY + 60;
                                }

                                if (line == xRazaoSocial)
                                {
                                    double textWidth = gfx.MeasureString(line, font1).Width;
                                    double centerX = (page.Width - textWidth) / 2;

                                    gfx.DrawString(line, font1, XBrushes.Black, new XPoint(centerX, y));
                                    y += gfx.MeasureString(line, font1).Height;
                                }
                                else
                                {
                                    gfx.DrawString(line, font1, XBrushes.Black, new XPoint(x, y));
                                    y += gfx.MeasureString(line, font1).Height;
                                }
                            }
                        }
                    }
                }

                document.Save(filePath);
                document.Close();

                await Share.RequestAsync(new ShareFileRequest
                {
                    Title = "Download do PDF",
                    File = new ShareFile(filePath)
                });
            }
            catch (Exception ex)
            {
                await App.Current.MainPage.DisplayAlert("erro", ex.ToString(), "ok");
            }
        }


        private (double x, double y) GetElementPosition(StackLayout stackLayout, Xamarin.Forms.View element)
        {
            double x = 0;
            double y = 0;

            Xamarin.Forms.VisualElement parent = element.Parent as Xamarin.Forms.VisualElement;
            while (parent != null && parent != stackLayout)
            {
                x += parent.X;
                y += parent.Y;
                parent = parent.Parent as Xamarin.Forms.VisualElement;
            }

            if (parent == stackLayout)
            {
                double elementX = element.X + stackLayout.X;
                double elementY = element.Y + stackLayout.Y;

                if (stackLayout.Padding != null)
                {
                    elementX += stackLayout.Padding.Left;
                    elementY += stackLayout.Padding.Top;
                }

                if (element.Margin != null)
                {
                    elementX += element.Margin.Left;
                    elementY += element.Margin.Top;
                }

                x += elementX;
                y += elementY;
            }

            return (x, y);
        }

        public void SendToPrint()
        {
            if (App.BluetoothLe == null)
            {
                return;
            }

            bool bOutraImpressora = false;

            if (App.BluetoothLe.Connect() == false)
            {
                App.Messages.ShowAsync("Nenhuma impressora bluetooth foi encontrada nos dispositivos pareados.");
                UtilNavidate.PopPopupNew();
                return;
            }
            var separador = "=".PadLeft(32, '=');

            if (App.BluetoothLe.GetNameDevice().ToUpper().Equals("MPT-II"))
            {
                separador = "=".PadLeft(iColunasPrinterMenor, '=');
            }
            else if (App.BluetoothLe.GetNameDevice().ToUpper().Equals("MPT-III") || App.BluetoothLe.GetNameDevice().ToUpper().Equals("DPP-350"))
            {
                separador = "=".PadLeft(iColunas, '=');
            }
            else
            {
                bOutraImpressora = true;
            }


            App.BluetoothLe.Write((xTitle + Environment.NewLine).RemoverAcentos(), "center");
            App.BluetoothLe.Write(xEmpresa.RemoverAcentos(), "center");
            App.BluetoothLe.Write(xEnderecoEmpresa.RemoverAcentos(), "left");
            if (bOutraImpressora)
                App.BluetoothLe.Write(separador + Environment.NewLine, "center");
            else
                App.BluetoothLe.Write(separador, "left");

            App.BluetoothLe.Write(xCliente.RemoverAcentos(), "left");

            if (bOutraImpressora)
                App.BluetoothLe.Write(separador + Environment.NewLine, "center");
            else
                App.BluetoothLe.Write(separador, "left");


            if (bOutraImpressora)
                App.BluetoothLe.Write(xheader_item.RemoverAcentos() + Environment.NewLine, "center");
            else
                App.BluetoothLe.Write(xheader_item.RemoverAcentos(), "left");

            if (bOutraImpressora)
                App.BluetoothLe.Write(separador + Environment.NewLine, "center");
            else
                App.BluetoothLe.Write(separador, "left");

            App.BluetoothLe.Write(itens.RemoverAcentos(), "left");
            App.BluetoothLe.Write((totais + Environment.NewLine).RemoverAcentos(), "left");
            App.BluetoothLe.Write((agradecimento.RemoverAcentos() + Environment.NewLine + Environment.NewLine), "right");
            UtilNavidate.PopPopupNew();
        }     
    }
}

using System;
using System.Linq;
using System.Windows.Input;
using Xamarin.Forms;
using Xamarin.HLP.Mobile.AppPE.Common;
using Xamarin.HLP.Mobile.AppPE.Model;
using Xamarin.HLP.Mobile.AppPE.Model.Cadastros;
using Xamarin.HLP.Mobile.AppPE.Model.Repository;
using Xamarin.HLP.Mobile.AppPE.View.Cliente;
using Xamarin.HLP.Mobile.AppPE.View.Pesquisas;
using Xamarin.HLP.Mobile.AppPE.ViewModel.Pesquisa;

namespace Xamarin.HLP.Mobile.AppPE.ViewModel.Cadastro
{
    public class ClienteViewModel : ViewModelComum<ClientesModel>
    {
        #region Properties
        public ICommand SearchRamoAtividadeCommand { get; set; }
        public ICommand SearchTransportadoraCommand { get; set; }

        public ICommand SearchRedespachoCommand { get; set; }
        public ICommand SearchCondicaoPagamentoCommand { get; set; }
        public ICommand SearchTabelaPrecoCommand { get; set; }

        public ICommand FoneEmailCommand { get; set; }
        public ICommand ContatosCommand { get; set; }
        public ICommand EnderecoCommand { get; set; }

        private string _labelContatos = "Contatos (0)";
        public string labelContatos
        {
            get { return _labelContatos; }
            set { _labelContatos = value; NotifyPropertyChanged(); }
        }


        private string _labelEnderecos = "Endereços (0)";
        public string labelEnderecos
        {
            get { return _labelEnderecos; }
            set { _labelEnderecos = value; NotifyPropertyChanged(); }
        }


        public bool needRestore { get; set; } = false;

        private byte _indexStCliente = 1;

        public byte indexStCliente
        {
            get { return _indexStCliente; }
            set
            {
                _indexStCliente = value;
                NotifyPropertyChanged();
                currentModel.stProspeccao = value == 0 ? "CE" : "CP";

                if (value == 0 && (Convert.ToDateTime(currentModel.dEfetivacao).Date == DateTime.Now.SqlMinDateTime()))
                    currentModel.dEfetivacao = DateTime.Today;

                enableDateEfetivacao = value == 0;


            }
        }

        private static readonly int?[] _diasFavoraveisMap =
        {
            null,
            2,
            3,
            4,
            5,
            6,
            0,
            1
        };

        private int _indexDiaFavoravel;
        public int indexDiaFavoravel
        {
            get { return _indexDiaFavoravel; }
            set
            {
                _indexDiaFavoravel = value;
                NotifyPropertyChanged();
                if (value >= 0 && value < _diasFavoraveisMap.Length)
                    currentModel.nDiaFavoravel = _diasFavoraveisMap[value];
            }
        }

        public void SincronizarIndexDiaFavoravel()
        {
            var valor = currentModel?.nDiaFavoravel;
            var idx = Array.IndexOf(_diasFavoraveisMap, valor);
            var newIdx = idx >= 0 ? idx : 0;
            if (_indexDiaFavoravel == newIdx) return;
            _indexDiaFavoravel = newIdx;
            NotifyPropertyChanged(nameof(indexDiaFavoravel));
        }

        private bool _enableDateEfetivacao;

        public bool enableDateEfetivacao
        {
            get { return _enableDateEfetivacao; }
            set
            {
                _enableDateEfetivacao = value;
                NotifyPropertyChanged();
            }
        }


        private bool _bUsaLimite;

        public bool bUsaLimite
        {
            get { return _bUsaLimite; }
            set
            {
                _bUsaLimite = value;
                NotifyPropertyChanged();
            }
        }

        private bool _bBuscouPorCnpj;

        public bool bBuscouPorCnpj
        {
            get { return _bBuscouPorCnpj; }
            set
            {
                _bBuscouPorCnpj = value;
                NotifyPropertyChanged();
            }
        }

        private int _bQuantidadeTelefoneCnpj;

        public int bQuantidadeTelefoneCnpj
        {
            get { return _bQuantidadeTelefoneCnpj; }
            set
            {
                _bQuantidadeTelefoneCnpj = value;
                NotifyPropertyChanged();
            }
        }


        private int _bQuantidadeContatoCnpj;

        public int bQuantidadeContatoCnpj
        {
            get { return _bQuantidadeContatoCnpj; }
            set
            {
                _bQuantidadeContatoCnpj = value;
                NotifyPropertyChanged();
            }
        }


        private int _bQuantidadeEmailCnpj;

        public int bQuantidadeEmailCnpj
        {
            get { return _bQuantidadeEmailCnpj; }
            set
            {
                _bQuantidadeEmailCnpj = value;
                NotifyPropertyChanged();
            }
        }

        private ListItemModel _ramoAtividade = new ListItemModel
        {
            Display = "Nenhum ramo de atividade selecionado"

        };

        public ListItemModel ramoAtividade
        {
            get { return _ramoAtividade; }
            set
            {
                _ramoAtividade = value;
                NotifyPropertyChanged();
            }
        }

        private ListItemModel _CondicaoPgto = new ListItemModel
        {
            Display = "Nenhuma condição de pagamento selecionada"
        };

        public ListItemModel CondicaoPgto
        {
            get { return _CondicaoPgto; }
            set
            {
                _CondicaoPgto = value;
                NotifyPropertyChanged();
            }
        }

        private ListItemModel _Transportadora = new ListItemModel
        {
            Display = "Nenhuma transportadora selecionada"
        };

        public ListItemModel Transportadora
        {
            get { return _Transportadora; }
            set
            {
                _Transportadora = value;
                NotifyPropertyChanged();
            }
        }

        private ListItemModel _Redespacho = new ListItemModel
        {
            Display = "Nenhum redespacho selecionado"
        };
        public ListItemModel Redespacho
        {
            get { return _Redespacho; }
            set
            {
                _Redespacho = value;
                NotifyPropertyChanged();
            }
        }

        private ListItemModel _tabelaPreco = new ListItemModel
        {
            Display = "Nenhuma tabela de preço selecionada"
        };

        public ListItemModel tabelaPreco
        {
            get { return _tabelaPreco; }
            set
            {
                _tabelaPreco = value;
                NotifyPropertyChanged();
            }
        }
        #endregion

        public ClienteViewModel()
        {

            Title = "CLIENTE";
            currentModel = new ClientesModel();

            SearchRamoAtividadeCommand = new Command(() =>
            {
                PagePesquisaPadrao pesquisa = new PagePesquisaPadrao(ramoAtividade,
                    PesquisaPadraoViewModel.Tabela.RAMO_ATIVIDADE)
                {
                    Title = "Ramo de atividade",
                };
                UtilNavidate.PushAsync(pesquisa);
            });

            SearchCondicaoPagamentoCommand = new Command(() =>
            {
                PagePesquisaPadrao pesquisa = new PagePesquisaPadrao(CondicaoPgto,
                    PesquisaPadraoViewModel.Tabela.TB_CONDICAO_PAGAMENTO)
                {
                    Title = "Condição de pagamento",
                };
                UtilNavidate.PushAsync(pesquisa);
            });
            SearchTabelaPrecoCommand = new Command(() =>
            {
                currentModel.idEmpresa = App.CurrentAspnetUserModel.objEmpresaAspnetUsersModel.idEmpresa;
                currentModel.idRamoAtividade = ramoAtividade.Id;

                PagePesquisaPadrao pesquisa = new PagePesquisaPadrao(tabelaPreco,
                    PesquisaPadraoViewModel.Tabela.TB_TABELA_PRECO, Filtro: currentModel)
                {
                    Title = "Tabela de preço",
                };
                UtilNavidate.PushAsync(pesquisa);
            });
            SearchTransportadoraCommand = new Command(() =>
            {
                PagePesquisaPadrao pesquisa = new PagePesquisaPadrao(Transportadora,
                    PesquisaPadraoViewModel.Tabela.TB_TRANSPORTADORA)
                {
                    Title = "Transportadora",
                };
                UtilNavidate.PushAsync(pesquisa);
            });


            SearchRedespachoCommand = new Command(() =>
            {
                PagePesquisaPadrao pesquisa = new PagePesquisaPadrao(Redespacho,
                    PesquisaPadraoViewModel.Tabela.TB_TRANSPORTADORA)
                {
                    Title = "Redespacho",
                };
                UtilNavidate.PushAsync(pesquisa);
            });

            FoneEmailCommand = new Command(() =>
            {
                UtilNavidate.PushAsync(new PageTelefonesCliente(currentModel));
            });

            ContatosCommand = new Command(() =>
            {
                UtilNavidate.PushAsync(new PageListagemContato(currentModel));
            });

            EnderecoCommand = new Command(() =>
            {
                UtilNavidate.PushAsync(new PageListagemEndereco(currentModel));
            });

            CancelCommand = new Command(async () =>
            {
                var result = await App.Messages.ShowConfirmAsync("Deseja realmente sair do lançamento ?");
                if (result)
                {
                    needRestore = true;
                    UtilNavidate.PopAsync();
                }
            });
        }

        public bool Initialize()
        {
            if (canExecuteInicial)
            {
                canExecuteInicial = false;

                var empresa = EmpresaRepository.GetDadosLimiteCreditoEmpresa();

                if (currentModel.idClientesOffLine == null)
                    bUsaLimite = empresa.stForcaLimiteCreditoCliente;
                else
                    bUsaLimite = currentModel.vLimiteCredito != null;


                if (currentModel.stProspeccao == null)
                {
                    currentModel.stProspeccao = "CE";
                }

                indexStCliente = (byte)(currentModel.stProspeccao.Equals("CE") ? 0 : 1);

                SincronizarIndexDiaFavoravel();

                if (currentModel.dEfetivacao == null)
                    currentModel.dEfetivacao = DateTime.Now.SqlMinDateTime();


                if (currentModel.idClientesOffLine != null && currentModel.idClientesOffLine != 0)
                {
                    if (currentModel.idRamoAtividade > 0)
                        ramoAtividade = RamoAtividadeRepository.GetItem(currentModel.idRamoAtividade);

                    if (currentModel.idTabelaPreco != null && currentModel.idTabelaPreco > 0)
                        tabelaPreco = TabelaPrecoRepository.GetItem(currentModel.idTabelaPreco ?? 0);

                    if (currentModel.idCondicaoPagamento != null && currentModel.idCondicaoPagamento > 0)
                        CondicaoPgto = CondicaoPagamentoRepository.GetItem(currentModel.idCondicaoPagamento ?? 0);

                    if (currentModel.idTransportadora != null && currentModel.idTransportadora > 0)
                        Transportadora = TransportadoraRepository.GetItem(currentModel.idTransportadora ?? 0);

                    if (currentModel.idRedespacho != null && currentModel.idRedespacho > 0)
                        Redespacho = TransportadoraRepository.GetItem(currentModel.idRedespacho ?? 0);

                }
                if (ramoAtividade == null || ramoAtividade.Id == 0)
                    ramoAtividade = RamoAtividadeRepository.GetFirstItem();
                if (tabelaPreco == null || tabelaPreco.Id == 0)
                    tabelaPreco = new ListItemModel { Display = "clique aqui para visualizar" };
                if (CondicaoPgto == null || CondicaoPgto.Id == 0)
                    CondicaoPgto = CondicaoPagamentoRepository.GetFirstItem();
                if (Transportadora == null || Transportadora.Id == 0)
                    Transportadora = TransportadoraRepository.GetFirstItem();

            }

            CountLabel();
            return canExecuteInicial;
        }

        public void CountLabel()
        {
            labelContatos = $"Contatos ({currentModel.lContato.Count})";
            labelEnderecos = $"Endereço ({currentModel.lEndereco.Count})";
        }

        public async void Save()
        {
            try
            {
                if (string.IsNullOrEmpty(currentModel.xRazaoSocial) || string.IsNullOrEmpty(currentModel.xFantasia))
                {
                    await App.Messages.ShowAsync("Informe valor para os campos que contenham (*)");
                    return;
                }

                if (ramoAtividade?.Id == 0)
                {
                    await App.Messages.ShowAsync("Ramo de atividade é obrigatório");
                    return;
                }

                if (tabelaPreco?.Id == 0)
                {
                    await App.Messages.ShowAsync("Tabela de preço é obrigatório");
                    return;
                }
                if (Transportadora?.Id == 0)
                {
                    await App.Messages.ShowAsync("Transportadora é obrigatório");
                    return;
                }
                if (CondicaoPgto.Id == 0)
                {
                    await App.Messages.ShowAsync("Condição de pagamento é obrigatório");
                    return;
                }
                currentModel.idTabelaPreco = tabelaPreco.Id;
                currentModel.idCondicaoPagamento = CondicaoPgto.Id;
                currentModel.idRamoAtividade = ramoAtividade.Id;
                currentModel.idTransportadora = Transportadora?.Id;
                currentModel.idRedespacho = Redespacho.Id;

                if (bUsaLimite == false)
                    currentModel.vLimiteCredito = null;

                else if (currentModel.vLimiteCredito == null)
                    currentModel.vLimiteCredito = 0;

                //validação de melhoria
                bool? bAplicaMelhoriaEmailsTelefoneTl = ConfiguracaoGeralRepositorio.GetMelhoriaEspecificaEmailsTelefoneTl(App.CurrentAspnetUserModel.objEmpresaAspnetUsersModel.idEmpresa);
                if (bAplicaMelhoriaEmailsTelefoneTl.GetValueOrDefault())
                {
                    if (string.IsNullOrEmpty(currentModel.xEmails))
                    {
                        await App.Messages.ShowAsync("Precisa ser preenchido ao menos um e-mail!  pelo usuário");
                        return;
                    }


                    var _quantidadeEmails = currentModel.xEmails.Split(',').Where(t => t != "").Count();
                    if ((bBuscouPorCnpj == true && _quantidadeEmails <= bQuantidadeEmailCnpj) || _quantidadeEmails == 0)
                    {
                        await App.Messages.ShowAsync("Precisa ser preenchido ao menos um e-mail pelo usuário!");
                        return;
                    }


                    if (string.IsNullOrEmpty(currentModel.xTelefones))
                    {
                        await App.Messages.ShowAsync("Precisa ser preenchido ao menos um telefone  pelo usuário!");
                        return;
                    }


                    var _quantidadeTelefone = currentModel.xTelefones.Split(',').Where(t => t != "").Count();
                    if ((bBuscouPorCnpj == true && _quantidadeTelefone <= bQuantidadeTelefoneCnpj) || _quantidadeTelefone == 0)
                    {
                        await App.Messages.ShowAsync("Precisa ser preenchido ao menos um telefone pelo usuário!");
                        return;
                    }

                    var _quantidadeContato = currentModel.lContato?.Count();
                    if ((bBuscouPorCnpj == true && _quantidadeContato <= bQuantidadeContatoCnpj) || _quantidadeContato == 0)
                    {
                        await App.Messages.ShowAsync("Precisa ser preenchido ao menos um contato pelo usuário!");
                        return;
                    }


                }

                ClienteRepository.Save(currentModel);
                //if (isNew && StaticModel.StaticFindClienteModel != null)
                //{
                //    // manipulação da page de pesquisa para ja vir pesquisado o cliente salvo
                //    StaticModel.StaticFindClienteModel.RegistrosToSearch.Add(new BasicPickerModel
                //    {
                //        Id = currentModel.idClientesOffLine ?? 0,
                //        Display = currentModel.xRazaoSocial,
                //        Detail = currentModel.xFantasia
                //    });
                //    StaticModel.StaticFindClienteModel.Filtro = currentModel.xRazaoSocial;
                //}


                // faço isso porque o objeto statico é a viewmodel da pagina antirior.

                // altero as informações do item selecionado no listar de clientes
                //if (!isNew)
                //{
                //    if (StaticModel.StaticFindClienteModel != null)
                //    {
                //        StaticModel.StaticFindClienteModel.SelectedItem.Display = currentModel.xRazaoSocial;
                //        StaticModel.StaticFindClienteModel.SelectedItem.Detail = currentModel.xFantasia;
                //    }
                //    StaticModel.StaticClientesModel.needRefresh = true;
                //}
                needRestore = true;
                UtilNavidate.PopAsync();
            }
            catch (Exception ex)
            {
                await App.Messages.ShowAsync(ex.Message);
            }

        }

    }
}

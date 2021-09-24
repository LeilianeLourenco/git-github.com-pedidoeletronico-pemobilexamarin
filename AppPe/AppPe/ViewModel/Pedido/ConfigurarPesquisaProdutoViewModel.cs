using System;
using System.Collections.Generic;
using System.Linq;
using Xamarin.Forms;
using Xamarin.HLP.Mobile.AppPE.Common;
using Xamarin.HLP.Mobile.AppPE.Model;
using Xamarin.HLP.Mobile.AppPE.Model.Lancamento;
using Xamarin.HLP.Mobile.AppPE.Model.Repository;
using Xamarin.HLP.Mobile.AppPE.View.Pesquisas;

namespace Xamarin.HLP.Mobile.AppPE.ViewModel.Pedido
{
    public class ConfigurarPesquisaProdutoViewModel : SearchCommom
    {

        public Command ChangeTipoOrdenacaoCommand { get; set; }

        public Command GoToCategoriaCommand { get; set; }
        public Command GoToRepresentacoesCommand { get; set; }

        private ListItemModel _representada;

        public ListItemModel representada
        {
            get { return _representada; }
            set { _representada = value; NotifyPropertyChanged(); }
        }


        private ListItemModel _categoria;

        public ListItemModel categoria
        {
            get { return _categoria; }
            set { _categoria = value; NotifyPropertyChanged(); }
        }

        public ConfiguracaoPesquisaProdutoModel configDoPedido { get; set; }

        public ConfigurarPesquisaProdutoViewModel()
        {
            ChangeTipoOrdenacaoCommand = new Command(async () =>
            {
                var ordem = await App.Messages.TipoOrdenacaoTask();
                if (ordem != -1)
                {
                    currentModel.Ordenacao = ordem;
                }
            });


            EfetivarPesquisaCommand = new Command(() =>
            {
                configDoPedido.bNeedRefresh = true;
                configDoPedido.paramRepresentacao = representada;
                configDoPedido.paramCategoria = categoria;
                configDoPedido.bUltimasCompras = currentModel.bUltimasCompras;
                configDoPedido.Ordenacao = currentModel.Ordenacao;

                UtilNavidate.PopAsync();
            });


            GoToCategoriaCommand = new Command(() =>
            {
                var page = new PageBasicList(categoria, lCategoria, "Categoria de produto");
                UtilNavidate.PushModalAsync(page);
            });

            GoToRepresentacoesCommand = new Command(() =>
            {
                var page = new PageBasicList(representada, lRepresentacoes, "Representação");
                UtilNavidate.PushModalAsync(page);
            });

        }
        public Command EfetivarPesquisaCommand { get; set; }

        public bool bChangeOrdenacao { get; set; }

        private ConfiguracaoPesquisaProdutoModel _currentModel = new ConfiguracaoPesquisaProdutoModel();

        public ConfiguracaoPesquisaProdutoModel currentModel
        {
            get { return _currentModel; }
            set { _currentModel = value; NotifyPropertyChanged(); }
        }


        private List<ListItemModel> _lRepresentacoes = new List<ListItemModel>();

        public List<ListItemModel> lRepresentacoes
        {
            get { return _lRepresentacoes; }
            set
            {
                _lRepresentacoes = value;
                NotifyPropertyChanged();
            }
        }



        private List<ListItemModel> _lCategoria = new List<ListItemModel>();

        public List<ListItemModel> lCategoria
        {
            get { return _lCategoria; }
            set
            {
                _lCategoria = value;
                NotifyPropertyChanged();
            }
        }



        public void GetRepresentacao()
        {
            try
            {
                if (categoria != null && categoria.Id != 0)
                    lRepresentacoes =
                        new List<ListItemModel>(RepresentadaRepository.GetListItemModel(categoria.Id));
                else
                    lRepresentacoes =
                        new List<ListItemModel>(RepresentadaRepository.GetListItemModel());

                representada = lRepresentacoes.FirstOrDefault();
            }
            catch (Exception ex)
            {
                ex.TrakException();
            }

        }

        public void GetCategorias()
        {
            try
            {
                Device.BeginInvokeOnMainThread(() =>
                {
                    if (representada != null && representada.Id != 0)
                        lCategoria =
                            new List<ListItemModel>(CategoriaRepository.GetListItemModel(representada.Id));
                    else
                        lCategoria = new List<ListItemModel>(CategoriaRepository.GetListItemModel(0));

                    categoria = lCategoria.FirstOrDefault();
                });

            }
            catch (Exception ex)
            {
                ex.TrakException();
            }

        }



        public bool Initialize()
        {
            if (canExecuteInicial)
            {
                Device.BeginInvokeOnMainThread(() =>
                {
                    canExecuteInicial = false;
                    IsBusy = true;

                    GetRepresentacao();
                    GetCategorias();
                    IsBusy = false;
                });
            }
            return canExecuteInicial;
        }


    }
}

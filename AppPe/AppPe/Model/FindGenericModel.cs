using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Windows.Input;
using Xamarin.Forms;
using Xamarin.HLP.Mobile.AppPE.Common;
using Xamarin.HLP.Mobile.AppPE.View.ListPopup;

namespace Xamarin.HLP.Mobile.AppPE.Model
{
    public class FindGenericModel : ModelComum
    {
        public Action ActionToSelectedChanged { get; set; }

        public ICommand OpenFindCommand { get; set; }
        public ICommand LimparRegistroCommand { get; set; }


        public FindGenericModel()
        {
            LimparRegistroCommand = new Command(() => { SelectedItem = null; });
        }

        public FindGenericModel(string title, string image)
        {
            Image = image;
            Display = title;
            Detail = "clique aqui para pesquisar";
        }

        public bool? ModalisOpenning { get; set; } = null;

        public FindGenericModel(List<BasicPickerModel> collection, int? currentId, string title = null, string image = null, Action acrionToSelectedChanged = null, bool JustAtivo = false)
        {
            ActionToSelectedChanged = acrionToSelectedChanged;
            Display = title;
            Image = image;
            registrosToSearchAll = collection;
            RegistrosToSearch = JustAtivo
            ? collection.Where(c => c.stAtivo).ToList()
            : collection;

            SetId(currentId);
            OpenFindCommand = new Command(ShowPageToFind);
            Detail = SelectedItem.Display;
            LimparRegistroCommand = new Command(ClearRegistro);
        }

        public void ClearRegistro()
        {
            SelectedItem = new BasicPickerModel();
            SelectedItem.IsSelected = false;
        }

        private string _display;
        public string Display
        {
            get { return string.IsNullOrEmpty(_display) == false ? _display.ToUpper() : "SEM TÍTULO"; }
            set { _display = value; NotifyPropertyChanged(); }
        }


        private string _detail;
        public string Detail
        {
            get { return _detail; }
            set { _detail = value; NotifyPropertyChanged(); }
        }

        private string _image = "Xamarin.HLP.Mobile.AppPE.Images.MenuIcon.ApplicationArrowRightMenuDefault.svg";
        public string Image
        {
            get { return _image; }
            set
            {
                if (!string.IsNullOrEmpty(value))
                    value = value.ToPathSvg();
                _image = value; NotifyPropertyChanged();
            }
        }

        private void ShowPageToFind()
        {
            bool bCanExecute = ModalisOpenning ?? false;
            if (bCanExecute == false)
            {
                if (registrosToSearchAll.Any())
                {
                    if (ModalisOpenning != null)
                        ModalisOpenning = true;
                    Listar();
                    try
                    {
                        var pageToFindGeneric = new PageFindGeneric(this);
                        UtilNavidate.PushAsync(pageToFindGeneric);
                    }
                    catch (Exception ex)
                    {
                        ex.TrakException();
                    }
                }
            }
        }

        private IEnumerable<Group<string, BasicPickerModel>> _registrosAgrupados;
        public IEnumerable<Group<string, BasicPickerModel>> RegistrosAgrupados
        {
            get { return _registrosAgrupados; }
            set { _registrosAgrupados = value; NotifyPropertyChanged(); }
        }

        private List<BasicPickerModel> _registrosToSearch;
        public List<BasicPickerModel> RegistrosToSearch
        {
            get { return _registrosToSearch; }
            set { _registrosToSearch = value; NotifyPropertyChanged(); }
        }

        private List<BasicPickerModel> _registrosToSearchAll;

        /// <summary>
        /// Propriedade só sera usada para controlar ativos e inativos
        /// </summary>
        public List<BasicPickerModel> registrosToSearchAll
        {
            get { return _registrosToSearchAll; }
            set { _registrosToSearchAll = value; NotifyPropertyChanged(); }
        }

        private BasicPickerModel _selectedItem = new BasicPickerModel();
        public BasicPickerModel SelectedItem
        {
            get { return _selectedItem; }
            set
            {
                try
                {


                    if (value == _selectedItem)
                        return;

                    if (value != null)
                        if ((_selectedItem != null) && (value != _selectedItem))
                            _selectedItem.IsSelected = false;

                    if (value != null)
                    {
                        value.IsSelected = true;
                        Detail = value.Display;
                    }

                    _selectedItem = value;
                    NotifyPropertyChanged();
                    ActionToSelectedChanged?.Invoke();
                }
                catch (Exception ex)
                {
                    ex.TrakException();
                }
            }
        }


        public int? GetId()
        {
            if (SelectedItem == null || SelectedItem.Id == 0)
                return null;
            return SelectedItem.Id;
        }

        public void SetId(int? id)
        {
            if (id == null) return;
            var item = registrosToSearchAll.FirstOrDefault(c => c.Id == id);
            if (item != null)
                SelectedItem = item;
        }

        public void Listar(string filtro = "")
        {
            ItensFiltrados = new List<BasicPickerModel>(this.RegistrosToSearch);
            if (!string.IsNullOrEmpty(filtro))
                ItensFiltrados = this.RegistrosToSearch.Where(l => (l.Display.ToLower().Contains(filtro.ToLower())) || l.Detail.ToLower().Contains(filtro.ToLower())).ToList();

            this.RegistrosAgrupados = (from item in ItensFiltrados
                                       orderby item.Display
                                       group item by item.DisplayGroup into grupos
                                       select new Group<string, BasicPickerModel>(grupos.Key, grupos)).ToList();


        }



        private List<BasicPickerModel> _ItensFiltrados = new List<BasicPickerModel>();
        public List<BasicPickerModel> ItensFiltrados
        {
            get { return _ItensFiltrados; }
            set { _ItensFiltrados = value; NotifyPropertyChanged(); }
        }

        private ObservableCollection<BasicPickerModel> _ItensToDisplay = new ObservableCollection<BasicPickerModel>();

        public ObservableCollection<BasicPickerModel> ItensToDisplay
        {
            get { return _ItensToDisplay; }
            set { _ItensToDisplay = value; NotifyPropertyChanged(); }
        }


        public int index { get; set; }

        private bool _isBusy = false;
        public bool IsBusy
        {
            get { return _isBusy; }
            set { _isBusy = value; NotifyPropertyChanged(); }
        }

        public void Refresh()
        {
            if (ItensFiltrados.Count != ItensToDisplay.Count)
            {
                if (!IsBusy) IsBusy = true;

                if (ItensFiltrados.Any())
                {
                    for (var i = 0; i < 20; i++)
                    {
                        try
                        {
                            var item = ItensFiltrados[index];
                            if (item != null)
                            {
                                ItensToDisplay.Add(item);
                            }
                            index++;
                        }
                        catch (Exception ex)
                        {
                            continue;
                        }
                    }
                }

            }
        }

        private string _filtro = "";
        public string Filtro
        {
            get { return _filtro; }
            set
            {
                _filtro = value;
                NotifyPropertyChanged();
            }
        }


        public bool RemoverItem(int id)
        {
            try
            {
                var itemtoSearchAll = registrosToSearchAll.FirstOrDefault(c => c.Id == id);
                registrosToSearchAll.Remove(itemtoSearchAll);

                var itemtoSearch = RegistrosToSearch.FirstOrDefault(c => c.Id == id);
                RegistrosToSearch.Remove(itemtoSearch);

                Filtro = "";

                return true;
            }
            catch (Exception)
            {
                return false;
            }
        }

    }
}

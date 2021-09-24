using System;
using System.Windows.Input;
using Xamarin.Forms;

namespace Xamarin.HLP.Mobile.AppPE.Controls.xaml
{
    public partial class SearchPE : Grid
    {
        public SearchPE()
        {
            InitializeComponent();
            txtFiltro.Completed += (sender, e) => { ExecuteButtom(); };
            txtFiltro.IsEnabled = btnExecutePesquisa.IsEnabled = IsEnabled;
            btnExecutePesquisa.Command = new Command(ExecuteButtom);
        }

        public Entry GetEntry()
        {
            return txtFiltro;
        }

        private string _Placeholder;

        public string Placeholder
        {
            get { return _Placeholder; }
            set
            {
                _Placeholder = value;
                txtFiltro.Placeholder = value;
            }
        }




        public static BindableProperty TextProperty =
         BindableProperty.Create<SearchPE, string>(bp => bp.Text, default(string));

        public string Text
        {
            get { return (string)this.GetValue(TextProperty); }
            set { this.SetValue(TextProperty, value); }
        }



        public static BindableProperty SearchCommandProperty =
           BindableProperty.Create<SearchPE, ICommand>(bp => bp.SearchCommand, default(ICommand));


        public ICommand SearchCommand
        {
            get { return (ICommand)this.GetValue(SearchCommandProperty); }
            set
            {
                this.SetValue(SearchCommandProperty, value);

            }
        }

        private void Entry_OnTextChanged(object sender, TextChangedEventArgs e)
        {
            Text = txtFiltro.Text;
        }


        public void ExecuteButtom()
        {
            try
            {
                SearchCommand?.Execute(null);
            }
            catch (Exception ex)
            {
                ex.TrakException();
            }
        }

        //private void BtnExecutePesquisa_OnClicked(object sender, EventArgs e)
        //{
        //    try
        //    {
        //        SearchCommand?.Execute(null);
        //    }
        //    catch (Exception ex)
        //    {
        //        ex.TrakException();
        //    }

        //}



    }
}

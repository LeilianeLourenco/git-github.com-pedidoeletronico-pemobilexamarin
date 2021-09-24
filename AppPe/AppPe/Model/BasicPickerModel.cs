using System;
using Xamarin.Forms;
using Xamarin.HLP.Mobile.AppPE.Common;

namespace Xamarin.HLP.Mobile.AppPE.Model
{
    public class BasicPickerModel : ModelComum
    {
        /// <summary>
        /// porcentagem de desconto da condição
        /// </summary>
        public double? vDescCondicao { get; set; }

        private const string xDisplay = "clique aqui para pesquisar";

        private string _display = "";
        public string Display
        {
            get { return !string.IsNullOrEmpty(_display) ? _display.ToUpper() : xDisplay; }
            set { _display = value; NotifyPropertyChanged(); }
        }

        private string _display2 = "";
        public string Display2
        {
            get { return _display2; }
            set { _display2 = value; NotifyPropertyChanged(); }
        }

        private bool _stAtivo = true;

        public bool stAtivo
        {
            get { return _stAtivo; }
            set { _stAtivo = value; }
        }


        private string _display3 = "";

        public string Display3
        {
            get { return _display3; }
            set { _display3 = value; NotifyPropertyChanged(); }
        }

        private string _detail = "";
        public string Detail
        {
            get { return _detail; }
            set { _detail = value; base.NotifyPropertyChanged(); }
        }


        private DateTime? _date;
        public DateTime? Date
        {
            get { return _date; }
            set
            {
                _date = value; NotifyPropertyChanged();
                iDate = (value ?? DateTime.Now).ToIntFull();
            }
        }

        private int _iDate;

        public int iDate
        {
            get { return _iDate; }
            set { _iDate = value; NotifyPropertyChanged(); }
        }

        private DateTime? _date2;
        public DateTime? Date2
        {
            get { return _date2; }
            set
            {
                _date2 = value; NotifyPropertyChanged();
                iDate2 = (value ?? DateTime.Now).ToIntFull();
            }
        }

        private int _iDate2;

        public int iDate2
        {
            get { return _iDate2; }
            set { _iDate2 = value; NotifyPropertyChanged(); }
        }

        private byte _status;
        public byte status
        {
            get { return _status; }
            set { _status = value; NotifyPropertyChanged(); }
        }

        private string _image = "";

        public string Image
        {
            get { return _image; }
            set
            {
                if (value != null)
                    _image = value.ToPathSvgPage();
                NotifyPropertyChanged();
            }
        }

        public object ImagemSync
        {
            get
            {
                if (bTrazerImagem)
                {
                    if (bProblemaSincronizacao)
                    {
                        return "StatusSincronismo_EstoqueInvalido".ToImagemPNG();
                    }
                    return IdOnline != null
                        ? "StatusSincronismo_Sincronizado".ToImagemPNG()
                        : "StatusSincronismo_NaoSincronizado".ToImagemPNG();
                }
                return false;
            }
        }


        public bool bTrazerImagem { get; set; } = true;

        private bool _bProblemaSincronizacao;

        public bool bProblemaSincronizacao
        {
            get { return _bProblemaSincronizacao; }
            set { _bProblemaSincronizacao = value; NotifyPropertyChanged(); }
        }




        private int? _IdOnline;

        public int? IdOnline
        {
            get { return _IdOnline; }
            set
            {
                _IdOnline = value;
                ColorDisplay = (value ?? 0) > 0 ? ColorStaticModel.CinzaPrincipal : ColorStaticModel.Rosa;
                NotifyPropertyChanged();
            }
        }


        private int _id;
        public int Id
        {
            get { return _id; }
            set
            {
                _id = value;
                base.NotifyPropertyChanged();
            }
        }


        private string _xid;
        public string XId
        {
            get { return _xid; }
            set { _xid = value; base.NotifyPropertyChanged(); }
        }

        public string DisplayGroup => this.Display[0].ToString().IsNumber() ? "#" : this.Display[0].ToString();


        private bool _isSelected;
        public bool IsSelected
        {
            get { return _isSelected; }
            set
            {
                _isSelected = value; NotifyPropertyChanged();
            }
        }

        private Color _colorDisplay = ColorStaticModel.Rosa;
        public Color ColorDisplay
        {
            get { return _colorDisplay; }
            set { _colorDisplay = value; NotifyPropertyChanged(); }
        }

        private Color _colorDetail = ColorStaticModel.AzulPrincipal;
        public Color ColorDetail
        {
            get { return _colorDetail; }
            set { _colorDetail = value; NotifyPropertyChanged(); }
        }


    }
}


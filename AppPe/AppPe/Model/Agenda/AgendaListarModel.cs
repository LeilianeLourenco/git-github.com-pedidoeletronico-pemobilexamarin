using System;
using System.Globalization;
using Xamarin.Forms;
using Xamarin.HLP.Mobile.AppPE.Common;

namespace Xamarin.HLP.Mobile.AppPE.Model.Agenda
{
    public class AgendaListarModel : ModelComum
    {
        #region Listar novo
        public int idAtividadeOffline { get; set; }

        private int? _idAtividade;
        public int? idAtividade
        {
            get { return _idAtividade; }
            set
            {
                _idAtividade = value; NotifyPropertyChanged();
            }
        }

        private int? _idClienteOffLine;
        public int? idClienteOffline
        {
            get { return _idClienteOffLine; }
            set
            {
                _idClienteOffLine = value; NotifyPropertyChanged();
            }
        }


        private string _xDescricaoAtividade;
        public string xDescricaoAtividade
        {
            get { return _xDescricaoAtividade; }
            set
            {
                _xDescricaoAtividade = value; NotifyPropertyChanged();
            }
        }

        private string _xVendedorVinculado;
        public string xVendedorVinculado
        {
            get { return _xVendedorVinculado; }
            set
            {
                _xVendedorVinculado = value; NotifyPropertyChanged();
            }
        }

        private string _xListaVendedores;
        public string xListaVendedores
        {
            get { return _xListaVendedores; }
            set
            {
                _xListaVendedores = value; NotifyPropertyChanged();
            }
        }

        private string _xEnderecoCompleto;
        public string xEnderecoCompleto
        {
            get { return _xEnderecoCompleto; }
            set
            {
                _xEnderecoCompleto = value; NotifyPropertyChanged();
            }
        }

        private DateTime? _dtInicioEvento;
        public DateTime? dtInicioEvento
        {
            get { return _dtInicioEvento; }
            set { _dtInicioEvento = value; NotifyPropertyChanged(); }
        }


        private DateTime? _dtFimEvento;
        public DateTime? dtFimEvento
        {
            get { return _dtFimEvento; }
            set { _dtFimEvento = value; NotifyPropertyChanged(); }
        }

        public string DisplaySincronizacao
        {
            get
            {
                return idAtividade != null && idAtividade > 0 ? "Sincronizado com sucesso" : "Não sincronizado";
            }
        }

        public Color ColorSincronizacao
        {
            get
            {
                if (idAtividade != null && idAtividade > 0)
                {
                    return ColorStaticModel.AzulPrincipal;
                }
                return ColorStaticModel.CinzaClaro;
            }
        }

        public string xNomeAtividade { get; set; }
        public string xAnotacaoPrivada { get; set; }

        public string xNomeCliente { get; set; }

        public string xTelefoneCliente { get; set; }
        public string xCor { get; set; }


        private bool _bEventoCancelado;
        public bool bEventoCancelado
        {
            get { return _bEventoCancelado; }
            set
            {
                _bEventoCancelado = value; NotifyPropertyChanged();
            }
        }

        private bool _bRealizado;
        public bool bRealizado
        {
            get { return _bRealizado; }
            set { _bRealizado = value; NotifyPropertyChanged(); }
        }

        private bool _bVerificaLocalizacao;
        public bool bVerificaLocalizacao
        {
            get { return _bVerificaLocalizacao; }
            set { _bVerificaLocalizacao = value; NotifyPropertyChanged(); }
        }

        private string _xAtraso;
        public string xAtraso
        {
            get { return _xAtraso; }
            set { _xAtraso = value; NotifyPropertyChanged(); }
        }



        private string _xPeriodoEvento;
        public string xPeriodoEvento
        {
            get
            {
                if (string.IsNullOrEmpty(_xPeriodoEvento))
                {
                    _xPeriodoEvento = $"{dtInicioEvento?.ToString("dd/MM/yyyy HH:mm")} até {dtFimEvento?.ToString("dd/MM/yyyy HH:mm")}";
                }
                return _xPeriodoEvento;
            }
            set { _xPeriodoEvento = value; NotifyPropertyChanged(); }
        }

        public Color ColorAtividade
        {
            get
            {
                switch (xCor)
                {
                    case "teal":
                        return Color.FromHex("39bbb0");
                    case "red":
                        return Color.FromHex("ff6b68");
                    case "purple":
                        return Color.FromHex("d066e2");
                    case "amber":
                        return Color.FromHex("ffc721");
                    case "blue":
                        return Color.FromHex("2196F3");
                    case "green":
                        return Color.FromHex("32c787");
                    case "cyan":
                        return Color.FromHex("00BCD4");
                    case "gray":
                        return Color.FromHex("868e96");
                    case "gray-dark":
                        return Color.FromHex("343a40");
                    case "black":
                        return Color.FromHex("000000");
                    case "pink":
                        return Color.FromHex("ff85af");
                    case "deep-purple":
                        return Color.FromHex("673AB7");
                    case "indigo":
                        return Color.FromHex("3F51B5");
                    case "light-blue":
                        return Color.FromHex("03A9F4");
                    case "light-green":
                        return Color.FromHex("8BC34A");
                    case "lime":
                        return Color.FromHex("CDDC39");
                    case "yellow":
                        return Color.FromHex("FFEB3B");
                    case "orange":
                        return Color.FromHex("FF9800");
                    case "deep-orange":
                        return Color.FromHex("FF5722");
                    case "brown":
                        return Color.FromHex("795548");
                    case "blue-grey":
                        return Color.FromHex("607D8B");
                    case "primary":
                        return Color.FromHex("2196F3");
                    case "secondary":
                        return Color.FromHex("868e96");
                    case "success":
                        return Color.FromHex("32c787");
                    case "info":
                        return Color.FromHex("03A9F4");
                    case "warning":
                        return Color.FromHex("ffc721");
                    case "danger":
                        return Color.FromHex("ff6b68");
                    case "dark":
                        return Color.FromHex("495057");
                    default:
                        return Color.FromHex("03A9F4");
                }
            }
        }



        private Color _ColorPendencia;
        public Color ColorPendencia
        {
            get { return _ColorPendencia; }
            set { _ColorPendencia = value; NotifyPropertyChanged(); }
        }

        public DateTime _dtInicioCheck;
        public DateTime dtInicioCheck
        {
            get { return _dtInicioCheck; }
            set { _dtInicioCheck = value; NotifyPropertyChanged(); }
        }

        public string _xTempoCheck;
        public string xTempoCheck
        {
            get
            {
                _xTempoCheck = _dtTempoCheck.ToString(@"hh\:mm\:ss\.ff");
                return _xTempoCheck;
            }
            set { _xTempoCheck = value; NotifyPropertyChanged(); }
        }

        public TimeSpan _dtTempoCheck;
        public TimeSpan dtTempoCheck
        {
            get { return _dtTempoCheck; }
            set
            {
                _dtTempoCheck = value;
                xTempoCheck = value.ToString();
                NotifyPropertyChanged();
            }
        }

        public string _xLocalCheckIn;
        public string xLocalCheckIn
        {
            get { return _xLocalCheckIn; }
            set { _xLocalCheckIn = value; NotifyPropertyChanged(); }
        }

        public string _xLocalCheckOut;
        public string xLocalCheckOut
        {
            get { return _xLocalCheckOut; }
            set { _xLocalCheckOut = value; NotifyPropertyChanged(); }
        }

        private bool _bCheckConfirmado = false;
        public bool bCheckConfirmado
        {
            get { return _bCheckConfirmado; }
            set
            {
                _bCheckConfirmado = value;
                NotifyPropertyChanged();
            }
        }       

        #endregion
    }
}

using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Threading.Tasks;
using System.Windows.Input;
using VSRESTClient.Core.Http;
using VSRESTClient.Core.Utils;
using VSRESTClient.UI.Commands;
using VSRESTClient.UI.Models;
using VSRESTClient.UI.Utils;

namespace VSRESTClient.UI.ViewModels
{
    public class MainPageViewModel : INotifyPropertyChanged
    {
        #region Commands
        public ICommand UpdateHttpMethodCommand { get; set; }
        public ICommand UrlFocusCommand { get; set; }
        public ICommand UrlLostFocusCommand { get; set; } 
        public ICommand SwitchOptionsTab { get; set; }
        public ICommand SendRequestCommand { get; set; }
        #endregion

        #region Public Properties

        public string CurrentHttpMethod
        {
            get
            {
                return _searchbarModel.CurrentHttpMethod;
            }
            set
            {
                var method = ParseEnumFromString<SupportedHttpMethods>(value);

                _searchbarModel.SetCurrentHttpMethod(method);

                OnPropertyChanged(nameof(CurrentHttpMethod));
            }
        }

        public string CurrentOptionsTabOpened 
        {
            get 
            {
                return _searchbarModel.CurrentOptionsTab.ToString();
            }
            set 
            {
                var convertedValue = ParseEnumFromString<OptionsPage>(value);

                _searchbarModel.SetCurrentOptionsTabOpened(convertedValue);

                OnPropertyChanged(nameof(CurrentOptionsTabOpened));
            }
        }

        public string Url
        {
            get
            {
                return _searchbarModel.RequestUrl;
            }
            set
            {
                _searchbarModel.SetUrl(value);

                OnPropertyChanged(nameof(Url));
            }
        }
        #endregion

        #region Events
        public event PropertyChangedEventHandler PropertyChanged = (sender, e) => { };

        #endregion

        #region Private Members
        private SearchbarModel _searchbarModel;
        private OptionsModel _optionsModel;
        private readonly WebClient _webClient;
        #endregion

        #region Constructor
        public MainPageViewModel()
        {
            UpdateHttpMethodCommand = new ParameterizedCommand(UpdateHttpMethodCallback);
            SwitchOptionsTab = new ParameterizedCommand(SwitchOptionsTabCallback);
            UrlFocusCommand = new RelayCommand(UrlFocusCallback);
            UrlLostFocusCommand = new RelayCommand(UrlLostFocusCallback);
            SendRequestCommand = new RelayCommand(async () => await SendRequestCallback());

            _searchbarModel = new SearchbarModel();
            _optionsModel = new OptionsModel();
            _webClient = new WebClient();

            CurrentHttpMethod = _searchbarModel.CurrentHttpMethod;
            Url = _searchbarModel.RequestUrl;
        }
        #endregion

        #region Public Methods
        public void OnPropertyChanged(string name)
        {
            PropertyChanged(this, new PropertyChangedEventArgs(name));
        } 
        public void AddParam(string name, string value)
        {
            _optionsModel.HttpParams.Add(new HttpParam(name, value));
        }
        public void FetchHttpParams(List<HttpParam> @params)
        {
            _optionsModel.HttpParams = @params;
        }
        public void RemoveParam(int index) 
        {
            _optionsModel.HttpParams.RemoveAt(index);
        }
        #endregion

        #region Callbacks

        private void UpdateHttpMethodCallback(object httpMethod)
        {
            CurrentHttpMethod = httpMethod as string;
        }
        public void SwitchOptionsTabCallback(object value)
        {
            CurrentOptionsTabOpened = value as string;
        }
        private void UrlFocusCallback()
        {
            if (Url.Equals(StaticStrings.DefaultUrl))
                Url = string.Empty;
        }
        private void UrlLostFocusCallback()
        {
            if (Url.Equals(string.Empty))
                Url = StaticStrings.DefaultUrl;
        }
        private async Task SendRequestCallback()
        {
            await _webClient.SendRequest(Url);
        }

        #endregion

        #region Helpers
        private T ParseEnumFromString<T>(string @string) where T : struct
        {
            Enum.TryParse<T>(@string, out var result);

            return result;
        } 
        #endregion

    }
}

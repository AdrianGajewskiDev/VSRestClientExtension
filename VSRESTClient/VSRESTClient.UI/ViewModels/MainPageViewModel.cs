using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Net;
using System.Threading.Tasks;
using System.Windows.Input;
using System.Windows.Media;
using VSRESTClient.Core.Http;
using VSRESTClient.Core.Utils;
using VSRESTClient.UI.Builders;
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
        public string ResponseContent 
        {
            get => _responseModel.Content;
            set
            {
                _responseModel.Content = value;
                OnPropertyChanged(nameof(ResponseContent));
            }
        }
        public string ResponseStatusCode 
        {
            get => $"Status Code: {ResponseStatusCodeNumber} {_responseModel.StatusCode}";
            set
            {
                var statusCode = ParseEnumFromString<HttpStatusCode>(value);
                _responseModel.StatusCode = statusCode;
                OnPropertyChanged(nameof(ResponseStatusCode));
            }
        }
        public string ResponseContentType 
        {
            get => $"Content Type: {_responseModel.ContentType ?? ""}";

            set
            {
                _responseModel.ContentType = value;
                OnPropertyChanged(nameof(ResponseContentType));
            }
        }
        public int ResponseStatusCodeNumber => (int)_responseModel.StatusCode;

        #endregion

        #region Events
        public event PropertyChangedEventHandler PropertyChanged = (sender, e) => { };

        #endregion

        #region Private Members
        private SearchbarModel _searchbarModel;
        private OptionsModel _optionsModel;
        private ResponseModel _responseModel;
        private readonly Core.Http.WebClient _webClient;
        #endregion

        #region Constructor
        public MainPageViewModel()
        {
            UpdateHttpMethodCommand = new ParameterizedCommand(UpdateHttpMethodCallback);
            SwitchOptionsTab = new ParameterizedCommand(SwitchOptionsTabCallback);
            UrlFocusCommand = new RelayCommand(UrlFocusCallback);
            UrlLostFocusCommand = new RelayCommand(UrlLostFocusCallback);
#pragma warning disable VSTHRD101 // Avoid unsupported async delegates
            SendRequestCommand = new RelayCommand(async () => await SendRequestCallbackAsync());
#pragma warning restore VSTHRD101 // Avoid unsupported async delegates

            _searchbarModel = new SearchbarModel();
            _optionsModel = new OptionsModel();
            _responseModel = new ResponseModel();
            _webClient = new Core.Http.WebClient();

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
        private async Task SendRequestCallbackAsync()
        {
            if (string.IsNullOrEmpty(Url) || Url.Equals(StaticStrings.DefaultUrl))
                return;

            var builder = new UrlBuilder(Url);

            if (_optionsModel.HttpParams.Any())
            {
                foreach (var query in _optionsModel.HttpParams)
                {
                    builder.AddQueryParam(query);
                }
            }

            var url = builder.Build();

            var response = await _webClient.SendRequestAsync(new HttpRequest(url, _optionsModel.HttpParams, _optionsModel.Headers, _searchbarModel.HttpMethod));

            ResponseContent = response.Content;
            ResponseStatusCode = response.StatusCode.ToString();
            ResponseContentType = response.ContentType;
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

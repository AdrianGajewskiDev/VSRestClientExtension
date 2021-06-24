using Newtonsoft.Json;
using Newtonsoft.Json.Linq;
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
        public ICommand UpdateHAuthorizationTypeCommand { get; set; }
        public ICommand UpdateAuthorizationAttachMethodCommand { get; set; }
        public ICommand UpdateBodyContentTypeCommand { get; set; }
        public ICommand UrlFocusCommand { get; set; }
        public ICommand UrlLostFocusCommand { get; set; }
        public ICommand SwitchOptionsTab { get; set; }
        public ICommand SendRequestCommand { get; set; }
        #endregion

        #region Public Properties
        public string CurrentBodyContentType 
        {
            get => _bodyContentModel.BodyContentType;
            set 
            {
                _bodyContentModel.BodyContentType = value;

                OnPropertyChanged(nameof(CurrentBodyContentType));
            }
        }
        public string CurrentBodyContent
        {
            get => _bodyContentModel.Content;
            set
            {
                _bodyContentModel.Content = value;

                OnPropertyChanged(nameof(CurrentBodyContent));
            }
        }
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
        public string CurrentAuthorizationType
        {
            get
            {
                return _authorizationModel.AuthorizationType.ToString();
            }
            set
            {
                var type = ParseEnumFromString<AuthorizationType>(value);

                _authorizationModel.SetCurrentAuthorizationType(type);
                
                ResetHeaders();

                OnPropertyChanged(nameof(CurrentAuthorizationType));
            }
        }
        private void ResetHeaders()
        {
            switch (CurrentAuthorizationType)
            {
                case "Bearer":
                    {
                        var authorizationHeader = _optionsModel.Headers.FirstOrDefault(x => x.Name == "Authorization");
                        _optionsModel.Headers.Clear();

                        if (authorizationHeader != null)
                            _optionsModel.Headers.Add(authorizationHeader);
                    }
                    break;
                case "BasicAuth": 
                    {
                        var username = _optionsModel.Headers.FirstOrDefault(x => x.Name == "Username");
                        var password = _optionsModel.Headers.FirstOrDefault(x => x.Name == "Password");
                        _optionsModel.Headers.Clear();

                        if (username != null)
                            _optionsModel.Headers.Add(username);

                        if (password != null)
                            _optionsModel.Headers.Add(password);
                    }
                    break;
                case "ApiKey":
                    {
                        var name = _optionsModel.Headers.FirstOrDefault(x => x.Name == CurrentApiKeyAuthorizationHeaderOrParamName);
                        var value = _optionsModel.Headers.FirstOrDefault(x => x.Name == CurrentApiKeyAuthorizationHeaderOrParamValue);
                        _optionsModel.Headers.Clear();

                        if (name != null)
                            _optionsModel.Headers.Add(name);

                        if (value != null)
                            _optionsModel.Headers.Add(value);
                    }
                    break;
                case "NoAuth":
                    {
                        _optionsModel.Headers.Clear();
                    }
                    break;
            }
          
        }
        public string CurrentAuthorizationAttachMethod
        {
            get
            {
                return _authorizationModel.AuthorizationAttachMethod.ToString();
            }
            set
            {
                var type = ParseEnumFromString<AuthorizationAttachMethod>(value);

                _authorizationModel.SetAuthorizationAttachMethod(type);

                OnPropertyChanged(nameof(CurrentAuthorizationAttachMethod));
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
                OnPropertyChanged(nameof(TextResponse));
                OnPropertyChanged(nameof(ImageResponse));
                OnPropertyChanged(nameof(HtmlResponse));
            }
        }
        public bool TextResponse 
        {
            get 
            {
                if (_responseModel.ContentType != null)
                    return (_responseModel.ContentType.Contains("text") || _responseModel.ContentType.Contains("json")) && !_responseModel.ContentType.Contains("html");
                else
                    return false;
            }
            set
            {

            }
        }
        public bool ImageResponse 
        {
            get
            {
                if (_responseModel.ContentType != null)
                    return _responseModel.ContentType.Contains("image");
                else
                    return false;
            }
        }
        public bool HtmlResponse 
        {
            get
            {
                if (_responseModel.ContentType != null)
                    return _responseModel.ContentType == "text/html";
                else
                    return false;
            }
        }
        public string CurrentBasicAuthorizationHeaderOrParamName
        {
            get => _authorizationModel.BasicAuthorizationHeaderOrParamName;
            set
            {
                _authorizationModel.BasicAuthorizationHeaderOrParamName = value;
                OnPropertyChanged(nameof(CurrentBasicAuthorizationHeaderOrParamName));
            }
        }
        public string CurrentBasicAuthorizationHeaderOrParamValue
        {
            get => _authorizationModel.BasicAuthorizationHeaderOrParamValue;
            set
            {
                _authorizationModel.BasicAuthorizationHeaderOrParamValue = value;
                OnPropertyChanged(nameof(CurrentBasicAuthorizationHeaderOrParamValue));
            }
        }
        public string CurrentApiKeyAuthorizationHeaderOrParamName
        {
            get => _authorizationModel.ApiKeyAuthorizationHeaderOrParamName;
            set
            {
                _authorizationModel.ApiKeyAuthorizationHeaderOrParamName = value;
                OnPropertyChanged(nameof(CurrentApiKeyAuthorizationHeaderOrParamName));
            }
        }
        public string CurrentApiKeyAuthorizationHeaderOrParamValue
        {
            get => _authorizationModel.ApiKeyAuthorizationHeaderOrParamValue;
            set
            {
                _authorizationModel.ApiKeyAuthorizationHeaderOrParamValue = value;
                OnPropertyChanged(nameof(CurrentApiKeyAuthorizationHeaderOrParamValue));
            }
        }
        public string JWTToken
        {
            get => _authorizationModel.Token;
            set
            {
                _authorizationModel.Token = value;
                OnPropertyChanged(nameof(JWTToken));
            }
        }
        public int ResponseStatusCodeNumber => (int)_responseModel.StatusCode;
        public List<Action> PrerequestActions = new List<Action>();
        #endregion

        #region Events
        public event PropertyChangedEventHandler PropertyChanged = (sender, e) => { };

        #endregion

        #region Private Members
        private SearchbarModel _searchbarModel;
        private OptionsModel _optionsModel;
        private ResponseModel _responseModel;
        private AuthorizationModel _authorizationModel;
        private BodyContentModel _bodyContentModel;
        private readonly Core.Http.WebClient _webClient;
        #endregion

        #region Constructor
        public MainPageViewModel()
        {
            UpdateHttpMethodCommand = new ParameterizedCommand(UpdateHttpMethodCallback);
            UpdateHAuthorizationTypeCommand = new ParameterizedCommand(UpdateAuthorizationTypeCallback);
            UpdateAuthorizationAttachMethodCommand = new ParameterizedCommand(UpdateAuthorizationAttachMethodCallback);
            UpdateBodyContentTypeCommand = new ParameterizedCommand(UpdateBodyContentTypeCallback);
            SwitchOptionsTab = new ParameterizedCommand(SwitchOptionsTabCallback);
            UrlFocusCommand = new RelayCommand(UrlFocusCallback);
            UrlLostFocusCommand = new RelayCommand(UrlLostFocusCallback);
#pragma warning disable VSTHRD101 // Avoid unsupported async delegates
            SendRequestCommand = new RelayCommand(async () => await SendRequestCallbackAsync());
#pragma warning restore VSTHRD101 // Avoid unsupported async delegates

            _searchbarModel = new SearchbarModel();
            _optionsModel = new OptionsModel();
            _responseModel = new ResponseModel();
            _authorizationModel = new AuthorizationModel();
            _webClient = new Core.Http.WebClient();
            _bodyContentModel = new BodyContentModel();

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
            foreach (var param in @params)
            {
                if (!_optionsModel.HttpParams.Any(x => x.Name == param.Name))
                {
                    _optionsModel.HttpParams.Add(param);
                }
            }
        }
        public void FetchHttpHeaders(List<HttpHeader> @params)
        {
            foreach (var param in @params)
            {
                if (!_optionsModel.Headers.Any(x => x.Name == param.Name))
                {
                    _optionsModel.Headers.Add(param);
                }
            }
        }
        public void RemoveParam(int index)
        {
            _optionsModel.HttpParams.RemoveAt(index);
        }
        public void AddPreRequestAction(Action action)
        {
            PrerequestActions.Add(action);
        }
        #endregion

        #region Callbacks

        private void UpdateHttpMethodCallback(object httpMethod)
        {
            CurrentHttpMethod = httpMethod as string;
        }
        private void UpdateAuthorizationTypeCallback(object parameter)
        {
            CurrentAuthorizationType = parameter as string;
        }
        private void UpdateAuthorizationAttachMethodCallback(object parameter)
        {
            CurrentAuthorizationAttachMethod = parameter as string;
        }
        private void UpdateBodyContentTypeCallback(object parameter)
        {
            CurrentBodyContentType = parameter as string;
        }
        private void SwitchOptionsTabCallback(object value)
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

            if (PrerequestActions.Any())
            {
                foreach (var action in PrerequestActions)
                {
                    action();
                }
            }

            var builder = new UrlBuilder(Url);

            if (_optionsModel.HttpParams.Any())
            {
                foreach (var query in _optionsModel.HttpParams)
                {
                    builder.AddQueryParam(query);
                }
            }

            switch (_authorizationModel.AuthorizationType)
            {
                case AuthorizationType.Bearer:
                    if (!_optionsModel.Headers.Any(x => x.Name == "Authorization"))
                        _optionsModel.Headers.Add(new HttpHeader("Authorization", $"Bearer {JWTToken}"));
                    break;
                case AuthorizationType.ApiKey:
                    {
                            switch (CurrentAuthorizationAttachMethod)
                            {
                                case "Headers":
                                    {
                                        var indexOfApiKeyName = _optionsModel.Headers.IndexOf(_optionsModel.Headers.FirstOrDefault(x => x.Name == CurrentApiKeyAuthorizationHeaderOrParamName));

                                        if(indexOfApiKeyName >= 0)
                                            _optionsModel.Headers[indexOfApiKeyName] = new HttpHeader(CurrentApiKeyAuthorizationHeaderOrParamName, CurrentApiKeyAuthorizationHeaderOrParamValue);
                                        else
                                            _optionsModel.Headers.Add(new HttpHeader(CurrentApiKeyAuthorizationHeaderOrParamName, CurrentApiKeyAuthorizationHeaderOrParamValue));
                                    }
                                    break;
                                case "QueryParams":
                                    {
                                        builder.AddQueryParam(new HttpParam(CurrentApiKeyAuthorizationHeaderOrParamName, CurrentApiKeyAuthorizationHeaderOrParamValue));
                                    }
                                    break;
                            }
                    }
                    break;
                case AuthorizationType.BasicAuth:

                    switch (CurrentAuthorizationAttachMethod)
                    {
                        case "Headers":
                            {
                                var indexOfUsername = _optionsModel.Headers.IndexOf(_optionsModel.Headers.FirstOrDefault(x => x.Name == "Username"));
                                var indexOfPassword = _optionsModel.Headers.IndexOf(_optionsModel.Headers.FirstOrDefault(x => x.Name == "Password"));

                                if (indexOfUsername >= 0)
                                    _optionsModel.Headers[indexOfUsername] = new HttpHeader("Username", CurrentBasicAuthorizationHeaderOrParamName);
                                else
                                    _optionsModel.Headers.Add(new HttpHeader("Username", CurrentBasicAuthorizationHeaderOrParamName));


                                if(indexOfPassword >= 0)
                                    _optionsModel.Headers[indexOfPassword] = new HttpHeader("Password", CurrentBasicAuthorizationHeaderOrParamValue);

                                else
                                    _optionsModel.Headers.Add(new HttpHeader("Password", CurrentBasicAuthorizationHeaderOrParamValue));
                            }
                            break;
                        case "QueryParams":
                            {
                                builder.AddQueryParam(new HttpParam("Username", CurrentBasicAuthorizationHeaderOrParamName));
                                builder.AddQueryParam(new HttpParam("Password", CurrentBasicAuthorizationHeaderOrParamValue));
                            }
                            break;
                    }
                    break;

                case AuthorizationType.NoAuth: 
                    {
                        ResetHeaders();
                    }break;
            }

            var url = builder.Build();

            var response = await _webClient.SendRequestAsync(new HttpRequest(url, _optionsModel.HttpParams, _optionsModel.Headers, _searchbarModel.HttpMethod, CurrentBodyContent, CurrentBodyContentType));

            var formatedContent = Beautifier.Instance.Format(response.Content, response.ContentType);

            ResponseContent = formatedContent;
            ResponseStatusCode = response.StatusCode.ToString();
            ResponseContentType = response.ContentType;

            ResetHeaders();
            ResetParams();
        }
        private void ResetParams()
        {
            _optionsModel.HttpParams.Clear();
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

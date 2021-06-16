using System;
using System.ComponentModel;
using System.Windows.Input;
using VSRESTClient.UI.Commands;
using VSRESTClient.UI.Models;
using VSRESTClient.UI.Utils;

namespace VSRESTClient.UI.ViewModels
{
    public class SearchbarViewModel : INotifyPropertyChanged
    {
        public ICommand UpdateHttpMethodCommand { get; set; }
        public ICommand UrlFocusCommand { get; set; }
        public ICommand UrlLostFocusCommand { get; set; }

        public string CurrentHttpMethod { 
            get 
            {
                return _searchbarModel.CurrentHttpMethod;
            } 
            set 
            {
                var method = ParseEnumFromString(value);

                _searchbarModel.SetCurrentHttpMethod(method);

                OnPropertyChanged(nameof(CurrentHttpMethod));
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

        public event PropertyChangedEventHandler PropertyChanged = (sender, e) => { };

        private SearchbarModel _searchbarModel;

        public SearchbarViewModel()
        {
            UpdateHttpMethodCommand = new ParameterizedCommand(UpdateHttpMethodCallback);
            UrlFocusCommand = new RelayCommand(UrlFocusCallback);
            UrlLostFocusCommand = new RelayCommand(UrlLostFocusCallback);

            _searchbarModel = new SearchbarModel();

            CurrentHttpMethod = _searchbarModel.CurrentHttpMethod;
            Url = _searchbarModel.RequestUrl;
        }
        public void OnPropertyChanged(string name)
        {
            PropertyChanged(this, new PropertyChangedEventArgs(name));
        }


        private void UpdateHttpMethodCallback(object httpMethod)
        {
            CurrentHttpMethod = httpMethod as string;
        }

        private void UrlFocusCallback()
        {
            if(Url.Equals(StaticStrings.DefaultUrl))
                Url = string.Empty;
        }

        private void UrlLostFocusCallback()
        {
            if (Url.Equals(string.Empty))
                Url = StaticStrings.DefaultUrl;
        }

        private SupportedHttpMethods ParseEnumFromString(string @string)
        {
            Enum.TryParse<SupportedHttpMethods>(@string, out var result);

            return result;
        }

    }
}

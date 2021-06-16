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

        public event PropertyChangedEventHandler PropertyChanged = (sender, e) => { };

        private SearchbarModel _searchbarModel;

        public void OnPropertyChanged(string name)
        {
            PropertyChanged(this, new PropertyChangedEventArgs(name));
        }


        public SearchbarViewModel()
        {
            UpdateHttpMethodCommand = new ParameterizedCommand(UpdateHttpMethodCallback);
            _searchbarModel = new SearchbarModel();
            CurrentHttpMethod = _searchbarModel.CurrentHttpMethod;
        }


        private void UpdateHttpMethodCallback(object httpMethod)
        {
            CurrentHttpMethod = httpMethod as string;
        }

        private SupportedHttpMethods ParseEnumFromString(string @string)
        {
            Enum.TryParse<SupportedHttpMethods>(@string, out var result);

            return result;
        }

    }
}

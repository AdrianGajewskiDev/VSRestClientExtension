using System;
using System.Windows;

namespace VSRESTClient.UI.Builders
{
    public interface IControlBuilder<T> where T: UIElement
    {
        IControlBuilder<T> AddControl<TControl>(Action<TControl> configuration) where TControl : UIElement, new();
        IControlBuilder<T> AddConfiguration(Action<T> configuration);
        T Build();
    }
}

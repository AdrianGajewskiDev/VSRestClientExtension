using System;
using System.Windows;
using System.Windows.Controls;

namespace VSRESTClient.UI.Builders
{
    public class StackPanelBuilder : IControlBuilder<StackPanel>
    {
        private StackPanel _source;

        public StackPanelBuilder()
        {
            _source = new StackPanel();
        }

        public IControlBuilder<StackPanel> AddConfiguration(Action<StackPanel> configuration)
        {
            configuration(_source);

            return this;
        }

        public IControlBuilder<StackPanel> AddControl<TControl>(System.Action<TControl> configuration)
            where TControl : UIElement, new()
        {
            var control = new TControl();

            configuration(control);

            _source.Children.Add(control);

            return this;
        }

        public StackPanel Build()
        {
            return _source;
        }
    }
}

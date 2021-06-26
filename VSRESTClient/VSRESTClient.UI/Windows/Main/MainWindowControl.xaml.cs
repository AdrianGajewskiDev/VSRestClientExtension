using System;
using System.Collections.Generic;
using System.Diagnostics.CodeAnalysis;
using System.Drawing;
using System.Linq;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using VSRESTClient.Core.Utils;
using VSRESTClient.UI.Builders;
using VSRESTClient.UI.Utils;
using VSRESTClient.UI.ViewModels;

namespace VSRESTClient.UI.Windows.Main
{
    /// <summary>
    /// Interaction logic for MainWindowControl.
    /// </summary>
    public partial class MainWindowControl : UserControl
    {
        private readonly MainPageViewModel viewModel;

        public MainWindowControl()
        {
            this.InitializeComponent();
            viewModel = new MainPageViewModel();
            this.DataContext = viewModel;
            viewModel.AddPreRequestAction(FetchHttpHeaders);
            viewModel.AddPreRequestAction(FetchHttpParams);
        }


        [SuppressMessage("Microsoft.Globalization", "CA1300:SpecifyMessageBoxOptions", Justification = "Sample code")]
        [SuppressMessage("StyleCop.CSharp.NamingRules", "SA1300:ElementMustBeginWithUpperCaseLetter", Justification = "Default event handler naming pattern")]
        private void OpenContextMenu(object sender, RoutedEventArgs e)
        {
            (sender as Button).ContextMenu.IsEnabled = true;
            (sender as Button).ContextMenu.PlacementTarget = (sender as Button);
            (sender as Button).ContextMenu.Placement = System.Windows.Controls.Primitives.PlacementMode.Bottom;
            (sender as Button).ContextMenu.IsOpen = true;
        }
        private void OnUrlFocus(object sender, RoutedEventArgs e)
        {
            viewModel.UrlFocusCommand.Execute(sender);
        }
        private void OnUrlLostFocus(object sender, RoutedEventArgs e)
        {
            viewModel.UrlLostFocusCommand.Execute(sender);
        }
        private void OnKeyUp(object sender, System.Windows.Input.KeyEventArgs e)
        {
            viewModel.Url = (sender as TextBox).Text;
        }
        private void AddNewParamsControl(object sender, RoutedEventArgs e)
        {
            StackPanel panel = new StackPanelBuilder()
            .AddConfiguration(conf =>
            {
                conf.Orientation = Orientation.Horizontal;
                conf.Width = 330;
            })
            .AddControl<TextBox>(conf =>
            {
                conf.Text = "Name...";
                conf.Width = 150;
            })
            .AddControl<TextBox>(conf =>
            {
                conf.Text = "Value...";
                conf.Width = 150;
            })
            .AddControl<Button>(conf =>
            {
                conf.Width = 20;
                conf.Content = "X";
                conf.Foreground = System.Windows.Media.Brushes.Red;
                conf.Click += (ss, ee) =>
                {
                    ParamsListWrapper.Children.Remove(FindParent<StackPanel>(conf));
                };
                conf.Style = (Style)this.FindResource("MenuTabsButtons");
            })
            .Build();

            ParamsListWrapper.Children.Add(panel);
        }
        private void FetchHttpParams()
        {
            var childPanels = ParamsListWrapper.Children.OfType<StackPanel>();

            List<HttpParam> @params = new List<HttpParam>();

            foreach (var panel in childPanels)
            {
                var textboxes = panel.Children.OfType<TextBox>().ToArray();

                if (textboxes[0].Text != StaticStrings.DefaultName && !string.IsNullOrEmpty(textboxes[0].Text) && textboxes[1].Text != StaticStrings.DefaultValue && !string.IsNullOrEmpty(textboxes[1].Text))
                {
                    
                    @params.Add(new HttpParam(textboxes[0].Text, textboxes[1].Text));
                }
            }

            viewModel.FetchHttpParams(@params);
        }
        public T FindParent<T>(DependencyObject child) where T : DependencyObject
        {
            //get parent item
            DependencyObject parentObject = VisualTreeHelper.GetParent(child);

            //we've reached the end of the tree
            if (parentObject == null) return null;

            //check if the parent matches the type we're looking for
            T parent = parentObject as T;
            if (parent != null)
                return parent;
            else
                return FindParent<T>(parentObject);
        }
        private void AddNewHeadersControl(object sender, RoutedEventArgs e)
        {
            StackPanel panel = new StackPanelBuilder()
            .AddConfiguration(conf =>
            {
                conf.Orientation = Orientation.Horizontal;
                conf.Width = 330;
            })
            .AddControl<TextBox>(conf =>
            {
                conf.Text = "Name...";
                conf.Width = 150;
            })
            .AddControl<TextBox>(conf =>
            {
                conf.Text = "Value...";
                conf.Width = 150;
            })
            .AddControl<Button>(conf =>
            {
                conf.Width = 20;
                conf.Content = "X";
                conf.Foreground = System.Windows.Media.Brushes.Red;
                conf.Click += (ss, ee) =>
                {
                    HeadersListWrapper.Children.Remove(FindParent<StackPanel>(conf));
                };
                conf.Style = (Style)this.FindResource("MenuTabsButtons");
            })
            .Build();

            HeadersListWrapper.Children.Add(panel);
        }
        private void FetchHttpHeaders()
        {
            var childPanels = HeadersListWrapper.Children.OfType<StackPanel>();

            List<HttpHeader> @params = new List<HttpHeader>();

            foreach (var panel in childPanels)
            {
                var textboxes = panel.Children.OfType<TextBox>().ToArray();

                if (textboxes[0].Text != StaticStrings.DefaultName && !string.IsNullOrEmpty(textboxes[0].Text) && textboxes[1].Text != StaticStrings.DefaultValue && !string.IsNullOrEmpty(textboxes[1].Text))
                {
                    @params.Add(new HttpHeader(textboxes[0].Text, textboxes[1].Text));
                }
            }

            viewModel.FetchHttpHeaders(@params);
        }
    }
}
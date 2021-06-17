using System.Diagnostics.CodeAnalysis;
using System.Drawing;
using System.Windows;
using System.Windows.Controls;
using VSRESTClient.UI.ViewModels;

namespace VSRESTClient.UI.Windows.Main
{
    /// <summary>
    /// Interaction logic for MainWindowControl.
    /// </summary>
    public partial class MainWindowControl : UserControl
    {

        private readonly SearchbarViewModel viewModel;
        /// <summary>
        /// Initializes a new instance of the <see cref="MainWindowControl"/> class.
        /// </summary>
        public MainWindowControl()
        {
            this.InitializeComponent();
            viewModel = new SearchbarViewModel();
            this.DataContext = viewModel;
        }

        /// <summary>
        /// Handles click on the button by displaying a message box.
        /// </summary>
        /// <param name="sender">The event sender.</param>
        /// <param name="e">The event args.</param>
        [SuppressMessage("Microsoft.Globalization", "CA1300:SpecifyMessageBoxOptions", Justification = "Sample code")]
        [SuppressMessage("StyleCop.CSharp.NamingRules", "SA1300:ElementMustBeginWithUpperCaseLetter", Justification = "Default event handler naming pattern")]
        private void button1_Click(object sender, RoutedEventArgs e)
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

        private void AddNewControl(object sender, RoutedEventArgs e)
        {
            StackPanel panel = new StackPanel()
            {
                Orientation = Orientation.Horizontal,
                Width = 230
            };

            var nameTextbox = new TextBox() 
            {
                Text = "Name...",
                Width = 100
            };

            var valueTextbox = new TextBox()
            {
                Text = "Name...",
                Width = 100
            };

            var button = new Button() 
            {
                Width = 20,
                Content = "X",
                Foreground = System.Windows.Media.Brushes.Red,
            };

           

            panel.Children.Add(nameTextbox);
            panel.Children.Add(valueTextbox);
            panel.Children.Add(button);

            button.Click += (ss, ee) =>
            {
                ParamsListWrapper.Children.Remove(panel);
            };

            ParamsListWrapper.Children.Add(panel);
        }
    }
}
using System.Diagnostics.CodeAnalysis;
using System.Drawing;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Media;
using VSRESTClient.UI.Builders;
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

            IControlBuilder<StackPanel> controlBuilder = new StackPanelBuilder();

            controlBuilder.AddConfiguration(conf =>
            {
                conf.Orientation = Orientation.Horizontal;
                conf.Width = 230;
            });

            controlBuilder.AddControl<TextBox>(conf =>
            {
                conf.Text = "Name...";
                conf.Width = 100;
            });

            controlBuilder.AddControl<TextBox>(conf =>
            {
                conf.Text = "Value...";
                conf.Width = 100;
            });

            controlBuilder.AddControl<Button>(conf =>
            {
                conf.Width = 20;
                conf.Content = "X";
                conf.Foreground = System.Windows.Media.Brushes.Red;
                conf.Click += (ss, ee) =>
                {
                    ParamsListWrapper.Children.Remove(FindParent<StackPanel>(conf));
                };
            });

            var panel = controlBuilder.Build();

            ParamsListWrapper.Children.Add(panel);
        }

        public  T FindParent<T>(DependencyObject child) where T : DependencyObject
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
    }
}
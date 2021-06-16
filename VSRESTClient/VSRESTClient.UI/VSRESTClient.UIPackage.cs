using Microsoft.VisualStudio.Shell;
using System;
using System.Runtime.InteropServices;
using System.Threading;
using Task = System.Threading.Tasks.Task;

namespace VSRESTClient.UI
{

    [PackageRegistration(UseManagedResourcesOnly = true, AllowsBackgroundLoading = true)]
    [Guid(VSRESTClientUIPackage.PackageGuidString)]
    [ProvideMenuResource("Menus.ctmenu", 1)]
    [ProvideToolWindow(typeof(VSRESTClient.UI.Windows.Main.MainWindow))]
    public sealed class VSRESTClientUIPackage : AsyncPackage
    {
        public const string PackageGuidString = "61d11d31-3a31-45e9-a41e-7593b48c2dfa";


    protected override async Task InitializeAsync(CancellationToken cancellationToken, IProgress<ServiceProgressData> progress)
    {
        await this.JoinableTaskFactory.SwitchToMainThreadAsync(cancellationToken);
        await VSRESTClient.UI.Windows.Main.MainWindowCommand.InitializeAsync(this);
    }

}
}

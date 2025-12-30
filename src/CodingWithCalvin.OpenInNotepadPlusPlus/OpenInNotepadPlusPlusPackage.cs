using System;
using System.Runtime.InteropServices;
using System.Threading;
using CodingWithCalvin.OpenInNotepadPlusPlus.Commands;
using CodingWithCalvin.OpenInNotepadPlusPlus.Dialogs;
using CodingWithCalvin.Otel4Vsix;
using Microsoft.VisualStudio.Shell;

namespace CodingWithCalvin.OpenInNotepadPlusPlus
{
    [PackageRegistration(UseManagedResourcesOnly = true, AllowsBackgroundLoading = true)]
    [InstalledProductRegistration(Vsix.Name, Vsix.Description, Vsix.Version)]
    [ProvideOptionPage(
        typeof(SettingsDialogPage),
        Vsix.Name,
        "General",
        101,
        111,
        true,
        new string[0],
        ProvidesLocalizedCategoryName = false
    )]
    [ProvideMenuResource("Menus.ctmenu", 1)]
    [Guid(PackageGuids.guidPackageString)]
    public sealed class OpenInNotepadPlusPlusPackage : AsyncPackage
    {
        protected override async System.Threading.Tasks.Task InitializeAsync(
            CancellationToken cancellationToken,
            IProgress<ServiceProgressData> progress
        )
        {
            await JoinableTaskFactory.SwitchToMainThreadAsync();

            var settings = (SettingsDialogPage)this.GetDialogPage(typeof(SettingsDialogPage));

            var builder = VsixTelemetry.Configure()
                .WithServiceName(Vsix.Name)
                .WithServiceVersion(Vsix.Version)
                .WithVisualStudioAttributes(this)
                .WithEnvironmentAttributes();

#if !DEBUG
            builder
                .WithOtlpHttp("https://api.honeycomb.io")
                .WithHeader("x-honeycomb-team", HoneycombConfig.ApiKey);
#endif

            builder.Initialize();

            OpenExecutableCommand.Initialize(this, settings);
        }

        protected override void Dispose(bool disposing)
        {
            if (disposing)
            {
                VsixTelemetry.Shutdown();
            }
            base.Dispose(disposing);
        }
    }
}

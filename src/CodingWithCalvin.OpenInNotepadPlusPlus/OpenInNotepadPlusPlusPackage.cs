using System;
using System.Runtime.InteropServices;
using System.Threading;
using CodingWithCalvin.OpenInNotepadPlusPlus.Commands;
using CodingWithCalvin.OpenInNotepadPlusPlus.Dialogs;
using CodingWithCalvin.OpenInNotepadPlusPlus.Helpers;
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

            Logger.Initialize(this, Vsix.Name);
            OpenExecutableCommand.Initialize(this, settings);
        }
    }
}

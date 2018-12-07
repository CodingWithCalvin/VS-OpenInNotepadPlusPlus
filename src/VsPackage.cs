using System;
using System.Runtime.InteropServices;
using System.Threading;
using Microsoft.VisualStudio.Shell;
using OpenInNotepadPlusPlus.Commands;
using OpenInNotepadPlusPlus.Helpers;

namespace OpenInNotepadPlusPlus
{
	[PackageRegistration(UseManagedResourcesOnly = true, AllowsBackgroundLoading = true)]
	[InstalledProductRegistration("#110", "#112", "1.1.12", IconResourceID = 400)]
	[ProvideOptionPage(typeof(Settings), Vsix.Name, "General",101,111, true, new string[0], ProvidesLocalizedCategoryName = false)]
	[ProvideMenuResource("Menus.ctmenu", 1)]
	[Guid(PackageGuids.guidPackageString)]
	public sealed class VsPackage : AsyncPackage
	{
		protected override async System.Threading.Tasks.Task InitializeAsync(CancellationToken cancellationToken, IProgress<ServiceProgressData> progress)
		{
			await JoinableTaskFactory.SwitchToMainThreadAsync();

			var settings = (Settings)this.GetDialogPage(typeof(Settings));
			
			Logger.Initialize(this, Vsix.Name);
			OpenNotepadPlusPlusCommand.Initialize(this, settings);
		}
	}
}

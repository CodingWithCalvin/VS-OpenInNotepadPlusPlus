using System.Runtime.InteropServices;
using Microsoft.VisualStudio.Shell;
using OpenInNotepadPlusPlus.Commands;
using OpenInNotepadPlusPlus.Helpers;

namespace OpenInNotepadPlusPlus
{
	[PackageRegistration(UseManagedResourcesOnly = true)]
	[InstalledProductRegistration("#110", "#112", "1.1.12", IconResourceID = 400)]
	[ProvideOptionPage(typeof(Settings), Vsix.Name, "General", 101, 111, true, new string[] { }, ProvidesLocalizedCategoryName = false)]
	[ProvideMenuResource("Menus.ctmenu", 1)]
	[Guid(PackageGuids.guidPackageString)]
	public sealed class VsPackage : Package
	{
		public static Settings Settings;

		protected override void Initialize()
		{
			Settings = (Settings)this.GetDialogPage(typeof(Settings));
			Logger.Initialize(this, Vsix.Name);
			OpenNotepadPlusPlusCommand.Initialize((Package)this);
			base.Initialize();
		}
	}
}

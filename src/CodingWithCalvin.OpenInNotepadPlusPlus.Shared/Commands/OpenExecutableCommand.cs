using System;
using System.ComponentModel.Design;
using System.Diagnostics;
using Microsoft.VisualStudio.Shell;
using EnvDTE80;
using EnvDTE;
using System.Windows.Forms;
using CodingWithCalvin.OpenInNotepadPlusPlus.Shared.Helpers;
using CodingWithCalvin.OpenInNotepadPlusPlus.Shared.Dialogs;
using CodingWithCalvin.OpenInNotepadPlusPlus.Vsix.x64;

namespace CodingWithCalvin.OpenInNotepadPlusPlus.Shared.Commands
{
	internal sealed class OpenExecutableCommand
	{
		private readonly Package _package;
	    private readonly SettingsDialogPage _settings;

		private OpenExecutableCommand(Package package, SettingsDialogPage settings)
		{
			this._package = package;
		    this._settings = settings;

		    var commandService = (OleMenuCommandService)ServiceProvider.GetService(typeof(IMenuCommandService));

		    if (commandService != null)
		    {
		        var menuCommandId = new CommandID(PackageGuids.guidOpenInNppCmdSet, PackageIds.OpenInNpp);
		        var menuItem = new MenuCommand(OpenPath, menuCommandId);
		        commandService.AddCommand(menuItem);
		    }
        }

		public static OpenExecutableCommand Instance { get; private set; }

		private IServiceProvider ServiceProvider => this._package;

		public static void Initialize(Package package, SettingsDialogPage settings)
		{
			Instance = new OpenExecutableCommand(package, settings);
		}

		private void OpenPath(object sender, EventArgs e)
		{
			var service = (DTE2)this.ServiceProvider.GetService(typeof(DTE));
			try
			{
                ThreadHelper.ThrowIfNotOnUIThread();
				var selectedFilePath = ProjectHelpers.GetSelectedPath(service);
				var executablePath = _settings.FolderPath;
				if (!string.IsNullOrEmpty(selectedFilePath) && !string.IsNullOrEmpty(executablePath))
				{
					OpenExecutable(executablePath, selectedFilePath);
				}
				else
				{
					MessageBox.Show("Couldn't resolve the folder");
				}
			}
			catch (Exception ex)
			{
				Logger.Log(ex);
			}
		}

		private static void OpenExecutable(string executablePath, string selectedFilePath)
		{
			var startInfo = new ProcessStartInfo
			{
				WorkingDirectory = selectedFilePath,
				FileName = $"\"{executablePath}\"",
				Arguments = $"\"{selectedFilePath}\"",
				CreateNoWindow = true,
				WindowStyle = ProcessWindowStyle.Hidden
			};

			using (System.Diagnostics.Process.Start(startInfo))
			{
				//TODO : Should this be empty?
			}
		}
	}
}

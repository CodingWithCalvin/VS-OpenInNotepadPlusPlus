using System;
using System.ComponentModel.Design;
using System.Diagnostics;
using System.IO;
using System.Windows.Forms;
using EnvDTE;
using EnvDTE80;
using Microsoft.VisualStudio.Shell;
using OpenInNotepadPlusPlus.Helpers;

namespace OpenInNotepadPlusPlus.Commands
{
	internal sealed class OpenNotepadPlusPlusCommand
	{
		private readonly Package _package;
	    private readonly Settings _settings;

		private OpenNotepadPlusPlusCommand(Package package, Settings settings)
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

		public static OpenNotepadPlusPlusCommand Instance { get; private set; }

		private IServiceProvider ServiceProvider => this._package;

		public static void Initialize(Package package, Settings settings)
		{
			Instance = new OpenNotepadPlusPlusCommand(package, settings);
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
					OpenNotepadPlusPlus(executablePath, selectedFilePath);
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

		private static void OpenNotepadPlusPlus(string executablePath, string selectedFilePath)
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

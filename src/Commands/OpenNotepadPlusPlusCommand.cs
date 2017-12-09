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

		private OpenNotepadPlusPlusCommand(Package package)
		{
			this._package = package;
			if (!(this.ServiceProvider.GetService(typeof(IMenuCommandService)) is OleMenuCommandService service))
			{
				return;
			}
			var command = new MenuCommand(this.OpenPath, new CommandID(PackageGuids.guidOpenInNppCmdSet, 256));
			service.AddCommand(command);
		}

		public static OpenNotepadPlusPlusCommand Instance { get; private set; }

		private IServiceProvider ServiceProvider => this._package;

		public static void Initialize(Package package)
		{
			Instance = new OpenNotepadPlusPlusCommand(package);
		}

		private void OpenPath(object sender, EventArgs e)
		{
			var service = (DTE2)this.ServiceProvider.GetService(typeof(DTE));
			try
			{
				var selectedFilePath = ProjectHelpers.GetSelectedPath(service);
				var executablePath = VsPackage.Settings.FolderPath;
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

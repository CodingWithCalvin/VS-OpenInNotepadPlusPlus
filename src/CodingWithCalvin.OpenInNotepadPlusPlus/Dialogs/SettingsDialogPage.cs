using System;
using System.ComponentModel;
using System.IO;
using Microsoft.VisualStudio.Shell;

namespace CodingWithCalvin.OpenInNotepadPlusPlus.Dialogs
{
    public class SettingsDialogPage : DialogPage
    {
        [Category("General")]
        [DisplayName("Install path")]
        [Description("The absolute path to the \"notepad++.exe\" file.")]
        public string FolderPath { get; set; }

        public override void LoadSettingsFromStorage()
        {
            base.LoadSettingsFromStorage();

            if (!string.IsNullOrEmpty(this.FolderPath))
            {
                return;
            }

            this.FolderPath = FindNotepadPlusPlus();
        }

        private static string FindNotepadPlusPlus()
        {
            var directoryInfo = new DirectoryInfo(
                Environment.GetFolderPath(Environment.SpecialFolder.ProgramFiles)
            );
            if (directoryInfo.Parent == null)
            {
                return null;
            }

            foreach (
                var directory in directoryInfo.Parent.GetDirectories(
                    directoryInfo.Name.Replace(" (x86)", string.Empty) + "*"
                )
            )
            {
                foreach (var fileSystemInfo in directory.GetDirectories("Notepad++"))
                {
                    var path = Path.Combine(fileSystemInfo.FullName, "notepad++.exe");
                    if (File.Exists(path))
                    {
                        return path;
                    }
                }
            }

            return null;
        }
    }
}

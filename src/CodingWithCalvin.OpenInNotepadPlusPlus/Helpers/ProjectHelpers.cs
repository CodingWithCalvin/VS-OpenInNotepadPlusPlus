using System;
using EnvDTE;
using EnvDTE80;
using Microsoft.VisualStudio.Shell;

namespace CodingWithCalvin.OpenInNotepadPlusPlus.Helpers
{
	internal static class ProjectHelpers
	{
		public static string GetSelectedPath(DTE2 dte)
		{
            ThreadHelper.ThrowIfNotOnUIThread();

			foreach (UIHierarchyItem selectedItem in (Array)dte.ToolWindows.SolutionExplorer.SelectedItems)
			{
				switch (selectedItem.Object)
				{
					case ProjectItem projectItem:
						return projectItem.FileNames[1];
					case Project project:
						return project.FullName;
					case Solution solution:
						return solution.FullName;
				}
			}
			return null;
		}
	}
}

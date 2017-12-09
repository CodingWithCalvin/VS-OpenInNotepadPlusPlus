using System;
using EnvDTE;
using EnvDTE80;

namespace OpenInNotepadPlusPlus.Helpers
{
	internal static class ProjectHelpers
	{
		public static string GetSelectedPath(DTE2 dte)
		{
			foreach (UIHierarchyItem selectedItem in (Array)dte.ToolWindows.SolutionExplorer.SelectedItems)
			{
				switch (selectedItem.Object)
				{
					case ProjectItem projectItem:
						return projectItem.Document?.FullName ?? projectItem.Properties?.Item("FullPath").ToString();
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

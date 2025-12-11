using System;
using System.IO;
using EnvDTE;
using EnvDTE80;
using Microsoft.VisualStudio.Shell;

namespace CodingWithCalvin.OpenInNotepadPlusPlus.Helpers
{
    internal static class ProjectHelpers
    {
        private const string UnloadedProjectGuid = "{67294A52-A4F0-11D2-AA88-00C04F688DDE}";

        public static string GetSelectedPath(DTE2 dte)
        {
            ThreadHelper.ThrowIfNotOnUIThread();

            foreach (
                UIHierarchyItem selectedItem in (Array)
                    dte.ToolWindows.SolutionExplorer.SelectedItems
            )
            {
                switch (selectedItem.Object)
                {
                    case ProjectItem projectItem:
                        return projectItem.FileNames[1];
                    case Project project:
                        return GetProjectPath(project, dte.Solution);
                    case Solution solution:
                        return solution.FullName;
                }
            }
            return null;
        }

        private static string GetProjectPath(Project project, Solution solution)
        {
            ThreadHelper.ThrowIfNotOnUIThread();

            bool isUnloaded = project.Kind.Equals(UnloadedProjectGuid, StringComparison.OrdinalIgnoreCase);

            if (!isUnloaded)
            {
                return project.FullName;
            }

            // For unloaded projects, FullName is empty but UniqueName contains
            // the relative path from the solution directory
            if (!string.IsNullOrEmpty(project.UniqueName) && !string.IsNullOrEmpty(solution?.FullName))
            {
                var solutionDirectory = Path.GetDirectoryName(solution.FullName);
                var projectPath = Path.Combine(solutionDirectory, project.UniqueName);
                if (File.Exists(projectPath))
                {
                    return projectPath;
                }
            }

            return null;
        }
    }
}

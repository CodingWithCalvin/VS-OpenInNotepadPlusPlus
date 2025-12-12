using System;
using System.IO;
using System.Runtime.InteropServices;
using EnvDTE;
using EnvDTE80;
using Microsoft.VisualStudio;
using Microsoft.VisualStudio.Shell;
using Microsoft.VisualStudio.Shell.Interop;

namespace CodingWithCalvin.OpenInNotepadPlusPlus.Helpers
{
    internal static class ProjectHelpers
    {
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
                    default:
                        // Handle unloaded projects - they don't expose a Project object
                        // but we can get the path via IVsMonitorSelection and IVsHierarchy
                        var path = GetPathFromHierarchy();
                        if (!string.IsNullOrEmpty(path))
                        {
                            return path;
                        }
                        break;
                }
            }
            return null;
        }

        private static string GetPathFromHierarchy()
        {
            ThreadHelper.ThrowIfNotOnUIThread();

            // Use IVsMonitorSelection to get the current selection's IVsHierarchy
            // This works for unloaded projects where selectedItem.Object is not a Project
            var monitorSelection = Package.GetGlobalService(typeof(SVsShellMonitorSelection)) as IVsMonitorSelection;
            if (monitorSelection == null)
            {
                return null;
            }

            var hr = monitorSelection.GetCurrentSelection(out var hierarchyPtr, out var itemId, out _, out _);
            if (hr != VSConstants.S_OK || hierarchyPtr == IntPtr.Zero)
            {
                return null;
            }

            try
            {
                var hierarchy = Marshal.GetObjectForIUnknown(hierarchyPtr) as IVsHierarchy;
                if (hierarchy == null)
                {
                    return null;
                }

                // GetCanonicalName returns the full path for project files
                if (hierarchy.GetCanonicalName(itemId, out var canonicalName) == VSConstants.S_OK
                    && !string.IsNullOrEmpty(canonicalName)
                    && File.Exists(canonicalName))
                {
                    return canonicalName;
                }
            }
            finally
            {
                Marshal.Release(hierarchyPtr);
            }

            return null;
        }

        private static string GetProjectPath(Project project, Solution solution)
        {
            ThreadHelper.ThrowIfNotOnUIThread();

            // Try FullName first - works for loaded projects and sometimes unloaded ones
            if (!string.IsNullOrEmpty(project.FullName) && File.Exists(project.FullName))
            {
                return project.FullName;
            }

            // Fallback: try UniqueName which may contain the relative or absolute path
            if (!string.IsNullOrEmpty(project.UniqueName))
            {
                // UniqueName might already be an absolute path
                if (Path.IsPathRooted(project.UniqueName) && File.Exists(project.UniqueName))
                {
                    return project.UniqueName;
                }

                // Try combining with solution directory
                if (!string.IsNullOrEmpty(solution?.FullName))
                {
                    var solutionDirectory = Path.GetDirectoryName(solution.FullName);
                    var projectPath = Path.Combine(solutionDirectory, project.UniqueName);
                    if (File.Exists(projectPath))
                    {
                        return projectPath;
                    }
                }
            }

            return null;
        }
    }
}

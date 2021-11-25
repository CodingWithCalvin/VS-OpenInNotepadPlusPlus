#if X64
using CodingWithCalvin.OpenInNotepadPlusPlus.Vsix.x64;
#elif X86
using CodingWithCalvin.OpenInNotepadPlusPlus.Vsix.x86;
#endif

using System.Reflection;
using System.Runtime.InteropServices;

[assembly: AssemblyTitle(VsixConstants.Name)]
[assembly: AssemblyDescription(VsixConstants.Description)]
[assembly: AssemblyConfiguration("")]
[assembly: AssemblyCompany(VsixConstants.Author)]
[assembly: AssemblyProduct(VsixConstants.Name)]
[assembly: AssemblyCopyright("")]
[assembly: AssemblyTrademark("")]
[assembly: AssemblyCulture("")]
[assembly: ComVisible(false)]

[assembly: AssemblyVersion(VsixConstants.Version)]
[assembly: AssemblyFileVersion(VsixConstants.Version)]

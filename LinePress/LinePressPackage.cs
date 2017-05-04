using System;
using System.Diagnostics.CodeAnalysis;
using System.Runtime.InteropServices;
using Microsoft.VisualStudio.Shell;
using LinePress.Options;

namespace LinePress
{
    [ProvideOptionPage(typeof(OptionsPage), "Line Press", "Options", 0, 0, true)]
    [PackageRegistration(UseManagedResourcesOnly = true)]
    [InstalledProductRegistration("#110", "#112", Vsix.Id, IconResourceID = 400)]
    [Guid(PackageGuidString)]
    [SuppressMessage("StyleCop.CSharp.DocumentationRules", "SA1650:ElementDocumentationMustBeSpelledCorrectly",
                     Justification = "pkgdef, VS and vsixmanifest are valid VS terms")]
    public sealed class LinePressPackage : Package
    {
        public const string PackageGuidString = "0267e91e-033e-4dbc-9f41-0efe67f515ae";
    }
}
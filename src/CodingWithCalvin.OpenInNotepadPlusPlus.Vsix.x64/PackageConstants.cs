using System;

namespace CodingWithCalvin.OpenInNotepadPlusPlus.Vsix.x64
{
    internal sealed partial class PackageGuids
    {
        public const string guidPackageString = "6aeabf47-7bdc-47b3-ade7-06f5bae6d868";
        public static Guid guidPackage = new Guid(guidPackageString);

        public const string guidOpenInNppCmdSetString = "f781199d-54a8-4a18-ac9d-a91f292587db";
        public static Guid guidOpenInNppCmdSet = new Guid(guidOpenInNppCmdSetString);

        public const string guidIconsString = "3a06fde0-497f-4b1f-9150-5b1c5879e8af";
        public static Guid guidIcons = new Guid(guidIconsString);
    }

    internal sealed partial class PackageIds
    {
        public const int OpenInNpp = 0x0100;
        public const int NotepadPlusPlus = 0x0001;
    }
}

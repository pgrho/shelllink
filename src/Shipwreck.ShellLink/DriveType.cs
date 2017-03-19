using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Shipwreck.ShellLink
{
    public enum DriveType
    {
        Unknown = 0,
        NoRootDir = 1,
        Removable = 2,
        Fixed = 3,
        Remote = 4,
        CDRom = 5,
        RamDisk = 6
    }
}

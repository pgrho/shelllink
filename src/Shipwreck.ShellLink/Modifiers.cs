using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Shipwreck.ShellLink
{
    [Flags]
    public enum Modifiers : byte
    {
        None = 0,
        Shift = 1,
        Control = 2,
        Alt = 4,
    }
}

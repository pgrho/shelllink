using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Shipwreck.ShellLink
{
    [Flags]
    public enum LinkInfoFlags
    {
        VolumeIDAndLocalBasePath = 1,
        CommonNetworkRelativeLinkAndPathSuffix = 2,
    }
}

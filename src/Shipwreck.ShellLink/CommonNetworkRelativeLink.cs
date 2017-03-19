using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Shipwreck.ShellLink
{
    public sealed class CommonNetworkRelativeLink
    {
        public string DeviceName { get; set; }
        public CommonNetworkRelativeLinkFlags Flags { get; set; }
        public string NetName { get; internal set; }
        public NetworkProviderType NetworkProviderType { get; set; }
        public string UnicodeDeviceName { get; set; }
        public string UnicodeNetName { get; set; }

        internal static CommonNetworkRelativeLink Parse(BinaryReader reader, ref byte[] bytes, ref StringBuilder sb)
        {
            var cnrl = new CommonNetworkRelativeLink();
            var cs = reader.ReadUInt32();
            cnrl.Flags = (CommonNetworkRelativeLinkFlags)reader.ReadInt32();

            var nno = reader.ReadUInt32();
            var dno = reader.ReadUInt32();

            cnrl.NetworkProviderType = (NetworkProviderType)reader.ReadInt32();

            var nnou = nno <= 0x14 ? 0 : reader.ReadUInt32();
            var dnou = nno <= 0x14 ? 0 : reader.ReadUInt32();

            if (nno > 0)
            {
                cnrl.NetName = reader.ReadAnsiString(ref bytes);
            }
            if (dno > 0)
            {
                cnrl.DeviceName = reader.ReadAnsiString(ref bytes);
            }
            if (nnou > 0)
            {
                cnrl.UnicodeNetName = reader.ReadUnicodeString(ref sb);
            }
            if (dnou > 0)
            {
                cnrl.UnicodeDeviceName = reader.ReadUnicodeString(ref sb);
            }

            return cnrl;
        }
    }
}
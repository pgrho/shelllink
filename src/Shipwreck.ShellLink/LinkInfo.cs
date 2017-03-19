using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Shipwreck.ShellLink
{
    public sealed class LinkInfo
    {
        public CommonNetworkRelativeLink CommonNetworkRelativeLink { get; set; }
        public string CommonPathSuffix { get; set; }
        public string LocalBasePath { get; set; }
        public string UnicodeCommonPathSuffix { get; set; }
        public string UnicodeLocalBasePath { get; set; }
        public VolumeID VolumeID { get; set; }

        internal static LinkInfo Parse(BinaryReader reader, ref byte[] bytes, ref StringBuilder sb)
        {
            var li = new LinkInfo();

            var size = reader.ReadUInt32();
            var hs = reader.ReadUInt32();
            var lif = (LinkInfoFlags)reader.ReadInt32();

            var vidOffset = reader.ReadUInt32();
            var lbpOffset = reader.ReadUInt32();
            var cnrlOffset = reader.ReadUInt32();
            var cpsOffset = reader.ReadUInt32();
            uint lbpuOffset, cpsuOffset;
            if (hs >= 0x24)
            {
                lbpuOffset = reader.ReadUInt32();
                cpsuOffset = reader.ReadUInt32();
            }
            else
            {
                lbpuOffset = cpsuOffset = 0;
            }
            if (vidOffset > 0)
            {
                li.VolumeID = VolumeID.Parse(reader, ref bytes, ref sb);
            }
            if (lbpOffset > 0)
            {
                li.LocalBasePath = reader.ReadAnsiString(ref bytes);
            }
            if (cnrlOffset > 0)
            {
                li.CommonNetworkRelativeLink = CommonNetworkRelativeLink.Parse(reader, ref bytes, ref sb);
            }
            if (cpsOffset > 0)
            {
                li.CommonPathSuffix = reader.ReadAnsiString(ref bytes);
            }
            if (lbpuOffset > 0)
            {
                li.UnicodeLocalBasePath = reader.ReadUnicodeString(ref sb);
            }
            if (cpsuOffset > 0)
            {
                li.UnicodeCommonPathSuffix = reader.ReadUnicodeString(ref sb);
            }

            return li;
        }
    }
}

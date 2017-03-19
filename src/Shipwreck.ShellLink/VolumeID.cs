using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Shipwreck.ShellLink
{
    public sealed class VolumeID
    {
        public DriveType DriveType { get; set; }
        public int SerialNumber { get; set; }
        public string VolumeLabel { get; set; }

        internal static VolumeID Parse(BinaryReader reader, ref byte[] bytes, ref StringBuilder sb)
        {
            var r = new VolumeID();
            var size = reader.ReadUInt32();
            r.DriveType = (DriveType)reader.ReadInt32();
            r.SerialNumber = reader.ReadInt32();

            var vlo = reader.ReadUInt32();
            if (vlo == 0x14)
            {
                vlo = reader.ReadUInt32();

                r.VolumeLabel = reader.ReadUnicodeString(ref sb);
            }
            else
            {
                r.VolumeLabel = reader.ReadAnsiString(ref bytes);
            }

            return r;
        }
    }
}

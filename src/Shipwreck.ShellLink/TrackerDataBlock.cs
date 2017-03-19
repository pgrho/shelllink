using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Shipwreck.ShellLink
{
    public sealed class TrackerDataBlock : DataBlock
    {
        internal const uint SIGNATURE = 0xA0000003;

        public string MachineID { get; set; }
        public Guid Droid1 { get; set; }
        public Guid Droid2 { get; set; }
        public Guid DroidBirth1 { get; set; }
        public Guid DroidBirth2 { get; set; }

        protected override uint GetSignature()
            => SIGNATURE;

        internal static TrackerDataBlock Parse(BinaryReader reader, int blockSize, ref byte[] bytes)
        {
            var r = new TrackerDataBlock();
            reader.ReadInt32();
            reader.ReadInt32();
            r.MachineID = reader.ReadAnsiString(ref bytes, blockSize - 80);
            r.Droid1 = reader.ReadGuid();
            r.Droid2 = reader.ReadGuid();
            r.DroidBirth1 = reader.ReadGuid();
            r.DroidBirth2 = reader.ReadGuid();

            return r;
        }

        protected override void WriteToCore(BinaryWriter writer)
        {
            writer.Write(0);
            writer.Write(0x50 + (MachineID?.Length ?? 8));
            writer.Write(MachineID, (Math.Max(16, MachineID?.Length ?? 0) + 3) & ~3);
            writer.Write(Droid1);
            writer.Write(Droid2);
            writer.Write(DroidBirth1);
            writer.Write(DroidBirth2);
        }
    }
}

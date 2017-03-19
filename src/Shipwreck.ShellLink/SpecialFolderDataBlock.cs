using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Shipwreck.ShellLink
{
    public sealed class SpecialFolderDataBlock : DataBlock
    {
        internal const uint SIGNATURE = 0xA0000005;

        public int SpecialFolderID { get; set; }

        public uint Offset { get; set; }

        protected override uint GetSignature()
            => SIGNATURE;

        internal static SpecialFolderDataBlock Parse(BinaryReader reader)
        {
            var r = new SpecialFolderDataBlock();
            r.SpecialFolderID = reader.ReadInt32();
            r.Offset = reader.ReadUInt32();
            return r;
        }

        protected override void WriteToCore(BinaryWriter writer)
        {
            var id = SpecialFolderID;
            writer.Write(id);
            writer.Write(Offset);
        }
    }
}

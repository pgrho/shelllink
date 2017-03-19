using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Shipwreck.ShellLink
{
    public sealed class KnownFolderDataBlock : DataBlock
    {
        internal const uint SIGNATURE = 0xA000000B;

        public Guid KnownFolderID { get; set; }

        public uint Offset { get; set; }

        protected override uint GetSignature()
            => SIGNATURE;

        internal static KnownFolderDataBlock Parse(BinaryReader reader)
        {
            var r = new KnownFolderDataBlock();
            r.KnownFolderID = reader.ReadGuid();
            r.Offset = reader.ReadUInt32();
            return r;
        }

        protected override void WriteToCore(BinaryWriter writer)
        {
            var id = KnownFolderID;
            writer.Write(id);
            writer.Write(Offset);
        }
    }
}

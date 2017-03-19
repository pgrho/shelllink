using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Shipwreck.ShellLink
{
    public sealed class ConsoleFEDataBlock : DataBlock
    {
        internal const uint SIGNATURE = 0xA0000004;

        public uint CodePage { get; set; }

        protected override uint GetSignature()
            => SIGNATURE;

        internal static ConsoleFEDataBlock Parse(BinaryReader reader)
        {
            var r = new ConsoleFEDataBlock();
            r.CodePage = reader.ReadUInt32();
            return r;
        }

        protected override void WriteToCore(BinaryWriter writer)
        {
            writer.Write(CodePage);
        }
    }
}

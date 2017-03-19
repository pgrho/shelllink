using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Shipwreck.ShellLink
{
    public sealed class DarwinDataBlock : DataBlock
    {
        private const int LENGTH = 260;

        internal const uint SIGNATURE = 0xA0000006;

        public string Data { get; set; }

        protected override uint GetSignature()
            => SIGNATURE;

        internal static DarwinDataBlock Parse(BinaryReader reader, ref byte[] bytes, ref StringBuilder sb)
        {
            var r = new DarwinDataBlock();

            reader.ReadAnsiString(ref bytes, LENGTH);
            r.Data = reader.ReadUnicodeString(ref sb, LENGTH);

            return r;
        }

        protected override void WriteToCore(BinaryWriter writer)
        {
            var ansi = string.IsNullOrEmpty(Data) ? null : Encoding.Default.GetBytes(Data);
            writer.Write(ansi, LENGTH);
            writer.Write(Data, LENGTH);
        }
    }
}

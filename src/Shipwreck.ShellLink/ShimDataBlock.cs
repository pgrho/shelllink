using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Shipwreck.ShellLink
{
    public sealed class ShimDataBlock : DataBlock
    {
        internal const uint SIGNATURE = 0xA0000008;

        public string LayerName { get; set; }

        protected override uint GetSignature()
            => SIGNATURE;

        internal static ShimDataBlock Parse(BinaryReader reader, int blockSize, ref StringBuilder sb)
        {
            var r = new ShimDataBlock();
            r.LayerName = reader.ReadUnicodeString(ref sb, (blockSize - 8) / 2);

            return r;
        }

        protected override void WriteToCore(BinaryWriter writer)
        {
            if (!string.IsNullOrEmpty(LayerName))
            {
                writer.Write(LayerName);
                writer.Write('\0');
            }
        }
    }
}

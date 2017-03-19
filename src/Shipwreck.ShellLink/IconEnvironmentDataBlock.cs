using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Shipwreck.ShellLink
{
    public sealed class IconEnvironmentDataBlock : DataBlock
    {
        private const int LENGTH = 260;

        internal const uint SIGNATURE = 0xA0000007;

        public string Target { get; set; }

        protected override uint GetSignature()
            => SIGNATURE;

        internal static IconEnvironmentDataBlock Parse(BinaryReader reader, ref byte[] bytes, ref StringBuilder sb)
        {
            var r = new IconEnvironmentDataBlock();

            reader.ReadAnsiString(ref bytes, LENGTH);
            r.Target = reader.ReadUnicodeString(ref sb, LENGTH);

            return r;
        }

        protected override void WriteToCore(BinaryWriter writer)
        {
            var ansi = string.IsNullOrEmpty(Target) ? null : Encoding.Default.GetBytes(Target);
            writer.Write(ansi, LENGTH);
            writer.Write(Target, LENGTH);
        }
    }
}

using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Shipwreck.ShellLink.OlePS;

namespace Shipwreck.ShellLink
{
    public sealed class PropertyStoreDataBlock : DataBlock
    {
        internal const int VERSION = 0x53505331;
        internal const uint SIGNATURE = 0xA0000009;

        private static readonly Guid StringName = Guid.Parse("D5CDD505-2E9C-101B-9397-08002B2CF9AE");

        public Guid FormatID { get; set; }

        protected override uint GetSignature()
            => SIGNATURE;

        public IDictionary<string, TypedPropertyValue> StringNamedValues { get; set; }
        public IDictionary<int, TypedPropertyValue> IntegerNamedValues { get; set; }

        internal static PropertyStoreDataBlock Parse(BinaryReader reader, ref byte[] bytes, ref StringBuilder sb)
        {
            var r = new PropertyStoreDataBlock();
            var size = reader.ReadUInt32();

            if (reader.ReadInt32() != VERSION)
            {
                throw new FormatException();
            }

            r.FormatID = reader.ReadGuid();

            if (r.FormatID == StringName)
            {
                var d = new Dictionary<string, TypedPropertyValue>();
                for (var vs = reader.ReadUInt32(); vs != 0; vs = reader.ReadUInt32())
                {
                    var ns = reader.ReadUInt32();
                    reader.ReadByte(); // 0
                    var n = reader.ReadUnicodeString(ref sb);

                    d[n] = TypedPropertyValue.Parse(reader, ref bytes, ref sb);
                }
                r.StringNamedValues = d;
            }
            else
            {
                var d = new Dictionary<int, TypedPropertyValue>();
                for (var vs = reader.ReadUInt32(); vs != 0; vs = reader.ReadUInt32())
                {
                    var ns = reader.ReadUInt32();
                    reader.ReadByte(); // 0
                    var n = reader.ReadInt32();

                    d[n] = TypedPropertyValue.Parse(reader, ref bytes, ref sb);
                }
                r.IntegerNamedValues = d;
            }

            return r;
        }

        protected override void WriteToCore(BinaryWriter writer)
        {
            var bs = writer.BaseStream;
            var bp = bs.Position;

            writer.Write(0);
            writer.Write(VERSION);

            if (StringNamedValues?.Count > 0)
            {
                writer.Write(StringName);

                foreach (var kv in StringNamedValues)
                {
                    var bp2 = bs.Position;
                    writer.Write(0);
                    writer.Write(2 + kv.Key.Length * 2);
                    writer.Write((byte)0);
                    writer.Write(kv.Key);
                    writer.Write('\0');

                    kv.Value.WriteTo(writer);

                    var lp2 = bs.Position;

                    bs.Position = bp2;
                    writer.Write((uint)(lp2 - bp2));

                    bs.Position = lp2;
                }
            }
            else
            {
                writer.Write(0L);
                writer.Write(0L);

                if (IntegerNamedValues != null)
                {
                    foreach (var kv in IntegerNamedValues)
                    {
                        var bp2 = bs.Position;
                        writer.Write(0);
                        writer.Write(kv.Key);
                        writer.Write((byte)0);

                        kv.Value.WriteTo(writer);

                        var lp2 = bs.Position;

                        bs.Position = bp2;
                        writer.Write((uint)(lp2 - bp2));

                        bs.Position = lp2;
                    }
                }
            }

            writer.Write(0);

            var lp = bs.Position;

            bs.Position = bp;
            writer.Write((uint)(lp - bp));

            bs.Position = lp;

            throw new NotImplementedException();
        }
    }
}

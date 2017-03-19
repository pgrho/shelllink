using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Shipwreck.ShellLink.OlePS;

namespace Shipwreck.ShellLink.PropStore
{
    public abstract class PropertyStore
    {
        private static readonly Guid StringName = Guid.Parse("D5CDD505-2E9C-101B-9397-08002B2CF9AE");

        internal const int VERSION = 0x53505331;
        public Guid FormatID { get; set; }

        /// <summary>
        /// <see cref="PropertyStore" />クラスの新しいインスタンスを初期化します。
        /// </summary>
        internal PropertyStore()
        {
        }

        internal static List<PropertyStore> Parse(BinaryReader reader, ref byte[] bytes, ref StringBuilder sb)
        {
            // [MS-PROPSTORE] 2.2
            List<PropertyStore> stores = null;
            for (var size = reader.ReadUInt32(); size != 0; size = reader.ReadUInt32())
            {
                if (reader.ReadInt32() != VERSION)
                {
                    throw new FormatException();
                }

                if (stores == null)
                {
                    stores = new List<PropStore.PropertyStore>();
                }

                var formatId = reader.ReadGuid();

                if (formatId == StringName)
                {
                    var r = new StringPropertyStore();
                    r.FormatID = formatId;

                    // [MS-PROPSTORE] 2.3.1

                    var d = new Dictionary<string, TypedPropertyValue>();
                    for (var vs = reader.ReadInt32(); vs != 0; vs = reader.ReadInt32())
                    {
                        var ns = reader.ReadUInt32();
                        reader.ReadByte(); // 0
                        int cc;
                        var n = reader.ReadUnicodeString(ref sb, out cc);

                        r.Values[n] = TypedPropertyValue.Parse(reader, vs - 9 - 2 * cc, ref bytes, ref sb);
                    }

                    stores.Add(r);
                }
                else
                {
                    var r = new IntegerPropertyStore();
                    r.FormatID = formatId;

                    // [MS-PROPSTORE] 2.3.2

                    var d = new Dictionary<int, TypedPropertyValue>();
                    for (var vs = reader.ReadInt32(); vs != 0; vs = reader.ReadInt32())
                    {
                        var n = reader.ReadInt32();
                        reader.ReadByte(); // 0

                        r.Values[n] = TypedPropertyValue.Parse(reader, vs - 13, ref bytes, ref sb);
                    }

                    stores.Add(r);
                }
            }

            return stores;
        }

        internal void WriteTo(BinaryWriter writer)
        {
            var bs = writer.BaseStream;
            var bp = bs.Position;

            writer.Write(0);
            writer.Write(VERSION);
            writer.Write(FormatID);

            WriteToCore(writer);

            var lp = bs.Position;

            bs.Position = bp;
            writer.Write((uint)(lp - bp));

            bs.Position = lp;
        }

        protected abstract void WriteToCore(BinaryWriter writer);
    }
}
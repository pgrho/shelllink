using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Shipwreck.ShellLink.OlePS;

namespace Shipwreck.ShellLink.PropStore
{
    public sealed class StringPropertyStore : PropertyStore
    {
        private Dictionary<string, TypedPropertyValue> _Values;

        public Dictionary<string, TypedPropertyValue> Values
            => _Values ?? (_Values = new Dictionary<string, TypedPropertyValue>());

        protected override void WriteToCore(BinaryWriter writer)
        {
            if (_Values != null)
            {
                var bs = writer.BaseStream;
                foreach (var kv in _Values)
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

            writer.Write(0);
        }
    }
}

using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Shipwreck.ShellLink.PropStore;

namespace Shipwreck.ShellLink
{
    public sealed class PropertyStoreDataBlock : DataBlock
    {
        internal const uint SIGNATURE = 0xA0000009;

        private Collection<PropertyStore> _Stores;

        protected override uint GetSignature()
            => SIGNATURE;

        public Collection<PropertyStore> Stores
            => _Stores ?? (_Stores = new Collection<PropertyStore>());

        internal static PropertyStoreDataBlock Parse(BinaryReader reader, ref byte[] bytes, ref StringBuilder sb)
        {
            // [MS-SHLLINK] 2.5.7

            var r = new PropertyStoreDataBlock();

            var pss = PropertyStore.Parse(reader, ref bytes, ref sb);

            if (pss != null)
            {
                foreach (var ps in pss)
                {
                    r.Stores.Add(ps);
                }
            }

            return r;
        }

        protected override void WriteToCore(BinaryWriter writer)
        {
            if (_Stores != null)
            {
                foreach (var s in Stores)
                {
                    s.WriteTo(writer);
                }
            }

            writer.Write(0);
        }
    }
}
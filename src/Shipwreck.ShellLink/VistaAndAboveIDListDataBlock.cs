using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Shipwreck.ShellLink
{
    public sealed class VistaAndAboveIDListDataBlock : DataBlock
    {
        internal const uint SIGNATURE = 0xA000000C;

        public IList<byte[]> IDList { get; set; }

        protected override uint GetSignature()
            => SIGNATURE;


        internal static VistaAndAboveIDListDataBlock Parse(BinaryReader reader)
        {
            var r = new VistaAndAboveIDListDataBlock();
            r.IDList = reader.ReadIDList();
            return r;
        }

        protected override void WriteToCore(BinaryWriter writer)
        {
            writer.WriteIDList(IDList);
        }
    }
}

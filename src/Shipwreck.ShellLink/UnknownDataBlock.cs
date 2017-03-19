using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Shipwreck.ShellLink
{
    public sealed class UnknownDataBlock : DataBlock
    {
        public uint Signature { get; set; }

        public byte[] Data { get; set; }

        protected override uint GetSignature()
            => Signature;

        protected override void WriteToCore(BinaryWriter writer)
        {
            if (Data != null)
            {
                writer.Write(Data, 0, Data.Length);
            }
        }
    }
}

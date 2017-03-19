using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Shipwreck.ShellLink
{

    public abstract class DataBlock
    {
        internal DataBlock() { }

        protected abstract uint GetSignature();

        internal void WriteTo(BinaryWriter writer)
        {
            var bs = writer.BaseStream;

            var bp = bs.Position;

            writer.Write(0u);
            writer.Write(GetSignature());
            WriteToCore(writer);

            var lp = bs.Position;

            bs.Position = bp;
            writer.Write((uint)(lp - bp));

            bs.Position = lp;
        }

        protected abstract void WriteToCore(BinaryWriter writer);
    }

}

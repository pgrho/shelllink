using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Shipwreck.ShellLink
{
    internal static partial class BinaryWriterExtensions
    {
        public static void Write(this BinaryWriter writer, Guid guid)
        {
            writer.Write(guid.ToByteArray());
        }

        public static void WriteIDList(this BinaryWriter writer, IList<byte[]> idList)
        {
            if (idList != null)
            {
                foreach (var id in idList)
                {
                    if (id != null)
                    {
                        writer.Write((ushort)id.Length);
                        writer.Write(id);
                    }
                }
                writer.Write((ushort)0);
            }
        }
    }
}

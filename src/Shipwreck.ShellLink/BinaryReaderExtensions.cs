using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Shipwreck.ShellLink
{
    internal static class BinaryReaderExtensions
    {
        public static Guid ReadGuid(this BinaryReader reader)
        {
            return new Guid(reader.ReadInt32(), reader.ReadInt16(), reader.ReadInt16(), reader.ReadByte(), reader.ReadByte(), reader.ReadByte(), reader.ReadByte(), reader.ReadByte(), reader.ReadByte(), reader.ReadByte(), reader.ReadByte());
        }

        public static string ReadStringData(this BinaryReader reader, bool isUnicode, ref StringBuilder sb, ref byte[] bytes)
        {
            var l = reader.ReadUInt16();
            return l == 0 ? string.Empty
                    : isUnicode ? reader.ReadUnicodeString(ref sb, length: l)
                    : reader.ReadAnsiString(ref bytes, length: l);
        }
        public static string ReadAnsiString(this BinaryReader reader, ref byte[] sb, int length = -1)
        {
            var i = 0;
            var l = 0;
            for (var by = reader.ReadByte(); length >= 0 || by != 0; by = reader.ReadByte())
            {
                if (sb == null)
                {
                    sb = new byte[16];
                }
                else
                {
                    var le = sb.Length;
                    for (; le <= i; le <<= 1)
                    {
                    }
                    if (le > sb.Length)
                    {
                        Array.Resize(ref sb, le);
                    }
                }
                sb[i++] = by;
                l = by != 0 ? i : l;

                if (i == length)
                {
                    break;
                }
            }

            return l == 0 ? string.Empty : Encoding.Default.GetString(sb, 0, l);
        }

        public static string ReadUnicodeString(this BinaryReader reader, ref StringBuilder sb, int length = -1)
        {
            int cc;
            return reader.ReadUnicodeString(ref sb, out cc, length: length);
        }
        public static string ReadUnicodeString(this BinaryReader reader, ref StringBuilder sb, out int count, int length = -1)
        {
            if (sb == null)
            {
                sb = new StringBuilder();
            }
            else
            {
                sb.Clear();
            }
            count = 0;
            var l = 0;
            for (var by = reader.ReadUInt16(); length >= 0 || by != 0; by = reader.ReadUInt16())
            {
                count++;
                sb.Append((char)by);

                l = by != 0 ? sb.Length : l;

                if (sb.Length == length)
                {
                    break;
                }
            }
            sb.Length = l;
            return sb.ToString();
        }

        public static IList<byte[]> ReadIDList(this BinaryReader reader)
        {
            var size = reader.ReadInt16();
            List<byte[]> ids = new List<byte[]>();

            for (var s = reader.ReadUInt16(); s != 0; s = reader.ReadUInt16())
            {
                ids.Add(reader.ReadBytes(s - 2));
            }
            return ids;
        }
    }
}
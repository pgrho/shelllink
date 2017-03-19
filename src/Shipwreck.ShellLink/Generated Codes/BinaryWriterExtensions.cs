using System.Collections.Generic;
using System.IO;

namespace Shipwreck.ShellLink
{
    partial class BinaryWriterExtensions
    {
        public static void Write(this BinaryWriter writer, IEnumerable<byte> value, int length)
		{
			var c = 0;
			if (value != null)
			{
				foreach (var v in value)
				{
					if (++c > length)
					{
						break;
					}
					writer.Write(v);
				}
			}
			for (; c < length; c++)
			{
				writer.Write(default(byte));
			}
		}

        public static void Write(this BinaryWriter writer, IEnumerable<int> value, int length)
		{
			var c = 0;
			if (value != null)
			{
				foreach (var v in value)
				{
					if (++c > length)
					{
						break;
					}
					writer.Write(v);
				}
			}
			for (; c < length; c++)
			{
				writer.Write(default(int));
			}
		}

        public static void Write(this BinaryWriter writer, IEnumerable<uint> value, int length)
		{
			var c = 0;
			if (value != null)
			{
				foreach (var v in value)
				{
					if (++c > length)
					{
						break;
					}
					writer.Write(v);
				}
			}
			for (; c < length; c++)
			{
				writer.Write(default(uint));
			}
		}

        public static void Write(this BinaryWriter writer, IEnumerable<char> value, int length)
		{
			var c = 0;
			if (value != null)
			{
				foreach (var v in value)
				{
					if (++c > length)
					{
						break;
					}
					writer.Write(v);
				}
			}
			for (; c < length; c++)
			{
				writer.Write(default(char));
			}
		}

    }
}
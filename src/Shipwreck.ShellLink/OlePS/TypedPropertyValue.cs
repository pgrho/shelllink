using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Shipwreck.ShellLink.OlePS
{
    public sealed class TypedPropertyValue
    {
        public ValueType Type { get; set; }
        public object Value { get; set; }

        public static TypedPropertyValue Parse(BinaryReader reader, int length, ref byte[] bytes, ref StringBuilder sb)
        {
            var r = new TypedPropertyValue();
            r.Type = (ValueType)reader.ReadInt16();
            reader.ReadInt16(); // 0

            switch (r.Type)
            {
                case ValueType.Empty:
                case ValueType.Null:
                    break;

                case ValueType.Int16:
                    r.Value = reader.ReadInt16();
                    reader.ReadInt16();
                    break;

                case ValueType.Int32:
                case ValueType.Int:
                    r.Value = reader.ReadInt32();
                    break;

                case ValueType.Single:
                    r.Value = reader.ReadSingle();
                    break;

                case ValueType.Double:
                    r.Value = reader.ReadDouble();
                    break;

                case ValueType.Currency:
                    r.Value = reader.ReadInt64() * 0.0001m;
                    break;

                case ValueType.Date:
                    r.Value = new DateTime(1899, 12, 30).AddDays(reader.ReadDouble());
                    break;

                case ValueType.CodePageString:
                case ValueType.String:
                case ValueType.Stream:
                case ValueType.Storage:
                case ValueType.StreamedObject:
                case ValueType.StoredObject:
                    r.Value = reader.ReadAnsiString(ref bytes, length: (reader.ReadInt32() + 3) & ~3);
                    break;

                case ValueType.Bool:
                    r.Value = reader.ReadByte() > 0;
                    reader.ReadByte();
                    reader.ReadInt16();
                    break;

                case ValueType.Decimal:
                    r.Value = reader.ReadDecimal();
                    break;

                case ValueType.SByte:
                    r.Value = reader.ReadSByte();
                    reader.ReadByte();
                    reader.ReadInt16();
                    break;

                case ValueType.Byte:
                    r.Value = reader.ReadByte();
                    reader.ReadByte();
                    reader.ReadInt16();
                    break;

                case ValueType.UInt16:
                    r.Value = reader.ReadUInt16();
                    reader.ReadInt16();
                    break;

                case ValueType.UInt32:
                case ValueType.UInt:
                case ValueType.Error:
                    r.Value = reader.ReadUInt32();
                    break;

                case ValueType.UInt64:
                    r.Value = reader.ReadUInt64();
                    break;

                case ValueType.Int64:
                    r.Value = reader.ReadInt64();
                    break;

                case ValueType.UnicodeString:
                    r.Value = reader.ReadUnicodeString(ref sb, length: (reader.ReadInt32() + 1) & ~1);
                    break;

                case ValueType.FileTime:
                    r.Value = DateTime.FromFileTimeUtc(reader.ReadInt64());
                    break;

                case ValueType.Blob:
                case ValueType.BlobObject:
                    {
                        var l = reader.ReadInt32();
                        r.Value = reader.ReadBytes(l);
                        for (var i = l % 4; 0 < i && i < 4; i++)
                        {
                            reader.ReadByte();
                        }
                        break;
                    }

                case ValueType.ClsID:
                    r.Value = reader.ReadGuid();
                    break;

                default:
                    throw new NotImplementedException($"Unimplemented Type 0x{r.Type:X}. See [MS-OLEPS] 2.15");
            }

            return r;
        }

        internal void WriteTo(BinaryWriter writer)
        {
            writer.Write((short)Type);
            writer.Write((short)0);

            switch (Type)
            {
                case ValueType.Empty:
                case ValueType.Null:
                    break;

                case ValueType.Int16:
                    writer.Write((short)Value);
                    break;

                case ValueType.Int32:
                case ValueType.Int:
                    writer.Write((int)Value);
                    break;

                case ValueType.Single:
                    writer.Write((float)Value);
                    break;

                case ValueType.Double:
                    writer.Write((double)Value);
                    break;

                case ValueType.Currency:
                    writer.Write((long)((decimal)Value * 10000));
                    break;

                case ValueType.Date:
                    writer.Write((((DateTime)Value) - new DateTime(1899, 12, 30)).TotalDays);
                    break;

                case ValueType.CodePageString:
                case ValueType.String:
                case ValueType.Stream:
                case ValueType.Storage:
                case ValueType.StreamedObject:
                case ValueType.StoredObject:
                    if (Value != null)
                    {
                        var ansi = Encoding.Default.GetBytes((string)Value);
                        writer.Write(ansi.Length);
                        writer.Write(ansi, (ansi.Length + 4) & ~3);
                    }
                    else
                    {
                        writer.Write(0);
                    }
                    break;

                case ValueType.Bool:
                    writer.Write((bool)Value ? (byte)0xff : (byte)0);
                    writer.Write((byte)0);
                    writer.Write((short)0);
                    break;

                case ValueType.Decimal:
                    writer.Write((decimal)Value);
                    break;

                case ValueType.SByte:
                    writer.Write((sbyte)Value);
                    writer.Write((byte)0);
                    writer.Write((short)0);
                    break;

                case ValueType.Byte:
                    writer.Write((byte)Value);
                    writer.Write((byte)0);
                    writer.Write((short)0);
                    break;

                case ValueType.UInt16:
                    writer.Write((ushort)Value);
                    writer.Write((short)0);
                    break;

                case ValueType.UInt32:
                case ValueType.UInt:
                case ValueType.Error:
                    writer.Write((uint)Value);
                    break;

                case ValueType.UInt64:
                    writer.Write((ulong)Value);
                    break;

                case ValueType.Int64:
                    writer.Write((long)Value);
                    break;

                case ValueType.UnicodeString:
                    if (Value != null)
                    {
                        var s = (string)Value;
                        writer.Write(s.Length);
                        writer.Write(s, (s.Length + 2) & ~1);
                    }
                    else
                    {
                        writer.Write(0);
                    }
                    break;

                case ValueType.FileTime:
                    writer.Write(((DateTime)Value).ToFileTimeUtc());
                    break;

                case ValueType.Blob:
                case ValueType.BlobObject:
                    if (Value != null)
                    {
                        var data = (byte[])Value;
                        writer.Write(data.Length);
                        writer.Write(data, (data.Length + 3) & ~3);
                    }
                    else
                    {
                        writer.Write(0);
                    }
                    break;
                case ValueType.ClsID:
                    writer.Write((Guid)Value);
                    break;

                default:
                    throw new NotImplementedException("[MS-OLEPS] 2.15");
            }
        }
    }
}
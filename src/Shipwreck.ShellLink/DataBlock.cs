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
        internal DataBlock()
        {
        }

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

        internal static IEnumerable<DataBlock> Parse(BinaryReader reader, ref byte[] bytes, ref StringBuilder sb)
        {
            var l = new List<DataBlock>(1);
            for (var bs = reader.ReadInt32(); bs > 4; bs = reader.ReadInt32())
            {
                var sig = reader.ReadUInt32();

                DataBlock db;
                switch (sig)
                {
                    case EnvironmentVariableDataBlock.SIGNATURE:
                        db = EnvironmentVariableDataBlock.Parse(reader, ref bytes, ref sb);
                        break;

                    case ConsoleDataBlock.SIGNATURE:
                        db = ConsoleDataBlock.Parse(reader, ref sb);
                        break;

                    case ConsoleFEDataBlock.SIGNATURE:
                        db = ConsoleFEDataBlock.Parse(reader);
                        break;

                    case DarwinDataBlock.SIGNATURE:
                        db = DarwinDataBlock.Parse(reader, ref bytes, ref sb);
                        break;

                    case IconEnvironmentDataBlock.SIGNATURE:
                        db = IconEnvironmentDataBlock.Parse(reader, ref bytes, ref sb);
                        break;

                    case KnownFolderDataBlock.SIGNATURE:
                        db = KnownFolderDataBlock.Parse(reader);
                        break;

                    case PropertyStoreDataBlock.SIGNATURE:
                        db = PropertyStoreDataBlock.Parse(reader, ref bytes, ref sb);
                        break;

                    case ShimDataBlock.SIGNATURE:
                        db = ShimDataBlock.Parse(reader, bs, ref sb);
                        break;

                    case SpecialFolderDataBlock.SIGNATURE:
                        db = SpecialFolderDataBlock.Parse(reader);
                        break;

                    case TrackerDataBlock.SIGNATURE:
                        db = TrackerDataBlock.Parse(reader, bs, ref bytes);
                        break;

                    case VistaAndAboveIDListDataBlock.SIGNATURE:
                        db = VistaAndAboveIDListDataBlock.Parse(reader);
                        break;

                    default:
                        db = new UnknownDataBlock()
                        {
                            Signature = sig,
                            Data = reader.ReadBytes(bs - 8)
                        };
                        break;
                }

                l.Add(db);
            }
            return l;
        }
    }
}
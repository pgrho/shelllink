using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Shipwreck.ShellLink
{
    public sealed class ShellShortcut
    {
        public FileAttributesFlags FileAttributes { get; set; }

        public DateTime? CreationTime { get; set; }
        public DateTime? AccessTime { get; set; }
        public DateTime? WriteTime { get; set; }
        public int FileSize { get; set; }
        public int IconIndex { get; set; }
        public ShowCommand ShowCommand { get; set; }
        public Key HotKey { get; set; }
        public Modifiers HotKeyModifiers { get; set; }
        public LinkInfo LinkInfo { get; set; }
        public IList<byte[]> LinkTargetIDList { get; set; }
        public string Name { get; private set; }
        public string RelativePath { get; private set; }
        public string WorkingDir { get; private set; }
        public string Arguments { get; private set; }
        public string IconLocation { get; private set; }

        public static ShellShortcut Load(string fileName)
        {
            using (var fs = new FileStream(fileName, FileMode.Open))
            {
                return Load(fs);
            }
        }

        public static ShellShortcut Load(Stream stream)
        {
            using (var br = new BinaryReader(stream, Encoding.Default, true))
            {
                return Load(br);
            }
        }

        public static ShellShortcut Load(BinaryReader reader)
        {
            if (reader.ReadInt32() != 0x4c)
            {
                throw new FormatException("ファイルヘッダーが無効です。");
            }
            var a = reader.ReadInt32();
            var b = reader.ReadInt32();
            var c = reader.ReadInt32();
            var d = reader.ReadInt32();

            if (a != 0x00021401
                || b != 0
                || c != 0xC0
                || d != 0x46000000)
            {
                throw new FormatException("ファイルヘッダーが無効です。");
            }

            var r = new ShellShortcut();

            var flags = (LinkFlags)reader.ReadInt32();

            r.FileAttributes = (FileAttributesFlags)reader.ReadInt32();

            var crt = reader.ReadInt64();
            r.CreationTime = crt != 0 ? DateTime.FromFileTimeUtc(crt) : (DateTime?)null;
            var acc = reader.ReadInt64();
            r.AccessTime = acc != 0 ? DateTime.FromFileTimeUtc(acc) : (DateTime?)null;
            var wrt = reader.ReadInt64();
            r.WriteTime = wrt != 0 ? DateTime.FromFileTimeUtc(wrt) : (DateTime?)null;

            r.FileSize = reader.ReadInt32();
            r.IconIndex = reader.ReadInt32();

            r.ShowCommand = (ShowCommand)reader.ReadInt32();

            r.HotKey = (Key)reader.ReadByte();
            r.HotKeyModifiers = (Modifiers)reader.ReadByte();

            reader.ReadInt16();
            reader.ReadInt32();
            reader.ReadInt32();

            byte[] bytes = null;
            StringBuilder sb = null;
            if ((flags & LinkFlags.HasLinkTargetIDList) == LinkFlags.HasLinkTargetIDList)
            {
                r.LinkTargetIDList = reader.ReadIDList();
            }
            if ((flags & LinkFlags.HasLinkInfo) == LinkFlags.HasLinkInfo)
            {
                r.LinkInfo = LinkInfo.Parse(reader, ref bytes, ref sb);
            }
            var isUnicode = (flags & LinkFlags.IsUnicode) == LinkFlags.IsUnicode;
            if ((flags & LinkFlags.HasName) == LinkFlags.HasName)
            {
                r.Name = reader.ReadStringData(isUnicode, ref sb, ref bytes);
            }
            if ((flags & LinkFlags.HasRelativePath) == LinkFlags.HasRelativePath)
            {
                r.RelativePath = reader.ReadStringData(isUnicode, ref sb, ref bytes);
            }
            if ((flags & LinkFlags.HasWorkingDir) == LinkFlags.HasWorkingDir)
            {
                r.WorkingDir = reader.ReadStringData(isUnicode, ref sb, ref bytes);
            }
            if ((flags & LinkFlags.HasArguments) == LinkFlags.HasArguments)
            {
                r.Arguments = reader.ReadStringData(isUnicode, ref sb, ref bytes);
            }
            if ((flags & LinkFlags.HasIconLocation) == LinkFlags.HasIconLocation)
            {
                r.IconLocation = reader.ReadStringData(isUnicode, ref sb, ref bytes);
            }

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

                r.ExtraData.Add(db);
            }

            return r;
        }

        private Collection<DataBlock> _ExtraData;

        public Collection<DataBlock> ExtraData
        {
            get
            {
                return _ExtraData ?? (_ExtraData = new Collection<DataBlock>());
            }
            set
            {
                if (value == _ExtraData)
                {
                    return;
                }
                _ExtraData?.Clear();
                if (value?.Count > 0)
                {
                    foreach (var item in value)
                    {
                        ExtraData.Add(item);
                    }
                }
            }
        }
    }
}
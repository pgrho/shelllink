using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Shipwreck.ShellLink
{
    public sealed class ConsoleDataBlock : DataBlock
    {
        internal const uint SIGNATURE = 0xA0000002;

        public FillAttributes Fill { get; set; }
        public FillAttributes PopupFill { get; set; }
        public short ScreenBufferSizeX { get; set; }
        public short ScreenBufferSizeY { get; set; }
        public short WindowSizeX { get; set; }
        public short WindowSizeY { get; set; }
        public short WindowOriginX { get; set; }
        public short WindowOriginY { get; set; }
        public uint FontSize { get; set; }
        public FontFamily FontFamily { get; set; }
        public uint FontWeight { get; set; }
        public string FaceName { get; set; }
        public uint CursorSize { get; set; }
        public bool IsFullScreen { get; set; }
        public bool IsQuickEdit { get; set; }
        public bool IsInsertMode { get; set; }
        public bool IsAutoPosition { get; set; }
        public uint HistoryBufferSize { get; set; }
        public uint NumberOfHistoryBuffers { get; set; }
        public bool IsHistoryNoDup { get; set; }

        public int[] ColorTable { get; set; }

        protected override uint GetSignature()
            => SIGNATURE;

        internal static ConsoleDataBlock Parse(BinaryReader reader, ref StringBuilder sb)
        {
            var r = new ConsoleDataBlock();
            r.Fill = (FillAttributes)reader.ReadInt16();
            r.PopupFill = (FillAttributes)reader.ReadInt16();
            r.ScreenBufferSizeX = reader.ReadInt16();
            r.ScreenBufferSizeY = reader.ReadInt16();
            r.WindowSizeX = reader.ReadInt16();
            r.WindowSizeY = reader.ReadInt16();
            r.WindowOriginX = reader.ReadInt16();
            r.WindowOriginY = reader.ReadInt16();
            reader.ReadInt32();
            reader.ReadInt32();
            r.FontSize = reader.ReadUInt32();
            r.FontFamily = (FontFamily)reader.ReadInt32();
            r.FontWeight = reader.ReadUInt32();
            r.FaceName = reader.ReadUnicodeString(ref sb, 32);
            r.CursorSize = reader.ReadUInt32();
            r.IsFullScreen = reader.ReadUInt32() > 0;
            r.IsQuickEdit = reader.ReadUInt32() > 0;
            r.IsInsertMode = reader.ReadUInt32() > 0;
            r.IsAutoPosition = reader.ReadUInt32() > 0;
            r.HistoryBufferSize = reader.ReadUInt32();
            r.NumberOfHistoryBuffers = reader.ReadUInt32();
            r.IsHistoryNoDup = reader.ReadUInt32() == 0;
            var ct = new int[16];
            for (var i = 0; i < 16; i++)
            {
                ct[i] = reader.ReadInt32();
            }

            r.ColorTable = ct;

            return r;
        }

        protected override void WriteToCore(BinaryWriter writer)
        {
            writer.Write((short)Fill);
            writer.Write((short)PopupFill);
            writer.Write(ScreenBufferSizeX);
            writer.Write(ScreenBufferSizeY);
            writer.Write(WindowSizeX);
            writer.Write(WindowSizeY);
            writer.Write(WindowOriginX);
            writer.Write(WindowOriginY);
            writer.Write(0u);
            writer.Write(0u);
            writer.Write(FontSize);
            writer.Write((int)FontFamily);
            writer.Write(FontWeight);

            writer.Write(FaceName, 32);

            writer.Write(CursorSize);
            writer.Write(IsFullScreen ? 0 : -1);
            writer.Write(IsQuickEdit ? 0 : -1);
            writer.Write(IsInsertMode ? 0 : -1);
            writer.Write(IsAutoPosition ? 0 : -1);
            writer.Write(HistoryBufferSize);
            writer.Write(NumberOfHistoryBuffers);
            writer.Write(IsHistoryNoDup ? -1 : 0);

            writer.Write(ColorTable, 16);
        }
    }
}

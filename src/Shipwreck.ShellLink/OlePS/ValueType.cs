using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Shipwreck.ShellLink.OlePS
{
    public enum ValueType : short
    {
        Empty = 0x0000,
        Null = 0x0001,
        Int16 = 0x0002,
        Int32 = 0x0003,
        Single = 0x0004,
        Double = 0x0005,
        Currency = 0x0006,
        Date = 0x0007,
        CodePageString = 0x0008,
        Error = 0x000A,
        Bool = 0x000B,
        Decimal = 0x000E,
        SByte = 0x0010,
        Byte = 0x0011,
        UInt16 = 0x0012,
        UInt32 = 0x0013,
        Int64 = 0x0014,
        UInt64 = 0x0015,
        Int = 0x0016,
        UInt = 0x0017,
        String = 0x001E,
        UnicodeString = 0x001F,
        FileTime = 0x0040,
        Blob = 0x0041,
        Stream = 0x0042,
        Storage = 0x0043,
        StreamedObject = 0x0044,
        StoredObject = 0x0045,
        BlobObject = 0x0046,
        CF = 0x0047,
        ClsID = 0x0048,
        VERSIONED_STREAM = 0x0049
    }
}

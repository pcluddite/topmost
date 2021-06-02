using System;

namespace Topmost.Interop
{
    [Flags]
    internal enum FORMAT : uint
    {
        MESSAGE_ALLOCATE_BUFFER = 0x00000100,
        MESSAGE_IGNORE_INSERTS  = 0x00000200,
        MESSAGE_FROM_STRING     = 0x00000400,
        MESSAGE_FROM_HMODULE    = 0x00000800,
        MESSAGE_FROM_SYSTEM     = 0x00001000,
        MESSAGE_ARGUMENT_ARRAY  = 0x00002000,
        MESSAGE_MAX_WIDTH_MASK  = 0x000000FF
    }
}

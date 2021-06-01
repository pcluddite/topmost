using System.Runtime.InteropServices;

namespace Topmost.Interop
{
    [StructLayout(LayoutKind.Sequential)]
    internal struct POINT
    {
        public int x;
        public int y;
    }
}

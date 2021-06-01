using System.Runtime.InteropServices;

namespace Topmost.Interop
{
    [StructLayout(LayoutKind.Sequential)]
    internal struct WINDOWPLACEMENT
    {
        public uint length;
        public WPF flags;
        public SW showCmd;
        public POINT ptMinPosition;
        public POINT ptMaxPosition;
        public RECT rcNormalPosition;
        public RECT rcDevice;

        public static WINDOWPLACEMENT Create()
        {
            WINDOWPLACEMENT placement = new WINDOWPLACEMENT();
            placement.length = (uint)Marshal.SizeOf(placement);
            return placement;
        }
    }
}

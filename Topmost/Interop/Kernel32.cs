using System;
using System.Runtime.InteropServices;
using System.Text;

namespace Topmost.Interop
{
    internal static class Kernel32
    {
        [DllImport("kernel32", SetLastError = true, CharSet = CharSet.Unicode)]
        private static extern int FormatMessage(FORMAT dwFlags, IntPtr lpSource, uint dwMessageId, LOCALE dwLanguageId, StringBuilder lpBuffer, uint nSize, IntPtr Arguments);

        public static string FormatMessage(int errorCode)
        {
            StringBuilder lpBuffer = new StringBuilder();
            int nSize = FormatMessage(FORMAT.MESSAGE_ALLOCATE_BUFFER | FORMAT.MESSAGE_FROM_SYSTEM | FORMAT.MESSAGE_IGNORE_INSERTS,
                (IntPtr)null, (uint)errorCode, LOCALE.USER_DEFAULT, lpBuffer, 0, (IntPtr)null);
            if (nSize == 0)
                return null;
            return lpBuffer.ToString(0, nSize);
        }
    }
}

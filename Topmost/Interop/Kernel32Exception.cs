using System;
using System.Runtime.InteropServices;

namespace Topmost.Interop
{
    internal class Kernel32Exception : NativeException
    {
        private const string MODULE = "kernel32.dll";

        public Kernel32Exception()
            : base()
        {
        }

        public Kernel32Exception(int lastError)
            : base(lastError)
        {
        }

        public Kernel32Exception(string message)
            : base(message)
        {
            LastError = Marshal.GetLastWin32Error();
        }

        public Kernel32Exception(string message, int lastError)
            : base(message, lastError)
        {
        }
    }
}

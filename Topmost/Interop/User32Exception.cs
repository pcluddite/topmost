using System;
using System.Runtime.InteropServices;

namespace Topmost.Interop
{
    internal class User32Exception : NativeException
    {
        private const string MODULE = "user32.dll";

        public User32Exception()
            : base()
        {
        }

        public User32Exception(int lastError)
            : base(lastError)
        {
        }

        public User32Exception(string message)
            : base(message)
        {
        }

        public User32Exception(string message, int lastError)
            : base(message, lastError)
        {
        }
    }
}

using System;
using System.Runtime.InteropServices;

namespace Topmost.Interop
{
    internal class User32Exception : Exception
    {
        public int LastError { get; private set;  }

        public User32Exception()
            : this(Marshal.GetLastWin32Error())
        {
        }

        public User32Exception(int lastError)
            : base("An error occurred in user32.dll (error code " + lastError + ")")
        {
        }

        public User32Exception(string message)
            : base(message)
        {
            LastError = Marshal.GetLastWin32Error();
        }

        public User32Exception(string message, int lastError)
            : base(message)
        {
            LastError = lastError;
        }
    }
}

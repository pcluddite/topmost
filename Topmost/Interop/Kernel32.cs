//
//    Topmost
//    Copyright (C) 2014-2021 Timothy Baxendale
//
//    This program is free software: you can redistribute it and/or modify
//    it under the terms of the GNU General Public License as published by
//    the Free Software Foundation, either version 3 of the License, or
//    (at your option) any later version.
//
//    This program is distributed in the hope that it will be useful,
//    but WITHOUT ANY WARRANTY; without even the implied warranty of
//    MERCHANTABILITY or FITNESS FOR A PARTICULAR PURPOSE.  See the
//    GNU General Public License for more details.
//
//    You should have received a copy of the GNU General Public License
//    along with this program.  If not, see <https://www.gnu.org/licenses/>.
//
using System;
using System.Runtime.InteropServices;
using System.Text;

namespace Topmost.Interop
{
    internal static class Kernel32
    {
        [DllImport("kernel32.dll", SetLastError = true, CharSet = CharSet.Unicode)]
        private static extern int FormatMessage(FORMAT_MESSAGE dwFlags, IntPtr lpSource, uint dwMessageId, LOCALE dwLanguageId, ref IntPtr lpBuffer, uint nSize, IntPtr Arguments);
        
        public static string FormatMessage(int errorCode)
        {
            IntPtr lpBuffer = IntPtr.Zero;
            try
            {
                int nSize = FormatMessage(FORMAT_MESSAGE.ALLOCATE_BUFFER | FORMAT_MESSAGE.FROM_SYSTEM | FORMAT_MESSAGE.IGNORE_INSERTS,
                    (IntPtr)null, (uint)errorCode, LOCALE.USER_DEFAULT, ref lpBuffer, 0, (IntPtr)null);
                if (nSize == 0)
                    return null;
                return Marshal.PtrToStringAuto(lpBuffer, nSize).Trim();
            }
            finally
            {
                if (lpBuffer != IntPtr.Zero) Marshal.FreeHGlobal(lpBuffer);
            }
        }
    }
}

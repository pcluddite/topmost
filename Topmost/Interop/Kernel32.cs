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
        private static extern int FormatMessage(FORMAT_MESSAGE dwFlags, IntPtr lpSource, int dwMessageId, LOCALE dwLanguageId, out StringBuilder lpBuffer, uint nSize, IntPtr Arguments);

        public static string FormatMessage(int errorCode)
        {
            StringBuilder lpBuffer = new StringBuilder(1024);
            int nSize = FormatMessage(FORMAT_MESSAGE.ALLOCATE_BUFFER | FORMAT_MESSAGE.FROM_SYSTEM | FORMAT_MESSAGE.IGNORE_INSERTS,
                (IntPtr)null, errorCode, LOCALE.USER_DEFAULT, out lpBuffer, 0, (IntPtr)null);
            if (nSize == 0)
                return null;
            return lpBuffer.ToString().Trim();
        }
    }
}

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

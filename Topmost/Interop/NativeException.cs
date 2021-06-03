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

namespace Topmost.Interop
{
    internal class NativeException : Exception
    {
        public int LastError { get; private set;  }

        public string ModuleName { get; private set; }

        private NativeException(string message)
            : base(message)
        {
        }

        public static NativeException Create(string moduleName, string funcName)
        {
            int lastError = Marshal.GetLastWin32Error();
            string message = Win32.FormatMessage(lastError);
            if (message == null)
                message = "An unknown error occurred when calling " + funcName + " in " + moduleName + " (error code " + lastError + ")";
            return new NativeException(message)
            {
                LastError = lastError,
                ModuleName = moduleName
            };
        }
    }
}

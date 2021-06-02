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
using System.Reflection;
using System.Runtime.InteropServices;

namespace Topmost.Interop
{
    internal abstract class NativeException : Exception
    {
        private const string FIELD_NAME_MODULE = "MODULE";

        private string _message;

        public override string Message { get { return _message; } }

        public virtual int LastError { get; protected set;  }
        
        public virtual string ModuleName
        {
            get
            {
                return GetModuleName(GetType());
            }
        }

        public NativeException()
            : this(Marshal.GetLastWin32Error())
        {
        }

        public NativeException(string message)
            : this(message, Marshal.GetLastWin32Error())
        {
        }

        public NativeException(int lastError)
            : this(null, lastError)
        {
            _message = "An error occurred in " + ModuleName + " (error code " + lastError + ")";
        }

        public NativeException(string message, int lastError)
        {
            _message = message;
            LastError = lastError;
        }

        internal static T CreateException<T>(string funcName) where T : NativeException
        {
            int lastError = Marshal.GetLastWin32Error();
            string message = Kernel32.FormatMessage(lastError);
            if (message == null)
                message = "An unknown error occurred when calling " + funcName + " in " + GetModuleName<T>() + " (error code " + lastError + ")";
            return (T)Activator.CreateInstance(typeof(T), message, lastError);
        }

        private static string GetModuleName(Type t)
        {
            FieldInfo field = t.GetField(FIELD_NAME_MODULE);
            if (!(field != null && field.FieldType == typeof(string) && field.IsLiteral && field.IsStatic))
                return null;
            return (string)field.GetValue(null);
        }   

        private static string GetModuleName<T>() where T : NativeException
        {
            return GetModuleName(typeof(T));
        }
    }
}

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
        }

        public Kernel32Exception(string message, int lastError)
            : base(message, lastError)
        {
        }
    }
}

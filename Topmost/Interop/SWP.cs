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

namespace Topmost.Interop
{
    [Flags]
    internal enum SWP : uint
    {
        NOSIZE          = 0x0001,
        NOMOVE          = 0x0002,
        NOZORDER        = 0x0004,
        NOREDRAW        = 0x0008,
        NOACTIVATE      = 0x0010,
        DRAWFRAME       = 0x0020,
        FRAMECHANGED    = 0x0020,
        SHOWWINDOW      = 0x0040,
        HIDEWINDOW      = 0x0080,
        NOCOPYBITS      = 0x0100,
        NOOWNERZORDER   = 0x0200,
        NOREPOSITION    = 0x0200,
        NOSENDCHANGING  = 0x0400,
        DEFERERASE      = 0x2000,
        ASYNCWINDOWPOS  = 0x4000
    }
}

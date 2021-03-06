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
    internal enum WS_EX : uint
    {
        LEFT                = 0x00000000,
        LTRREADING          = 0x00000000,
        RIGHTSCROLLBAR      = 0x00000000,
        DLGMODALFRAME       = 0x00000001,
        NOPARENTNOTIFY      = 0x00000004,
        TOPMOST             = 0x00000008,
        ACCEPTFILES         = 0x00000010,
        TRANSPARENT         = 0x00000020,
        TOOLWINDOW          = 0x00000080,
        WINDOWEDGE          = 0x00000100,
        CLIENTEDGE          = 0x00000200,
        CONTEXTHELP         = 0x00000400,
        RIGHT               = 0x00001000,
        RTLREADING          = 0x00002000,
        LEFTSCROLLBAR       = 0x00004000,
        CONTROLPARENT       = 0x00010000,
        STATICEDGE          = 0x00020000,
        APPWINDOW           = 0x00040000,
        LAYERED             = 0x00080000,
        NOINHERITLAYOUT     = 0x00100000,
        NOREDIRECTIONBITMAP = 0x00200000,
        LAYOUTRTL           = 0x00400000,
        COMPOSITED          = 0x02000000,
        NOACTIVATE          = 0x08000000,
        OVERLAPPEDWINDOW    = WINDOWEDGE | CLIENTEDGE,
        PALETTEWINDOW       = WINDOWEDGE | TOOLWINDOW | TOPMOST
    }
}

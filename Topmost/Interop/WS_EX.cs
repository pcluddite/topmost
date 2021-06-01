﻿using System;

namespace Topmost.Interop
{
    [Flags]
    internal enum WS_EX : int
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
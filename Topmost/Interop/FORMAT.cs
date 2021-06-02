﻿//
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
    internal enum FORMAT : uint
    {
        MESSAGE_ALLOCATE_BUFFER = 0x00000100,
        MESSAGE_IGNORE_INSERTS  = 0x00000200,
        MESSAGE_FROM_STRING     = 0x00000400,
        MESSAGE_FROM_HMODULE    = 0x00000800,
        MESSAGE_FROM_SYSTEM     = 0x00001000,
        MESSAGE_ARGUMENT_ARRAY  = 0x00002000,
        MESSAGE_MAX_WIDTH_MASK  = 0x000000FF
    }
}

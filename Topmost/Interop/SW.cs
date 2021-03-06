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
    public enum SW : int
    {
        HIDE            = 0,
        SHOWNORMAL      = 1,
        NORMAL          = 1,
        SHOWMINIMIZED   = 2,
        SHOWMAXIMIZED   = 3,
        MAXIMIZE        = 3,
        SHOWNOACTIVE    = 4,
        SHOW            = 5,
        MINIMIZE        = 6,
        SHOWMINNOACTIVE = 7,
        SHOWNA          = 8,
        RESTORE         = 9,
        SHOWDEFAULT     = 10,
        FORCEMINIMIZE   = 11
    }
}

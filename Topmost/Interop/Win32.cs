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
using System.Collections.Generic;
using System.Runtime.InteropServices;
using System.Text;

namespace Topmost.Interop
{
    internal static class Win32
    {
        public const string KERNEL32 = "KERNEL32.DLL";
        public const string USER32 = "USER32.DLL";

        [DllImport(KERNEL32, SetLastError = true, CharSet = CharSet.Unicode)]
        private static extern int FormatMessage(FORMAT_MESSAGE dwFlags, 
                                                IntPtr lpSource,
                                                uint dwMessageId,
                                                LOCALE dwLanguageId,
                                                ref IntPtr lpBuffer,
                                                uint nSize,
                                                IntPtr Arguments);

        public static string FormatMessage(int errorCode)
        {
            IntPtr lpBuffer = IntPtr.Zero;
            try
            {
                int nSize = FormatMessage(FORMAT_MESSAGE.ALLOCATE_BUFFER | FORMAT_MESSAGE.FROM_SYSTEM | FORMAT_MESSAGE.IGNORE_INSERTS,
                    (IntPtr)null, (uint)errorCode, LOCALE.USER_DEFAULT, ref lpBuffer, 0, (IntPtr)null);
                if (nSize == 0)
                    return null;
                return Marshal.PtrToStringAuto(lpBuffer, nSize).Trim();
            }
            finally
            {
                if (lpBuffer != IntPtr.Zero) Marshal.FreeHGlobal(lpBuffer);
            }
        }

        [DllImport(USER32, SetLastError = true)]
        public static extern IntPtr FindWindow(string lpClassName, string lpWindowName);

        [DllImport(USER32, SetLastError = true)]
        private static extern int GetWindowLong(IntPtr hWnd, GWL nIndex);

        public static WS_EX GetExStyle(IntPtr hWnd)
        {
            return (WS_EX)GetWindowLong(hWnd, GWL.EXSTYLE);
        }

        [DllImport(USER32, SetLastError = true)]
        [return: MarshalAs(UnmanagedType.Bool)]
        private static extern bool GetWindowPlacement(IntPtr hWnd, out WINDOWPLACEMENT lpwndpl);

        public static WINDOWPLACEMENT GetWindowPlacement(IntPtr hWnd)
        {
            WINDOWPLACEMENT plac = WINDOWPLACEMENT.Create();
            if (!GetWindowPlacement(hWnd, out plac))
                throw NativeException.Create(USER32, "GetWindowPlacement");
            return plac;
        }

        public static bool IsMaximized(IntPtr hWnd)
        {
            return GetWindowPlacement(hWnd).showCmd == SW.SHOWMAXIMIZED;
        }

        public static bool IsMinimized(IntPtr hWnd)
        {
            return GetWindowPlacement(hWnd).showCmd == SW.SHOWMINIMIZED;
        }

        [DllImport(USER32, SetLastError = true, CharSet = CharSet.Auto)]
        private static extern int GetWindowText(IntPtr hWnd, StringBuilder lpString, int nMaxCount);

        public static string GetWindowText(IntPtr hWnd)
        {
            int size = GetWindowTextLength(hWnd);
            if (size == 0)
                return null;
            StringBuilder sb = new StringBuilder(size + 1);
            if (GetWindowText(hWnd, sb, size + 1) == 0)
                throw NativeException.Create(USER32, "GetWindowText");
            return sb.ToString();
        }

        [DllImport(USER32, SetLastError = true, CharSet = CharSet.Auto)]
        private static extern int GetWindowTextLength(IntPtr hWnd);

        [DllImport(USER32)]
        [return: MarshalAs(UnmanagedType.Bool)]
        public static extern bool IsWindowEnabled(IntPtr hWnd);

        [DllImport(USER32)]
        [return: MarshalAs(UnmanagedType.Bool)]
        public static extern bool IsWindowVisible(IntPtr hWnd);

        [DllImport(USER32, SetLastError = true)]
        [return: MarshalAs(UnmanagedType.Bool)]
        private static extern bool SetWindowPos(IntPtr hWnd, IntPtr hWndInsertAfter, int X, int Y, int cx, int cy, SWP uFlags);

        public static void SetZOrder(IntPtr hWnd, HWND hWndInsertAfter)
        {
            if (!SetWindowPos(hWnd, (IntPtr)hWndInsertAfter, 0, 0, 0, 0, SWP.NOMOVE | SWP.NOSIZE | SWP.NOACTIVATE))
                throw NativeException.Create(USER32, "SetWindowPos");
        }

        public static IEnumerable<IntPtr> EnumWindows()
        {
            return new WindowEnum();
        }

        private class WindowEnum : IEnumerable<IntPtr>
        {
            private delegate bool CallBackPtr(IntPtr hwnd, int lParam);

            [DllImport(USER32, SetLastError = true)]
            [return: MarshalAs(UnmanagedType.Bool)]
            private static extern bool EnumWindows(CallBackPtr callPtr, int lPar);

            private IList<IntPtr> handles = new List<IntPtr>();

            public WindowEnum()
            {
                if (!EnumWindows(AddHandle, 0))
                    throw NativeException.Create(USER32, "EnumWindows");
            }

            private bool AddHandle(IntPtr hWnd, int lParam)
            {
                if (hWnd == IntPtr.Zero)
                    return false;
                handles.Add(hWnd);
                return true;
            }

            #region IEnumerable<Window> Members

            public IEnumerator<IntPtr> GetEnumerator()
            {
                return handles.GetEnumerator();
            }

            #endregion

            #region IEnumerable Members

            System.Collections.IEnumerator System.Collections.IEnumerable.GetEnumerator()
            {
                return GetEnumerator();
            }

            #endregion
        }
    }
}

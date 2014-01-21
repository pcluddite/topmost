using System;
using System.Collections.Generic;
using System.Runtime.InteropServices;
using System.Text;
using System.Drawing;

namespace Topmost
{
    public class WinList {
        [DllImport("user32.dll")]
        private static extern int EnumWindows(CallBackPtr callPtr, int lPar);

        [DllImport("user32.dll")]
        [return: MarshalAs(UnmanagedType.Bool)]
        static extern bool SetWindowPos(IntPtr hWnd, IntPtr hWndInsertAfter, int X, int Y, int cx, int cy, int uFlags);

        [DllImport("user32.dll", SetLastError = true)]
        [return: MarshalAs(UnmanagedType.Bool)]
        private static extern bool GetWindowPlacement(IntPtr hWnd, out WINDOWPLACEMENT lpwndpl);

        [DllImport("user32.dll")]
        [return: MarshalAs(UnmanagedType.Bool)]
        private static extern bool IsWindowVisible(IntPtr hWnd);

        [DllImport("user32.dll")]
        [return: MarshalAs(UnmanagedType.Bool)]
        static extern bool GetWindowRect(IntPtr hWnd, out RECT lpRect);

        private struct WINDOWPLACEMENT {
            public int length;
            public int flags;
            public int showCmd;
            public Point ptMinPosition;
            public Point ptMaxPosition;
            public Rectangle rcNormalPosition;
        }

        [DllImport("user32.dll")]
        private static extern IntPtr FindWindow(string lpClassName, string lpWindowName);

        [DllImport("user32.dll")]
        [return: MarshalAs(UnmanagedType.Bool)]
        private static extern bool IsWindowEnabled(IntPtr hWnd);

        delegate bool CallBackPtr(int hwnd, int lParam);

        private List<int> list;
        private static CallBackPtr callBackPtr;

        [DllImport("user32.dll", SetLastError = true)]
        static extern IntPtr GetWindow(IntPtr hWnd, int uCmd);

        [DllImport("user32.dll")]
        private static extern IntPtr GetForegroundWindow();

        public WinList() {
            list = new List<int>();
        }

        private bool Report(int hwnd, int lParam) {
            list.Add(hwnd);
            return true;
        }

        public int WinGetState(IntPtr hwnd) {
            IntPtr exists = GetWindow(hwnd, 0);
            int state = 1;
            if (exists == (IntPtr)0) { return 0; }
            if (IsWindowVisible(hwnd)) { state += 2; }
            if (IsWindowEnabled(hwnd)) { state += 4; }
            if (GetForegroundWindow() == hwnd) { state += 8; }
            WINDOWPLACEMENT plac;
            GetWindowPlacement(hwnd, out plac);
            if (plac.showCmd == 2) { state += 16; }
            if (plac.showCmd == 3) { state += 32; }
            return state;
        }

        public List<int> List() {
            list.Clear();
            callBackPtr = new CallBackPtr(Report);
            EnumWindows(callBackPtr, 0);
            return list;
        }

        public List<string> List(int flag) {
            List<int> list = this.List();
            List<string> returnList = new List<string>();
            foreach (int hwnd in list) {
                if ((WinGetState((IntPtr)hwnd) & flag) == flag) {
                    string title = WinGetTitle((IntPtr)hwnd);
                    if (title.Trim().CompareTo("") == 0) { continue; }
                    returnList.Add(title);
                }
            }
            return returnList;
        }

        [DllImport("user32.dll", CharSet = CharSet.Auto)]
        private static extern int GetWindowTextLength(IntPtr hWnd);

        [DllImport("user32.dll", CharSet = CharSet.Auto)]
        private static extern int GetWindowText(IntPtr hWnd, StringBuilder lpString, int nMaxCount);

        public string WinGetTitle(IntPtr hwnd) {
            int capacity = GetWindowTextLength(hwnd) + 1;
            StringBuilder sb = new StringBuilder(capacity);
            GetWindowText(hwnd, sb, capacity);
            return sb.ToString();
        }

        public void SetTopmost(IntPtr hwnd) {
            WINDOWPLACEMENT loc = new WINDOWPLACEMENT();
            loc.length = Marshal.SizeOf(loc);
            if (!GetWindowPlacement(hwnd, out loc)) {
                throw new Exception("Unable to get the window placement!");
            }
            RECT rect = new RECT();
            GetWindowRect(hwnd, out rect);
            SetWindowPos(hwnd, (IntPtr)SpecialWindowHandles.HWND_TOPMOST,
                loc.rcNormalPosition.X, loc.rcNormalPosition.Y, (rect.Right - rect.Left), (rect.Bottom - rect.Top), SetWindowPosFlags.NOACTIVATE);
        }

        public void RemoveTopmost(IntPtr hwnd) {
            WINDOWPLACEMENT loc = new WINDOWPLACEMENT();
            loc.length = Marshal.SizeOf(loc);
            if (!GetWindowPlacement(hwnd, out loc)) {
                throw new Exception("Unable to get the window placement!");
            }
            RECT rect = new RECT();
            GetWindowRect(hwnd, out rect);
            SetWindowPos(hwnd, (IntPtr)SpecialWindowHandles.HWND_NOTOPMOST,
                loc.rcNormalPosition.X, loc.rcNormalPosition.Y, (rect.Right - rect.Left), (rect.Bottom - rect.Top), SetWindowPosFlags.NOACTIVATE);
        }

        public IntPtr WinGetHandle(string title) {
            return FindWindow(null, title);
        }

        [StructLayout(LayoutKind.Sequential)]
        public struct RECT {
            public int Left;        // x position of upper-left corner
            public int Top;         // y position of upper-left corner
            public int Right;       // x position of lower-right corner
            public int Bottom;      // y position of lower-right corner
        }
    }

    public static class SetWindowPosFlags {
        public static readonly int
        NOSIZE = 0x0001,
        NOMOVE = 0x0002,
        NOZORDER = 0x0004,
        NOREDRAW = 0x0008,
        NOACTIVATE = 0x0010,
        DRAWFRAME = 0x0020,
        FRAMECHANGED = 0x0020,
        SHOWWINDOW = 0x0040,
        HIDEWINDOW = 0x0080,
        NOCOPYBITS = 0x0100,
        NOOWNERZORDER = 0x0200,
        NOREPOSITION = 0x0200,
        NOSENDCHANGING = 0x0400,
        DEFERERASE = 0x2000,
        ASYNCWINDOWPOS = 0x4000;
    }

    public enum SpecialWindowHandles {
        // ReSharper disable InconsistentNaming
        /// <summary>
        ///     Places the window at the bottom of the Z order. If the hWnd parameter identifies a topmost window, the window loses its topmost status and is placed at the bottom of all other windows.
        /// </summary>
        HWND_TOP = 0,
        /// <summary>
        ///     Places the window above all non-topmost windows (that is, behind all topmost windows). This flag has no effect if the window is already a non-topmost window.
        /// </summary>
        HWND_BOTTOM = 1,
        /// <summary>
        ///     Places the window at the top of the Z order.
        /// </summary>
        HWND_TOPMOST = -1,
        /// <summary>
        ///     Places the window above all non-topmost windows. The window maintains its topmost position even when it is deactivated.
        /// </summary>
        HWND_NOTOPMOST = -2
        // ReSharper restore InconsistentNaming
    }
}

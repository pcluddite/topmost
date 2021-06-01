using System;
using System.Text;
using Topmost.Interop;
using System.Collections.Generic;

namespace Topmost
{
    public class Window : IEquatable<Window>
    {
        public IntPtr Handle { get; private set; }

        protected bool Valid { get { return Handle != IntPtr.Zero; } }

        public bool Visible { get { return User32.IsWindowVisible(Handle); } }

        public bool Enabled { get { return User32.IsWindowEnabled(Handle); } }

        public bool Maximized { get { return GetPlacement().showCmd == SW.SHOWMAXIMIZED; } }

        public bool Minimized { get { return GetPlacement().showCmd == SW.SHOWMINIMIZED; } }

        public string Title
        {
            get
            {
                int size = User32.GetWindowTextLength(Handle);
                if (size == 0)
                    return null;
                StringBuilder sb = new StringBuilder(size + 1);
                if (User32.GetWindowText(Handle, sb, size + 1) > 0)
                    return sb.ToString();
                return string.Empty;
            }
        }

        public bool Topmost
        {
            get
            {
                WS_EX style = (WS_EX)User32.GetWindowLong(Handle, GWL.STYLE);
                return (style & WS_EX.TOPMOST) == WS_EX.TOPMOST;
            }
            set
            {
                IntPtr hWndInsertAfter = (IntPtr)(value ? HWND.TOPMOST : HWND.NOTOPMOST);
                if (!User32.SetWindowPos(Handle, hWndInsertAfter, 0, 0, 0, 0, SWP.NOMOVE | SWP.NOSIZE | SWP.NOACTIVATE))
                    throw new InvalidOperationException();
            }
        }

        private RECT GetRectangle()
        {
            RECT rect;
            if (!User32.GetWindowRect(Handle, out rect))
                throw new InvalidOperationException();
            return rect;
        }

        private WINDOWPLACEMENT GetPlacement()
        {
            WINDOWPLACEMENT placement = WINDOWPLACEMENT.Create();
            if (!User32.GetWindowPlacement(Handle, out placement))
                throw new InvalidOperationException();
            return placement;
        }

        protected Window(IntPtr hWnd)
        {
            Handle = hWnd;
        }

        public static Window FindWindow(string title)
        {
            return FindWindow(title, null);
        }

        public static Window FindWindow(string title, string className)
        {
            Window w = new Window(User32.FindWindow(className, title));
            if (w.Valid)
                return w;
            return null;
        }

        public static IEnumerable<Window> GetAllWindows()
        {
            return new WindowEnum();
        }

        public override string ToString()
        {
            if (Valid)
            {
                string title = Title;
                if (title != null)
                    return title;
            }
            return "<handle> " + Handle;
        }

        public override int GetHashCode()
        {
            return Handle.GetHashCode();
        }

        #region IEquatable<Window> Members

        public override bool Equals(object obj)
        {
            return Equals(obj as Window);
        }

        public bool Equals(Window other)
        {
            if (other == null)
                return false;
            return Handle == other.Handle;
        }

        #endregion

        private class WindowEnum : IEnumerable<Window>
        {
            private IList<IntPtr> handles = new List<IntPtr>();

            public WindowEnum()
            {
                if (!User32.EnumWindows(AddHandle, 0))
                    throw new InvalidOperationException();
            }

            private bool AddHandle(int hWnd, int lParam)
            {
                if (hWnd == 0)
                    return false;
                handles.Add(new IntPtr(hWnd));
                return true;
            }

            #region IEnumerable<Window> Members

            public IEnumerator<Window> GetEnumerator()
            {
                foreach (IntPtr hWnd in handles)
                    yield return new Window(hWnd);
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

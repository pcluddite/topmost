using System;
using System.Collections.Generic;
using System.Linq;
using Topmost.Interop;

namespace Topmost
{
    public class Window : IEquatable<Window>
    {
        public IntPtr Handle { get; private set; }

        public bool Visible { get { return User32.IsWindowVisible(Handle); } }

        public bool Enabled { get { return User32.IsWindowEnabled(Handle); } }

        public bool Maximized { get { return User32.IsMaximized(Handle); } }

        public bool Minimized { get { return User32.IsMinimized(Handle); } }

        public string Title { get { return User32.GetWindowText(Handle); } }

        public bool Topmost
        {
            get
            {
                return (User32.GetExStyle(Handle) & WS_EX.TOPMOST) == WS_EX.TOPMOST;
            }
            set
            {
                User32.SetZOrder(Handle, value ? HWND.TOPMOST : HWND.NOTOPMOST);
            }
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
            IntPtr hWnd = User32.FindWindow(className, title);
            if (hWnd == (IntPtr)null)
                throw NativeException.CreateException<User32Exception>("FindWindow");
            return new Window(hWnd);
        }

        public static IEnumerable<Window> GetAllWindows()
        {
            return User32.EnumWindows().Select(hWnd => new Window(hWnd));
        }

        public override string ToString()
        {
            string title = Title;
            if (title == null) 
                return "<HWND> 0x" + Handle.ToString("X").PadLeft(8, '0');
            return title;
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
    }
}

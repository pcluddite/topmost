using System;
using System.Collections.Generic;
using System.Linq;
using System.Windows.Forms;

namespace Topmost
{
    public partial class MainForm : Form
    {
        public MainForm()
        {
            InitializeComponent();
        }

        private void Form1_Load(object sender, EventArgs e)
        {
            refreshTimer.Start();
        }

        private void timer1_Tick(object sender, EventArgs e)
        {
            IEnumerable<Window> newWindows = Window.GetAllWindows().Where(wnd => wnd.Visible && wnd.Title != null);
            IEnumerable<Window> oldWindows = checkedListBox.Items.Cast<Window>().ToList();

            foreach (Window wnd in newWindows.Except(oldWindows))
            {
                checkedListBox.Items.Add(wnd);
            }

            foreach (Window wnd in oldWindows.Except(newWindows))
            {
                checkedListBox.Items.Remove(wnd);
            }
        }

        private void checkedListBox1_ItemCheck(object sender, ItemCheckEventArgs e)
        {
            CheckedListBox listBox = (CheckedListBox)sender;
            ((Window)listBox.Items[e.Index]).Topmost = (e.NewValue == CheckState.Checked);
        }

        private void notifyIcon1_DoubleClick(object sender, EventArgs e)
        {
            this.Show();
        }

        private void Form1_Resize(object sender, EventArgs e)
        {
            if (WindowState == FormWindowState.Minimized)
            {
                this.Hide();
            }
        }

        private void Form1_VisibleChanged(object sender, EventArgs e)
        {
            if (this.Visible && WindowState == FormWindowState.Minimized)
            {
                WindowState = FormWindowState.Normal;
            }
        }
    }
}

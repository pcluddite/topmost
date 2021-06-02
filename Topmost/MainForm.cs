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
using System.Linq;
using System.Windows.Forms;

namespace Topmost
{
    public partial class MainForm : Form
    {
        public MainForm()
        {
            InitializeComponent();
            Refresh();
        }

        private void MainForm_Load(object sender, EventArgs e)
        {
            refreshTimer.Start();
        }

        private void refreshTimer_Tick(object sender, EventArgs e)
        {
            Refresh();
        }

        public override void Refresh()
        {
            base.Refresh();
            IEnumerable<Window> newWindows = Window.GetAllWindows().Where(wnd => wnd.Visible && wnd.Handle != Handle && wnd.Title != null);
            ISet<Window> selected = new HashSet<Window>(windowListView.SelectedItems.Cast<ListViewItem>().Select(item => (Window)item.Tag));
            windowListView.Items.Clear();
            int zOrder = 0;
            foreach (Window wnd in newWindows)
            {
                ListViewItem item = GetListViewItem(++zOrder, wnd);
                if (selected.Contains(wnd))
                    item.Selected = true;
                windowListView.Items.Add(item);
            }
        }

        private ListViewItem GetListViewItem(int zOrder, Window wnd)
        {
            ListViewItem item = new ListViewItem();
            item.Text = (wnd.Topmost ? -1 : zOrder).ToString();
            item.SubItems.Add("0x" + wnd.Handle.ToString("X").PadLeft(8, '0'));
            item.SubItems.Add(wnd.Title);
            item.Tag = wnd;
            item.Checked = wnd.Topmost;
            return item;
        }

        private void listView_ItemCheck(object sender, ItemCheckEventArgs e)
        {
            ListView listBox = (ListView)sender;
            ((Window)listBox.Items[e.Index].Tag).Topmost = (e.NewValue == CheckState.Checked);
        }

        private void trayIcon_DoubleClick(object sender, EventArgs e)
        {
            Show();
        }

        private void MainForm_Resize(object sender, EventArgs e)
        {
            if (WindowState == FormWindowState.Minimized)
                Hide();
        }

        private void MainForm_VisibleChanged(object sender, EventArgs e)
        {
            if (Visible)
            {
                if (WindowState == FormWindowState.Minimized)
                    WindowState = FormWindowState.Normal;
                refreshTimer.Start();
            }
            else
            {
                refreshTimer.Stop();
            }
        }
    }
}

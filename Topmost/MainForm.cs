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

        private void MainForm_Load(object sender, EventArgs e)
        {
            refreshTimer.Start();
        }

        private void refreshTimer_Tick(object sender, EventArgs e)
        {
            IEnumerable<Window> newWindows = Window.GetAllWindows().Where(wnd => wnd.Visible && wnd.Title != null);
            IEnumerable<Window> oldWindows = windowListView.Items.Cast<ListViewItem>().Select(i => (Window)i.Tag).ToArray();

            foreach (Window wnd in newWindows.Except(oldWindows))
            {
                if (wnd.Handle != Handle) // don't show this window
                    windowListView.Items.Add(GetListViewItem(wnd));
            }

            foreach (Window wnd in oldWindows.Except(newWindows))
            {
                for (int i = windowListView.Items.Count - 1; i >= 0; --i)
                {
                    if (wnd.Equals(windowListView.Items[i].Tag))
                        windowListView.Items.RemoveAt(i);
                }
            }
        }

        private ListViewItem GetListViewItem(Window wnd)
        {
            ListViewItem item = new ListViewItem();
            item.Text = wnd.Title;
            item.SubItems.Add("0x" + wnd.Handle.ToString("X").PadLeft(8, '0'));
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

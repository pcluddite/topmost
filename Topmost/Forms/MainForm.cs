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
using Topmost.Interop;

namespace Topmost.Forms
{
    internal partial class MainForm : Form
    {
        public MainForm()
        {
            InitializeComponent();
            RefreshList();
        }

        private void MainForm_Load(object sender, EventArgs e)
        {
            refreshTimer.Start();
        }

        private void refreshTimer_Tick(object sender, EventArgs e)
        {
            RefreshList();
        }

        private void RefreshList()
        {
            try
            {
                IEnumerable<Window> currWindows = Window.GetAllWindows().Where(wnd => wnd.Visible && wnd.Title != null && wnd.Handle != Handle);
                IEnumerable<Window> prevWindows = windowListView.Items.Cast<ListViewItem>().Select(i => (Window)i.Tag).ToArray();

                foreach (Window wnd in currWindows)
                {
                    ListViewItem item = null;
                    for (int i = windowListView.Items.Count - 1; i >= 0 && item == null; --i)
                    {
                        if (wnd.Equals(windowListView.Items[i].Tag))
                            item = windowListView.Items[i];
                    }
                    if (item == null)
                    {
                        windowListView.Items.Add(GetListViewItem(wnd));
                    }
                    else
                    {
                        UpdateListViewItem(item);
                    }
                }

                foreach (Window wnd in prevWindows.Except(currWindows))
                {
                    for (int i = windowListView.Items.Count - 1; i >= 0; --i)
                    {
                        if (wnd.Equals(windowListView.Items[i].Tag))
                        {
                            windowListView.Items.RemoveAt(i);
                            break;
                        }
                    }
                }
            }
            catch (NativeException ex)
            {
                EndFatally(ex.Message);
            }
        }

        private void EndFatally(string msg)
        {
            refreshTimer.Stop();
            windowListView.Enabled = false;
            ShowError(msg + Environment.NewLine + "Please restart the application.");
        }

        private void ShowError(string msg)
        {
            MessageBox.Show(this, msg, Text, MessageBoxButtons.OK, MessageBoxIcon.Error);
        }

        private ListViewItem GetListViewItem(Window wnd)
        {
            ListViewItem item = new ListViewItem();
            item.Tag = wnd;
            item.Name = wnd.Handle.ToString();
            item.SubItems.Add("0x" + wnd.Handle.ToString("X").PadLeft(8, '0'));
            UpdateListViewItem(item);
            return item;
        }

        private void UpdateListViewItem(ListViewItem item)
        {
            Window wnd = (Window)item.Tag;
            item.Text = wnd.Title;
            item.Checked = wnd.Topmost;
        }

        private void listView_ItemCheck(object sender, ItemCheckEventArgs e)
        {
            try
            {
                ListView listBox = (ListView)sender;
                ((Window)listBox.Items[e.Index].Tag).Topmost = (e.NewValue == CheckState.Checked);
            }
            catch (NativeException ex)
            {
                EndFatally(ex.Message);
            }
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

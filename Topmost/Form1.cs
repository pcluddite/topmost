using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Windows.Forms;

namespace Topmost {
    public partial class Form1 : Form {

        WinList winLister;
        List<string> oldList;

        public Form1() {
            winLister = new WinList();
            oldList = new List<string>();
            InitializeComponent();
        }

        private void Form1_Load(object sender, EventArgs e) {
            timer1.Start();
        }

        private void timer1_Tick(object sender, EventArgs e) {
            List<string> newList = winLister.List(2);
            foreach (string s in newList) {
                if (!oldList.Contains(s)) {
                    checkedListBox1.Items.Add(s);
                }
            }
            foreach (string s in oldList) {
                if (!newList.Contains(s)) {
                    checkedListBox1.Items.Remove(s);
                }
            }
            oldList = newList;
        }

        private void checkedListBox1_ItemCheck(object sender, ItemCheckEventArgs e) {
            if (e.NewValue == CheckState.Checked) {
                winLister.SetTopmost(winLister.WinGetHandle(checkedListBox1.Items[e.Index].ToString()));
            }
            else {
                winLister.RemoveTopmost(winLister.WinGetHandle(checkedListBox1.Items[e.Index].ToString()));
            }
        }

        private void notifyIcon1_DoubleClick(object sender, EventArgs e) {
            this.Show();
        }

        private void Form1_Resize(object sender, EventArgs e) {
            if (WindowState == FormWindowState.Minimized) {
                this.Hide();
            }
        }

        private void Form1_VisibleChanged(object sender, EventArgs e) {
            if (this.Visible && WindowState == FormWindowState.Minimized) {
                WindowState = FormWindowState.Normal;
            }
        }
    }
}

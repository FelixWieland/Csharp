using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using System.Threading;

namespace Labyrinth {
    public partial class Form1 : Form {
        public Labyrinth dun;

        public Form1() {
            InitializeComponent();
            Thread check = new Thread(checker);
            check.Start();
        }
        public void checker() {
            while (true) {
                
                Thread.Sleep(1000);
            }
        }

        private void generate_btn_Click(object sender, EventArgs e) {
            int size = 35;
            this.Height = size * 25;
            this.Width = size* 25 + 10;
            dun = new Labyrinth(0, size);
            String data = Visualizer.toHTML(dun.Dungeon, size);
            browser.DocumentText = data;
        }

        private void loadlevel_btn_Click(object sender, EventArgs e) {
            //setzt den Quelltext der Webseite
            browser.DocumentText = "";
        }

        private void browser_DocumentCompleted(object sender, WebBrowserDocumentCompletedEventArgs e) {

        }

        private void showLog_Click(object sender, EventArgs e) {
            try {
                MessageBox.Show(dun.log());
            }
            catch {
                MessageBox.Show("");
            }
            
        }
    }
}

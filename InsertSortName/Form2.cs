using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace InsertSortName
{
    public partial class Form2 : Form
    {
        public Form1 mainForm;
        public string msg;

        public Form2()
        {
            InitializeComponent();
        }

        private void Form2_Shown(object sender, EventArgs e)
        {
            Msg1.Text = msg;
            this.Location = new Point(mainForm.Location.X + mainForm.Width / 3, mainForm.Location.Y + mainForm.Height / 3);
        }

        private void BtnOverwrite_Click(object sender, EventArgs e)
        {
            mainForm.isOverwrite = true;
            if (chkBatch.Checked) mainForm.isBatch = true;
            this.Close();
        }

        private void button1_Click(object sender, EventArgs e)
        {
            mainForm.isOverwrite = false;
            if (chkBatch.Checked) mainForm.isBatch = true;
            this.Close();
        }

        
    }
}

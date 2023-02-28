using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace InsertFileNumber
{
    public partial class Form4 : Form
    {
        string imgPath;
        public Form4()
        {
            InitializeComponent();
        }

        private void Form4_Load(object sender, EventArgs e)
        {
            this.FormBorderStyle = FormBorderStyle.None;
        }

        private void Form4_Shown(object sender, EventArgs e)
        {
            this.Visible = false;
        }

        private void Form4_MouseDown(object sender, MouseEventArgs e)
        {
            //if (e.Button == MouseButtons.Left)
            //{
            //    this.Capture = false;
            //    Message msg = Message.Create(this.Handle, 0XA1, new IntPtr(2), IntPtr.Zero);
            //    this.WndProc(ref msg);
            //}
        }

        public void LoadImage(string path)
        {
            //Console.WriteLine("imageLoad");
            if (path != "")
            {
                var ext = System.IO.Path.GetExtension(path);
                if (ext.Equals(".png", StringComparison.CurrentCultureIgnoreCase) ||
                    ext.Equals(".jpg", StringComparison.CurrentCultureIgnoreCase) ||
                    ext.Equals(".jpeg", StringComparison.CurrentCultureIgnoreCase))
                {
                    if (imgPath == path) return;
                    pictureBox1.Load(path);
                    float width, height;
                    width = pictureBox1.Image.Width;
                    height = pictureBox1.Image.Height;

                    float x, y;
                    x = width / (width + height) * 512;
                    y = height / (width + height) * 512;

                    this.Size = new Size((int)x, (int)y);

                    imgPath = path;
                }

            }
        }

    }
}

using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Diagnostics;
using System.Drawing;
using System.Drawing.Imaging;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Controls;
using System.Windows.Forms;
using static System.Windows.Forms.VisualStyles.VisualStyleElement.Button;

namespace InsertFileNumber
{
    public partial class Form3 : Form
    {
        public string filePath;

        
        public Form3()
        {
            InitializeComponent();
            chkStretch.Checked = InsertFileNumber.Properties.Settings.Default.chkStretch;
            pictureBox1.BackColor = InsertFileNumber.Properties.Settings.Default.ImgBackground;
        }

        private void Form3_Shown(object sender, EventArgs e)
        {
            this.Location = InsertFileNumber.Properties.Settings.Default.ImgViewerLocation;
            this.Size = InsertFileNumber.Properties.Settings.Default.ImgViewerSize;
        }

        public void LoadImage()
        {
            if (filePath != "") 
            {
                var ext = System.IO.Path.GetExtension(filePath);
                if (ext.Equals(".png", StringComparison.CurrentCultureIgnoreCase) || 
                    ext.Equals(".jpg", StringComparison.CurrentCultureIgnoreCase) || 
                    ext.Equals(".jpeg", StringComparison.CurrentCultureIgnoreCase))
                {
                    //System.Windows.Forms.Timer timer;
                    


                    pictureBox1.Load(filePath);
                }

            }
        }

        public void ResetImage()
        {
            string directory = AppDomain.CurrentDomain.BaseDirectory;
            directory = directory + "/empty.png";

            if (!File.Exists(directory))
            {
                Bitmap flag = new Bitmap(512, 512);
                pictureBox1.Image = flag;
                pictureBox1.Image.Save(directory, ImageFormat.Png);
            }

            pictureBox1.Load(directory);
        }

        private void BtnSetBackground_Click(object sender, EventArgs e)
        {
            if (colorDialog1.ShowDialog() == DialogResult.OK)
                pictureBox1.BackColor = colorDialog1.Color;

        }

        private void chkStretch_CheckedChanged(object sender, EventArgs e)
        {
            if (chkStretch.Checked)
                pictureBox1.SizeMode = PictureBoxSizeMode.Zoom;
            else
                pictureBox1.SizeMode = PictureBoxSizeMode.CenterImage;

        }

        private void BtnOpenImg_Click(object sender, EventArgs e)
        {
            if (!File.Exists(filePath))
            {
                return;
            }

            System.Diagnostics.Process.Start(filePath);
        }

        private void BtnOpenPath_Click(object sender, EventArgs e)
        {
            if (!File.Exists(filePath))
            {
                return;
            }

            // combine the arguments together
            // it doesn't matter if there is a space after ','
            string argument = "/select, \"" + filePath + "\"";
            System.Diagnostics.Process.Start("explorer.exe", "/select, \"" + filePath);

            Console.WriteLine(Path.GetDirectoryName(filePath));
            //Process.Start(Path.GetDirectoryName(filePath),argument);
        }

        private void Form3_FormClosing(object sender, FormClosingEventArgs e)
        {
            InsertFileNumber.Properties.Settings.Default.chkStretch = chkStretch.Checked;
            InsertFileNumber.Properties.Settings.Default.ImgBackground = pictureBox1.BackColor;
            InsertFileNumber.Properties.Settings.Default.ImgViewerLocation = this.Location;
            InsertFileNumber.Properties.Settings.Default.ImgViewerSize = this.Size;
            InsertFileNumber.Properties.Settings.Default.Save();
        }
    }
}

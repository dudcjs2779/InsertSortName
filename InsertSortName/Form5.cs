using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace InsertFileNumber2
{
    public partial class Form5 : Form
    {
        Form1 form1;

        bool preCheckedImageList;
        int preImageListWidth;

        public Form5()
        {
            InitializeComponent();

            form1 = Application.OpenForms.OfType<Form1>().First();

            ChkCopyOverwrite.Checked = form1.isCopyOverwrite;
            ChkPreview.Checked = form1.showPreview;
            ChkImageList.Checked = form1.showImageList;
            preCheckedImageList = form1.showImageList;

            int num = form1.previewWidth;
            TxtPreviewWidth.Text = num.ToString();

            num = form1.imageListWidth;
            TxtImageListWidth.Text = num.ToString();
            preImageListWidth = form1.imageListWidth;
        }

        private void ChkImageList_CheckedChanged(object sender, EventArgs e)
        {
            if (preCheckedImageList != ChkImageList.Checked)
            {
                MessageBox.Show("재시작이 필요하며 활성화시 파일을 불러오는데 시간이 오래 걸릴 수 있습니다.");
            }
        }

        private void BtnApply_Click(object sender, EventArgs e)
        {
            form1.isCopyOverwrite = ChkCopyOverwrite.Checked;
            form1.showPreview = ChkPreview.Checked;
            form1.showImageList = ChkImageList.Checked;
            form1.previewWidth = int.Parse(TxtPreviewWidth.Text);
            form1.imageListWidth = int.Parse(TxtImageListWidth.Text);

            if (preCheckedImageList != ChkImageList.Checked || int.Parse(TxtImageListWidth.Text) != preImageListWidth)
            {
                form1.Close();
            }

            this.Close();
        }

    }
}

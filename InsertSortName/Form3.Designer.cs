namespace InsertFileNumber
{
    partial class Form3
    {
        /// <summary>
        /// Required designer variable.
        /// </summary>
        private System.ComponentModel.IContainer components = null;

        /// <summary>
        /// Clean up any resources being used.
        /// </summary>
        /// <param name="disposing">true if managed resources should be disposed; otherwise, false.</param>
        protected override void Dispose(bool disposing)
        {
            if (disposing && (components != null))
            {
                components.Dispose();
            }
            base.Dispose(disposing);
        }

        #region Windows Form Designer generated code

        /// <summary>
        /// Required method for Designer support - do not modify
        /// the contents of this method with the code editor.
        /// </summary>
        private void InitializeComponent()
        {
            this.colorDialog1 = new System.Windows.Forms.ColorDialog();
            this.tableLayoutPanel1 = new System.Windows.Forms.TableLayoutPanel();
            this.flowLayoutPanel1 = new System.Windows.Forms.FlowLayoutPanel();
            this.chkStretch = new System.Windows.Forms.CheckBox();
            this.BtnSetBackground = new System.Windows.Forms.Button();
            this.BtnOpenImg = new System.Windows.Forms.Button();
            this.BtnOpenPath = new System.Windows.Forms.Button();
            this.pictureBox1 = new System.Windows.Forms.PictureBox();
            this.tableLayoutPanel1.SuspendLayout();
            this.flowLayoutPanel1.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.pictureBox1)).BeginInit();
            this.SuspendLayout();
            // 
            // tableLayoutPanel1
            // 
            this.tableLayoutPanel1.ColumnCount = 1;
            this.tableLayoutPanel1.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Percent, 100F));
            this.tableLayoutPanel1.Controls.Add(this.flowLayoutPanel1, 0, 0);
            this.tableLayoutPanel1.Controls.Add(this.pictureBox1, 0, 1);
            this.tableLayoutPanel1.Dock = System.Windows.Forms.DockStyle.Fill;
            this.tableLayoutPanel1.Location = new System.Drawing.Point(0, 0);
            this.tableLayoutPanel1.Name = "tableLayoutPanel1";
            this.tableLayoutPanel1.RowCount = 2;
            this.tableLayoutPanel1.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Absolute, 34F));
            this.tableLayoutPanel1.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Absolute, 20F));
            this.tableLayoutPanel1.Size = new System.Drawing.Size(615, 655);
            this.tableLayoutPanel1.TabIndex = 0;
            // 
            // flowLayoutPanel1
            // 
            this.flowLayoutPanel1.Controls.Add(this.chkStretch);
            this.flowLayoutPanel1.Controls.Add(this.BtnSetBackground);
            this.flowLayoutPanel1.Controls.Add(this.BtnOpenImg);
            this.flowLayoutPanel1.Controls.Add(this.BtnOpenPath);
            this.flowLayoutPanel1.Dock = System.Windows.Forms.DockStyle.Fill;
            this.flowLayoutPanel1.Location = new System.Drawing.Point(0, 0);
            this.flowLayoutPanel1.Margin = new System.Windows.Forms.Padding(0);
            this.flowLayoutPanel1.Name = "flowLayoutPanel1";
            this.flowLayoutPanel1.Size = new System.Drawing.Size(615, 34);
            this.flowLayoutPanel1.TabIndex = 2;
            // 
            // chkStretch
            // 
            this.chkStretch.AutoSize = true;
            this.chkStretch.Checked = true;
            this.chkStretch.CheckState = System.Windows.Forms.CheckState.Checked;
            this.chkStretch.Location = new System.Drawing.Point(5, 5);
            this.chkStretch.Margin = new System.Windows.Forms.Padding(5);
            this.chkStretch.Name = "chkStretch";
            this.chkStretch.Size = new System.Drawing.Size(94, 19);
            this.chkStretch.TabIndex = 1;
            this.chkStretch.Text = "자동 크기";
            this.chkStretch.TextAlign = System.Drawing.ContentAlignment.MiddleCenter;
            this.chkStretch.UseVisualStyleBackColor = true;
            this.chkStretch.CheckedChanged += new System.EventHandler(this.chkStretch_CheckedChanged);
            // 
            // BtnSetBackground
            // 
            this.BtnSetBackground.AutoSize = true;
            this.BtnSetBackground.Location = new System.Drawing.Point(109, 5);
            this.BtnSetBackground.Margin = new System.Windows.Forms.Padding(5);
            this.BtnSetBackground.Name = "BtnSetBackground";
            this.BtnSetBackground.Size = new System.Drawing.Size(97, 25);
            this.BtnSetBackground.TabIndex = 2;
            this.BtnSetBackground.Text = "배경색";
            this.BtnSetBackground.UseVisualStyleBackColor = true;
            this.BtnSetBackground.Click += new System.EventHandler(this.BtnSetBackground_Click);
            // 
            // BtnOpenImg
            // 
            this.BtnOpenImg.AutoSize = true;
            this.BtnOpenImg.Location = new System.Drawing.Point(216, 5);
            this.BtnOpenImg.Margin = new System.Windows.Forms.Padding(5);
            this.BtnOpenImg.Name = "BtnOpenImg";
            this.BtnOpenImg.Size = new System.Drawing.Size(97, 25);
            this.BtnOpenImg.TabIndex = 3;
            this.BtnOpenImg.Text = "이미지 열기";
            this.BtnOpenImg.UseVisualStyleBackColor = true;
            this.BtnOpenImg.Click += new System.EventHandler(this.BtnOpenImg_Click);
            // 
            // BtnOpenPath
            // 
            this.BtnOpenPath.AutoSize = true;
            this.BtnOpenPath.Location = new System.Drawing.Point(323, 5);
            this.BtnOpenPath.Margin = new System.Windows.Forms.Padding(5);
            this.BtnOpenPath.Name = "BtnOpenPath";
            this.BtnOpenPath.Size = new System.Drawing.Size(97, 25);
            this.BtnOpenPath.TabIndex = 4;
            this.BtnOpenPath.Text = "이미지 경로";
            this.BtnOpenPath.UseVisualStyleBackColor = true;
            this.BtnOpenPath.Click += new System.EventHandler(this.BtnOpenPath_Click);
            // 
            // pictureBox1
            // 
            this.pictureBox1.BackColor = System.Drawing.Color.Black;
            this.pictureBox1.Dock = System.Windows.Forms.DockStyle.Fill;
            this.pictureBox1.Location = new System.Drawing.Point(0, 34);
            this.pictureBox1.Margin = new System.Windows.Forms.Padding(0);
            this.pictureBox1.Name = "pictureBox1";
            this.pictureBox1.Size = new System.Drawing.Size(615, 621);
            this.pictureBox1.SizeMode = System.Windows.Forms.PictureBoxSizeMode.Zoom;
            this.pictureBox1.TabIndex = 3;
            this.pictureBox1.TabStop = false;
            // 
            // Form3
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(8F, 15F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(615, 655);
            this.Controls.Add(this.tableLayoutPanel1);
            this.Name = "Form3";
            this.Text = "Picture";
            this.FormClosing += new System.Windows.Forms.FormClosingEventHandler(this.Form3_FormClosing);
            this.Shown += new System.EventHandler(this.Form3_Shown);
            this.tableLayoutPanel1.ResumeLayout(false);
            this.flowLayoutPanel1.ResumeLayout(false);
            this.flowLayoutPanel1.PerformLayout();
            ((System.ComponentModel.ISupportInitialize)(this.pictureBox1)).EndInit();
            this.ResumeLayout(false);

        }

        #endregion
        private System.Windows.Forms.ColorDialog colorDialog1;
        private System.Windows.Forms.TableLayoutPanel tableLayoutPanel1;
        private System.Windows.Forms.CheckBox chkStretch;
        private System.Windows.Forms.FlowLayoutPanel flowLayoutPanel1;
        private System.Windows.Forms.Button BtnSetBackground;
        private System.Windows.Forms.Button BtnOpenImg;
        private System.Windows.Forms.Button BtnOpenPath;
        private System.Windows.Forms.PictureBox pictureBox1;
    }
}
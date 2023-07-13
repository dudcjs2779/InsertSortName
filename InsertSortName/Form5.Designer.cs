namespace InsertFileNumber2
{
    partial class Form5
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
            this.ChkCopyOverwrite = new System.Windows.Forms.CheckBox();
            this.ChkPreview = new System.Windows.Forms.CheckBox();
            this.ChkImageList = new System.Windows.Forms.CheckBox();
            this.TxtPreviewWidth = new System.Windows.Forms.TextBox();
            this.label1 = new System.Windows.Forms.Label();
            this.BtnApply = new System.Windows.Forms.Button();
            this.label2 = new System.Windows.Forms.Label();
            this.TxtImageListWidth = new System.Windows.Forms.TextBox();
            this.SuspendLayout();
            // 
            // ChkCopyOverwrite
            // 
            this.ChkCopyOverwrite.AutoSize = true;
            this.ChkCopyOverwrite.Location = new System.Drawing.Point(26, 54);
            this.ChkCopyOverwrite.Name = "ChkCopyOverwrite";
            this.ChkCopyOverwrite.Size = new System.Drawing.Size(304, 19);
            this.ChkCopyOverwrite.TabIndex = 0;
            this.ChkCopyOverwrite.Text = "\"폴더로 복사\" 작업시 파일을 덮어씁니다.";
            this.ChkCopyOverwrite.UseVisualStyleBackColor = true;
            // 
            // ChkPreview
            // 
            this.ChkPreview.AutoSize = true;
            this.ChkPreview.Location = new System.Drawing.Point(26, 102);
            this.ChkPreview.Name = "ChkPreview";
            this.ChkPreview.Size = new System.Drawing.Size(419, 19);
            this.ChkPreview.TabIndex = 1;
            this.ChkPreview.Text = "마우스 커서를 번호에 올려놓으면 미리보기를 보여줍니다.";
            this.ChkPreview.UseVisualStyleBackColor = true;
            // 
            // ChkImageList
            // 
            this.ChkImageList.Checked = true;
            this.ChkImageList.CheckState = System.Windows.Forms.CheckState.Checked;
            this.ChkImageList.Location = new System.Drawing.Point(26, 176);
            this.ChkImageList.Name = "ChkImageList";
            this.ChkImageList.Size = new System.Drawing.Size(419, 19);
            this.ChkImageList.TabIndex = 2;
            this.ChkImageList.Text = "리스트 목록에 이미지를 표시합니다.";
            this.ChkImageList.UseVisualStyleBackColor = true;
            this.ChkImageList.CheckedChanged += new System.EventHandler(this.ChkImageList_CheckedChanged);
            // 
            // TxtPreviewWidth
            // 
            this.TxtPreviewWidth.Location = new System.Drawing.Point(207, 129);
            this.TxtPreviewWidth.Name = "TxtPreviewWidth";
            this.TxtPreviewWidth.Size = new System.Drawing.Size(63, 25);
            this.TxtPreviewWidth.TabIndex = 3;
            // 
            // label1
            // 
            this.label1.AutoSize = true;
            this.label1.Location = new System.Drawing.Point(46, 134);
            this.label1.Name = "label1";
            this.label1.Size = new System.Drawing.Size(162, 15);
            this.label1.TabIndex = 4;
            this.label1.Text = "미리보기의 가로 크기: ";
            // 
            // BtnApply
            // 
            this.BtnApply.Font = new System.Drawing.Font("굴림", 9F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(129)));
            this.BtnApply.Location = new System.Drawing.Point(400, 268);
            this.BtnApply.Name = "BtnApply";
            this.BtnApply.Size = new System.Drawing.Size(75, 26);
            this.BtnApply.TabIndex = 5;
            this.BtnApply.Text = "적용";
            this.BtnApply.UseVisualStyleBackColor = true;
            this.BtnApply.Click += new System.EventHandler(this.BtnApply_Click);
            // 
            // label2
            // 
            this.label2.AutoSize = true;
            this.label2.Location = new System.Drawing.Point(46, 207);
            this.label2.Name = "label2";
            this.label2.Size = new System.Drawing.Size(274, 15);
            this.label2.TabIndex = 7;
            this.label2.Text = "리스트 목록 이미지 크기(재시작 필요): ";
            // 
            // TxtImageListWidth
            // 
            this.TxtImageListWidth.Location = new System.Drawing.Point(319, 202);
            this.TxtImageListWidth.Name = "TxtImageListWidth";
            this.TxtImageListWidth.Size = new System.Drawing.Size(63, 25);
            this.TxtImageListWidth.TabIndex = 6;
            // 
            // Form5
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(8F, 15F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(487, 306);
            this.Controls.Add(this.label2);
            this.Controls.Add(this.TxtImageListWidth);
            this.Controls.Add(this.BtnApply);
            this.Controls.Add(this.label1);
            this.Controls.Add(this.TxtPreviewWidth);
            this.Controls.Add(this.ChkImageList);
            this.Controls.Add(this.ChkPreview);
            this.Controls.Add(this.ChkCopyOverwrite);
            this.Name = "Form5";
            this.Text = "Form5";
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private System.Windows.Forms.CheckBox ChkCopyOverwrite;
        private System.Windows.Forms.CheckBox ChkPreview;
        private System.Windows.Forms.CheckBox ChkImageList;
        private System.Windows.Forms.TextBox TxtPreviewWidth;
        private System.Windows.Forms.Label label1;
        private System.Windows.Forms.Button BtnApply;
        private System.Windows.Forms.Label label2;
        private System.Windows.Forms.TextBox TxtImageListWidth;
    }
}
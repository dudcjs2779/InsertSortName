namespace InsertFileNumber2
{
    partial class Form1
    {
        /// <summary>
        /// 필수 디자이너 변수입니다.
        /// </summary>
        private System.ComponentModel.IContainer components = null;

        /// <summary>
        /// 사용 중인 모든 리소스를 정리합니다.
        /// </summary>
        /// <param name="disposing">관리되는 리소스를 삭제해야 하면 true이고, 그렇지 않으면 false입니다.</param>
        protected override void Dispose(bool disposing)
        {
            if (disposing && (components != null))
            {
                components.Dispose();
            }
            base.Dispose(disposing);
        }

        #region Windows Form 디자이너에서 생성한 코드

        /// <summary>
        /// 디자이너 지원에 필요한 메서드입니다. 
        /// 이 메서드의 내용을 코드 편집기로 수정하지 마세요.
        /// </summary>
        private void InitializeComponent()
        {
            System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(Form1));
            this.StartNum = new System.Windows.Forms.TextBox();
            this.ChkOverwrite = new System.Windows.Forms.CheckBox();
            this.BtnListClear2 = new System.Windows.Forms.Button();
            this.BtnRemove2 = new System.Windows.Forms.Button();
            this.BtnListClear1 = new System.Windows.Forms.Button();
            this.BtnRemove1 = new System.Windows.Forms.Button();
            this.WorkListView = new System.Windows.Forms.ListView();
            this.label2 = new System.Windows.Forms.Label();
            this.OrderListView = new System.Windows.Forms.ListView();
            this.BtnChange1 = new System.Windows.Forms.Button();
            this.BtnImgViewer = new System.Windows.Forms.Button();
            this.BtnToTop = new System.Windows.Forms.Button();
            this.BtnToEnd = new System.Windows.Forms.Button();
            this.BtnCopyToFolder = new System.Windows.Forms.Button();
            this.BtnToFolder = new System.Windows.Forms.Button();
            this.button1 = new System.Windows.Forms.Button();
            this.printPreviewDialog1 = new System.Windows.Forms.PrintPreviewDialog();
            this.BtnSetting = new System.Windows.Forms.Button();
            this.SuspendLayout();
            // 
            // StartNum
            // 
            resources.ApplyResources(this.StartNum, "StartNum");
            this.StartNum.Name = "StartNum";
            // 
            // ChkOverwrite
            // 
            resources.ApplyResources(this.ChkOverwrite, "ChkOverwrite");
            this.ChkOverwrite.Checked = true;
            this.ChkOverwrite.CheckState = System.Windows.Forms.CheckState.Checked;
            this.ChkOverwrite.Name = "ChkOverwrite";
            this.ChkOverwrite.UseVisualStyleBackColor = true;
            // 
            // BtnListClear2
            // 
            resources.ApplyResources(this.BtnListClear2, "BtnListClear2");
            this.BtnListClear2.Name = "BtnListClear2";
            this.BtnListClear2.UseVisualStyleBackColor = true;
            this.BtnListClear2.Click += new System.EventHandler(this.BtnListClear2_Click);
            // 
            // BtnRemove2
            // 
            resources.ApplyResources(this.BtnRemove2, "BtnRemove2");
            this.BtnRemove2.Name = "BtnRemove2";
            this.BtnRemove2.UseVisualStyleBackColor = true;
            this.BtnRemove2.Click += new System.EventHandler(this.BtnRemove2_Click);
            // 
            // BtnListClear1
            // 
            resources.ApplyResources(this.BtnListClear1, "BtnListClear1");
            this.BtnListClear1.Name = "BtnListClear1";
            this.BtnListClear1.UseVisualStyleBackColor = true;
            this.BtnListClear1.Click += new System.EventHandler(this.BtnListClear1_Click);
            // 
            // BtnRemove1
            // 
            resources.ApplyResources(this.BtnRemove1, "BtnRemove1");
            this.BtnRemove1.Name = "BtnRemove1";
            this.BtnRemove1.UseVisualStyleBackColor = true;
            this.BtnRemove1.Click += new System.EventHandler(this.BtnRemove1_Click);
            // 
            // WorkListView
            // 
            resources.ApplyResources(this.WorkListView, "WorkListView");
            this.WorkListView.HideSelection = false;
            this.WorkListView.Name = "WorkListView";
            this.WorkListView.UseCompatibleStateImageBehavior = false;
            this.WorkListView.ItemDrag += new System.Windows.Forms.ItemDragEventHandler(this.WorkListView_ItemDrag);
            this.WorkListView.ItemSelectionChanged += new System.Windows.Forms.ListViewItemSelectionChangedEventHandler(this.WorkListView_ItemSelectionChanged);
            this.WorkListView.SelectedIndexChanged += new System.EventHandler(this.WorkListView_SelectedIndexChanged);
            this.WorkListView.DragDrop += new System.Windows.Forms.DragEventHandler(this.WorkListView_DragDrop);
            this.WorkListView.DragEnter += new System.Windows.Forms.DragEventHandler(this.WorkListView_DragEnter);
            this.WorkListView.KeyDown += new System.Windows.Forms.KeyEventHandler(this.WorkListView_KeyDown);
            this.WorkListView.MouseDown += new System.Windows.Forms.MouseEventHandler(this.WorkListView_MouseDown);
            this.WorkListView.MouseLeave += new System.EventHandler(this.WorkListView_MouseLeave);
            this.WorkListView.MouseMove += new System.Windows.Forms.MouseEventHandler(this.WorkListView_MouseMove);
            // 
            // label2
            // 
            resources.ApplyResources(this.label2, "label2");
            this.label2.Name = "label2";
            // 
            // OrderListView
            // 
            resources.ApplyResources(this.OrderListView, "OrderListView");
            this.OrderListView.HideSelection = false;
            this.OrderListView.Name = "OrderListView";
            this.OrderListView.UseCompatibleStateImageBehavior = false;
            this.OrderListView.ItemDrag += new System.Windows.Forms.ItemDragEventHandler(this.OrderListView_ItemDrag);
            this.OrderListView.ItemSelectionChanged += new System.Windows.Forms.ListViewItemSelectionChangedEventHandler(this.OrderListView_ItemSelectionChanged);
            this.OrderListView.DragDrop += new System.Windows.Forms.DragEventHandler(this.OrderListView_DragDrop);
            this.OrderListView.DragEnter += new System.Windows.Forms.DragEventHandler(this.OrderListView_DragEnter);
            this.OrderListView.KeyDown += new System.Windows.Forms.KeyEventHandler(this.OrderListView_KeyDown);
            this.OrderListView.MouseDown += new System.Windows.Forms.MouseEventHandler(this.OrderListView_MouseDown);
            this.OrderListView.MouseLeave += new System.EventHandler(this.OrderListView_MouseLeave);
            this.OrderListView.MouseMove += new System.Windows.Forms.MouseEventHandler(this.OrderListView_MouseMove);
            // 
            // BtnChange1
            // 
            resources.ApplyResources(this.BtnChange1, "BtnChange1");
            this.BtnChange1.Name = "BtnChange1";
            this.BtnChange1.UseVisualStyleBackColor = true;
            this.BtnChange1.Click += new System.EventHandler(this.btnChange1_Click);
            // 
            // BtnImgViewer
            // 
            resources.ApplyResources(this.BtnImgViewer, "BtnImgViewer");
            this.BtnImgViewer.Name = "BtnImgViewer";
            this.BtnImgViewer.UseVisualStyleBackColor = true;
            this.BtnImgViewer.Click += new System.EventHandler(this.BtnImgViewer_Click);
            // 
            // BtnToTop
            // 
            resources.ApplyResources(this.BtnToTop, "BtnToTop");
            this.BtnToTop.Name = "BtnToTop";
            this.BtnToTop.UseVisualStyleBackColor = true;
            this.BtnToTop.Click += new System.EventHandler(this.BtnToTop_Click);
            // 
            // BtnToEnd
            // 
            resources.ApplyResources(this.BtnToEnd, "BtnToEnd");
            this.BtnToEnd.Name = "BtnToEnd";
            this.BtnToEnd.UseVisualStyleBackColor = true;
            this.BtnToEnd.Click += new System.EventHandler(this.BtnToEnd_Click);
            // 
            // BtnCopyToFolder
            // 
            resources.ApplyResources(this.BtnCopyToFolder, "BtnCopyToFolder");
            this.BtnCopyToFolder.Name = "BtnCopyToFolder";
            this.BtnCopyToFolder.UseVisualStyleBackColor = true;
            this.BtnCopyToFolder.Click += new System.EventHandler(this.BtnCopyToFolder_Click);
            // 
            // BtnToFolder
            // 
            resources.ApplyResources(this.BtnToFolder, "BtnToFolder");
            this.BtnToFolder.Name = "BtnToFolder";
            this.BtnToFolder.UseVisualStyleBackColor = true;
            this.BtnToFolder.Click += new System.EventHandler(this.BtnToFolder_Click);
            // 
            // button1
            // 
            resources.ApplyResources(this.button1, "button1");
            this.button1.Name = "button1";
            this.button1.UseVisualStyleBackColor = true;
            this.button1.Click += new System.EventHandler(this.button1_Click);
            // 
            // printPreviewDialog1
            // 
            resources.ApplyResources(this.printPreviewDialog1, "printPreviewDialog1");
            this.printPreviewDialog1.Name = "printPreviewDialog1";
            // 
            // BtnSetting
            // 
            resources.ApplyResources(this.BtnSetting, "BtnSetting");
            this.BtnSetting.Name = "BtnSetting";
            this.BtnSetting.UseVisualStyleBackColor = true;
            this.BtnSetting.Click += new System.EventHandler(this.BtnSetting_Click);
            // 
            // Form1
            // 
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Inherit;
            resources.ApplyResources(this, "$this");
            this.Controls.Add(this.BtnSetting);
            this.Controls.Add(this.BtnToFolder);
            this.Controls.Add(this.BtnCopyToFolder);
            this.Controls.Add(this.BtnToEnd);
            this.Controls.Add(this.BtnToTop);
            this.Controls.Add(this.BtnChange1);
            this.Controls.Add(this.BtnImgViewer);
            this.Controls.Add(this.StartNum);
            this.Controls.Add(this.ChkOverwrite);
            this.Controls.Add(this.BtnRemove2);
            this.Controls.Add(this.OrderListView);
            this.Controls.Add(this.button1);
            this.Controls.Add(this.BtnRemove1);
            this.Controls.Add(this.BtnListClear2);
            this.Controls.Add(this.label2);
            this.Controls.Add(this.BtnListClear1);
            this.Controls.Add(this.WorkListView);
            this.KeyPreview = true;
            this.Name = "Form1";
            this.FormClosing += new System.Windows.Forms.FormClosingEventHandler(this.Form1_FormClosing);
            this.Shown += new System.EventHandler(this.Form1_Shown);
            this.SizeChanged += new System.EventHandler(this.Form1_SizeChanged);
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion
        private System.Windows.Forms.CheckBox ChkOverwrite;
        private System.Windows.Forms.TextBox StartNum;
        private System.Windows.Forms.Button BtnRemove2;
        private System.Windows.Forms.Button BtnListClear2;
        private System.Windows.Forms.Button BtnRemove1;
        private System.Windows.Forms.Button BtnListClear1;
        private System.Windows.Forms.ListView WorkListView;
        private System.Windows.Forms.Label label2;
        private System.Windows.Forms.ListView OrderListView;
        private System.Windows.Forms.Button BtnImgViewer;
        private System.Windows.Forms.Button BtnChange1;
        private System.Windows.Forms.Button BtnToTop;
        private System.Windows.Forms.Button BtnToEnd;
        private System.Windows.Forms.Button BtnCopyToFolder;
        private System.Windows.Forms.Button BtnToFolder;
        private System.Windows.Forms.Button button1;
        private System.Windows.Forms.PrintPreviewDialog printPreviewDialog1;
        private System.Windows.Forms.Button BtnSetting;
    }
}


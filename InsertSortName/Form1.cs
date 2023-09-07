using Microsoft.WindowsAPICodePack.Dialogs;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Configuration;
using System.Data;
using System.Diagnostics;
using System.Drawing;
using System.IO;
using System.Linq;
using System.Windows.Forms;
using System.Threading;
using Task = System.Threading.Tasks.Task;

//using static System.Windows.Forms.VisualStyles.VisualStyleElement;

namespace InsertFileNumber2
{
    public partial class Form1 : Form
    {
        private readonly SynchronizationContext synchronizationContext;
        private DateTime previousTime = DateTime.Now;

        List<ImageInfo> workList = new List<ImageInfo>();
        List<ListViewItem> workListView_SelectedItemList = new List<ListViewItem>();
        public ImageList imageList = new ImageList();

        List<ImageInfo> orderList = new List<ImageInfo>();
        List<ListViewItem> orderListView_SelectedItemList = new List<ListViewItem>();

        List<int> workListindexList = new List<int>();
        List<int> orderListindexList = new List<int>();

        Form3 imgForm;
        Form5 form5;

        public Form4 form4 = new Form4();

        public bool isOverwrite;
        public bool isBatch;

        public bool isCopyOverwrite;
        public bool showPreview;
        public bool showImageList;
        public int previewWidth;
        public int imageListWidth;

        int index = 0;
        int columnNum;

        public Form1()
        {
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F); // for design in 96 DPI
            InitializeComponent();

            ChkOverwrite.Checked = InsertFileNumber2.Properties.Settings.Default.numberOverwrite;
            isCopyOverwrite = InsertFileNumber2.Properties.Settings.Default.chkCopyOverwrite;
            showPreview = InsertFileNumber2.Properties.Settings.Default.chkPreview;
            previewWidth = InsertFileNumber2.Properties.Settings.Default.PreviewWidth;
            showImageList = InsertFileNumber2.Properties.Settings.Default.chkImageList;
            imageListWidth = InsertFileNumber2.Properties.Settings.Default.ImageListWidth;
            form4.Show();

            if (!showImageList) columnNum = 0;
            else columnNum = 1;

            Init_WorkListView();
            Init_OrderListView();

            synchronizationContext = SynchronizationContext.Current; //context from UI thread  
        }

        private void Form1_Shown(object sender, EventArgs e)
        {
            this.Location = InsertFileNumber2.Properties.Settings.Default.MainFormLocation;
            this.Size = InsertFileNumber2.Properties.Settings.Default.MainFormSize;
        }

        private void Init_WorkListView()
        {
            // 리스트뷰 아이템을 업데이트 하기 시작.
            // 업데이트가 끝날 때까지 UI 갱신 중지.
            WorkListView.BeginUpdate();

            this.Refresh();
            WorkListView.View = System.Windows.Forms.View.Details; ;// 목록 형으로 보이기
            WorkListView.GridLines = true; // 그리드 라인을 보여준다.
            WorkListView.FullRowSelect = true;  // 선택은 행으로 하기.

            WorkListView.AllowDrop = true; // drag and drop 허용

            // drag and drop
            //this.ImgListView.DragDrop += new System.Windows.Forms.DragEventHandler(this.ImgListView_DragDrop);
            //this.ImgListView.DragEnter += new System.Windows.Forms.DragEventHandler(this.ImgListView_DragEnter);

            if (showImageList)
            {
                WorkListView.Columns.Add("Image", imageListWidth);        
                WorkListView.Columns.Add("#", 40);        
                WorkListView.Columns.Add("Name", 500);
                WorkListView.Columns.Add("Path", 1000);

                imageList.ImageSize = new Size(imageListWidth, imageListWidth);
                imageList.ColorDepth = ColorDepth.Depth32Bit;
                WorkListView.SmallImageList = imageList;
            }
            else
            {
                WorkListView.Columns.Add("#", 40);        
                WorkListView.Columns.Add("Name", 500);
                WorkListView.Columns.Add("Path", 1000);
            }

            WorkListView.EndUpdate();
        }

        //=============커서위치의 아이템=================
        private ListViewItem GetItemFromPoint(System.Windows.Forms.ListView listView, Point mousePosition)
        {
            // translate the mouse position from screen coordinates to 
            // client coordinates within the given ListView
            Point localPoint = listView.PointToClient(mousePosition);
            return listView.GetItemAt(localPoint.X, localPoint.Y);
        }

        private void WorkListView_DragEnter(object sender, DragEventArgs e)
        {
            // If the data is a file or a bitmap, display the copy cursor.
            if (e.Data.GetDataPresent(DataFormats.FileDrop))
            {
                e.Effect = DragDropEffects.Copy; //포인터
            }
            else if (e.Data.GetDataPresent(typeof(List<ListViewItem>)))
            {
                e.Effect = DragDropEffects.Move;
            }
            else
            {
                e.Effect = DragDropEffects.None;
            }

        }

        private void WorkListView_DragDrop(object sender, DragEventArgs e)
        {
            if (e.Data.GetDataPresent(DataFormats.FileDrop))
            {
                // 중복 아이템 리스트
                List<ImageInfo> repeatFiles = new List<ImageInfo>();
                string[] files = (string[])e.Data.GetData(DataFormats.FileDrop);

                if (files.Length >= 1)
                {
                    for (int i = 0; i < files.Length; i++)
                    {
                        // 중복 검사
                        ImageInfo repeatItem = workList.Find(x => x.filePath == files[i]);
                        if (repeatItem != null)
                        {
                            if (!isBatch)
                            {
                                using (Form2 newForm = new Form2())
                                {
                                    newForm.mainForm = this;
                                    newForm.msg = Path.GetFileName(files[i]) + " 파일이 중복됩니다.";
                                    if (newForm.ShowDialog() == DialogResult.OK)
                                    {
                                        //Form2 가 닫힐때까지 기다림
                                    }
                                }
                            }

                            if (isOverwrite)
                            {
                                repeatFiles.Add(repeatItem);
                                if (!isBatch) isOverwrite = false;
                            }
                            else
                            {
                                //건너뛰기
                                continue;
                            }
                            //Console.WriteLine("ExitForm2");
                        }

                        if (showImageList)
                        {
                            Image image = Image.FromFile(files[i]);
                            image = resizeImage(image, new Size(256, 256));
                            imageList.Images.Add(index.ToString(), image);
                            ListViewItem listViewItem = new ListViewItem(new string[] { "", index.ToString(), Path.GetFileName(files[i]), files[i] }, index);
                            WorkListView.Items.Add(listViewItem);
                        }
                        else
                        {
                            ListViewItem listViewItem = new ListViewItem(new string[] {index.ToString(), Path.GetFileName(files[i]), files[i] }, index);
                            WorkListView.Items.Add(listViewItem);
                        }

                        //imageList 추가
                        ImageInfo listItem = new ImageInfo();
                        listItem.filePath = files[i];
                        listItem.indexNum = index;

                        workList.Add(listItem);
                        index++;
                    }

                    // 중복 제거
                    if (repeatFiles.Count > 0)
                    {
                        for (int i = 0; i < repeatFiles.Count; i++)
                        {
                            int index = repeatFiles[i].indexNum;

                            ListViewItem item = WorkListView.FindItemWithText(index.ToString());

                            WorkListView.Items.Remove(item);
                            workList.Remove(repeatFiles[i]);
                        }
                    }
                    isBatch = false;
                }
            }

            if (e.Data.GetDataPresent(typeof(List<ListViewItem>)))
            {
                var dropItems = (List<ListViewItem>)e.Data.GetData(typeof(List<ListViewItem>));

                if (dropItems[0].ListView == WorkListView)
                {
                    AddLvToLv(WorkListView, workList, workList, dropItems, workListindexList, AddType.ToBetween);
                }
                else
                {
                    AddLvToLv(WorkListView, orderList, workList, dropItems, orderListindexList, AddType.ToBetween);
                }
            }
        }

        private static System.Drawing.Image resizeImage(System.Drawing.Image imgToResize, Size size)
        {
            //Get the image current width  
            int sourceWidth = imgToResize.Width;
            //Get the image current height  
            int sourceHeight = imgToResize.Height;
            float nPercent = 0;
            float nPercentW = 0;
            float nPercentH = 0;
            //Calulate  width with new desired size  
            nPercentW = ((float)size.Width / (float)sourceWidth);
            //Calculate height with new desired size  
            nPercentH = ((float)size.Height / (float)sourceHeight);
            if (nPercentH < nPercentW)
            {
                nPercent = nPercentH;
            }
            else
            {
                nPercent = nPercentW;
            }

            //New Width  
            int destWidth = (int)(sourceWidth * nPercent);
            //New Height  
            int destHeight = (int)(sourceHeight * nPercent);
            int space = Math.Abs(destWidth - destHeight);

            Bitmap bmp = new Bitmap(size.Width, size.Height);
            Graphics g = Graphics.FromImage(bmp);
            g.Clear(Color.White);

            if (destHeight < destWidth)
            {
                g.DrawImage(imgToResize, 0, space / 2, destWidth, destHeight);
            }
            else
            {
                g.DrawImage(imgToResize, space / 2, 0, destWidth, destHeight);
            }
            g.Dispose();
            return (System.Drawing.Image)bmp;
        }

        private void WorkListView_ItemDrag(object sender, ItemDragEventArgs e)
        {
            ItemDragFromListView(e, WorkListView);
        }

        void ItemDragFromListView(ItemDragEventArgs e, ListView startListView)
        {
            // create array or collection for all selected items
            var items = new List<ListViewItem>();
            // add dragged one first
            items.Add((ListViewItem)e.Item);
            // optionally add the other selected ones
            foreach (ListViewItem item in startListView.SelectedItems)
            {
                if (!items.Contains(item))
                {
                    items.Add(item);
                }

            }
            // pass the items to move...
            startListView.DoDragDrop(items, DragDropEffects.Move);
        }

        void ChangeFileName(bool isQuick, bool isCopy)
        {
            if (OrderListView.Items.Count == 0)
            {
                MessageBox.Show("리스트가 비어있습니다.");
                return;
            }

            int fileNum;
            string folderPath = Path.GetDirectoryName(OrderListView.Items[0].SubItems[2 + columnNum].Text);
            string newPath;

            string[] fileNames = new string[OrderListView.Items.Count];
            string[] oriFilePaths = new string[OrderListView.Items.Count];

            if (Application.OpenForms.OfType<Form3>().Any()) imgForm.ResetImage();
            form4.ResetImage();

            for (int i = 0; i < OrderListView.Items.Count; i++)
            {
                fileNames[i] = OrderListView.Items[i].SubItems[1 + columnNum].Text;
                oriFilePaths[i] = OrderListView.Items[i].SubItems[2 + columnNum].Text;

                if (StartNum.Text == "") fileNum = 0 + i;
                else fileNum = int.Parse(StartNum.Text) + i;

                string str_fileNum;
                if (ChkOverwrite.Checked)
                {
                    str_fileNum = string.Format("{0:D5}", fileNum);
                    fileNames[i] = fileNames[i].Substring(5);
                    fileNames[i] = str_fileNum + fileNames[i];
                }
                else
                {
                    str_fileNum = string.Format("{0:D5}", fileNum);
                    fileNames[i] = str_fileNum + "-" + fileNames[i];
                }

                //빠른 변환
                if (isQuick)
                {

                    newPath = Path.Combine(folderPath, fileNames[i]);
                    OrderListView.Items[i].SubItems[1 + columnNum].Text = Path.GetFileName(newPath);
                    OrderListView.Items[i].SubItems[2 + columnNum].Text = newPath;
                    orderList[i].filePath = newPath;

                    System.IO.File.Move(oriFilePaths[i], newPath);
                }
            }

            // 폴더로 이동 또는 복사
            if (!isQuick)
            {
                CommonOpenFileDialog dialog = new CommonOpenFileDialog();
                dialog.InitialDirectory = folderPath;
                dialog.IsFolderPicker = true; // true면 폴더 선택 false면 파일 선택

                string newFolderPath;
                if (dialog.ShowDialog() == CommonFileDialogResult.Ok)
                {
                    newFolderPath = dialog.FileName;
                }
                else
                {
                    return;
                }

                for (int i = 0; i < fileNames.Length; i++)
                {
                    newPath = Path.Combine(newFolderPath, fileNames[i]);

                    if (isCopy)
                    {
                        try
                        {
                            System.IO.File.Copy(oriFilePaths[i], newPath, isCopyOverwrite);
                        }
                        catch
                        {
                            MessageBox.Show("같은 이름의 파일이 존재합니다.");
                            for (int j = 0; j < fileNames.Length; j++)
                            {
                                OrderListView.Items[j].SubItems[1 + columnNum].Text = Path.GetFileName(oriFilePaths[j]);
                                OrderListView.Items[j].SubItems[2 + columnNum].Text = oriFilePaths[j];
                                orderList[j].filePath = oriFilePaths[j];
                            }
                            return;
                        }
                    }
                    else
                    {

                        OrderListView.Items[i].SubItems[1 + columnNum].Text = Path.GetFileName(newPath);
                        OrderListView.Items[i].SubItems[2 + columnNum].Text = newPath;
                        orderList[i].filePath = newPath;

                        System.IO.File.Move(oriFilePaths[i], newPath);
                    }

                }
            }

            //OrderListView.Items.Clear();
            //orderList.Clear();
            //ImgListView.Items.Clear();
            //imageList.Clear();
        }

        private void btnChange1_Click(object sender, EventArgs e)
        {
            ChangeFileName(true, false);
        }

        private void BtnToFolder_Click(object sender, EventArgs e)
        {
            ChangeFileName(false, false);
        }

        private void BtnCopyToFolder_Click(object sender, EventArgs e)
        {
            ChangeFileName(false, true);
        }

        private void BtnImgViewer_Click(object sender, EventArgs e)
        {
            if (Application.OpenForms.OfType<Form3>().Any())
            {
                imgForm.Close();
            }

            imgForm = new Form3();
            imgForm.Show();
        }

        void Init_OrderListView()
        {
            // 리스트뷰 아이템을 업데이트 하기 시작.
            // 업데이트가 끝날 때까지 UI 갱신 중지.
            OrderListView.BeginUpdate();

            this.Refresh();
            OrderListView.View = System.Windows.Forms.View.Details; ;// 목록 형으로 보이기
            OrderListView.GridLines = true; // 그리드 라인을 보여준다.
            OrderListView.FullRowSelect = true;  // 선택은 행으로 하기.

            OrderListView.AllowDrop = true; // drag and drop 허용
            // drag and drop
            //this.ImgListView.DragDrop += new System.Windows.Forms.DragEventHandler(this.ImgListView_DragDrop);
            //this.ImgListView.DragEnter += new System.Windows.Forms.DragEventHandler(this.ImgListView_DragEnter);

            if (showImageList)
            {
                OrderListView.Columns.Add("Image", imageListWidth);
                OrderListView.Columns.Add("#", 40);        
                OrderListView.Columns.Add("Name", 500);
                OrderListView.Columns.Add("Path", 1000);
                imageList.ImageSize = new Size(imageListWidth, imageListWidth);
                imageList.ColorDepth = ColorDepth.Depth32Bit;
                OrderListView.EndUpdate();
                OrderListView.SmallImageList = imageList;
            }
            else
            {
                OrderListView.Columns.Add("#", 40);        
                OrderListView.Columns.Add("Name", 500);
                OrderListView.Columns.Add("Path", 1000);
            }

            OrderListView.EndUpdate();
        }

        void AddLvToLv(ListView targetListView, List<ImageInfo> startList, List<ImageInfo> endList, List<ListViewItem> viewItems, List<int> indexList, AddType aType)
        {

            //중복 아이템
            List<ImageInfo> repeatFiles = new List<ImageInfo>();

            // 마우스 포인터에 있는 아이템
            ListViewItem pointItem = GetItemFromPoint(targetListView, Cursor.Position);
            int rowNum = targetListView.Items.IndexOf(pointItem);

            var items = viewItems;

            ListView focusedListView = items[0].ListView;
            int selectIndex = items[0].Index;
            int itemsLength = viewItems.Count;

            bool isOwnListView = targetListView.Equals(focusedListView);

            indexList.Clear();

            // 자기 리스트에서 순서 변경
            if (isOwnListView)
            {
                if (aType == AddType.ToBetween && targetListView.SelectedItems.Contains(pointItem)) return;

                if (items[0].Index < rowNum) rowNum = rowNum - items.Count;


                for (int i = 0; i < items.Count; i++)
                {
                    items[i].ListView.Items.Remove(items[i]);
                    startList.Remove(startList.Find(x => x.indexNum == int.Parse(items[i].SubItems[0 + columnNum].Text)));
                }

                switch (aType)
                {
                    case AddType.ToUp:
                        for (int i = 0; i < items.Count; i++)
                        {
                            if (selectIndex - 1 < 0)
                                selectIndex++;

                            // 리스트 형식으로 전환
                            ImageInfo listItem = new ImageInfo();
                            listItem.filePath = items[i].SubItems[2 + columnNum].Text;
                            listItem.indexNum = int.Parse(items[i].SubItems[0 + columnNum].Text);

                            //Console.WriteLine(rowNum2.Index);
                            // 리스트 뷰/리스트 이동

                            targetListView.Items.Insert(i + selectIndex - 1, items[i]);
                            endList.Insert(i + selectIndex - 1, listItem);
                        }
                        //Console.WriteLine(items[0].Index);
                        focusedListView.Items[selectIndex].Focused = true;
                        items[0].EnsureVisible();
                        break;

                    case AddType.ToDown:
                        for (int i = 0; i < items.Count; i++)
                        {
                            if (selectIndex + 1 > focusedListView.Items.Count)
                                selectIndex--;

                            // 리스트 형식으로 전환
                            ImageInfo listItem = new ImageInfo();
                            listItem.filePath = items[i].SubItems[2 + columnNum].Text;
                            listItem.indexNum = int.Parse(items[i].SubItems[0 + columnNum].Text);

                            // 리스트 뷰/리스트 이동
                            targetListView.Items.Insert(i + selectIndex + 1, items[i]);
                            endList.Insert(i + selectIndex + 1, listItem);
                        }
                        items[items.Count - 1].EnsureVisible();
                        break;

                    case AddType.ToBetween:
                        for (int i = 0; i < items.Count; i++)
                        {
                            // 리스트 형식으로 전환
                            ImageInfo listItem = new ImageInfo();
                            listItem.filePath = items[i].SubItems[2 + columnNum].Text;
                            listItem.indexNum = int.Parse(items[i].SubItems[0 + columnNum].Text);

                            // 리스트 뷰 이동
                            //rowNum = -1 => 빈공간에 드랍
                            if (rowNum == -1) targetListView.Items.Add(items[i]);
                            else targetListView.Items.Insert(i + rowNum + 1, items[i]);

                            // 리스트에 아이템 저장
                            if (rowNum == -1) endList.Add(listItem);
                            else endList.Insert(i + rowNum + 1, listItem);
                        }
                        focusedListView.Items[rowNum + 1].Focused = true;
                        break;

                    case AddType.ToEnd:
                        for (int i = 0; i < items.Count; i++)
                        {
                            // 리스트 형식으로 전환
                            ImageInfo listItem = new ImageInfo();
                            listItem.filePath = items[i].SubItems[2 + columnNum].Text;
                            listItem.indexNum = int.Parse(items[i].SubItems[0 + columnNum].Text);

                            //Console.WriteLine(rowNum2.Index);
                            // 리스트 뷰/리스트 이동

                            targetListView.Items.Add(items[i]);
                            endList.Add(listItem);
                        }
                        //Console.WriteLine(items[0].Index);
                        focusedListView.SelectedItems.Clear();
                        focusedListView.Items[selectIndex].Selected = true;
                        break;
                }
                return;
            }
            // 각각 다른 리스트에서 자리변경
            else
            {
                for (int i = 0; i < items.Count; i++)
                {
                    //중복 검사
                    ImageInfo repeatItem = endList.Find(x => x.filePath == items[i].SubItems[2 + columnNum].Text);
                    if (repeatItem != null)
                    {
                        if (!isBatch)
                        {
                            using (Form2 newForm = new Form2())
                            {
                                newForm.mainForm = this;
                                newForm.msg = Path.GetFileName(repeatItem.filePath) + " 파일이 중복됩니다.";
                                if (newForm.ShowDialog() == DialogResult.OK)
                                {
                                    //Form2 가 닫힐때까지 기다림
                                }
                            }
                        }

                        if (isOverwrite)
                        {
                            repeatFiles.Add(repeatItem);
                            if (!isBatch) isOverwrite = false;
                        }
                        else continue;  //건너뛰기

                        //Console.WriteLine("ExitForm2");
                    }

                    //아이템 드랍 전에 리스트뷰에서 제거
                    items[i].ListView.Items.Remove(items[i]);
                    startList.Remove(startList.Find(x => x.indexNum == int.Parse(items[i].SubItems[0 + columnNum].Text)));

                    // 리스트 형식으로 전환
                    ImageInfo listItem = new ImageInfo();
                    listItem.filePath = items[i].SubItems[2 + columnNum].Text;
                    listItem.indexNum = int.Parse(items[i].SubItems[0 + columnNum].Text);

                    switch (aType)
                    {
                        case AddType.ToBetween:
                            // 리스트 뷰 이동
                            //rowNum = -1 => 빈공간에 드랍
                            if (rowNum == -1) targetListView.Items.Add(items[i]);
                            else targetListView.Items.Insert(i + rowNum + 1, items[i]);

                            // 리스트에 아이템 저장
                            if (rowNum == -1) endList.Add(listItem);
                            else endList.Insert(i + rowNum + 1, listItem);

                            break;

                        case AddType.ToTop:
                            targetListView.Items.Insert(i, items[i]);
                            endList.Insert(i, listItem);
                            break;

                        case AddType.ToEnd:
                            targetListView.Items.Add(items[i]);
                            endList.Add(listItem);
                            break;
                    }
                }

                //중복 제거
                if (repeatFiles.Count > 0)
                {
                    for (int i = 0; i < repeatFiles.Count; i++)
                    {
                        int index = repeatFiles[i].indexNum;

                        ListViewItem item = targetListView.FindItemWithText(index.ToString());

                        targetListView.Items.Remove(item);
                        endList.Remove(repeatFiles[i]);
                    }
                }
                isBatch = false;
            }

            focusedListView.Focus();
            if (focusedListView.Items.Count > 0)
            {
                try
                {
                    focusedListView.Items[selectIndex].Selected = true;
                }
                catch
                {
                    focusedListView.Items[selectIndex - 1].Selected = true;

                }
            }

        }

        private void OrderListView_DragEnter(object sender, DragEventArgs e)
        {
            if (e.Data.GetDataPresent(typeof(List<ListViewItem>)))
            {
                e.Effect = DragDropEffects.Move;
            }
            else
            {
                e.Effect = DragDropEffects.None;
            }
        }

        private void OrderListView_DragDrop(object sender, DragEventArgs e)
        {
            if (e.Data.GetDataPresent(typeof(List<ListViewItem>)))
            {
                var dropItems = (List<ListViewItem>)e.Data.GetData(typeof(List<ListViewItem>));

                if (dropItems[0].ListView == OrderListView)
                {
                    AddLvToLv(OrderListView, orderList, orderList, dropItems, orderListindexList, AddType.ToBetween);
                }
                else
                {
                    AddLvToLv(OrderListView, workList, orderList, dropItems, workListindexList, AddType.ToBetween);
                }
            }
        }

        private void OrderListView_ItemDrag(object sender, ItemDragEventArgs e)
        {
            ItemDragFromListView(e, OrderListView);
        }

        private void button1_Click(object sender, EventArgs e)
        {
            Console.Clear();
        }

        private void BtnListClear1_Click(object sender, EventArgs e)
        {
            WorkListView.Items.Clear();
            workList.Clear();
            workListindexList.Clear();
        }

        private void BtnRemove1_Click(object sender, EventArgs e)
        {
            DeleteItem(WorkListView, workList, workListindexList, workListView_SelectedItemList);
        }

        private void BtnListClear2_Click(object sender, EventArgs e)
        {
            OrderListView.Items.Clear();
            orderList.Clear();
            orderListindexList.Clear();
        }

        private void BtnRemove2_Click(object sender, EventArgs e)
        {
            DeleteItem(OrderListView, orderList, orderListindexList, orderListView_SelectedItemList);
        }

        private void BtnToTop_Click(object sender, EventArgs e)
        {
            if (WorkListView.SelectedItems.Count > 0)
            {

                //선택된 아이템
                workListView_SelectedItemList.Clear();
                foreach (ListViewItem item in WorkListView.SelectedItems)
                {
                    workListView_SelectedItemList.Add(item);
                }

                AddLvToLv(OrderListView, workList, orderList, workListView_SelectedItemList, workListindexList, AddType.ToTop);
            }
        }

        private void BtnToEnd_Click(object sender, EventArgs e)
        {
            if (WorkListView.SelectedItems.Count > 0)
            {
                // 선택된 아이템
                workListView_SelectedItemList.Clear();
                foreach (ListViewItem item in WorkListView.SelectedItems)
                {
                    workListView_SelectedItemList.Add(item);
                }

                AddLvToLv(OrderListView, workList, orderList, workListView_SelectedItemList, workListindexList, AddType.ToEnd);
            }
        }

        private void Form1_SizeChanged(object sender, EventArgs e)
        {
            int form_w = (this.Size.Width - 240) / 2;
            int listView_h = this.Size.Height - 160;

            WorkListView.Size = new Size(form_w, listView_h);
            OrderListView.Size = new Size(form_w, listView_h);

            WorkListView.Location = new Point(40, 45);
            OrderListView.Location = new Point(108 + form_w, 45);

            BtnToTop.Location = new Point(43 + form_w, BtnToTop.Location.Y);
            BtnToEnd.Location = new Point(43 + form_w, listView_h);

            BtnListClear1.Location = new Point(40 + form_w - 166, BtnListClear1.Location.Y);
            BtnRemove1.Location = new Point(40 + form_w - 83, BtnRemove1.Location.Y);

            BtnListClear2.Location = new Point(108 + form_w * 2 - 166, BtnListClear1.Location.Y);
            BtnRemove2.Location = new Point(108 + form_w * 2 - 83, BtnRemove1.Location.Y);
        }

        private void WorkListView_KeyDown(object sender, KeyEventArgs e)
        {
            if (e.KeyCode == Keys.Delete)
            {
                DeleteItem(WorkListView, workList, workListindexList, workListView_SelectedItemList);
            }
            else if (e.Modifiers == Keys.Control && e.KeyCode == Keys.A)
            {
                SelectAll(WorkListView);
            }
            else if (e.Modifiers == Keys.Control && e.KeyCode == Keys.X)
            {
                e.Handled = true;
                AddItemsShrotCut(WorkListView, WorkListView, workList, workList, workListView_SelectedItemList, workListindexList, AddType.ToEnd);
            }
            else if (e.Modifiers == Keys.Control && (e.KeyCode == Keys.Up || e.KeyCode == Keys.W))
            {
                AddItemsShrotCut(WorkListView, WorkListView, workList, workList, workListView_SelectedItemList, workListindexList, AddType.ToUp);
            }
            else if (e.Modifiers == Keys.Control && (e.KeyCode == Keys.Down || e.KeyCode == Keys.S))
            {
                AddItemsShrotCut(WorkListView, WorkListView, workList, workList, workListView_SelectedItemList, workListindexList, AddType.ToDown);
            }
            else if (e.KeyCode == Keys.W)
            {
                SelectItem(e, WorkListView, workListindexList, true);
            }
            else if (e.KeyCode == Keys.S)
            {
                SelectItem(e, WorkListView, workListindexList, false);
            }
            else if (e.Modifiers == Keys.Shift && (e.KeyCode == Keys.Right || e.KeyCode == Keys.D))
            {
                e.Handled = true;
                e.SuppressKeyPress = true;
                AddItemsShrotCut(WorkListView, OrderListView, workList, orderList, workListView_SelectedItemList, workListindexList, AddType.ToEnd);
            }
            else if (e.Modifiers == (Keys.Control | Keys.Shift) && e.KeyCode == Keys.Space)
            {
                e.Handled = true;
                RemoveColorItem(WorkListView);
            }
            else if (e.Modifiers == Keys.Control && e.KeyCode == Keys.Space)
            {
                e.Handled = true;
                AddColorItem(WorkListView);
            }

        }

        private void OrderListView_KeyDown(object sender, KeyEventArgs e)
        {
            if (e.KeyCode == Keys.Delete)
            {
                DeleteItem(OrderListView, orderList, orderListindexList, orderListView_SelectedItemList);
            }
            else if (e.Modifiers == Keys.Control && e.KeyCode == Keys.A)
            {
                SelectAll(OrderListView);
            }
            else if (e.Modifiers == Keys.Control  && e.KeyCode == Keys.X)
            {
                e.Handled = true;
                AddItemsShrotCut(OrderListView, OrderListView, orderList, orderList, orderListView_SelectedItemList, orderListindexList, AddType.ToEnd);
            }
            else if (e.Modifiers == Keys.Control && (e.KeyCode == Keys.Up || e.KeyCode == Keys.W))
            {
                AddItemsShrotCut(OrderListView, OrderListView, orderList, orderList, orderListView_SelectedItemList, orderListindexList, AddType.ToUp);
            }
            else if (e.Modifiers == Keys.Control && (e.KeyCode == Keys.Down || e.KeyCode == Keys.S))
            {
                AddItemsShrotCut(OrderListView, OrderListView, orderList, orderList, orderListView_SelectedItemList, orderListindexList, AddType.ToDown);
            }
            else if (e.KeyCode == Keys.W)
            {
                SelectItem(e, OrderListView, orderListindexList, true);
            }
            else if (e.KeyCode == Keys.S)
            {
                SelectItem(e, OrderListView, orderListindexList, false);
            }
            else if (e.Modifiers == Keys.Shift && (e.KeyCode == Keys.Right || e.KeyCode == Keys.A))
            {
                e.Handled = true;
                e.SuppressKeyPress = true;
                AddItemsShrotCut(OrderListView, WorkListView, orderList, workList, orderListView_SelectedItemList, orderListindexList, AddType.ToEnd);
            }
            else if (e.Modifiers == (Keys.Control | Keys.Shift) && e.KeyCode == Keys.Space)
            {
                e.Handled = false;
                RemoveColorItem(OrderListView);
            }
            else if (e.Modifiers == Keys.Control && e.KeyCode == Keys.Space)
            {
                e.Handled = false;
                AddColorItem(OrderListView);
            }
        }

        private void DeleteItem(ListView listView, List<ImageInfo> dataList, List<int> indexList, List<ListViewItem> selectList)
        {
            if (listView.SelectedItems.Count > 0)
            {
                selectList.Clear();

                foreach (ListViewItem item in listView.SelectedItems)
                {
                    selectList.Add(item);
                }
                int focusIndex = selectList[0].Index - 1;

                for (int i = 0; i < selectList.Count; i++)
                {
                    listView.Items.Remove(selectList[i]);
                    dataList.Remove(dataList.Find(x => x.indexNum == int.Parse(selectList[i].SubItems[0 + columnNum].Text)));
                    indexList.Remove(selectList[i].Index);

                }
                if (!listView.Focused) listView.Focus();
                if (listView.FocusedItem != null) listView.FocusedItem.Selected = true;
            }
        }

        private void SelectAll(ListView listView)
        {
            for (int i = 0; i < listView.Items.Count; i++)
            {
                listView.Items[i].Selected = true;

                if(i == listView.Items.Count - 1)
                {
                    listView.Items[i].Focused = true;
                    listView.Items[i].EnsureVisible();
                }
            }
        }

        private void SelectItem(KeyEventArgs e, ListView listView, List<int> indexList, bool isUp)
        {
            e.Handled = true;
            e.SuppressKeyPress = true;
            if (indexList.Count == 0) return;

            int targetIndex = indexList.Last(); 

            if (isUp) targetIndex = targetIndex - 1;
            else targetIndex = targetIndex + 1;

            if (e.Shift)
            {
                if (isUp && targetIndex < 0)
                    return;
                else if (!isUp && targetIndex > listView.Items.Count - 1)
                    return;

                int piviotIndex = indexList[0];

                if (!indexList.Contains(targetIndex))
                {
                    listView.Items[targetIndex].Selected = true;
                    listView.Items[targetIndex].Focused = true;
                    listView.Items[targetIndex].EnsureVisible();
                }
                else
                {
                    if (isUp) targetIndex = targetIndex + 1;
                    else targetIndex = targetIndex - 1;

                    listView.Items[targetIndex].Selected = false;
                    listView.Items[targetIndex].Focused = true;
                    listView.Items[targetIndex].EnsureVisible();
                }
            }
            else
            {
                if (isUp && targetIndex < 0)
                    targetIndex = targetIndex + 1;
                else if (!isUp && targetIndex > listView.Items.Count - 1)
                    targetIndex = targetIndex - 1;

                if (listView.SelectedItems.Count > 0)
                {
                    foreach (ListViewItem item in listView.SelectedItems)
                    {
                        item.Selected = false;
                    }
                }

                listView.Items[targetIndex].Selected = true;
                listView.Items[targetIndex].Focused = true;
                listView.Items[targetIndex].EnsureVisible();
            }
        }

        void AddItemsShrotCut(ListView startListView, ListView targetListView, List<ImageInfo> startList, List<ImageInfo> endList, List<ListViewItem> viewItems, List<int> indexList, AddType aType)
        {
            if (startListView.SelectedItems.Count > 0)
            {
                // 선택된 아이템
                viewItems.Clear();
                foreach (ListViewItem item in startListView.SelectedItems)
                {
                    viewItems.Add(item);
                }

                AddLvToLv(targetListView, startList, endList, viewItems, indexList, aType);

            }
        }

        void AddColorItem(ListView listView)
        {
            foreach (ListViewItem item in listView.SelectedItems)
            {
                item.BackColor = Color.LightGreen;
            }
        }

        void RemoveColorItem(ListView listView)
        {
            foreach (ListViewItem item in listView.SelectedItems)
            {
                item.BackColor = Color.Empty;
            }
        }
        
        int num1;

        private async void WorkListView_ItemSelectionChanged(object sender, ListViewItemSelectionChangedEventArgs e)
        {
            if (e.IsSelected)
            {
                workListindexList.Add(e.ItemIndex);
            }
            else
            {
                workListindexList.Remove(e.ItemIndex);
            }

            if (Application.OpenForms.OfType<Form3>().Any())
            {
                if (WorkListView.SelectedItems.Count > 0 && WorkListView.Focused)
                {
                    await Task.Run(() =>
                    {
                        num1++;
                        UpdateImage(e, WorkListView, workListindexList);
                    });

                    //Console.WriteLine(count1++);
                }
            }
        }

        private async void OrderListView_ItemSelectionChanged(object sender, ListViewItemSelectionChangedEventArgs e)
        {
            if (e.IsSelected)
            {
                orderListindexList.Add(e.ItemIndex);
            }
            else
            {
                orderListindexList.Remove(e.ItemIndex);
            }

            if (Application.OpenForms.OfType<Form3>().Any())
            {
                if (OrderListView.SelectedItems.Count > 0 && OrderListView.Focused)
                {
                    await Task.Run(() =>
                    {
                        num1++;
                        UpdateImage(e, OrderListView, orderListindexList);
                    });

                    //Console.WriteLine(count1++);
                }
            }
        }

        public void UpdateImage(ListViewItemSelectionChangedEventArgs e, ListView listView, List<int> indexList)
        {
            var timeNow = DateTime.Now;

            if ((DateTime.Now - previousTime).Milliseconds <= 5) 
            {
                previousTime = timeNow;
                return;
            }

            synchronizationContext.Post(new SendOrPostCallback(o =>
            {
                //Console.WriteLine("UpdateUI");

                if(indexList.Count > 0 && listView.Items.Count > 0)
                {
                    //Console.WriteLine(indexList.Last());
                    imgForm.filePath = listView.Items[indexList.Last()].SubItems[2 + columnNum].Text;
                    imgForm.LoadImage();
                }
            }), num1);

            previousTime = timeNow;
        }

        private void Form1_FormClosing(object sender, FormClosingEventArgs e)
        {
            InsertFileNumber2.Properties.Settings.Default.numberOverwrite = ChkOverwrite.Checked;
            InsertFileNumber2.Properties.Settings.Default.MainFormLocation = this.Location;
            InsertFileNumber2.Properties.Settings.Default.MainFormSize = this.Size;
            InsertFileNumber2.Properties.Settings.Default.numberOverwrite = ChkOverwrite.Checked;
            InsertFileNumber2.Properties.Settings.Default.chkCopyOverwrite = isCopyOverwrite;
            InsertFileNumber2.Properties.Settings.Default.chkPreview = showPreview;
            InsertFileNumber2.Properties.Settings.Default.PreviewWidth = previewWidth;
            InsertFileNumber2.Properties.Settings.Default.chkImageList = showImageList;
            InsertFileNumber2.Properties.Settings.Default.ImageListWidth = imageListWidth;
            InsertFileNumber2.Properties.Settings.Default.Save();
        }

        void PopUpImage(ListView listView)
        {
            if (showPreview && listView.PointToClient(Cursor.Position).X < listView.Columns[0].Width)
            {
                ListViewItem pointItem = GetItemFromPoint(listView, Cursor.Position);

                if (pointItem == null) return;

                form4.Visible = true;
                Point point = new Point(Cursor.Position.X + 10, Cursor.Position.Y + 10);
                form4.Location = point;

                ListView focusedList;

                if (WorkListView.SelectedItems.Count > 0) focusedList = WorkListView;
                else focusedList = OrderListView;

                if (!focusedList.Focused)
                {
                    focusedList.Focus();
                }

                form4.LoadImage(pointItem.SubItems[2 + columnNum].Text, previewWidth);
            }
            else
            {
                form4.Visible = false;
            }
        }

        private void WorkListView_MouseMove(object sender, MouseEventArgs e)
        {
            PopUpImage(WorkListView);
        }

        private void WorkListView_MouseLeave(object sender, EventArgs e)
        {
            form4.Visible = false;
        }

        private void OrderListView_MouseMove(object sender, MouseEventArgs e)
        {
            PopUpImage(OrderListView);
        }

        private void OrderListView_MouseLeave(object sender, EventArgs e)
        {
            form4.Visible = false;
        }

        private void WorkListView_MouseDown(object sender, MouseEventArgs e)
        {
            if(OrderListView.SelectedItems.Count > 0) OrderListView.SelectedItems.Clear();
        }

        private void OrderListView_MouseDown(object sender, MouseEventArgs e)
        {
            if(WorkListView.SelectedItems.Count > 0) WorkListView.SelectedItems.Clear();

        }

        private void BtnSetting_Click(object sender, EventArgs e)
        {
            if (Application.OpenForms.OfType<Form5>().Any())
            {
                form5.Close();
                form5 = new Form5();
                form5.Show();
            }
            else
            {
                form5 = new Form5();
                form5.Show();
            }

        }

        private void WorkListView_SelectedIndexChanged(object sender, EventArgs e)
        {

        }
    }

    class ImageInfo
    {
        public string filePath;
        public int indexNum;
    }

    enum AddType
    {
        ToTop,
        ToEnd,
        ToBetween,
        ToUp,
        ToDown,
    }


}

using Microsoft.WindowsAPICodePack.Dialogs;
using InsertFileNumber;
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

namespace InsertSortName
{
    public partial class Form1 : Form
    {
        private readonly SynchronizationContext synchronizationContext;
        private DateTime previousTime = DateTime.Now;

        List<ImageInfo> imageList = new List<ImageInfo>();
        List<ListViewItem> imgListView_SelectedItemList = new List<ListViewItem>();

        List<ImageInfo> orderList = new List<ImageInfo>();
        List<ListViewItem> orderListView_SelectedItemList = new List<ListViewItem>();

        List<int> imageListindexList = new List<int>();
        List<int> orderListindexList = new List<int>();

        Form3 imgForm;
        Form4 form4 = new Form4();

        public bool isOverwrite;
        public bool isBatch;

        int index = 0;

        public Form1()
        {
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F); // for design in 96 DPI
            InitializeComponent();
            Init_ImgListView();
            Init_OrderListView();

            ChkOverwrite.Checked = InsertFileNumber.Properties.Settings.Default.numberOverwrite;
            form4.Show();

            synchronizationContext = SynchronizationContext.Current; //context from UI thread  
        }

        private void Form1_Shown(object sender, EventArgs e)
        {
            this.Location = InsertFileNumber.Properties.Settings.Default.MainFormLocation;
            this.Size = InsertFileNumber.Properties.Settings.Default.MainFormSize;
        }

        private void Init_ImgListView()
        {
            // 리스트뷰 아이템을 업데이트 하기 시작.
            // 업데이트가 끝날 때까지 UI 갱신 중지.
            ImgListView.BeginUpdate();

            this.Refresh();
            ImgListView.View = System.Windows.Forms.View.Details; ;// 목록 형으로 보이기
            ImgListView.GridLines = true; // 그리드 라인을 보여준다.
            ImgListView.FullRowSelect = true;  // 선택은 행으로 하기.

            ImgListView.AllowDrop = true; // drag and drop 허용
            // drag and drop
            //this.ImgListView.DragDrop += new System.Windows.Forms.DragEventHandler(this.ImgListView_DragDrop);
            //this.ImgListView.DragEnter += new System.Windows.Forms.DragEventHandler(this.ImgListView_DragEnter);

            ImgListView.Columns.Add("#", 40);        //컬럼추가
            ImgListView.Columns.Add("Name", 500);
            ImgListView.Columns.Add("Path", 1000);

            ImgListView.EndUpdate();
        }

        //=============커서위치의 아이템=================
        private ListViewItem GetItemFromPoint(System.Windows.Forms.ListView listView, Point mousePosition)
        {
            // translate the mouse position from screen coordinates to 
            // client coordinates within the given ListView
            Point localPoint = listView.PointToClient(mousePosition);
            return listView.GetItemAt(localPoint.X, localPoint.Y);
        }

        private void ImgListView_DragEnter(object sender, DragEventArgs e)
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

        private void ImgListView_DragDrop(object sender, DragEventArgs e)
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
                        ImageInfo repeatItem = imageList.Find(x => x.filePath == files[i]);
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

                        //ImgListView 추가
                        string[] viewItemCol = new string[3];
                        viewItemCol[0] = index.ToString();
                        viewItemCol[1] = Path.GetFileName(files[i]);
                        viewItemCol[2] = files[i];

                        ListViewItem viewItem = new ListViewItem(viewItemCol);

                        ImgListView.Items.Add(viewItem);

                        //imageList 추가
                        ImageInfo listItem = new ImageInfo();
                        listItem.filePath = files[i];
                        listItem.indexNum = index;

                        imageList.Add(listItem);
                        index++;
                    }

                    // 중복 제거
                    if (repeatFiles.Count > 0)
                    {
                        for (int i = 0; i < repeatFiles.Count; i++)
                        {
                            int index = repeatFiles[i].indexNum;

                            ListViewItem item = ImgListView.FindItemWithText(index.ToString());

                            ImgListView.Items.Remove(item);
                            imageList.Remove(repeatFiles[i]);

                            //Console.WriteLine(item.SubItems[0].Text + " / " + item.SubItems[1].Text + " / " + item.SubItems[2].Text);
                        }
                    }
                    isBatch = false;
                }
            }

            if (e.Data.GetDataPresent(typeof(List<ListViewItem>)))
            {
                var dropItems = (List<ListViewItem>)e.Data.GetData(typeof(List<ListViewItem>));

                if (dropItems[0].ListView == ImgListView)
                {
                    AddLvToLv(ImgListView, imageList, imageList, dropItems, imageListindexList, AddType.ToBetween);
                }
                else
                {
                    AddLvToLv(ImgListView, orderList, imageList, dropItems, orderListindexList, AddType.ToBetween);
                }
            }
        }

        private void ImgListView_ItemDrag(object sender, ItemDragEventArgs e)
        {
            ItemDragFromListView(e, ImgListView);
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
                    //Console.WriteLine(item.SubItems[0].Text);
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
            string folderPath = Path.GetDirectoryName(OrderListView.Items[0].SubItems[2].Text);
            string newPath;

            string[] fileNames = new string[OrderListView.Items.Count];
            string[] oriFilePaths = new string[OrderListView.Items.Count];

            for (int i = 0; i < OrderListView.Items.Count; i++)
            {
                fileNames[i] = OrderListView.Items[i].SubItems[1].Text;
                oriFilePaths[i] = OrderListView.Items[i].SubItems[2].Text;

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
                    if (Application.OpenForms.OfType<Form3>().Any()) imgForm.ResetImage();

                    newPath = Path.Combine(folderPath, fileNames[i]);
                    OrderListView.Items[i].SubItems[1].Text = Path.GetFileName(newPath);
                    OrderListView.Items[i].SubItems[2].Text = newPath;
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
                    if (Application.OpenForms.OfType<Form3>().Any()) imgForm.ResetImage();

                    newPath = Path.Combine(newFolderPath, fileNames[i]);

                    OrderListView.Items[i].SubItems[1].Text = Path.GetFileName(newPath);
                    OrderListView.Items[i].SubItems[2].Text = newPath;
                    orderList[i].filePath = newPath;

                    if (isCopy)
                    {
                        if (oriFilePaths[i] != newPath)
                        {
                            System.IO.File.Copy(oriFilePaths[i], newPath);
                        }
                        else
                        {
                            System.IO.File.Move(oriFilePaths[i], newPath);
                        }
                    }
                    else System.IO.File.Move(oriFilePaths[i], newPath);
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

            OrderListView.Columns.Add("#", 40);        //컬럼추가
            OrderListView.Columns.Add("Name", 500);
            OrderListView.Columns.Add("Path", 1000);

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
                    startList.Remove(startList.Find(x => x.indexNum == int.Parse(items[i].SubItems[0].Text)));
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
                            listItem.filePath = items[i].SubItems[2].Text;
                            listItem.indexNum = int.Parse(items[i].SubItems[0].Text);

                            //Console.WriteLine(rowNum2.Index);
                            // 리스트 뷰/리스트 이동

                            targetListView.Items.Insert(i + selectIndex - 1, items[i]);
                            endList.Insert(i + selectIndex - 1, listItem);
                        }
                        //Console.WriteLine(items[0].Index);
                        focusedListView.Items[selectIndex].Focused = true;
                        break;

                    case AddType.ToDown:
                        for (int i = 0; i < items.Count; i++)
                        {
                            if (selectIndex + 1 > focusedListView.Items.Count)
                                selectIndex--;

                            // 리스트 형식으로 전환
                            ImageInfo listItem = new ImageInfo();
                            listItem.filePath = items[i].SubItems[2].Text;
                            listItem.indexNum = int.Parse(items[i].SubItems[0].Text);

                            // 리스트 뷰/리스트 이동
                            targetListView.Items.Insert(i + selectIndex + 1, items[i]);
                            endList.Insert(i + selectIndex + 1, listItem);
                        }
                        break;

                    case AddType.ToBetween:
                        for (int i = 0; i < items.Count; i++)
                        {
                            // 리스트 형식으로 전환
                            ImageInfo listItem = new ImageInfo();
                            listItem.filePath = items[i].SubItems[2].Text;
                            listItem.indexNum = int.Parse(items[i].SubItems[0].Text);

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
                            listItem.filePath = items[i].SubItems[2].Text;
                            listItem.indexNum = int.Parse(items[i].SubItems[0].Text);

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
                    ImageInfo repeatItem = endList.Find(x => x.filePath == items[i].SubItems[2].Text);
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
                    startList.Remove(startList.Find(x => x.indexNum == int.Parse(items[i].SubItems[0].Text)));

                    // 리스트 형식으로 전환
                    ImageInfo listItem = new ImageInfo();
                    listItem.filePath = items[i].SubItems[2].Text;
                    listItem.indexNum = int.Parse(items[i].SubItems[0].Text);

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

                        //Console.WriteLine(item.SubItems[0].Text + " / " + item.SubItems[1].Text + " / " + item.SubItems[2].Text);
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
                    AddLvToLv(OrderListView, imageList, orderList, dropItems, imageListindexList, AddType.ToBetween);
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
            ImgListView.Items.Clear();
            imageList.Clear();
        }

        private void BtnRemove1_Click(object sender, EventArgs e)
        {
            DeleteItem(ImgListView, imageList, imgListView_SelectedItemList);
        }

        private void BtnListClear2_Click(object sender, EventArgs e)
        {
            OrderListView.Items.Clear();
            orderList.Clear();
        }

        private void BtnRemove2_Click(object sender, EventArgs e)
        {
            DeleteItem(OrderListView, orderList, orderListView_SelectedItemList);
        }

        private void BtnToTop_Click(object sender, EventArgs e)
        {
            if (ImgListView.SelectedItems.Count > 0)
            {

                //선택된 아이템
                imgListView_SelectedItemList.Clear();
                foreach (ListViewItem item in ImgListView.SelectedItems)
                {
                    imgListView_SelectedItemList.Add(item);
                }

                AddLvToLv(OrderListView, imageList, orderList, imgListView_SelectedItemList, imageListindexList, AddType.ToTop);
            }
        }

        private void BtnToEnd_Click(object sender, EventArgs e)
        {
            if (ImgListView.SelectedItems.Count > 0)
            {
                // 선택된 아이템
                imgListView_SelectedItemList.Clear();
                foreach (ListViewItem item in ImgListView.SelectedItems)
                {
                    imgListView_SelectedItemList.Add(item);
                }

                AddLvToLv(OrderListView, imageList, orderList, imgListView_SelectedItemList, imageListindexList, AddType.ToEnd);
            }
        }

        private void Form1_SizeChanged(object sender, EventArgs e)
        {
            int form_w = (this.Size.Width - 240) / 2;
            int listView_h = this.Size.Height - 160;

            ImgListView.Size = new Size(form_w, listView_h);
            OrderListView.Size = new Size(form_w, listView_h);

            ImgListView.Location = new Point(40, 45);
            OrderListView.Location = new Point(108 + form_w, 45);

            BtnToTop.Location = new Point(43 + form_w, BtnToTop.Location.Y);
            BtnToEnd.Location = new Point(43 + form_w, listView_h);

            BtnListClear1.Location = new Point(40 + form_w - 166, BtnListClear1.Location.Y);
            BtnRemove1.Location = new Point(40 + form_w - 83, BtnRemove1.Location.Y);

            BtnListClear2.Location = new Point(108 + form_w * 2 - 166, BtnListClear1.Location.Y);
            BtnRemove2.Location = new Point(108 + form_w * 2 - 83, BtnRemove1.Location.Y);
        }

        private void ImgListView_KeyDown(object sender, KeyEventArgs e)
        {
            if (e.KeyCode == Keys.Delete)
            {
                DeleteItem(ImgListView, imageList, imgListView_SelectedItemList);
            }
            else if (e.Modifiers == Keys.Control && e.KeyCode == Keys.A)
            {
                SelectAll(ImgListView);
            }
            else if (e.Modifiers == Keys.Control && e.KeyCode == Keys.X)
            {
                e.Handled = true;
                AddItemsShrotCut(ImgListView, ImgListView, imageList, imageList, imgListView_SelectedItemList, imageListindexList, AddType.ToEnd);
            }
            else if (e.Modifiers == Keys.Control && (e.KeyCode == Keys.Up || e.KeyCode == Keys.W))
            {
                AddItemsShrotCut(ImgListView, ImgListView, imageList, imageList, imgListView_SelectedItemList, imageListindexList, AddType.ToUp);
            }
            else if (e.Modifiers == Keys.Control && (e.KeyCode == Keys.Down || e.KeyCode == Keys.S))
            {
                AddItemsShrotCut(ImgListView, ImgListView, imageList, imageList, imgListView_SelectedItemList, imageListindexList, AddType.ToDown);
            }
            else if (e.KeyCode == Keys.W)
            {
                SelectItem(e, ImgListView, imageListindexList, true);
            }
            else if (e.KeyCode == Keys.S)
            {
                SelectItem(e, ImgListView, imageListindexList, false);
            }
            else if (e.Modifiers == Keys.Shift && (e.KeyCode == Keys.Right || e.KeyCode == Keys.D))
            {
                e.Handled = true;
                e.SuppressKeyPress = true;
                AddItemsShrotCut(ImgListView, OrderListView, imageList, orderList, imgListView_SelectedItemList, imageListindexList, AddType.ToEnd);
            }
            else if (e.Modifiers == (Keys.Control | Keys.Shift) && e.KeyCode == Keys.Space)
            {
                e.Handled = true;
                RemoveColorItem(ImgListView);
            }
            else if (e.Modifiers == Keys.Control && e.KeyCode == Keys.Space)
            {
                e.Handled = true;
                AddColorItem(ImgListView);
            }

        }

        private void OrderListView_KeyDown(object sender, KeyEventArgs e)
        {
            if (e.KeyCode == Keys.Delete)
            {
                DeleteItem(OrderListView, orderList, orderListView_SelectedItemList);
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
                AddItemsShrotCut(OrderListView, ImgListView, orderList, imageList, orderListView_SelectedItemList, orderListindexList, AddType.ToEnd);
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

        private void DeleteItem(ListView listView, List<ImageInfo> dataList, List<ListViewItem> selectList)
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
                    dataList.Remove(dataList.Find(x => x.indexNum == int.Parse(selectList[i].SubItems[0].Text)));
                }

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
                }
                else
                {
                    if (isUp) targetIndex = targetIndex + 1;
                    else targetIndex = targetIndex - 1;

                    listView.Items[targetIndex].Selected = false;
                    listView.Items[targetIndex].Focused = true;
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

        private async void ImgListView_ItemSelectionChanged(object sender, ListViewItemSelectionChangedEventArgs e)
        {
            if (e.IsSelected)
            {
                imageListindexList.Add(e.ItemIndex);
            }
            else
            {
                imageListindexList.Remove(e.ItemIndex);
            }

            if (Application.OpenForms.OfType<Form3>().Any())
            {
                if (ImgListView.SelectedItems.Count > 0 && ImgListView.Focused)
                {
                    await Task.Run(() =>
                    {
                        num1++;
                        UpdateImage(e, ImgListView, imageListindexList);
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

            if ((DateTime.Now - previousTime).Milliseconds <= 10) 
            {
                //Console.WriteLine("return");
                previousTime = timeNow;
                return;
            }

            synchronizationContext.Post(new SendOrPostCallback(o =>
            {
                //Console.WriteLine("UpdateUI");

                if(indexList.Count > 0 && listView.Items.Count > 0)
                {
                    //Console.WriteLine(indexList.Last());
                    imgForm.filePath = listView.Items[indexList.Last()].SubItems[2].Text;
                    imgForm.LoadImage();
                }
                

            }), num1);

            previousTime = timeNow;
        }

        private void Form1_FormClosing(object sender, FormClosingEventArgs e)
        {
            InsertFileNumber.Properties.Settings.Default.numberOverwrite = ChkOverwrite.Checked;
            InsertFileNumber.Properties.Settings.Default.MainFormLocation = this.Location;
            InsertFileNumber.Properties.Settings.Default.MainFormSize = this.Size;
            InsertFileNumber.Properties.Settings.Default.Save();
        }

        void PopUpImage(ListView listView)
        {
            if (listView.PointToClient(Cursor.Position).X < 40)
            {
                ListViewItem pointItem = GetItemFromPoint(listView, Cursor.Position);

                if (pointItem == null) return;

                form4.Visible = true;
                Point point = new Point(Cursor.Position.X + 10, Cursor.Position.Y + 10);
                form4.Location = point;

                ListView focusedList;

                if (ImgListView.SelectedItems.Count > 0) focusedList = ImgListView;
                else focusedList = OrderListView;

                if (!focusedList.Focused)
                {
                    focusedList.Focus();
                }

                form4.LoadImage(pointItem.SubItems[2].Text);
            }
            else
            {
                form4.Visible = false;
            }
        }

        private void ImgListView_MouseMove(object sender, MouseEventArgs e)
        {
            PopUpImage(ImgListView);
        }

        private void ImgListView_MouseLeave(object sender, EventArgs e)
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

        private void ImgListView_MouseDown(object sender, MouseEventArgs e)
        {
            if(OrderListView.SelectedItems.Count > 0) OrderListView.SelectedItems.Clear();
        }

        private void OrderListView_MouseDown(object sender, MouseEventArgs e)
        {
            if(ImgListView.SelectedItems.Count > 0) ImgListView.SelectedItems.Clear();

        }

        private void button2_Click(object sender, EventArgs e)
        {
            for (int i = 0; i < orderListindexList.Count; i++)
            {
                Console.WriteLine(orderListindexList[i]);
            }
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

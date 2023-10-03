using System.Windows.Forms;

namespace File_Management
{
    partial class FolderForm
    {
        /// <summary>
        /// 必需的设计器变量。
        /// </summary>
        private System.ComponentModel.IContainer components = null;

        /// <summary>
        /// 清理所有正在使用的资源。
        /// </summary>
        /// <param name="disposing">如果应释放托管资源，为 true；否则为 false。</param>
        protected override void Dispose(bool disposing)
        {
            if (disposing && (components != null))
            {
                components.Dispose();
            }
            base.Dispose(disposing);
        }

        #region Windows 窗体设计器生成的代码

        /// <summary>
        /// 设计器支持所需的方法 - 不要修改
        /// 使用代码编辑器修改此方法的内容。
        /// </summary>
        private void InitializeComponent()
        {
            this.components = new System.ComponentModel.Container();
            this.Formatting = new System.Windows.Forms.Button();
            this.FolderTreeView = new System.Windows.Forms.TreeView();
            this.DetailListView = new System.Windows.Forms.ListView();
            this.colName = ((System.Windows.Forms.ColumnHeader)(new System.Windows.Forms.ColumnHeader()));
            this.colModifyTime = ((System.Windows.Forms.ColumnHeader)(new System.Windows.Forms.ColumnHeader()));
            this.colType = ((System.Windows.Forms.ColumnHeader)(new System.Windows.Forms.ColumnHeader()));
            this.colSize = ((System.Windows.Forms.ColumnHeader)(new System.Windows.Forms.ColumnHeader()));
            this.FileMenuStrip1 = new System.Windows.Forms.ContextMenuStrip(this.components);
            this.打开ToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.删除ToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.重命名ToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.DiskSize = new System.Windows.Forms.Label();
            this.BlockSize = new System.Windows.Forms.Label();
            this.DirTip = new System.Windows.Forms.Label();
            this.DirTextBox = new System.Windows.Forms.TextBox();
            this.BacktoParent = new System.Windows.Forms.Button();
            this.CreateFile = new System.Windows.Forms.Button();
            this.CreateFolder = new System.Windows.Forms.Button();
            this.FileMenuStrip1.SuspendLayout();
            this.SuspendLayout();
            // 
            // Formatting
            // 
            this.Formatting.BackColor = System.Drawing.SystemColors.ButtonFace;
            this.Formatting.Location = new System.Drawing.Point(12, 22);
            this.Formatting.Margin = new System.Windows.Forms.Padding(3, 2, 3, 2);
            this.Formatting.Name = "Formatting";
            this.Formatting.Size = new System.Drawing.Size(99, 28);
            this.Formatting.TabIndex = 0;
            this.Formatting.Text = "格式化";
            this.Formatting.UseVisualStyleBackColor = false;
            this.Formatting.Click += new System.EventHandler(this.Formatting_Click);
            // 
            // FolderTreeView
            // 
            this.FolderTreeView.Location = new System.Drawing.Point(12, 58);
            this.FolderTreeView.Margin = new System.Windows.Forms.Padding(3, 2, 3, 2);
            this.FolderTreeView.Name = "FolderTreeView";
            this.FolderTreeView.Size = new System.Drawing.Size(233, 520);
            this.FolderTreeView.TabIndex = 0;
            this.FolderTreeView.AfterSelect += new System.Windows.Forms.TreeViewEventHandler(this.FolderTreeView_AfterSelect);
            this.FolderTreeView.DoubleClick += new System.EventHandler(this.FolderTreeView_DoubleClick);
            // 
            // DetailListView
            // 
            this.DetailListView.Columns.AddRange(new System.Windows.Forms.ColumnHeader[] {
            this.colName,
            this.colModifyTime,
            this.colType,
            this.colSize});
            this.DetailListView.ContextMenuStrip = this.FileMenuStrip1;
            this.DetailListView.HideSelection = false;
            this.DetailListView.Location = new System.Drawing.Point(245, 58);
            this.DetailListView.Margin = new System.Windows.Forms.Padding(3, 2, 3, 2);
            this.DetailListView.Name = "DetailListView";
            this.DetailListView.Size = new System.Drawing.Size(904, 520);
            this.DetailListView.TabIndex = 1;
            this.DetailListView.UseCompatibleStateImageBehavior = false;
            this.DetailListView.View = System.Windows.Forms.View.Details;
            this.DetailListView.SelectedIndexChanged += new System.EventHandler(this.DetailListView_SelectedIndexChanged);
            this.DetailListView.MouseClick += new System.Windows.Forms.MouseEventHandler(this.FileMenuStrip1_MouseClick);
            // 
            // colName
            // 
            this.colName.Text = "名称";
            this.colName.Width = 200;
            // 
            // colModifyTime
            // 
            this.colModifyTime.Text = "修改日期";
            this.colModifyTime.Width = 250;
            // 
            // colType
            // 
            this.colType.Text = "类型";
            this.colType.Width = 150;
            // 
            // colSize
            // 
            this.colSize.Text = "文件大小";
            this.colSize.Width = 170;
            // 
            // FileMenuStrip1
            // 
            this.FileMenuStrip1.ImageScalingSize = new System.Drawing.Size(20, 20);
            this.FileMenuStrip1.Items.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.打开ToolStripMenuItem,
            this.删除ToolStripMenuItem,
            this.重命名ToolStripMenuItem});
            this.FileMenuStrip1.Name = "FileMenuStrip1";
            this.FileMenuStrip1.Size = new System.Drawing.Size(124, 76);
            // 打开ToolStripMenuItem
            // 
            this.打开ToolStripMenuItem.Name = "打开ToolStripMenuItem";
            this.打开ToolStripMenuItem.Size = new System.Drawing.Size(123, 24);
            this.打开ToolStripMenuItem.Text = "打开";
            this.打开ToolStripMenuItem.Click += new System.EventHandler(this.打开ToolStripMenuItem_Click);
            // 
            // 删除ToolStripMenuItem
            // 
            this.删除ToolStripMenuItem.Name = "删除ToolStripMenuItem";
            this.删除ToolStripMenuItem.Size = new System.Drawing.Size(123, 24);
            this.删除ToolStripMenuItem.Text = "删除";
            this.删除ToolStripMenuItem.Click += new System.EventHandler(this.删除ToolStripMenuItem_Click);
            // 
            // 重命名ToolStripMenuItem
            // 
            this.重命名ToolStripMenuItem.Name = "重命名ToolStripMenuItem";
            this.重命名ToolStripMenuItem.Size = new System.Drawing.Size(123, 24);
            this.重命名ToolStripMenuItem.Text = "重命名";
            this.重命名ToolStripMenuItem.Click += new System.EventHandler(this.重命名ToolStripMenuItem_Click);
            // 
            // DiskSize
            // 
            this.DiskSize.BackColor = System.Drawing.SystemColors.ButtonHighlight;
            this.DiskSize.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle;
            this.DiskSize.Location = new System.Drawing.Point(245, 551);
            this.DiskSize.Name = "DiskSize";
            this.DiskSize.Size = new System.Drawing.Size(486, 26);
            this.DiskSize.TabIndex = 4;
            this.DiskSize.Text = "DiskSize";
            this.DiskSize.TextAlign = System.Drawing.ContentAlignment.MiddleCenter;
            // 
            // BlockSize
            // 
            this.BlockSize.BackColor = System.Drawing.SystemColors.ButtonHighlight;
            this.BlockSize.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle;
            this.BlockSize.Location = new System.Drawing.Point(731, 551);
            this.BlockSize.Name = "BlockSize";
            this.BlockSize.Size = new System.Drawing.Size(410, 26);
            this.BlockSize.TabIndex = 5;
            this.BlockSize.Text = "BlockSize";
            this.BlockSize.TextAlign = System.Drawing.ContentAlignment.MiddleCenter;
            // 
            // DirTip
            // 
            this.DirTip.Location = new System.Drawing.Point(463, 25);
            this.DirTip.Name = "DirTip";
            this.DirTip.Size = new System.Drawing.Size(127, 25);
            this.DirTip.TabIndex = 0;
            this.DirTip.Text = "当前文件路径为：";
            this.DirTip.TextAlign = System.Drawing.ContentAlignment.MiddleCenter;
            this.DirTip.Click += new System.EventHandler(this.label1_Click);
            // 
            // DirTextBox
            // 
            this.DirTextBox.Location = new System.Drawing.Point(587, 22);
            this.DirTextBox.Margin = new System.Windows.Forms.Padding(3, 2, 3, 2);
            this.DirTextBox.Name = "DirTextBox";
            this.DirTextBox.Size = new System.Drawing.Size(555, 25);
            this.DirTextBox.TabIndex = 0;
            this.DirTextBox.TextChanged += new System.EventHandler(this.DirTextBox_TextChanged);
            // 
            // BacktoParent
            // 
            this.BacktoParent.BackColor = System.Drawing.SystemColors.ButtonFace;
            this.BacktoParent.ForeColor = System.Drawing.SystemColors.ActiveCaptionText;
            this.BacktoParent.Location = new System.Drawing.Point(117, 22);
            this.BacktoParent.Margin = new System.Windows.Forms.Padding(3, 2, 3, 2);
            this.BacktoParent.Name = "BacktoParent";
            this.BacktoParent.Size = new System.Drawing.Size(123, 28);
            this.BacktoParent.TabIndex = 0;
            this.BacktoParent.Text = "返回上级目录";
            this.BacktoParent.UseVisualStyleBackColor = false;
            this.BacktoParent.Click += new System.EventHandler(this.BacktoParent_Click);
            // 
            // CreateFile
            // 
            this.CreateFile.BackColor = System.Drawing.SystemColors.ButtonFace;
            this.CreateFile.ForeColor = System.Drawing.SystemColors.ActiveCaptionText;
            this.CreateFile.Location = new System.Drawing.Point(245, 22);
            this.CreateFile.Margin = new System.Windows.Forms.Padding(3, 2, 3, 2);
            this.CreateFile.Name = "CreateFile";
            this.CreateFile.Size = new System.Drawing.Size(100, 28);
            this.CreateFile.TabIndex = 6;
            this.CreateFile.Text = "新建文件";
            this.CreateFile.UseVisualStyleBackColor = false;
            this.CreateFile.Click += new System.EventHandler(this.CreateFile_Click);
            // 
            // CreateFolder
            // 
            this.CreateFolder.BackColor = System.Drawing.SystemColors.ButtonFace;
            this.CreateFolder.ForeColor = System.Drawing.SystemColors.ActiveCaptionText;
            this.CreateFolder.Location = new System.Drawing.Point(351, 22);
            this.CreateFolder.Margin = new System.Windows.Forms.Padding(3, 2, 3, 2);
            this.CreateFolder.Name = "CreateFolder";
            this.CreateFolder.Size = new System.Drawing.Size(107, 28);
            this.CreateFolder.TabIndex = 7;
            this.CreateFolder.Text = "新建文件夹";
            this.CreateFolder.UseVisualStyleBackColor = false;
            this.CreateFolder.Click += new System.EventHandler(this.CreateFolder_Click);
            // 
            // FolderForm
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(8F, 15F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(1161, 581);
            this.Controls.Add(this.CreateFolder);
            this.Controls.Add(this.CreateFile);
            this.Controls.Add(this.DirTextBox);
            this.Controls.Add(this.BacktoParent);
            this.Controls.Add(this.DirTip);
            this.Controls.Add(this.Formatting);
            this.Controls.Add(this.BlockSize);
            this.Controls.Add(this.DiskSize);
            this.Controls.Add(this.DetailListView);
            this.Controls.Add(this.FolderTreeView);
            this.Margin = new System.Windows.Forms.Padding(3, 2, 3, 2);
            this.Name = "FolderForm";
            this.Text = "Form1";
            this.Load += new System.EventHandler(this.FolderForm_Load);
            this.FileMenuStrip1.ResumeLayout(false);
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        private void DetailListView_MouseClick(object sender, MouseEventArgs e)
        {
            throw new System.NotImplementedException();
        }

        #endregion
        private Button Formatting;
        private TreeView FolderTreeView;
        private ListView DetailListView;
        private Label DiskSize;
        private Label BlockSize;
        private Label DirTip;
        private TextBox DirTextBox;
        private Button BacktoParent;
        private Button CreateFile;
        private Button CreateFolder;
        private ColumnHeader colName;
        private ColumnHeader colModifyTime;
        private ColumnHeader colType;
        private ColumnHeader colSize;
        private ContextMenuStrip FileMenuStrip1;
        private ToolStripMenuItem 打开ToolStripMenuItem;
        private ToolStripMenuItem 删除ToolStripMenuItem;
        private ToolStripMenuItem 重命名ToolStripMenuItem;
    }
}


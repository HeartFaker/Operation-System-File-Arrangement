using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Diagnostics;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using static System.Windows.Forms.VisualStyles.VisualStyleElement;

namespace File_Management
{
    public partial class NotePadForm : Form
    {
        //从主框架引用函数
        public FolderForm folderForm;
        //文件名
        public string fileName;
        //标记本次打开文件后是否改动过
        public bool isEdited = false;
        //构造函数
        public NotePadForm(string name, FolderForm root)
        {
            fileName = name;
            folderForm = root;
            InitializeComponent();
        }

        private void NotePadForm_Load(object sender, EventArgs e)
        {
            //声明名称
            this.Text = fileName + ".txt";
            //定义容器获取已有内容
            Catalog.node node = new Catalog.node();
            node = folderForm.catalog.searchNode(folderForm.catalog.root, fileName, 0);
            string content = folderForm.virtualDisk.readFileContent(node.file);
            if (content != null)
            {
                //打开后显示已有内容
                TextArea.AppendText(content);
            }
            //不知为何需要重新声明一下，许是C#语言要求
            isEdited = false;
        }

        //关闭页面时自动调用
        protected override void OnFormClosing(FormClosingEventArgs e)
        {
            //用户未编辑直接退出
            if (isEdited==false)
            {
                e.Cancel = false;   
            }
            else
            {
                //提问用户是否想保存
                DialogResult res = MessageBox.Show("你想将更改保存到" + folderForm.curPath + "> "+ fileName +".txt吗？", "提示信息", MessageBoxButtons.YesNoCancel, MessageBoxIcon.Information);
                //确定保存
                if(res==DialogResult.Yes)
                {
                    //定义容器存放内容
                    Catalog.node curFCB = folderForm.catalog.searchNode(folderForm.catalog.root, fileName, 0);
                    //原有大小与位置
                    int oldSize = curFCB.file.size;
                    int oldPos = curFCB.file.initPos;
                    //定义容器显示内容
                    string content = TextArea.Text.Trim();
                    //文件大小
                    curFCB.file.size = content.Length;
                    //文件编辑时间
                    curFCB.file.modifyTime = DateTime.Now.ToLocalTime().ToString();
                    //在内存上给文件分配空间
                    if (curFCB.file.size > 0)
                    {
                        if (folderForm.virtualDisk.space_left< curFCB.file.size)
                        {
                            MessageBox.Show("磁盘剩余空间不足！");
                        }
                        else
                        {
                            //删去已有文件
                            folderForm.virtualDisk.delFileContent(oldPos, oldSize);
                            //添加当前文件
                            folderForm.virtualDisk.arrangeSpace(curFCB.file, content);
                        }
                    }
                    //更新文件信息
                    folderForm.fileLoad(folderForm.curRoot);
                    //更新磁盘信息
                    folderForm.DiskSituation();
                }
                //不保存，直接退出
                else if(res==DialogResult.No)
                {
                    e.Cancel = false;
                }
                //取消，继续编辑
                else if(res == DialogResult.Cancel)
                {
                    e.Cancel = true;
                }
            }
        }

        private void textBox1_TextChanged(object sender, EventArgs e)
        {
            //文本变化后自动改变
            isEdited = true;
        }
    }
}

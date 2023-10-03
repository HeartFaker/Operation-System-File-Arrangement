using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Security.Cryptography.X509Certificates;
using System.Text;
using System.Threading.Tasks;
using Microsoft.VisualBasic;
using System.IO;
using System.Windows.Forms;
using File_Management;
using System.Xml.Linq;
using static System.Windows.Forms.VisualStyles.VisualStyleElement;
using static System.Net.WebRequestMethods;
using System.Runtime.Remoting.Messaging;
using System.Runtime.Serialization.Formatters.Binary;
using static File_Management.Catalog;
using System.Diagnostics;

namespace File_Management
{
    public partial class FolderForm : Form
    {
        //创建目录
        public Catalog catalog = new Catalog();
        //创建根节点
        public Catalog.node rootNode;
        public Catalog.node curRoot = new Catalog.node();
        public VirtualBase virtualDisk = new VirtualBase(1000, 2);
        //记录当前路径
        public string curPath = "root";

        public FolderForm() 
        {
            //页面开始位置设置为屏幕中央
            this.StartPosition = FormStartPosition.CenterScreen;
            //设置根节点为root
            FCB root = new FCB("root", 1, "", 1);
            rootNode = new Catalog.node(root);
            //当前节点指向根节点
            curRoot = rootNode;
            //目录根节点设置
            catalog.root = rootNode;
            InitializeComponent();
        }

        //显示磁盘当前信息
        public void DiskSituation()
        {
            DiskSize.Text = "磁盘容量：" + virtualDisk.size + " Bit";
            BlockSize.Text = "剩余容量：" + virtualDisk.blockSize*virtualDisk.space_left + " Bit";
        }

        //加载页面
        private void FolderForm_Load(object sender, EventArgs e)
        {
            init();
            fileLoad(rootNode);
            treeViewLoad(rootNode.leftChild);
            DiskSituation();
        }

        //添加文件的列表显示
        public void addListView(Catalog.node pNode)
        {
            ListViewItem file = new ListViewItem();
            switch (pNode.file.type)
            {
                //如果为文件，设置显示
                case 0:
                    file = new ListViewItem(new string[]
                    {
                        pNode.file.fileName,
                        pNode.file.modifyTime,
                        "文本文档",
                        pNode.file.size.ToString() + "B"
                    });
                    //元素标签定义为0
                    file.Tag = 0;
                    break;
                //如果为文件夹，设置显示
                case 1:
                    file = new ListViewItem(new string[]
                    {
                        pNode.file.fileName,
                        pNode.file.modifyTime,
                        "文件夹",
                        pNode.file.size.ToString() + "B"
                    });
                    //元素标签定义为1
                    file.Tag = 1;
                    break;
                default:
                    break;
            }
            //添加元素
            DetailListView.Items.Add(file);
        }

        //加载文件（夹）显示
        public void fileLoad(Catalog.node pNode)
        {
            DetailListView.Items.Clear();

            if (pNode.file.fileName == "root") //当前目录为root时禁用返回上一层按钮
                BacktoParent.Enabled = false;
            else
                BacktoParent.Enabled = true;
            DirTextBox.Text = curPath;   //更新路径

            //按照左孩子-右兄弟树的结构, 依次显示该目录下的文件夹/文件
            if (pNode.leftChild == null)
                return;
            Catalog.node tmp = pNode.leftChild;
            //添加列表元素
            addListView(tmp);
            //加载所有右兄弟
            tmp = tmp.rightSibling;
            while (tmp != null)
            {
                addListView(tmp);
                tmp = tmp.rightSibling;
            }
        }

        //添加树形列表显示
        public void treeViewLoad(Catalog.node pNode)
        {
            //当前节点空，不需要显示
            if(pNode == null)
            {
                return;
            }
            //添加树节点
            TreeNode tn = new TreeNode();
            //树节点为文件，更新属性
            if(pNode.file.type == 1)
            {
                tn.Name = pNode.file.fileName;
                tn.Text = pNode.file.fileName;
                tn.Tag = 1;
            }
            //树节点为文件夹，更新属性
            else
            {
                tn.Name = pNode.file.fileName + ".txt";
                tn.Text = pNode.file.fileName + ".txt";
                tn.Tag = 0;
            }
            //根节点下则添加节点显示
            if(pNode.parent==rootNode)
            {
                FolderTreeView.Nodes.Add(tn);
            }
            else
            {
                //遍历添加
                foreach (TreeNode n in FolderTreeView.Nodes)
                {
                    addTreeNode(n, pNode.parent.file.fileName, tn);
                }
            }
            //递归左孩子右兄弟
            treeViewLoad(pNode.leftChild);
            treeViewLoad(pNode.rightSibling);
        }

        //添加树节点
        public TreeNode addTreeNode(TreeNode tnParent, string tnStr, TreeNode newTn)
        {
            if (tnParent == null)
                return null;
            if (tnParent.Name == tnStr)
            {
                tnParent.Nodes.Add(newTn);
            }

            TreeNode tnRet = null;
            foreach (TreeNode tn in tnParent.Nodes)
            {
                tnRet = addTreeNode(tn, tnStr, newTn);
                if (tnRet != null)
                {
                    tnRet.Nodes.Add(newTn);
                    break;
                }
            }
            return tnRet;
        }

        //点击创建文件按钮
        private void CreateFile_Click(object sender, EventArgs e)
        {
            //弹出窗口提示
            string str = Interaction.InputBox("请输入文件的名称", "字符串", "请输入文件名", 100, 100);
            if (str != "")
            {
                //如果名字不相同
                if (noSameName(str, curRoot, 0))
                {
                    //获取创建时间
                    string time = DateTime.Now.ToLocalTime().ToString(); 
                    //更新属性
                    Catalog.node tmp = new Catalog.node();
                    tmp.file.fileName = str;
                    tmp.file.modifyTime = time;
                    tmp.file.type = 0;
                    tmp.file.size = 0;
                    //添加
                    addListView(tmp);
                    catalog.AddtoTree(curRoot.file.fileName, new FCB(str, 0, time, 0)); 
                }
                else
                {
                    MessageBox.Show("已存在名为" + str + ".txt的文件，创建失败！");
                }
            }
            //目录树更新
            FolderTreeView.Nodes.Clear();
            treeViewLoad(rootNode.leftChild);
        }

        //点击“返回上一级目录”按钮
        private void BacktoParent_Click(object sender, EventArgs e)
        {
            //更新当前路径
            curPath = curPath.Replace("> " + curRoot.file.fileName, "");
            //当前节点指向上一级
            curRoot = curRoot.parent;
            if(curRoot == rootNode)
            {
                curPath = "root";
            }
            //更新页面显示
            DirTextBox.Text = curPath;
            fileLoad(curRoot);
        }

        //点击列表元素
        private void FileMenuStrip1_MouseClick(object sender, MouseEventArgs e)
        {
            // 获取鼠标右键点击的ListViewItem
            ListViewItem clickedItem = DetailListView.GetItemAt(e.X, e.Y);
            if (e.Button == MouseButtons.Right)
            {
                // 如果存在点击的ListViewItem
                if (clickedItem != null)
                {
                    // 设置FileMenuStrip1显示的位置
                    FileMenuStrip1.Show(DetailListView, e.Location);
                    //读取当前右键点击的ListView项目的信息
                    ListViewItem selectedItem = DetailListView.SelectedItems[0];
                    //将信息存储到MenuStript的Tag标签中进行传递
                    if (selectedItem.SubItems[2].Text == "文本文档")
                    {
                        //去掉文件（夹）大小的最后一位'B'，方便将其调整为int型存入Tag
                        selectedItem.SubItems[3].Text = selectedItem.SubItems[3].Text.TrimEnd('B');
                        //将信息存入Tag
                        FileMenuStrip1.Tag = new Catalog.node(new FCB(selectedItem.SubItems[0].Text, 0, selectedItem.SubItems[1].Text, int.Parse(selectedItem.SubItems[3].Text)));
                    }
                    else if (selectedItem.SubItems[2].Text == "文件夹")
                    {
                        //去掉文件（夹）大小的最后一位'B'，方便将其调整为int型存入Tag
                        selectedItem.SubItems[3].Text = selectedItem.SubItems[3].Text.TrimEnd('B');
                        //将信息存入Tag
                        FileMenuStrip1.Tag = new Catalog.node(new FCB(selectedItem.SubItems[0].Text, 1, selectedItem.SubItems[1].Text, int.Parse(selectedItem.SubItems[3].Text)));
                    }
                }
            }
            //鼠标左键点击
            else if (e.Button == MouseButtons.Left)
            {
                // 如果存在点击的ListViewItem
                if (clickedItem != null)
                {
                    switch (clickedItem.Tag)
                    {
                        //文件夹
                        case 1:
                            curPath = "";
                            //用来计算路径
                            curRoot = catalog.searchNode(rootNode, clickedItem.Text, 1);
                            Catalog.node pNode = curRoot;
                            Stack<string> path = new Stack<string>();
                            path.Push(pNode.file.fileName);
                            //更新路径
                            while (pNode.parent != null)
                            {
                                path.Push(pNode.parent.file.fileName);
                                pNode = pNode.parent;
                            }
                            while (path.Count() != 0)
                            {
                                curPath += "> " + path.Pop();
                            }
                            DirTextBox.Text = curPath;
                            //刷新界面
                            fileLoad(curRoot);
                            break;
                        //文件
                        case 0:
                            NotePadForm file = new NotePadForm(clickedItem.Text.Replace(".txt", ""), this);
                            file.Show();
                            break;
                        default:
                            break;
                    }
                }
            }
        }

        //小状态栏中点击打开
        private void 打开ToolStripMenuItem_Click(object sender, EventArgs e)
        {
            //从Tag中读取信息并存储到data中
            Catalog.node data = FileMenuStrip1.Tag as Catalog.node;
            if (data.file.type == 0)
            {
                NotePadForm file = new NotePadForm(data.file.fileName, this);
                file.Show();
            }
            else
            {
                curPath = "";
                //用来计算路径
                Stack<string> path = new Stack<string>();
                path.Push(data.file.fileName);
                curRoot = catalog.searchNode(rootNode, data.file.fileName, 1);
                //更新路径
                while (data.parent != null)
                {
                    path.Push(data.parent.file.fileName);
                    data = data.parent;
                }
                while (path.Count() != 0)
                {
                    curPath += "> " + path.Pop();
                }
                DirTextBox.Text = curPath;
                //刷新界面
                fileLoad(curRoot);
            }
            //Debug.Text = "" + data.file.initPos;
            //Debug.Text = "" + virtualDisk.readFileContent(data.file);
        }

        //状态栏中点击删除
        private void 删除ToolStripMenuItem_Click(object sender, EventArgs e)
        {
            //从Tag中读取信息并存储到data中
            Catalog.node data = FileMenuStrip1.Tag as Catalog.node;
            if (data.file.type == 0)
            {
                //删除文件并更新
                Catalog.node delFile = catalog.searchNode(rootNode, data.file.fileName, data.file.type);
                catalog.delFile(data.file.fileName);
                virtualDisk.delFileContent(delFile.file.initPos, delFile.file.size);
                fileLoad(curRoot);
            }
            else
            {
                //删除文件夹并更新
                catalog.delFolder(data.file.fileName);
                fileLoad(curRoot);
            }
            //界面更新
            FolderTreeView.Nodes.Clear();
            treeViewLoad(rootNode.leftChild);
            DiskSituation();
        }

        //状态栏中点击重命名
        private void 重命名ToolStripMenuItem_Click(object sender, EventArgs e)
        {
            //获取信息
            Catalog.node data = FileMenuStrip1.Tag as Catalog.node;
            //弹窗命名
            string str = Interaction.InputBox("请输入新名称", "字符串", "", 100, 100);
            if(!string.IsNullOrEmpty(str))
            {
                Catalog.node renameFile = catalog.searchNode(rootNode, data.file.fileName, data.file.type);
                renameFile.file.fileName = str;
            }
            //更新界面
            fileLoad(curRoot);
            FolderTreeView.Nodes.Clear();
            treeViewLoad(rootNode.leftChild);
            DiskSituation();
        }

        //双击目录树
        private void FolderTreeView_DoubleClick(object sender, EventArgs e)
        {
            //获取点击信息
            TreeNode tn = FolderTreeView.SelectedNode;
            switch (tn.Tag)
            {
                //文件夹
                case 1:
                    curPath = "";
                    //用来计算路径
                    Stack<string> path = new Stack<string>();
                    path.Push(tn.Text);
                    curRoot = catalog.searchNode(rootNode, tn.Text, 1);
                    //更新路径
                    while (tn.Parent != null)
                    {
                        path.Push(tn.Parent.Text);
                        tn = tn.Parent;
                    }
                    while (path.Count() != 0)
                    {
                        curPath += "> " + path.Pop();
                    }
                    DirTextBox.Text = curPath;
                    //刷新界面
                    fileLoad(curRoot);
                    break;
                //文件
                case 0:
                    NotePadForm file = new NotePadForm(tn.Text.Replace(".txt", ""), this);
                    file.Show();
                    break;
                default:
                    break;
            }   
        }

        //判断是否有重名
        public bool noSameName(string name, node root, int type)
        {
            //从根节点开始遍历
            root = root.leftChild;
            //根节点为空直接返回true，即没有重名
            if (root == null)
            {
                return true;
            }
            //找到重名返回false
            else if (root.file.fileName == name && root.file.type == type)
            {
                return false;
            }
            else
            {
                //临时节点暂存，辅助遍历查找重名
                node tmp = root.rightSibling;
                while (tmp != null)
                {
                    if (tmp.file.fileName == name && tmp.file.type == type)
                    {
                        return false;
                    }
                    tmp = tmp.rightSibling;
                }
                return true;
            }
        }

        //点击“格式化”按钮
        private void Formatting_Click(object sender, EventArgs e)
        {
            //弹窗问询
            DialogResult confirm = MessageBox.Show("确定清空磁盘？", "提示信息", MessageBoxButtons.OKCancel, MessageBoxIcon.Information);
            if (confirm == DialogResult.OK)
            {
                catalog.delcatalog(catalog.root);
                for (int i = 0; i < virtualDisk.blockSize; i++)
                {
                    virtualDisk.memory[i] = "";
                    virtualDisk.bitMap[i] = -1;
                    virtualDisk.space_left= virtualDisk.blockNum;
                }
                //更新界面
                fileLoad(rootNode);
                DiskSituation();
                curPath = "";
                DirTextBox.Text = curPath;
                FolderTreeView.Nodes.Clear();
                //更新日志（即删除日志文件）
                if (System.IO.File.Exists(Application.StartupPath + "\\info\\catalog.txt") && System.IO.File.Exists(Application.StartupPath + "\\info\\BitMap.txt") && System.IO.File.Exists(Application.StartupPath + "\\info\\Disk.txt"))
                {
                    System.IO.File.Delete(Application.StartupPath + "\\info\\catalog.txt");
                    System.IO.File.Delete(Application.StartupPath + "\\info\\BitMap.txt");
                    System.IO.File.Delete(Application.StartupPath + "\\info\\Disk.txt");
                }
            }
        }

        //用来debug
        //public void myDebug()
        //{
        //    Debug.Text = "Error Happen";
        //}

        //点击添加文件夹按钮
        private void CreateFolder_Click(object sender, EventArgs e)
        {
            //弹窗提示
            string str = Interaction.InputBox("请输入文件夹的名称", "字符串", "请输入文件名", 100, 100);
            if (str != "")
            {
                if (noSameName(str, curRoot, 0))
                {
                    //获取创建时间
                    string time = DateTime.Now.ToLocalTime().ToString();
                    Catalog.node tmp = new Catalog.node();
                    tmp.file.fileName = str;
                    tmp.file.modifyTime = time;
                    tmp.file.type = 1;
                    tmp.file.size = 0;
                    addListView(tmp);
                    catalog.AddtoTree(curRoot.file.fileName, new FCB(str, 1, time, 0));
                }
                else
                {
                    MessageBox.Show("已存在名为" + str + "的文件夹，创建失败！");
                }
            }
            //目录树更新
            FolderTreeView.Nodes.Clear();
            treeViewLoad(rootNode.leftChild);
        }

        //初始化，读取日志文件
        public void init()
        {
            //如果不存在日志文件夹则创建空文件夹
            if (!Directory.Exists(Application.StartupPath + "\\info"))
            {
                Directory.CreateDirectory(Application.StartupPath + "\\info");
            }
            //如果不存在日志文件则创建空内容
            if (!System.IO.File.Exists(Application.StartupPath + "\\info\\catalog.txt") && !System.IO.File.Exists(Application.StartupPath + "\\info\\BitMap.txt") && !System.IO.File.Exists(Application.StartupPath + "\\info\\Disk.txt"))
            {
                System.IO.File.Create(Application.StartupPath + "\\info\\catalog.txt").Close();
                //初始化文件
                StreamWriter writerCatalog = System.IO.File.AppendText(Application.StartupPath + "\\info\\catalog.txt");
                writerCatalog.WriteLine("END");
                writerCatalog.Close();
                System.IO.File.Create(Application.StartupPath + "\\info\\BitMap.txt").Close();
                StreamWriter writerBitMap = new StreamWriter(Application.StartupPath + "\\info\\BitMap.txt");
                for (int i = 0; i < virtualDisk.blockNum; i++)
                {
                    writerBitMap.WriteLine("-1");
                }
                writerBitMap.Close();
                System.IO.File.Create(Application.StartupPath + "\\info\\Disk.txt").Close();
            }

            //读目录文件
            string pathCatalog = Application.StartupPath + "\\info\\catalog.txt";
            if (System.IO.File.Exists(pathCatalog))
            {
                StreamReader reader = new StreamReader(pathCatalog);
                string str = "";
                while (str!="END")
                {
                    string parentName = reader.ReadLine();
                    //读到end就结束
                    if(parentName=="END")
                    {
                        break;
                    }
                    //逐行读取信息
                    string name = reader.ReadLine();
                    int type = int.Parse(reader.ReadLine());
                    string modifyTime = reader.ReadLine();
                    int size = int.Parse(reader.ReadLine());
                    int initPos = int.Parse(reader.ReadLine());
                    str = reader.ReadLine();
                    FCB a =new FCB(name,type,modifyTime,size);
                    a.initPos = initPos;
                    catalog.AddtoTree(parentName, new Catalog.node(a).file);
                }
                reader.Close();
            }

            // 读位图文件
            string pathBitMap = Application.StartupPath + "\\info\\BitMap.txt";
            if (System.IO.File.Exists(pathBitMap))
            {
                StreamReader reader = new StreamReader(pathBitMap);
                for (int i = 0; i < virtualDisk.blockNum; i++)
                {
                    virtualDisk.bitMap[i] = int.Parse(reader.ReadLine());
                }
                reader.Close();
            }
            //Debug.Text = "" + virtualDisk.bitMap[0] + virtualDisk.bitMap[1];

            //读磁盘文件
            string pathDisk = Application.StartupPath + "\\info\\Disk.txt";
            if (System.IO.File.Exists(pathDisk))
            {
                StreamReader reader = new StreamReader(pathDisk);
                for (int i = 0; i < virtualDisk.blockNum; i++)
                {
                     virtualDisk.memory[i] = reader.ReadLine();
                }
                reader.Close();
            }
            //Debug.Text = "" + virtualDisk.memory[0] + virtualDisk.memory[1];
        }

        //退出时保存目录，位图和磁盘信息
        protected override void OnFormClosing(FormClosingEventArgs e)
        {
            string path = Application.StartupPath;

            //保存目录
            if (System.IO.File.Exists(path + "\\info\\catalog.txt"))
                System.IO.File.Delete(path + "\\info\\catalog.txt");
            string PathCatalog = path + "\\info\\catalog.txt";
            StreamWriter writerCatalog = System.IO.File.AppendText(PathCatalog);
            
            //利用栈结构辅助遍历目录树
            Queue<Catalog.node> q = new Queue<Catalog.node>();
            Catalog.node tmp = new Catalog.node();
            tmp = catalog.root;
            q.Enqueue(tmp);
            while (q.Count() != 0)
            {
                tmp = q.Dequeue();
                tmp = tmp.leftChild;
                while (tmp != null)
                {
                    q.Enqueue(tmp);
                    Catalog.node parent = catalog.currentRootName(rootNode, tmp.file.fileName, tmp.file.type);
                    writerCatalog.WriteLine(parent.file.fileName);
                    writerCatalog.WriteLine(tmp.file.fileName);
                    writerCatalog.WriteLine(tmp.file.type);
                    writerCatalog.WriteLine(tmp.file.modifyTime);
                    writerCatalog.WriteLine(tmp.file.size);
                    if (tmp.file.type == 0)
                    {
                        writerCatalog.WriteLine(tmp.file.initPos);
                    }
                    else if (tmp.file.type == 1)
                    {
                        writerCatalog.WriteLine(-1);
                    }
                    writerCatalog.WriteLine("#");
                    //不断搜索右兄弟直到叶节点
                    tmp = tmp.rightSibling;
                }
            }
            //结尾加上END标识结束
            writerCatalog.WriteLine("END");
            writerCatalog.Close();

            //保存位图
            if (System.IO.File.Exists(path + "\\info\\BitMap.txt"))
                System.IO.File.Delete(path + "\\info\\BitMap.txt");
            string PathBitMap = path + "\\info\\BitMap.txt";
            StreamWriter writerBitMap = new StreamWriter(PathBitMap);
            for (int i = 0; i < virtualDisk.blockNum; i++)
            {
                writerBitMap.WriteLine(virtualDisk.bitMap[i]);
            }
            writerBitMap.Close();

            //保存磁盘内容
            if (System.IO.File.Exists(path + "\\info\\Disk.txt"))
                System.IO.File.Delete(path + "\\info\\Disk.txt");
            string PathDisk = path + "\\info\\Disk.txt";
            StreamWriter writerDisk = new StreamWriter(PathDisk);
            for (int i = 0; i < virtualDisk.blockNum; i++)
            {
                writerDisk.WriteLine(virtualDisk.memory[i]);
            }
            writerDisk.Close();
        }

        //label及“当前路径为”
        private void label1_Click(object sender, EventArgs e)
        {

        }

        private void DirTextBox_TextChanged(object sender, EventArgs e)
        {

        }

        private void DetailListView_SelectedIndexChanged(object sender, EventArgs e)
        {

        }

        private void FolderTreeView_AfterSelect(object sender, TreeViewEventArgs e)
        {

        }
    }
}

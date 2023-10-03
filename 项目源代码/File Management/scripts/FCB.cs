using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Xml.Linq;
using static File_Management.Catalog;

namespace File_Management
{
    //设置FCB类，存放文件的基础信息
    public class FCB
    {
        //文件名
        public string fileName;
        //文件在内存中初始存放的位置
        public int initPos;         
        /*
        文件标识：
        txt文件为0，文件夹为1
        */
        public int type;
        //修改时间         
        public string modifyTime;
        //文件大小
        public int size;    

        //构造函数
        public FCB() { }
        //重载构造函数
        public FCB(string name, int type, string time, int size)
        {
            //文件名
            this.fileName = name;
            //文件类型
            this.type = type;
            //修改时间 
            this.modifyTime = time;
            //文件大小
            this.size = size;
        }
    }

    //目录类，存放目录信息
    public class Catalog
    {
        //节点类
        public class node
        {
            //设定FCB
            public FCB file = new FCB();
            //利用树的二叉树表示法，即左孩子右兄弟
            //树的左孩子
            public node leftChild = null;
            //树的右兄弟
            public node rightSibling = null;
            //树的父母节点
            public node parent = null;

            //构造函数
            public node() { }
            //重载构造函数
            public node(FCB fcb)
            {
                file.fileName = fcb.fileName;
                file.modifyTime = fcb.modifyTime;
                file.type = fcb.type;
                file.size = fcb.size;
                file.initPos = fcb.initPos;
            }
        }
        //公共变量
        public node root;

        //构造函数
        public Catalog() 
        {
            root = null;
        }

        //根据目录根节点递归删除某个目录
        public void delcatalog(node initNode)
        {
            //遍历到叶节点返回
            if (initNode == null)
            {
                return;
            }
            //清空左孩子
            if (initNode.leftChild != null)
            {
                delcatalog(initNode.leftChild);
                initNode.leftChild = null;
            }
            //清空右兄弟
            if (initNode.rightSibling != null)
            {
                delcatalog(initNode.rightSibling);
                initNode.rightSibling = null;
            }
            //清空根节点
            initNode = null;                 
        }

        //根据节点名字来搜索某个节点并返回
        public node searchNode(node initNode, string fileName, int type)
        {
            //遍历到叶节点返回
            if (initNode == null)
            {
                return null;
            }
            //找到后返回
            if (initNode.file.fileName == fileName && initNode.file.type == type)
            {
                return initNode;
            }
            //未找到则返回null
            if (initNode.leftChild == null && initNode.rightSibling == null)
            {
                return null;
            }
            //不断寻找
            else
            {
                //搜索孩子节点
                node Child = searchNode(initNode.leftChild, fileName, type);
                if (Child != null)
                {
                    //左孩子节点不为空继续搜索
                    return Child;
                }
                else
                {
                    //开始搜索右兄弟
                    return searchNode(initNode.rightSibling, fileName, type);
                }
            }
        }

        //根据文件（夹）名添加文件（夹）到目录树里
        public void AddtoTree(string parentName,FCB fcb)
        {
            //不存在根节点就直接返回
            if (root == null)
            {
                return;
            }
            //根据名字找到父母节点
            node parentNode = searchNode(root, parentName, 1);
            //如果父母节点左孩子为空，则加入
            if(parentNode.leftChild == null)
            {
                parentNode.leftChild = new node(fcb);
                parentNode.leftChild.parent = parentNode;
            }
            else
            {
                //临时节点暂存，辅助遍历找到插入位置
                node tmp = parentNode.leftChild;
                while(tmp.rightSibling!=null)
                {
                    tmp = tmp.rightSibling;
                }
                tmp.rightSibling = new node(fcb);
                tmp.rightSibling.parent = parentNode;
            }
        }

        //根据文件名删除一个文件
        public void delFile(string name)
        {
            //找到删除的文件节点
            node pNode = searchNode(root, name, 0);  
            node parentNode = pNode.parent;
            //如果文件是左孩子节点，直接删除（替换）
            if(parentNode.leftChild==pNode)
            {
                parentNode.leftChild = pNode.rightSibling;
            }
            else
            {
                //临时节点暂存，辅助遍历实现删除
                node tmp = parentNode.leftChild;
                while(tmp.rightSibling!=pNode)
                {
                    tmp = tmp.rightSibling;
                }
                tmp.rightSibling = pNode.rightSibling;
            }
            pNode = null;
        }

        //根据文件夹名称删除一个文件夹
        public void delFolder(string name)
        {
            //找到删除的文件夹节点
            node delNode = searchNode(root, name, 1);
            node parentNode = delNode.parent;
            //如果文件夹是左孩子节点，直接删除（替换）
            if ( parentNode.leftChild==delNode)
            {
                parentNode.leftChild = delNode.rightSibling;
            }
            else
            {
                //临时节点暂存，辅助遍历实现删除
                node tmp = parentNode.leftChild;
                while(tmp.rightSibling!=delNode)
                {
                    tmp = tmp.rightSibling;
                }
                tmp.rightSibling = delNode.rightSibling;
            }
            //以此文件夹为根节点删除其下所有目录
            delcatalog(delNode);
        }

        //找到当前节点的最近根节点
        public node currentRootName(node pNode, string name, int type)
        {
            //空
            if (pNode == null)
            { 
                return null;
            }
            //孩子为空，也不合理
            if (pNode.leftChild== null)
            { 
                return null; 
            }
            else
            {
                //找到后返回节点
                if (pNode.leftChild.file.fileName == name && pNode.leftChild.file.type == type)
                {
                    return pNode;
                }
                else
                {
                    node parent = pNode;
                    node tmp = pNode.leftChild.rightSibling;
                    while (tmp != null)
                    {
                        //找到后返回节点
                        if (tmp.file.fileName == name && tmp.file.type == type)
                        { 
                            return parent; 
                        }
                        //不断指向右兄弟
                        else
                        { 
                            tmp = tmp.rightSibling;
                        }
                    }
                    //在左孩子中递归
                    if (currentRootName(parent.leftChild, name, type) != null)   
                    {
                        return currentRootName(parent.leftChild, name, type);
                    }
                    //在右兄弟中递归
                    else
                    {
                        return currentRootName(parent.leftChild.rightSibling, name, type);
                    }
                }
            }
        }
    }
}

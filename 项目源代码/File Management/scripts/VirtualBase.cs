using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace File_Management
{
    //设置虚拟内存类，模拟文件及内容在内存中的运作
    public class VirtualBase
    {
        //公共变量
        //磁盘容量
        public int size;
        //磁盘剩余块数
        public int space_left;
        //块大小
        public int blockSize;
        //磁盘块数
        public int blockNum;
        //内存空间
        public string[] memory = new string[] {};
        //位图
        public int[] bitMap = new int[] {};      

        //构造函数，构造磁盘容量和块大小，并初始化各种变量
        public VirtualBase(int size, int blocksize)
        {
            //磁盘容量
            this.size = size;
            //单个块大小
            this.blockSize = blocksize;
            //磁盘块数
            this.blockNum = size / blockSize;
            //磁盘剩余块数
            this.space_left = blockNum;
            //申请内存空间
            this.memory = new string[blockNum];
            //申请位图空间
            this.bitMap = new int[blockNum];
            //初始化位表和内存
            for (int i = 0; i < blockNum; i++)
            {
                //初始化位图表为全部可用
                this.bitMap[i] = -1;
                //初始化内存内容为空
                this.memory[i] = "";   
            }
        }

        /*读取文件内容*/
        public string readFileContent(FCB fcb)
        {
            //当初始位置为-1时，文件无内容
            if (fcb.initPos == -1)
            {
                return null;
            }
            else
            {
                //定义容器存放文件内容
                string content ="";
                //计算文件需要占用的块数
                int blocks = fcb.size / blockSize + (fcb.size % blockSize != 0 ? 1 : 0);
                //已经拼接的块数
                int merged = 0;
                //定义位置不断获取内容
                int pos = fcb.initPos;
                while (merged != blocks)
                {
                    //拼接内存的一个单元的数据
                    content += memory[pos];
                    //跳转到下一个存储单元
                    pos = bitMap[pos];          
                    merged++;
                }
                return content;
            }
        }

        /*删除文件内容*/
        public void delFileContent(int initPos, int size)
        {
            //计算文件占用的块数
            int blocks = size / blockSize + (size % blockSize != 0 ? 1 : 0);
            //已经删除的块数
            int deled = 0;
            //定义位置指向删除内容
            int pos = initPos;
            while (deled != blocks)
            {
                //逐内存单元的清空
                memory[pos] = "";        
                space_left++;
                //先记录即将跳转的位置
                int next = bitMap[pos];
                //清空该位
                bitMap[pos] = -1;  
                //更新指针
                pos = next;
                deled++;
            }
        }

        //给新加入的文件分配空间
        public bool arrangeSpace(FCB fcb, string content)
        {
            //计算文件需要占用的块数
            int blocks = fcb.size / blockSize + (fcb.size % blockSize != 0 ? 1 : 0);
            //当剩余的块数足够放下这个文件
            if (blocks <= space_left)
            {
                //该文件开始存放的位置
                int pos = 0;
                //遍历每一块
                for (int i = 0; i < blockNum; i++)
                {
                    //如果位图表示空
                    if (bitMap[i] == -1)
                    {
                        //剩余容量-1
                        space_left--;
                        //将此空位作为文件内容起始点
                        pos = i;
                        fcb.initPos = pos;
                        //在这个位置的内存块中存放块大小的内容
                        memory[i] = content.Substring(0, blockSize);
                        break;
                    }
                }
                //从找到位置开始存放内容
                for (int j = 1, i = pos + 1; j < blocks && i < blockNum; i++)
                {
                    //找到下一个空位
                    if (bitMap[i] == -1)
                    {
                        //剩余容量-1
                        space_left--;
                        //更新位图
                        bitMap[pos] = i;
                        pos = i;
                        if (blocks == 1)
                        {
                            //更新内存
                            memory[i] = content;
                        }
                        else
                        {
                            if (j != blocks - 1)
                            {
                                memory[i] = content.Substring(j * blockSize, blockSize);
                            }
                            else
                            {
                                // 文件结束
                                bitMap[i] = -2; 
                                if (fcb.size % blockSize != 0)
                                {
                                    memory[i] = content.Substring(j * blockSize, content.Length - j * blockSize);
                                }
                                else
                                {
                                    memory[i] = content.Substring(j * blockSize, blockSize);
                                }
                            }
                        }
                        j++;
                    }
                }
                return true;
            }
            //内存不够
            else{ 
                return false; 
            }
        }
    }
}

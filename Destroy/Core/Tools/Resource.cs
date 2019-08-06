using System;
using System.Collections.Generic;
using System.Drawing;
using System.IO;

namespace Destroy
{
    /// <summary>
    /// 资源读取模块 使用Resouce.Load&lt;Type&gt;加载资源文件 <see langword="static"/>
    /// </summary>
    public static class Resource
    {
        /// <summary>
        /// 默认资源文件所处位置
        /// </summary>
        public static string ResoucePath = Environment.CurrentDirectory + "\\Resouce";

        /// <summary>
        /// 资源名字和完整路径
        /// </summary>
        public static Dictionary<string, string> Name_Path = new Dictionary<string, string>();

        /// <summary>
        /// 初始化,将Resource下的文件检测一边,并保存其路径
        /// </summary>
        public static void Init()
        {
            if (!Directory.Exists(ResoucePath))
            {
                Directory.CreateDirectory(ResoucePath);
            }
            FindALLInPath(ResoucePath);
        }


        /// <summary>
        /// 加载资源文件,如果类型不支持或者没有对应文件,则返回null
        /// 不能区分重名文件,参数名字不包括扩展名
        /// </summary>
        /// <typeparam name="T">资源类型,目前仅支持文本和Image两种,其他的会返回null</typeparam>
        /// <param name="name">不包括扩展名的文件名,只要处于Resource文件夹下的文件都可以搜索到</param>
        /// <returns></returns>
        public static T Load<T>(string name) where T : class
        {
            if (!Name_Path.ContainsKey(name))
                return null;
            Type type = typeof(T);
            string Path = Name_Path[name];
            if (type == typeof(Image))
            {
                Image image = Image.FromFile(Path);
                return image as T;
            }
            else if (type == typeof(StreamReader))
            {
                return GetReader(Path) as T;
            }
            else if (type == typeof(string))
            {
                return ReadAllText(Path) as T;
            }
            else
            {
                return null;
            }
        }

        /// <summary>
        /// 从路径下查找所有文件,并加入字典
        /// </summary>
        private static void FindALLInPath(string sSourcePath)
        {
            void FindInPath(DirectoryInfo dir)
            {
                foreach (FileInfo f in dir.GetFiles("*.*", SearchOption.TopDirectoryOnly))
                {
                    string name = f.ToString().Split('.')[0];
                    string path = dir + @"\" + f.ToString();
                    if (!Name_Path.ContainsKey(name))
                        Name_Path.Add(name, path);
                }
            }
            //在指定目录及子目录下查找文件,在list中列出子目录及文件
            DirectoryInfo Dir = new DirectoryInfo(sSourcePath);
            DirectoryInfo[] DirSub = Dir.GetDirectories();
            if (DirSub.Length <= 0)
            {
                FindInPath(Dir);
            }
            int t = 1;
            foreach (DirectoryInfo d in DirSub)//查找子目录 
            {
                FindALLInPath(Dir + @"\" + d.ToString());
                if (t == 1)
                {
                    FindInPath(Dir);
                    t = t + 1;
                }
            }
        }

        private static string ReadAllText(string Path)
        {
            StreamReader reader = GetReader(Path);
            string str = reader.ReadToEnd();
            reader.Close();
            return str;
        }

        private static StreamReader GetReader(string Path)
        {
            FileStream fs = new FileStream(Path, System.IO.FileMode.Open, System.IO.FileAccess.Read);
            StreamReader sr = new StreamReader(fs, System.Text.Encoding.UTF8);
            return sr;
        }

        private static StreamWriter GetWriter(string Path)
        {
            FileStream fs = new FileStream(Path, System.IO.FileMode.Open, System.IO.FileAccess.Write);
            StreamWriter sw = new StreamWriter(fs, System.Text.Encoding.UTF8);
            return sw;
        }
    }

    /// <summary>
    /// 将字符串和Image对应起来,渲染的时候如果检测到有对应Image,则换成使用Image而非字符渲染
    /// 使用方式,渲染图片的前提是图片一定要被对应为一个ACSII字符. 想要显示为图片就要指定为对应字符
    /// 相应的,如果一个字符被指定为一个Image进行渲染,那么就不会再显示这个字符本身了
    /// </summary>
    public static class ImageConvertor
    {
        /// <summary>
        /// 对应字典
        /// </summary>
        public static Dictionary<string, Image> ImageDic = new Dictionary<string, Image>();

        /// <summary>
        /// 将对应加入字典
        /// </summary>
        public static void Init()
        {

        }

    }
}

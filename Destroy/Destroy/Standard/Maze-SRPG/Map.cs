using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Destroy;

namespace Destroy.Standard
{
    /// <summary>
    /// 地图加载,从文件中加载地图
    /// </summary>
    public static class MapLoader
    {
        /// <summary>
        /// 加载指定资源名字作为地图
        /// </summary>
        public static void LoadMap(string name)
        {
            List<Vector2> list = new List<Vector2>();
            StreamReader sr = Resource.Load<StreamReader>(name);
            int y = 23;
            string str = "";
            while ((str = sr.ReadLine()) != null)
            {
                for (int i = 0; i < str.Length; i++)
                {
                    if (str[i] == 'X')
                    {
                        list.Add(new Vector2(i, y));
                    }
                }
                y--;
            }
            CreateWall(list);
        }

        /// <summary>
        /// 创建地图对象
        /// </summary>
        public static GameObject CreateWall(List<Vector2> list)
        {
            GameObject gameObject = new GameObject("Wall", "Wall");
            Mesh mesh = gameObject.AddComponent<Mesh>();
            mesh.Init(list);
            var renderer =  gameObject.AddComponent<Renderer>();

            RenderPoint rp = new RenderPoint("■", Color.White, Config.DefaultBackColor, (int)Layer.Environment);
            renderer.Rendering(rp);

            gameObject.AddComponent<Collider>();
            return gameObject;
        }

    }
}

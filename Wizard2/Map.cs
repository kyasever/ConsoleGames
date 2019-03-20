using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Destroy;

namespace Wizard2
{
    public static class Map
    {
        public static void LoadMap()
        {
            List<Vector2> list = new List<Vector2>();
            StreamReader sr = Resource.Load<StreamReader>("Scene1");
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

        public static GameObject CreateWall(List<Vector2> list)
        {
            GameObject gameObject = new GameObject("Wall", "Wall");
            Mesh mesh = gameObject.AddComponent<Mesh>();
            mesh.Init(list);
            var renderer =  gameObject.AddComponent<Renderer>();

            RenderPoint rp = new RenderPoint("■", Colour.White, Config.DefaultBackColor, (int)Layer.Environment);
            renderer.Rendering(rp);

            gameObject.AddComponent<Collider>();
            return gameObject;
        }

    }
}

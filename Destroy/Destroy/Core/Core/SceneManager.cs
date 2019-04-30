namespace Destroy
{
    using System;
    using System.Collections.Generic;

    /// <summary>
    /// 场景基类, 用于保存游戏物体的引用
    /// </summary>
    public class Scene
    {
        /// <summary>
        /// 场景的名字
        /// </summary>
        public string Name { get; private set; }

        /// <summary>
        /// 场景中的游戏对象的数量
        /// </summary>
        public int GameObjectCount => GameObjects.Count;

        internal List<GameObject> GameObjects { get; private set; }

        internal Dictionary<string, List<GameObject>> GameObjectsWithTag { get; private set; }

        /// <summary>
        /// 用类名当名字
        /// </summary>
        public Scene()
        {
            Name = GetType().Name;
            GameObjects = new List<GameObject>();
            GameObjectsWithTag = new Dictionary<string, List<GameObject>>();
        }

        /// <summary>
        /// 指定名字
        /// </summary>
        public Scene(string name)
        {
            Name = name;
            GameObjects = new List<GameObject>();
            GameObjectsWithTag = new Dictionary<string, List<GameObject>>();
        }

        /// <summary>
        /// 在创建的时候会进行调用
        /// </summary>
        public virtual void OnStart() { }

        /// <summary>
        /// 在销毁这个Scene时调用
        /// </summary>
        public virtual void OnDestroy() { }
    }

    /// <summary>
    /// 默认场景
    /// </summary>
    internal class DefaultScene : Scene
    {
        public override void OnStart()
        {
            //创建摄像机
            GameObject camera = new GameObject("Camera", "MainCamera")
            {
                Position = new Vector2(0, 0)
            };
            Camera cam = camera.AddComponent<Camera>();
        }

        public override void OnDestroy() { }
    }

    /// <summary>
    /// 场景加载模式
    /// </summary>
    public enum LoadSceneMode
    {
        /// <summary>
        /// 创建之后只保留这个Scene 并把DontDestroyOnLoad放入这个Scene
        /// </summary>
        Single = 0,
        /// <summary>
        /// 这个Scene作为额外的Scene创建. 依然以之前场景作为主场景
        /// </summary>
        Additive = 1,
    }

    /// <summary>
    /// 场景管理器
    /// </summary>
    public static class SceneManager
    {
        /// <summary>
        /// 场景数量
        /// </summary>
        public static int SceneCount => Scenes.Count;

        /// <summary>
        /// 当前激活的场景,所有新创建的对象都自动属于当前激活的场景
        /// </summary>
        internal static Scene ActiveScene { get; set; }

        /// <summary>
        /// 字符串索引,保存着所有场景
        /// </summary>
        internal static Dictionary<string, Scene> Scenes { get; set; }

        private static Scene dontDestroyOnLoadScene { get; set; }

        internal static DefaultScene DefaultScene { get; set; }


        /// <summary>
        /// 在初始化的时候创建两个默认的Scene. 这两个Scene永远不会被销毁
        /// </summary>
        public static void Init()
        {
            Scenes = new Dictionary<string, Scene>();
            dontDestroyOnLoadScene = new Scene();
            DefaultScene = new DefaultScene();
            Load(DefaultScene, LoadSceneMode.Additive);
            Scenes.Add(dontDestroyOnLoadScene.Name, dontDestroyOnLoadScene);
            ActiveScene = DefaultScene;
        }


        /// <summary>
        /// 设置某个场景为活动场景
        /// </summary>
        public static void SetActiveScene(Scene scene)
        {
            ActiveScene = scene;
        }

        /// <summary>
        /// 通过字符串,设置某个场景为活动场景
        /// </summary>
        public static void SetActiveScene(string str)
        {
            if (Scenes.ContainsKey(str))
                ActiveScene = Scenes[str];
        }

        /// <summary>
        /// 加载场景
        /// </summary>
        public static void Load(Scene newScene, LoadSceneMode loadSceneMode = LoadSceneMode.Single)
        {
            if (newScene == null)
            {
                throw new Exception("Null Exception");
            }
            if (loadSceneMode == LoadSceneMode.Single)
            {
                //如果原来激活的不是默认场景, 则销毁原来的场景
                if (ActiveScene != DefaultScene)
                {
                    //销毁所有之前场景的游戏物体, 如果dontdestroy保存着它的对象,则不销毁
                    foreach (GameObject gameObject in ActiveScene.GameObjects)
                    {
                        if (!dontDestroyOnLoadScene.GameObjects.Contains(gameObject))
                            DestroyObject(gameObject);
                    }
                    //移除之前场景的引用
                    Scenes.Remove(ActiveScene.Name);
                    //销毁回调事件
                    ActiveScene.OnDestroy();
                }
                //设置新场景为激活场景
                ActiveScene = newScene;
                //将DontDestroyOnLoad中保存的对象加入新的激活场景
                foreach (GameObject gameObject in dontDestroyOnLoadScene.GameObjects)
                {
                    gameObject.AddToScene(ActiveScene);
                }
                //初始化新场景
                newScene.OnStart();
            }
            if (loadSceneMode == LoadSceneMode.Additive)
            {
                Scene lastScene = ActiveScene;
                //切换当前Scene为Active, 然后新建的GameObject都是这个Scene的
                ActiveScene = newScene;
                //调用这个场景的初始化方法 创建属于这个场景的对象等等
                ActiveScene.OnStart();
                //切回之前的Scene作为主场景
                ActiveScene = lastScene;
            }
            //将这个Scene加入dict
            Scenes.Add(newScene.Name, newScene);
        }

        /// <summary>
        /// 在当前场景中根据名字寻找游戏物体, 若有多个同名物体也只返回一个。
        /// </summary>
        public static GameObject Find(string name)
        {
            GameObject result = null;
            ActiveScene.GameObjects.ForEach(gameObject => { if (gameObject.Name == name) result = gameObject; });
            return result;
        }

        /// <summary>
        /// 在当前场景中根据标签寻找游戏物体, 若有多个则返回多个。
        /// </summary>
        public static List<GameObject> FindWithTag(string tag)
        {
            List<GameObject> gameObjects = null;
            if (ActiveScene.GameObjectsWithTag.ContainsKey(tag))
                gameObjects = ActiveScene.GameObjectsWithTag[tag];
            return gameObjects;
        }

        /// <summary>
        /// 在指定场景中根据名字寻找游戏物体, 若有多个同名物体也只返回一个。
        /// </summary>
        public static GameObject Find(string sceneName, string name)
        {
            Scene scene = null;

            if (SceneManager.Scenes.ContainsKey(sceneName))
                scene = SceneManager.Scenes[sceneName];

            if (scene == null)
                return null;

            GameObject result = null;
            scene.GameObjects.ForEach(gameObject => { if (gameObject.Name == name) result = gameObject; });
            return result;
        }

        /// <summary>
        /// 在指定场景中根据标签寻找游戏物体, 若有多个则返回多个。
        /// </summary>
        public static List<GameObject> FindWithTag(string sceneName, string tag)
        {
            Scene scene = null;

            if (SceneManager.Scenes.ContainsKey(sceneName))
                scene = SceneManager.Scenes[sceneName];
            if (scene == null)
                return null;

            List<GameObject> gameObjects = null;

            if (scene.GameObjectsWithTag.ContainsKey(tag))
                gameObjects = scene.GameObjectsWithTag[tag];

            return gameObjects;
        }

        /// <summary>
        /// 销毁一个游戏物体
        /// </summary>
        public static void DestroyObject(GameObject gameObject)
        {
            //进行这个设置的时候,就已经关掉了所有组件了.对于系统来说相当于已经被移除
            gameObject.SetActive(false);
            //在最后移除它
            RuntimeEngine.GetSystem<DeleteSystem>().GameObjectsToDelete.Add(gameObject);
        }


        /// <summary>
        /// 将一个游戏物体的引用加入DontDestroyOnLoad
        /// </summary>
        public static void DontDestroyOnLoad(GameObject gameObject)
        {
            if (!dontDestroyOnLoadScene.GameObjects.Contains(gameObject))
                gameObject.AddToScene(dontDestroyOnLoadScene);

            if (gameObject.ChildCount != 0)
            {
                foreach (var go in gameObject.Transform.Childs)
                {
                    DontDestroyOnLoad(go.GameObject);
                }
            }
        }

    }
}
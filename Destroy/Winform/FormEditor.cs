


namespace Destroy.Winform
{
    using System;
    using System.Collections.Generic;
    using System.Drawing;
    using System.Reflection;
    using System.Windows.Forms;

    /// <summary>
    /// 界面窗体交互部分
    /// </summary>
    public partial class FormEditor : Form
    {
        /// <summary>
        /// 单例
        /// </summary>
        public static FormEditor Instanse { get; private set; }

        /// <summary>
        /// 是否打开编辑器,非编辑器模式关掉不需要的功能.
        /// </summary>
        public static bool EditorMode = true;

        /// <summary>
        /// 
        /// </summary>
        public FormEditor()
        {
            Instanse = this;
            InitializeComponent();

            windowSize = new Size(Config.ScreenWidth * (int)Config.CharWidth * Config.RendererSize.X / 2, Config.ScreenHeight * Config.RendererSize.Y);
            panelGameWindow.Size = windowSize;
            panelGame.Size = windowSize;
            if (EditorMode)
                this.Size = new Size(windowSize.Width + 500, windowSize.Height + 200);
            else
            {
                menuStrip1.Visible = false;
                statusStrip1.Visible = false;
                panelCom.Visible = false;
                panelObj.Visible = false;
                panelMsg.Visible = false;
                this.Size = new Size(windowSize.Width + 20, windowSize.Height + 50);
            }
                
        }

        /// <summary>
        /// 
        /// </summary>
        public void UpdateForm()
        {
            if (!EditorMode)
                return;
            RefrestGameObjects();
            if (CurrertGameObject != null)
            {
                SetRightTreeView(CurrertGameObject, false);
            }
            toolStripStatusLabelMousePosition.Text = Input.MousePosition.ToString();
        }

        public void UpdateLabel2(string s)
        {
            toolStripStatusLabel1.Text = s;
        }

        /// <summary>
        /// 用来保存节点和游戏对象的对应关系
        /// </summary>
        public static Dictionary<TreeNode, GameObject> tree_NodeDic = new Dictionary<TreeNode, GameObject>();

        /// <summary>
        /// 当前被选中的游戏物体
        /// </summary>
        public GameObject CurrertGameObject;

        /// <summary>
        /// 要打开的游戏窗体的大小
        /// </summary>
        private Size windowSize;

        /// <summary>
        /// 向listbos里添加MSG
        /// </summary>
        public void AddMessage(string msg)
        {
            listBoxMsg.Items.Add(msg);
            listBoxMsg.TopIndex = listBoxMsg.Items.Count - 1;
        }

        #region 游戏场景树 将游戏物体加入到指定TreeView中
        /// <summary>
        /// 当前场景中的活动对象,只有当活动对象的数量改变时或者100毫秒检测时,才会刷新游戏物体列表
        /// </summary>
        private static int GameObjectCount = 0;

        /// <summary>
        /// 刷新场景树的数据
        /// 随时间刷新,感觉不太好,但也没啥特别方便的办法了
        /// </summary>
        public void RefrestGameObjects()
        {
            Dictionary<string, Scene> scenedic = SceneManager.Scenes;
            Scene activeScene = SceneManager.ActiveScene;

            int count = activeScene.GameObjectCount;

            if (count != GameObjectCount)
            {
                treeViewObj.Nodes.Clear();

                foreach (Scene scene in scenedic.Values)
                {
                    //将这个场景的节点加入根节点
                    TreeNode treeNodeSceneRoot = treeViewObj.Nodes.Add(scene.Name);

                    //找到场景的物体列表
                    PropertyInfo gameObjectsWithTagProp = scene.GetType().GetProperty("GameObjectsWithTag",
                        BindingFlags.Instance | BindingFlags.NonPublic);
                    Dictionary<string, List<GameObject>> activeGameObjectsWithTag =
                        gameObjectsWithTagProp.GetValue(scene) as Dictionary<string, List<GameObject>>;

                    foreach (string tag in activeGameObjectsWithTag.Keys)
                    {
                        TreeNode treeNodeTag = treeNodeSceneRoot.Nodes.Add(tag);
                        //将所有子物体加入这个节点
                        foreach (GameObject gameObject in activeGameObjectsWithTag[tag])
                        {
                            //如果这个是根节点,那么递归调用添加其所有子节点
                            if (gameObject.Parent == null)
                            {
                                AddGameObject(treeNodeTag, gameObject);
                            }
                        }
                        //所有节点都展开一层
                        treeNodeTag.Expand();
                    }

                    //如果这个场景是激活的,那么展开这个节点 并改变颜色
                    if (scene == activeScene)
                    {
                        treeNodeSceneRoot.ForeColor = System.Drawing.Color.Orange;
                        treeNodeSceneRoot.Expand();
                    }
                    else
                    {
                        treeNodeSceneRoot.ForeColor = System.Drawing.Color.Gray;
                    }
                }
                GameObjectCount = count;
            }

        }

        /// <summary>
        /// 递归添加GameObjects节点
        /// </summary>
        private void AddGameObject(TreeNode treeNode, GameObject gameObject)
        {
            TreeNode cNode = treeNode.Nodes.Add(gameObject.Name);
            //不活跃的为灰色
            if (gameObject.IsActive)
            {
                cNode.ForeColor = System.Drawing.Color.Black;
            }
            else
            {
                cNode.ForeColor = System.Drawing.Color.Gray;
            }
            tree_NodeDic.Add(cNode, gameObject);

            if (gameObject.ChildCount > 0)
            {
                foreach (GameObject v in gameObject.Childs)
                {
                    AddGameObject(cNode, v);
                }
            }
        }

        #endregion

        #region 绘制游戏场景


        /// <summary>
        /// panel绘制方法
        /// </summary>
        public void Draw()
        {
            using (Graphics graphics = panelGameWindow.CreateGraphics())
            {
                //lock(PreRenderer.bufferBitmap)
                //{
                    graphics.DrawImage(EditorRuntime.bufferBitmap, new PointF(0, 0));

                //}
                EditorSystem.RenderCount++;
            }
        }

        #endregion

        #region 组件信息树 根据游戏物体生成其组件信息一览
        /// <summary>
        /// 可能会有点隐患.应该考虑给每一个Node一个id,作为key,这样应该就好了,
        /// id和对象池一样 
        /// </summary>
        private TreeNode AddNode(TreeNodeCollection rootNode, string key, string value)
        {
            TreeNode oNode;

            //GameObject信息
            if (rootNode.ContainsKey(key))
            {
                oNode = rootNode[key];
                if (oNode.Text != value)
                    oNode.Text = value;
            }
            else
            {
                oNode = rootNode.Add(key, value);
            }
            return oNode;
        }

        /// <summary>
        /// 加入Node,key-value都取字符串名字
        /// </summary>
        private TreeNode AddNode(TreeNodeCollection rootNode, string key_value)
        {
            return AddNode(rootNode, key_value, key_value);
        }


        /// <summary>
        /// 将一个任意对象加入一个节点,根据类型进行不同的处理,key value都根据obj的状态酌情处理
        /// </summary>
        private TreeNode AddNode(TreeNodeCollection rootNode, object obj, string name)
        {
            if (obj == null)
            {
                return AddNode(rootNode, name, name + " : null");
            }
            Type type = obj.GetType();

            string info, value;

            TreeNode node;


            if (type == typeof(List<Vector2>))
            {
                List<Vector2> list = obj as List<Vector2>;
                info = name + " : " + type.Name + "[" + list.Count + "]";
                node = AddNode(rootNode, info);
                int i = 0;
                foreach (var v in list)
                {
                    value = v.ToString();
                    AddNode(node.Nodes, i.ToString(), value);
                    i++;
                }
            }
            else
            {
                info = name + " : ";
                value = obj.ToString();
                node = AddNode(rootNode, info, info + value);
            }
            return node;
        }

        private TreeNode AddGameObjectNode(TreeNodeCollection rootNode, GameObject gameObject)
        {
            TreeNode oNode = AddNode(rootNode, "GameObject", "GameObject:" + gameObject.Name);

            if (!gameObject.IsActive)
            {
                oNode.ForeColor = System.Drawing.Color.Gray;
            }
            else
            {
                oNode.ForeColor = System.Drawing.Color.Orange;
            }

            //属性默认显示
            BindingFlags flag = BindingFlags.Instance | BindingFlags.NonPublic | BindingFlags.Public;
            foreach (PropertyInfo pInfo in gameObject.GetType().GetProperties(flag))
            {
                HideInInspector inspectorF = pInfo.GetCustomAttribute<HideInInspector>();
                if (inspectorF != null)
                {
                    continue;
                }
                AddNode(oNode.Nodes, pInfo.GetValue(gameObject), pInfo.Name);
            }

            //不管是private的还是public的字段,只要挂了ShowInI就可以显示,某则一律隐藏
            foreach (FieldInfo fInfo in gameObject.GetType().GetFields(flag))
            {
                ShowInInspector inspectorF = fInfo.GetCustomAttribute<ShowInInspector>();
                if (inspectorF != null)
                {
                    var obj = fInfo.GetValue(gameObject);
                    if (obj != null)
                    {
                        AddNode(oNode.Nodes, fInfo.GetValue(gameObject), fInfo.Name);
                    }
                }
            }

            return oNode;
        }

        private TreeNode AddCompontNode(TreeNodeCollection rootNode, Component component)
        {
            if (component.GetType().GetCustomAttribute<HideInInspector>() != null)
            {
                return null;
            }
            TreeNode cNode = AddNode(rootNode, component.GetType().FullName);

            if (component.Enable)
            {
                cNode.ForeColor = System.Drawing.Color.Blue;
            }
            else
            {
                cNode.ForeColor = System.Drawing.Color.Gray;
            }


            //属性默认显示
            BindingFlags flag = BindingFlags.Instance | BindingFlags.NonPublic | BindingFlags.Public;
            foreach (PropertyInfo pInfo in component.GetType().GetProperties(flag))
            {
                HideInInspector inspectorF = pInfo.GetCustomAttribute<HideInInspector>();
                if (inspectorF != null)
                {
                    continue;
                }
                AddNode(cNode.Nodes, pInfo.GetValue(component), pInfo.Name);
            }

            //不管是private的还是public的字段,只要挂了ShowInI就可以显示,某则一律隐藏
            foreach (FieldInfo fInfo in component.GetType().GetFields(flag))
            {
                ShowInInspector inspectorF = fInfo.GetCustomAttribute<ShowInInspector>();
                if (inspectorF != null)
                {
                    var obj = fInfo.GetValue(component);
                    if (obj != null)
                    {
                        AddNode(cNode.Nodes, fInfo.GetValue(component), fInfo.Name);
                    }
                }
            }
            return cNode;
        }

        /// <summary>
        /// 刷新右侧组件信息界面
        /// </summary>
        public void SetRightTreeView(GameObject gameObject, bool firstOpen)
        {
            Dictionary<Type, Component> components =
                RefHelper.GetPrivateProperty<Dictionary<Type, Component>>(gameObject, "ComponentDict");

            var node = AddGameObjectNode(treeViewCom.Nodes, gameObject);
            if (firstOpen)
            {
                node.Expand();
            }

            foreach (Component component in components.Values)
            {
                node = AddCompontNode(treeViewCom.Nodes, component);
                if (firstOpen && node != null)
                {
                    node.Expand();
                }
            }
        }

        #endregion

        #region 界面回调
        private void TreeViewObj_AfterSelect(object sender, TreeViewEventArgs e)
        {
            //如果有对应的GameObject 刷新对应组件的数据
            if (tree_NodeDic.ContainsKey(treeViewObj.SelectedNode))
            {
                CurrertGameObject = tree_NodeDic[treeViewObj.SelectedNode];
                treeViewCom.Nodes.Clear();
                SetRightTreeView(CurrertGameObject, true);
            }
        }

        private void FormEditor_KeyDown(object sender, KeyEventArgs e)
        {
            if (treeViewObj.Focused || treeViewCom.Focused)
            {
                //一个权宜之计,当检测到按键的时候,自动把焦点设置成别的,避免TreeView在那搞事情
                //最好的做法还是重写一下TreeView.然后把那东西的键盘删了
                //或者搞一个选中设置.
                panelGame.Focus();
            }
        }

        private void PanelGameWindow_MouseMove(object sender, MouseEventArgs e)
        {
            EditorSystem.MousePosition = e.Location.ToVector2Int();
        }

        private void FormEditor_FormClosing(object sender, FormClosingEventArgs e)
        {
            RuntimeEngine.Enabled = false;
            //关闭主窗口时关闭所有线程
            System.Environment.Exit(0);
        }


        #endregion
    }
}

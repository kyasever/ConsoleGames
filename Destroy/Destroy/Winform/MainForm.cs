//TODO : 彩色TextBox
namespace Destroy.Winform
{
    using Destroy;
    using Destroy.Windows;
    using System;
    using System.Collections.Generic;
    using System.Drawing;
    using System.Drawing.Drawing2D;
    using System.Drawing.Text;
    using System.Reflection;
    using System.Threading;
    using System.Windows.Forms;
    using System.Threading.Tasks;

    /// <summary>
    /// Editor的主窗口
    /// </summary>
    public partial class MainForm : Form
    {
        /// <summary>
        /// 单例
        /// </summary>
        public static MainForm Instanse { get; private set; }

        /// <summary>
        /// 单例
        /// </summary>
        public MainForm()
        {
            Instanse = this;
            InitializeComponent();
        }

        /// <summary>
        /// 用来保存节点和游戏对象的对应关系
        /// </summary>
        public static Dictionary<TreeNode, GameObject> tree_NodeDic = new Dictionary<TreeNode, GameObject>();

        /// <summary>
        /// 当前被选中的游戏物体
        /// </summary>
        public GameObject CurrertGameObject;

        #region 窗口设置
        private int GameObjectPanelWide = 150;
        private int ComponentPanelMinWide = 250;
        private int MessagePanelMinHeight = 150;
        private Vector2Int windowSize;
        /// <summary>
        /// 开始的时调用一次,设置窗口大小
        /// </summary>
        public void InitSize()
        {
            //透明模式 不用了
            //BackColor = Color.Yellow;
            //TransparencyKey = Color.FromArgb(0, Color.Yellow);
            //panelGame.Visible = false;

            this.Location = new Point(20, 20);
            windowSize = new Vector2Int(Config.ScreenWidth * (int)Config.CharWidth * 8, Config.ScreenHeight * 16);
            //设置此窗口的位置
            //this.Location = new Point(windowPos.X - LocationDis.X, windowPos.Y - LocationDis.Y);
            int formWidth = windowSize.X + GameObjectPanelWide + ComponentPanelMinWide + 15;
            int formHeight = windowSize.Y + MessagePanelMinHeight + 88;
            this.Size = new Size(formWidth, formHeight);
        }

        /// <summary>
        /// 重新给Editor排版
        /// </summary>
        public void RefreshEditoerPosition()
        {
            //设置五个分窗口的位置
            int Height = panelMain.Size.Height;
            int Width = panelMain.Size.Width;
            groupBoxGameObjects.Location = new Point(0, 0);
            groupBoxGameObjects.Size = new Size(GameObjectPanelWide, Height);

            panelGame.Location = new Point(GameObjectPanelWide, 0);
            panelGame.Size = new Size(windowSize.X, windowSize.Y);

            groupBoxComponent.Location = new Point(GameObjectPanelWide + windowSize.X, 0);
            int ComponentPanelWide = Math.Max(ComponentPanelMinWide, Width - windowSize.X - GameObjectPanelWide);
            groupBoxComponent.Size = new Size(ComponentPanelWide, windowSize.Y);

            groupBoxMessage.Location = new Point(GameObjectPanelWide, windowSize.Y);
            int MessagePanelHeight = Math.Max(MessagePanelMinHeight, Height - windowSize.Y);
            int MessagePanelWidth = (Width - GameObjectPanelWide) / 2;
            groupBoxMessage.Size = new Size(MessagePanelWidth, MessagePanelHeight);

            groupBoxOther.Location = new Point(GameObjectPanelWide + MessagePanelWidth, windowSize.Y);
            groupBoxOther.Size = new Size(MessagePanelWidth, MessagePanelHeight);
        }

        /// <summary>
        /// 向listbos里添加MSG
        /// </summary>
        public void AddMessage(string msg)
        {
            listBoxMessage.Items.Add(msg);
            listBoxMessage.TopIndex = listBoxMessage.Items.Count - 1;
        }
        #endregion

        #region 左侧游戏场景树界面处理方法
        /// <summary>
        /// 当前场景中的活动对象,只有当活动对象的数量改变时或者100毫秒检测时,才会刷新游戏物体列表
        /// </summary>
        private static int GameObjectCount = 0;

        /// <summary>
        /// 刷新场景树的数据
        /// 随时间刷新,感觉不太好,但也没啥特别方便的办法了
        /// 把非active的折叠起来,把UI一层 感觉这样可行
        /// </summary>
        public void RefrestGameObjects()
        {
            Dictionary<string, Scene> scenedic = SceneManager.Scenes;
            Scene activeScene = SceneManager.ActiveScene;

            int count = activeScene.GameObjectCount;

            if (count != GameObjectCount)
            {
                treeViewLeft.Nodes.Clear();

                foreach (Scene scene in scenedic.Values)
                {
                    //将这个场景的节点加入根节点
                    TreeNode treeNodeSceneRoot = treeViewLeft.Nodes.Add(scene.Name);

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
            if (gameObject.Active)
            {
                cNode.ForeColor = Color.Black;
            }
            else
            {
                cNode.ForeColor = Color.Gray;
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

        #region 中间游戏场景的界面处理
        /// <summary>
        /// panel绘制方法
        /// </summary>
        public void Draw(List<RenderPoint> drawList)
        {
            using (Graphics graphics = panelGame.CreateGraphics())
            {
                Bitmap bitmap = new Bitmap(windowSize.X, windowSize.Y);
                Graphics g = Graphics.FromImage(bitmap);
                for (int i = 0; i < drawList.Count; i++)
                {
                    if (i > 0 && CharUtils.GetCharWidth(drawList[i - 1].Str[0]) == 2)
                    {
                        continue;
                    }

                    RenderPoint rp = drawList[i];
                    //这个字体制表符是占两位的.....
                    //RenderPoint rp = new RenderPoint(Destroy.Template.BoxDrawingSupply.boxVerticalLeft.ToString(),-1);
                    Color foreColor = rp.ForeColor.ToColor();
                    Color backColor = rp.BackColor.ToColor();

                    SolidBrush solidBrushFore = new SolidBrush(foreColor);
                    SolidBrush solidBrushBack = new SolidBrush(backColor);
                    Font font;
                    //if (CharUtils.IsTabChar(rp.Str[0]))
                    //{
                    //    font = new Font("Consolas", 14, FontStyle.Bold);
                    //}
                    //else
                    //{

                    //    font = new Font("新宋体", 14, FontStyle.Bold);
                    //}

                    font = new Font("Consolas", 13, FontStyle.Bold);

                    int pixelX = i % (Config.ScreenWidth * (int)Config.CharWidth) * RenderPoint.Size.X / 2;
                    int pixelY = i / (Config.ScreenWidth * (int)Config.CharWidth) * RenderPoint.Size.Y;
                    //DebugLog(new Vector2Int(pixelX, pixelY).ToString());
                    Point point = new Point(pixelX, pixelY);

                    g.FillRectangle(solidBrushBack, new RectangleF(new Point(point.X + 4, point.Y),
                        new SizeF(RenderPoint.Size.X, RenderPoint.Size.Y)));
                    g.DrawString(rp.Str, font, solidBrushFore, point);


                }
                graphics.DrawImage(bitmap, new PointF(0, 0));
            }
        }

        #endregion

        #region 右侧组件信息的界面处理

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
            Type type = obj.GetType();

            string info, value;

            TreeNode node;

            if (type == typeof(List<Vector2Int>))
            {
                List<Vector2Int> list = obj as List<Vector2Int>;
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

            if (!gameObject.Active)
            {
                oNode.ForeColor = Color.Gray;
            }
            else
            {
                oNode.ForeColor = Color.Orange;
            }

            TreeNode position = AddNode(oNode.Nodes, "Position", "Position:" + gameObject.Position.ToString());
            TreeNode localPosition = AddNode(oNode.Nodes, "Local", "LocalPos:" + gameObject.LocalPosition.ToString());
            TreeNode count = AddNode(oNode.Nodes, "CC", "ComponentsCount:" + gameObject.ComponentCount.ToString());
            if (gameObject.ChildCount > 0)
            {
                TreeNode childCount = AddNode(oNode.Nodes, "ChildC", "ChildCount:" + gameObject.ChildCount.ToString());
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
                cNode.ForeColor = Color.Blue;
            }
            else
            {
                cNode.ForeColor = Color.Gray;
            }

            Type componentType = component.GetType();

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

            var node = AddGameObjectNode(treeViewRight.Nodes, gameObject);
            if (firstOpen)
            {
                node.Expand();
            }

            foreach (Component component in components.Values)
            {
                node = AddCompontNode(treeViewRight.Nodes, component);
                if (firstOpen && node != null)
                {
                    node.Expand();
                }
            }
        }

        #endregion

        #region 窗体事件
        private Task resizeTask;

        /// <summary>
        /// 重新选择节点之后会产生刷新
        /// </summary>
        private void treeView1_AfterSelect(object sender, TreeViewEventArgs e)
        {
            //如果有对应的GameObject 刷新对应组件的数据
            if (tree_NodeDic.ContainsKey(treeViewLeft.SelectedNode))
            {
                CurrertGameObject = tree_NodeDic[treeViewLeft.SelectedNode];
                treeViewRight.Nodes.Clear();
                SetRightTreeView(CurrertGameObject, true);
            }
        }

        private void MainForm_KeyDown(object sender, KeyEventArgs e)
        {
            if (treeViewLeft.Focused || treeViewRight.Focused)
            {
                //一个权宜之计,当检测到按键的时候,自动把焦点设置成别的,避免TreeView在那搞事情
                //最好的做法还是重写一下TreeView.然后把那东西的键盘删了
                //或者搞一个选中设置.
                panelGame.Focus();
            }
        }

        private void panelGame_MouseMove(object sender, MouseEventArgs e)
        {
            EditorSystem.MousePosition = e.Location.ToVector2Int();
        }

        private void MainForm_Resize(object sender, EventArgs e)
        {
            if (resizeTask != null && !resizeTask.IsCompleted)
            {
                return;
            }
            resizeTask = Task.Run(async () =>
            {
                await Task.Delay(200);
                Invoke(new Action(RefreshEditoerPosition));
            });
        }

        private void MainForm_FormClosing(object sender, FormClosingEventArgs e)
        {
            RuntimeEngine.Enabled = false;
            //关闭主窗口时关闭所有线程
            System.Environment.Exit(0);
        }

        #endregion
    }
}

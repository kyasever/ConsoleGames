namespace Destroy
{
    using System;
    using System.Collections;
    using System.Collections.Generic;

    /// <summary>
    /// DestroyUI 用这个创建常用UI组件
    /// </summary>
    public static class UIFactroy
    {
        /// <summary>
        /// 文本框UI组件
        /// </summary>
        /// <param name="pos">文本框的起点位置</param>
        /// <param name="height">高度,不包含边框,表示这个文本框可以容纳几行字符</param>
        /// <param name="width">宽度,单条信息的宽度</param>
        /// <returns></returns>
        public static TextBox CreateTextBox(Vector2 pos, int height, int width)
        {
            GameObject gameObject = new GameObject("TextBox", "UI");
            gameObject.Position = pos;

            //添加一个TextBox控件,用于寻找对应的Lable
            TextBox textBox = gameObject.AddComponent<TextBox>();
            //添加Label控件
            for (int i = height; i > 0; i--)
            {
                Renderer label = CreateLabel(pos + new Vector2(1, i), "", width);
                label.GameObject.Parent = gameObject;

                textBox.Labels.Add(label);
            }

            //边框
            Rectangle rectangle = new Rectangle(width + 2, height + 2);
            GameObject drawLine = new GameObject("BoxDrawing", "UI");
            drawLine.Parent = gameObject;
            drawLine.LocalPosition = new Vector2(0, 0);
            Mesh mesh = drawLine.AddComponent<Mesh>();
            mesh.Init(rectangle.PosList);
            Renderer renderer = drawLine.AddComponent<Renderer>();
            renderer.Init(RendererMode.UI, -1);
            renderer.Rendering(rectangle.Str, Config.DefaultForeColor, Config.DefaultBackColor);
            textBox.boxDrawing = drawLine;

            return textBox;
        }

        /// <summary>
        /// 文本列表,可以使用光标选择,并带有滚动条.
        /// </summary>
        public static ListBox CreateListBox(Vector2 pos, int height, int width)
        {
            GameObject gameObject = new GameObject("ListBox", "UI");
            gameObject.Position = pos;

            //添加一个TextBox控件,用于寻找对应的Lable
            ListBox textBox = gameObject.AddComponent<ListBox>();
            textBox.Init(height, width);
            //边框
            Rectangle rectangle = new Rectangle(width + 2, height + 2);
            GameObject drawLine = new GameObject("BoxDrawing", "UI");
            drawLine.Parent = gameObject;
            drawLine.LocalPosition = new Vector2(0, 0);
            Mesh mesh = drawLine.AddComponent<Mesh>();
            mesh.Init(rectangle.PosList);
            Renderer renderer = drawLine.AddComponent<Renderer>();
            renderer.Init(RendererMode.UI, -1);
            renderer.Rendering(rectangle.Str, Config.DefaultForeColor, Config.DefaultBackColor);

            //进度条

            return textBox;
        }

        /// <summary>
        /// 创建一个Lable组件,不带有默认文字
        /// </summary>
        public static Renderer CreateLabel(Vector2 pos, int width)
        {
            GameObject lable = new GameObject("Label", "UI");
            //初始化位置
            lable.Position = pos;

            //添加一个宽度等同于width的Mesh
            Mesh mesh = lable.AddComponent<Mesh>();
            List<Vector2> meshList = new List<Vector2>();
            for (int i = 0; i < width; i++)
            {
                meshList.Add(new Vector2(i, 0));
            }
            mesh.Init(meshList);
            //添加一个Renderer组件
            Renderer renderer = lable.AddComponent<Renderer>();
            renderer.Init(RendererMode.UI, -1);
            //不进行初始化,手动进行添加
            return renderer;
        }

        /// <summary>
        /// 创建一个Lable
        /// </summary>
        public static Renderer CreateLabel(Vector2 pos, string text, int width)
        {
            Renderer renderer = CreateLabel(pos, width);
            renderer.Rendering(text);
            return renderer;
        }

    }


    /// <summary>
    /// 文本框组件
    /// </summary>
    public class TextBox : Component
    {
        /// <summary>
        /// 最后返回的应该是TextBox组件,可以通过label更改每一条的信息,
        /// </summary>
        public List<Renderer> Labels = new List<Renderer>();
        /// <summary>
        /// 边框的对象
        /// </summary>
        public GameObject boxDrawing;
        /// <summary>
        /// 更改对应行上面的字符,也可以通过获取Labels自己找对应的Label组件
        /// </summary>
        public bool SetText(string str, int line)
        {
            if (line > Labels.Count)
            {
                return false;
            }
            else if (line <= 0)
            {
                return false;
            }
            else
            {
                Labels[line - 1].Rendering(str);
                return true;
            }
        }
    }

    /// <summary>
    /// 原Label组件被删除,现在用Renderer代替.
    /// 请通过ListBox组件中的CreateLabel方法进行创建
    /// </summary>
    public class ListBoxItem : Component
    {
        /// <summary>
        /// 背景色
        /// </summary>
        public Color BackColor;
        /// <summary>
        /// 序号
        /// </summary>
        public int Index;
        /// <summary>
        /// 被点击时产生的事件
        /// </summary>
        public Action<int> OnClick;


        private Renderer renderer;
        private RayCastTarget rtarget;
        internal override void Initialize()
        {
            renderer = GetComponent<Renderer>();
            rtarget = GetComponent<RayCastTarget>();
            if (rtarget == null)
                rtarget = AddComponent<RayCastTarget>();
            rtarget.OnClickEvent += () =>
            {
                OnClick(Index);
            };
        }

        /// <summary>
        /// 设置选中状态
        /// </summary>
        public void SetSelected(bool selected)
        {
            if (selected)
            {
                renderer.SetBackColor(BackColor);
            }
            else
            {
                renderer.SetBackColor(Config.DefaultBackColor);
            }
        }
    }

    /// <summary>
    /// 列表组件
    /// 只有在视窗内的label才会active true,别的都是false.当移动的时候就是全体Label整体上/下平移.
    /// </summary>
    [ShowInInspector]
    public class ListBox : Script
    {
        /// <summary>
        /// 实现Ilist来管理Item
        /// </summary>
        public class ListBoxItemList : IList<ListBoxItem>
        {
            private Vector2 firstPos;
            /// <summary>
            /// 通过内部列表实现接口
            /// </summary>
            public List<ListBoxItem> mList;

            /// <summary>
            /// 构造的时候要告诉列表第一个字符串的位置在哪
            /// </summary>
            /// <param name="FirstPos"></param>
            public ListBoxItemList(Vector2 FirstPos)
            {
                firstPos = FirstPos;
                mList = new List<ListBoxItem>();
            }

            /// <summary>
            /// 最后一个对象
            /// </summary>
            public ListBoxItem LastListBoxItem
            {
                get
                {
                    if (mList.Count > 0)
                        return mList[mList.Count - 1];
                    else
                        return null;
                }
            }

            #region 实现List接口,在增删的过程中同时同步相应变量
            /// <summary>
            /// 
            /// </summary>
            public ListBoxItem this[int index] { get => ((IList<ListBoxItem>)mList)[index]; set => ((IList<ListBoxItem>)mList)[index] = value; }

            /// <summary>
            /// 
            /// </summary>
            public int Count => ((IList<ListBoxItem>)mList).Count;

            /// <summary>
            /// 
            /// </summary>
            public bool IsReadOnly => ((IList<ListBoxItem>)mList).IsReadOnly;

            /// <summary>
            /// 
            /// </summary>
            public void Add(ListBoxItem item)
            {
                if (LastListBoxItem != null)
                    item.LocalPosition = LastListBoxItem.GameObject.LocalPosition + new Vector2(0, -1);
                else
                    item.LocalPosition = firstPos;
                ((IList<ListBoxItem>)mList).Add(item);
                item.Index = Count - 1;
            }

            /// <summary>
            /// 
            /// </summary>
            public void Clear()
            {
                foreach (ListBoxItem l in mList)
                {
                    GameObject.Destroy(l.GameObject);
                }
                ((IList<ListBoxItem>)mList).Clear();
            }

            /// <summary>
            /// 
            /// </summary>
            public bool Contains(ListBoxItem item)
            {
                return ((IList<ListBoxItem>)mList).Contains(item);
            }

            /// <summary>
            /// 
            /// </summary>
            public void CopyTo(ListBoxItem[] array, int arrayIndex)
            {
                ((IList<ListBoxItem>)mList).CopyTo(array, arrayIndex);
            }

            /// <summary>
            /// 
            /// </summary>
            public IEnumerator<ListBoxItem> GetEnumerator()
            {
                return ((IList<ListBoxItem>)mList).GetEnumerator();
            }

            /// <summary>
            /// 
            /// </summary>
            public int IndexOf(ListBoxItem item)
            {
                return item.Index;
            }

            /// <summary>
            /// 
            /// </summary>
            public void Insert(int index, ListBoxItem item)
            {
                item.Position = mList[index].Position;
                for (int i = index; i < mList.Count; i++)
                {
                    mList[i].Position += new Vector2(0, -1);
                }
                ((IList<ListBoxItem>)mList).Insert(index, item);
            }

            /// <summary>
            /// 
            /// </summary>
            public bool Remove(ListBoxItem item)
            {
                int i = IndexOf(item);
                if (i > 0 && i < mList.Count - 1)
                {
                    RemoveAt(i);
                    return true;
                }
                else
                {
                    return false;
                }
            }

            /// <summary>
            /// 
            /// </summary>
            public void RemoveAt(int index)
            {
                GameObject.Destroy(mList[index].GameObject);
                for (int i = index; i < mList.Count; i++)
                {
                    mList[i].Position += new Vector2(0, 1);
                }
                ((IList<ListBoxItem>)mList).RemoveAt(index);
            }

            IEnumerator IEnumerable.GetEnumerator()
            {
                return ((IList<ListBoxItem>)mList).GetEnumerator();
            }

            #endregion
        }

        /// <summary>
        /// 边框对象
        /// </summary>
        public GameObject boxDrawing;

        /// <summary>
        /// 列表
        /// </summary>
        [HideInInspector]
        public ListBoxItemList Items;

        /// <summary>
        /// 长款
        /// </summary>
        public int Height, Width;



        private int selectedIndex = 0;

        /// <summary>
        /// 当前被选中Item的序号
        /// </summary>
        public int SelectedIndex
        {
            get => selectedIndex;
            set
            {
                if (selectedIndex != value)
                {
                    Items[value].SetSelected(true);
                    Items[selectedIndex].SetSelected(false);
                }
                selectedIndex = value;
            }
        }

        /// <summary>
        /// 当前视图最上面的Item的序号
        /// </summary>
        [ShowInInspector]
        public int TopIndex;
        /// <summary>
        /// 当前视图最下面的Item的序号
        /// </summary>
        [ShowInInspector]
        public int EndIndex;

        /// <summary>
        /// 初始化
        /// </summary>
        public void Init(int height, int width)
        {
            TopIndex = 0; EndIndex = 1;
            Height = height; Width = width;
            Items = new ListBoxItemList(new Vector2(1, height));
        }

        /// <summary>
        /// 使用这个方法来新建一个ListItem
        /// </summary>
        public ListBoxItem CreateLabelItem(string text)
        {
            GameObject label = new GameObject("Label" + Items.Count.ToString(), "UI");
            //初始化位置
            label.Parent = GameObject;

            //添加一个宽度等同于width的Mesh
            Mesh mesh = label.AddComponent<Mesh>();
            List<Vector2> meshList = new List<Vector2>();
            for (int i = 0; i < Width; i++)
            {
                meshList.Add(new Vector2(i, 0));
            }
            mesh.Init(meshList);
            //添加一个Renderer组件
            Renderer renderer = label.AddComponent<Renderer>();
            renderer.Init(RendererMode.UI, -1);
            //添加一个Label组件
            ListBoxItem itemCom = label.AddComponent<ListBoxItem>();
            itemCom.BackColor = Color.Yellow;
            itemCom.OnClick += OnClickAction;

            renderer.Rendering(text);

            //不进行初始化,手动进行添加
            return itemCom;
        }

        /// <summary>
        /// 当Item被点击的时候产生的事件
        /// </summary>
        public void OnClickAction(int index)
        {
            SelectedIndex = index;
        }

        /// <summary>
        /// 整体上滚
        /// </summary>
        /// <returns></returns>
        public bool RollUp()
        {
            //如果第一个label的坐标已经是height,说明到顶了,不能再滚动了
            if (Items[0].LocalPosition.Y == Height)
            {
                return false;
            }
            //所有label向下移动一格
            foreach (ListBoxItem v in Items)
            {
                v.LocalPosition += new Vector2(0, -1);
            }
            TopIndex--;
            EndIndex--;
            return true;
        }

        /// <summary>
        /// 整体下滚
        /// </summary>
        /// <returns></returns>
        public bool RollDown()
        {
            //如果最后一个label的坐标已经是1,说明到底了
            if (Items[Items.Count - 1].LocalPosition.Y == 1)
            {
                return false;
            }
            //所有label向下移动一格
            foreach (ListBoxItem v in Items)
            {
                v.LocalPosition += new Vector2(0, 1);
            }
            TopIndex++;
            EndIndex++;
            return true;
        }

        /// <summary>
        /// Update
        /// </summary>
        public override void Update()
        {
            //没有项的时候不操作
            if (Items.Count == 0)
            {
                return;
            }

            //调整开始目录和结束目录
            if (Items.Count >= Height)
            {
                EndIndex = TopIndex + Height - 1;
            }
            else
            {
                EndIndex = TopIndex + Items.Count - 1;
            }

            //移动到边界时滚动
            if (Input.GetKeyDown(ConsoleKey.Q))
            {
                if (SelectedIndex > 0)
                {
                    SelectedIndex--;
                    if (SelectedIndex < TopIndex)
                        RollUp();
                }
            }
            else if (Input.GetKeyDown(ConsoleKey.E))
            {
                if (SelectedIndex < Items.Count - 1)
                {
                    SelectedIndex++;
                    if (SelectedIndex > EndIndex)
                        RollDown();
                }
            }

            //隐藏位于屏幕外边的label
            foreach (ListBoxItem v in Items)
            {
                v.GameObject.SetActive(false);
            }

            for (int i = TopIndex; i <= EndIndex; i++)
            {
                Items[i].GameObject.SetActive(true);
            }
        }
    }
}
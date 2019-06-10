using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace Destroy.Winform
{
    public partial class FormEditor : Form
    {
        public FormEditor()
        {
            InitializeComponent();
            windowSize = new Size(Config.ScreenWidth * (int)Config.CharWidth * Config.RendererSize.X / 2, Config.ScreenHeight * Config.RendererSize.Y);
            panelGameWindow.Size = windowSize;
            panelGame.Size = windowSize;
            this.Size = new Size(windowSize.Width + 500, windowSize.Height + 200);
        }


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
    }
}

using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;

namespace Destroy.Winform
{
    /// <summary>
    /// 将编辑器的生命周期加入整体生命周期来管理
    /// 所有编辑器操作使用异步的方式进行更新,不会影响主线程
    /// 同时负责将Winform的API对接到引擎里
    /// </summary>
    public class EditorSystem : DestroySystem
    {
        /// <summary>
        /// 用于指示当前鼠标所处位置
        /// </summary>
        public static Vector2 MousePosition = new Vector2(-8, -8);

        private static FormEditor mainForm = FormEditor.Instanse;

        /// <summary>
        /// 初始化
        /// </summary>
        public override void Start()
        {
            mainForm = FormEditor.Instanse;
        }

        /// <summary>
        /// 预渲染计数
        /// </summary>
        public static int PreRenderCount = 0;
        /// <summary>
        /// 渲染计数
        /// </summary>
        public static int RenderCount = 0;
        /// <summary>
        /// 引擎计数
        /// </summary>
        public static int TotalCount = 0;

        private float lastTime = 0f;
        /// <summary>
        /// 更新
        /// </summary>
        public override void Update()
        {
            //帧数统计
            TotalCount++;
            if(Time.TotalTime - lastTime > 1f)
            {
                string s = new StringBuilder().AppendFormat("预渲染帧数:{0} | 游戏帧数:{1} | 逻辑帧数:{2}", PreRenderCount, RenderCount, TotalCount).ToString();
                mainForm.UpdateLabel2(s);
                lastTime = Time.TotalTime;
                PreRenderCount = 0;
                RenderCount = 0;
                TotalCount = 0;
            }

            //更新编辑器
            FormEditor.Instanse.Invoke(new Action(() =>
            {
                FormEditor.Instanse.UpdateForm();
            }));
        }


        /// <summary>
        /// 使用异步的方式执行渲染,由于GDI的性能瓶颈限制大约只有40帧.
        /// 所以实际帧率可能不是很乐观
        /// </summary>
        public static void Renderer(List<RenderPoint> list)
        {
            EditorRuntime.renderList = list;
        }

        /// <summary>
        /// 获得鼠标位置
        /// </summary>
        public static Vector2 GetMousePositionPixel() { return MousePosition; }

        /// <summary>
        /// Debug输出
        /// </summary>
        /// <param name="msg"></param>
        public static void DebugLog(string msg)
        {
            mainForm.Invoke(new Action(() =>
            {
                mainForm.AddMessage(msg);
            }));
        }
    }
}

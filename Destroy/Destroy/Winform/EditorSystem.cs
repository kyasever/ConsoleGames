using System;
using System.Collections.Generic;
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

        private static Task updateTask, drawTask;

        /// <summary>
        /// 初始化
        /// </summary>
        public override void Start()
        {
            mainForm = FormEditor.Instanse;
        }

        /// <summary>
        /// 更新
        /// </summary>
        public override void Update()
        {
            if (updateTask != null && !updateTask.IsCompleted)
            {
                return;
            }

            updateTask = Task.Run(() =>
            {
                mainForm.Invoke(new Action(() =>
                {
                    mainForm.RefrestGameObjects();
                    if (mainForm.CurrertGameObject != null)
                    {
                        mainForm.SetRightTreeView(mainForm.CurrertGameObject, false);
                    }
                }));
            });
        }


        /// <summary>
        /// 使用异步的方式执行渲染,由于GDI的性能瓶颈限制大约只有40帧.
        /// 所以实际帧率可能不是很乐观
        /// </summary>
        public static void Renderer(List<RenderPoint> list)
        {
            if (drawTask != null && !drawTask.IsCompleted)
            {
                return;
            }
            drawTask = Task.Run(() =>
            {
                //return;
                mainForm.Invoke(new Action(() =>
                {
                    mainForm.Draw(list);
                }));
            });
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

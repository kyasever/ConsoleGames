namespace Destroy
{
    using System;
    using System.Collections.Generic;

    /// <summary>
    /// 处理鼠标事件 通常只有UI和这个有关 游戏物体请用物理系统检测
    /// </summary>
    public class EventHandlerSystem : DestroySystem
    {
        /// <summary>
        /// UI射线检测组件
        /// </summary>
        public Dictionary<Vector2Int, List<RayCastTarget>> UITargets { get; private set; } = new Dictionary<Vector2Int, List<RayCastTarget>>();

        private bool KeyDown = false;
        private Vector2Int mousePos = new Vector2Int(-100, -100);

        /// <summary>
        /// Update
        /// </summary>
        public override void Update()
        {
            bool k = Input.GetMouseButton(MouseButton.Left);
            Vector2Int newPos = Input.MousePosition - Camera.Main.Position;

            //当检测到按键抬起的时候,视作点击.检测点击回调.这里不能用getkeydown
            if (KeyDown == true && k == false)
            {
                if (UITargets.ContainsKey(newPos))
                {
                    foreach (var target in UITargets[newPos])
                    {
                        if (target.Enable)
                        {
                            //不加event就可以这么写,加了就不能. 有点奇怪
                            target.OnClickEvent?.Invoke();
                        }
                    }
                }
            }
            //当产生位置移动的时候,产生移动进入离开事件回调.
            if (newPos != mousePos)
            {
                List<RayCastTarget> needToOutList = new List<RayCastTarget>();
                List<RayCastTarget> needToInList = new List<RayCastTarget>();

                if (UITargets.ContainsKey(mousePos))
                {
                    foreach (var target in UITargets[mousePos])
                    {
                        if (target.Enable)
                        {
                            needToOutList.Add(target);
                        }
                    }
                }

                if (UITargets.ContainsKey(newPos))
                {
                    foreach (var target in UITargets[newPos])
                    {
                        if (target.Enable)
                        {
                            //如果还是从一个组件移动到这个组件,那么两次都不唤醒.
                            if (needToOutList.Contains(target))
                            {
                                needToOutList.Remove(target);
                            }
                            else
                            {
                                needToInList.Add(target);
                            }
                        }
                    }
                }

                foreach (var t in needToOutList)
                {
                    t.OnMoveOutEvent?.Invoke();
                }

                foreach (var t in needToInList)
                {
                    t.OnMoveInEvent?.Invoke();
                }
            }

            KeyDown = k;
            mousePos = newPos;
        }

        /// <summary>
        /// 加入系统
        /// </summary>
        public void AddToSystem(RayCastTarget UITarget, Vector2Int position)
        {
            foreach (Vector2Int dis in UITarget.colliderList)
            {
                Vector2Int pos = position + dis;
                if (UITargets.ContainsKey(pos))
                {
                    UITargets[pos].Add(UITarget);
                }
                else
                {
                    UITargets.Add(pos, new List<RayCastTarget>() { UITarget });
                }
            }
        }

        /// <summary>
        /// 移除系统
        /// </summary>
        public void RemoveFromSystem(RayCastTarget UITarget, Vector2Int position)
        {
            foreach (Vector2Int dis in UITarget.colliderList)
            {
                Vector2Int pos = position + dis;
                if (UITargets.ContainsKey(pos))
                {
                    UITargets[pos].Remove(UITarget);
                    if (UITargets[pos].Count == 0)
                    {
                        UITargets.Remove(pos);
                    }
                }
            }
        }
    }
}

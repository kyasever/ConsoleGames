namespace Destroy
{
    using System.Collections.Generic;

    /*
     * 只有这个问题很大 还是应该整合进phy
     * 特殊处理吧. UI的特点就是不基于摄像机 别的也没了
     * 
     */
    /// <summary>
    /// 处理鼠标事件 通常只有UI和这个有关 游戏物体请用物理系统检测
    /// </summary>
    public class EventHandlerSystem : DestroySystem
    {
        public List<Collider> UICollection = new List<Collider>();

        /// <summary>
        /// UI射线检测组件
        /// </summary>
        public Dictionary<Vector2, List<Collider>> UITargets { get; private set; } = new Dictionary<Vector2, List<Collider>>();

        private bool KeyDown = false;
        private Vector2 mousePos = new Vector2(-100, -100);

        /// <summary>
        /// Update
        /// </summary>
        public override void Update()
        {
            UITargets = new Dictionary<Vector2, List<Collider>>();
            foreach (var collider in UICollection)
            {
                if (!collider.Enable)
                    continue;
                foreach (var dis in collider.ColliderList)
                {
                    Vector2 pos = collider.GameObject.Transform.Position + dis;
                    if (UITargets.ContainsKey(pos))
                    {
                        UITargets[pos].Add(collider);
                    }
                    else
                    {
                        UITargets.Add(pos, new List<Collider>() { collider });
                    }
                }
            }

            bool k = Input.GetMouseButton(MouseButton.Left);
            Vector2 newPos = Input.MousePosition - Camera.Main.Position;

            //当检测到按键抬起的时候,视作点击.检测点击回调.这里不能用getkeydown
            if (KeyDown == true && k == false)
            {
                if (UITargets.ContainsKey(newPos))
                {
                    foreach (var target in UITargets[newPos])
                    {
                        //不加event就可以这么写,加了就不能. 有点奇怪
                        target.OnClickEvent?.Invoke();
                    }
                }
            }
            //当产生位置移动的时候,产生移动进入离开事件回调.
            if (newPos != mousePos)
            {
                List<Collider> needToOutList = new List<Collider>();
                List<Collider> needToInList = new List<Collider>();

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
    }
}

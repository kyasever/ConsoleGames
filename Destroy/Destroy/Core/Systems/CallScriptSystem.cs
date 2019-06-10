using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Destroy
{
    using System.Collections.Generic;

    internal class StartSystem : DestroySystem
    {
        /// <summary>
        /// 等待唤醒Start函数的列表
        /// </summary>
        public List<Script> StartScriptCollection = new List<Script>();


        public override void Update()
        {
            List<Script> disabledScripts = new List<Script>();

            for (int i = 0; i < StartScriptCollection.Count; i++)
            {
                Script script = StartScriptCollection[i];

                if (script.Enable == false)
                {
                    //添加进未激活列表
                    disabledScripts.Add(script);
                    continue;
                }
                //执行Start方法
                script.Start();
                UpdateSystem updateSystem = RuntimeEngine.GetSystem<UpdateSystem>();
                //添加进Update执行列表
                updateSystem.UpdateScriptCollection.Add(script);
            }

            //清空列表
            StartScriptCollection.Clear();
            //添加未激活列表
            StartScriptCollection.AddRange(disabledScripts);
        }
    }

    internal class UpdateSystem : DestroySystem
    {
        public List<Script> UpdateScriptCollection;

        public override void Start()
        {
            UpdateScriptCollection = new List<Script>();
        }

        public override void Update()
        {
            foreach (Script script in UpdateScriptCollection)
            {
                if (!script.Enable)
                    continue;
                script.Update();
            }
        }
    }

    internal class DeleteSystem : DestroySystem
    {
        public List<GameObject> GameObjectsToDelete;

        public override void Start()
        {
            GameObjectsToDelete = new List<GameObject>();
        }

        public override void Update()
        {
            foreach (GameObject gameObject in GameObjectsToDelete)
            {
                foreach (Component component in gameObject.ComponentDict.Values)
                {
                    //把每个组件与系统解绑
                    component.OnRemove();
                }
                //从场景中删除这个物体
                gameObject.Scene.GameObjects.Remove(gameObject);
                gameObject.Scene.GameObjectsWithTag[gameObject.Tag].Remove(gameObject);
            }
        }
    }
}

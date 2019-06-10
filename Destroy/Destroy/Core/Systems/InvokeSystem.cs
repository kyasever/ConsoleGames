namespace Destroy
{
    using System.Collections.Generic;
    using System.Reflection;

    internal class InvokeSystem : DestroySystem
    {
        private class InvokeRequest
        {
            public object Instance;
            public string MethodName;
            public float DelayTime;

            public InvokeRequest(object instance, string methodName, float delayTime)
            {
                Instance = instance;
                MethodName = methodName;
                DelayTime = delayTime;
            }
        }

        private class CancleRequest
        {
            public object Instance;
            public string MethodName;

            public CancleRequest(object instance, string methodName)
            {
                Instance = instance;
                MethodName = methodName;
            }
        }

        private class DelayAction
        {
            public System.Action Action;
            public float DelayTime;

            public DelayAction(System.Action action, float delayTime)
            {
                Action = action;
                DelayTime = delayTime;
            }
        }

        private List<InvokeRequest> requests = new List<InvokeRequest>();

        private List<DelayAction> delayActions = new List<DelayAction>();

        public void AddInvokeRequest(object instance, string methodName, float delayTime)
        {
            requests.Add(new InvokeRequest(instance, methodName, delayTime));
        }

        public void CancleInvokeRequest(object instance, string methodName)
        {
            for (int i = 0; i < requests.Count; i++)
            {
                InvokeRequest request = requests[i];
                if (request.Instance == instance && request.MethodName == methodName)
                    requests.Remove(request);
            }
        }

        public bool IsInvoking(object instance, string methodName)
        {
            foreach (InvokeRequest request in requests)
            {
                if (request.Instance == instance && request.MethodName == methodName)
                    return true;
            }
            return false;
        }

        public void AddDelayAction(System.Action action, float delayTime)
        {
            delayActions.Add(new DelayAction(action, delayTime));
        }

        public void RemoveDelayAction(System.Action action)
        {
            for (int i = 0; i < delayActions.Count; i++)
            {
                DelayAction delayAction = delayActions[i];
                if (delayAction.Action == action)
                    delayActions.Remove(delayAction);
            }
        }

        public bool IsDelaying(System.Action action)
        {
            foreach (DelayAction delayAction in delayActions)
            {
                if (delayAction.Action == action)
                    return true;
            }
            return false;
        }

        public override void Start()
        {
            requests = new List<InvokeRequest>();
            delayActions = new List<DelayAction>();
        }

        public override void Update()
        {
            List<InvokeRequest> removeRequests = new List<InvokeRequest>();
            for (int i = 0; i < requests.Count; i++)
            {
                InvokeRequest request = requests[i];

                request.DelayTime -= Time.DeltaTime;
                if (request.DelayTime > 0)
                    continue;
                removeRequests.Add(request); //准备移除
                if (request.Instance == null)
                    continue;
                //调用
                MethodInfo methodInfo = request.Instance.GetType().GetMethod(request.MethodName);
                methodInfo?.Invoke(request.Instance, null);
            }
            //移除
            foreach (InvokeRequest request in removeRequests)
                requests.Remove(request);

            List<DelayAction> removeActions = new List<DelayAction>();
            for (int i = 0; i < delayActions.Count; i++)
            {
                DelayAction delayAction = delayActions[i];

                delayAction.DelayTime -= Time.DeltaTime;
                if (delayAction.DelayTime > 0)
                    continue;
                removeActions.Add(delayAction); //准备移除
                if (delayAction.Action == null)
                    continue;
                delayAction.Action();           //调用
            }
            //移除
            foreach (DelayAction delayAction in removeActions)
                delayActions.Remove(delayAction);
        }
    }
}
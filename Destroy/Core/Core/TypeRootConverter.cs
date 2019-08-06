namespace Destroy
{
    using System;
    using System.Collections.Generic;
    using System.Linq;
    using System.Reflection;

    /// <summary>
    /// 根类转换器 <see langword="static"/>
    /// </summary>
    public static class TypeRootConverter
    {
        /// <summary>
        /// 每一个类型与他的根类
        /// </summary>
        private static Dictionary<Type, Type> typeRoot;

        /// <summary>
        /// 每一个系统与他的根系统
        /// </summary>
        private static Dictionary<Type, Type> systemRoot;

        /// <summary>
        /// 将调用这个方法的程序集的类型加入索引.
        /// </summary>
        public static void InitByOtherAssembly()
        {
            Type[] types = Assembly.GetCallingAssembly().GetTypes();
            Init(types.ToList());
        }

        /// <summary>
        /// 初始化 
        /// </summary>
        internal static void Init()
        {
            typeRoot = new Dictionary<Type, Type>();
            systemRoot = new Dictionary<Type, Type>();

            //TODO:可能还需要依赖更多DLL

            //开发者的游戏中的所有类型
            Type[] entryTypes = Assembly.GetEntryAssembly().GetTypes();

            //Destroy程序集中所有类型
            Type[] currentTypes = Assembly.GetExecutingAssembly().GetTypes();

            List<Type> allTypes = entryTypes.ToList();
            allTypes.AddRange(currentTypes.ToList());

            Init(allTypes);
        }

        private static void Init(List<Type> typeList)
        {
            List<Type> roots = new List<Type>();
            List<Type> systemRoots = new List<Type>();

            foreach (Type type in typeList)
            {
                if (type.BaseType == typeof(Script))
                    roots.Add(type);
                else if (type.BaseType == typeof(Component))
                    roots.Add(type);
                //系统也弄一套一样的东西
                else if (type.BaseType == typeof(DestroySystem))
                    systemRoots.Add(type);
            }

            foreach (Type type in typeList)
            {
                foreach (Type root in roots)
                {
                    if (type.IsSubclassOf(root) || type == root) //是 || 直接/间接继承
                    {
                        typeRoot.Add(type, root);
                        break;
                    }
                }
                foreach (Type root in systemRoots)
                {
                    if (type.IsSubclassOf(root) || type == root) //是 || 直接/间接继承
                    {
                        systemRoot.Add(type, root);
                        break;
                    }
                }
            }
        }

        internal static Type GetComponentRoot(Type type)
        {
            if (typeRoot.ContainsKey(type))
                return typeRoot[type];
            return null;
        }

        internal static Type GetSystemRoot(Type type)
        {
            if (systemRoot.ContainsKey(type))
                return systemRoot[type];
            return null;
        }
    }
}
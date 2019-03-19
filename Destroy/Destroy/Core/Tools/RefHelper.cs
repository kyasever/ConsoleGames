namespace Destroy
{
    using System;
    using System.Collections.Generic;
    using System.Reflection;

    /// <summary>
    /// 反射帮助
    /// </summary>
    public static class RefHelper
    {
        /// <summary>
        /// 获取private属性
        /// </summary>
        public static T GetPrivateProperty<T>(this object instance, string propertyname)
        {
            BindingFlags flag = BindingFlags.Instance | BindingFlags.NonPublic | BindingFlags.Static;
            Type type = instance.GetType();
            PropertyInfo field = type.GetProperty(propertyname, flag);
            return (T)field.GetValue(instance, null);
        }
    }
}
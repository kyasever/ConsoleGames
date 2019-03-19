
// 一些自定义属性,挂载这个属性的会在编辑器中显示
namespace Destroy
{
    using System;

    /// <summary>
    /// 从原则上来说,所有字段都是默认隐藏的,可以加ShowInInspector来显示字段
    /// </summary>
    [AttributeUsage(AttributeTargets.Class | AttributeTargets.Field)]
    public class ShowInInspector : Attribute
    {
    }

    /// <summary>
    /// 默认的组件和属性都是显示的,如果要隐藏或者组件属性,要加HideInInspector
    /// </summary>
    [AttributeUsage(AttributeTargets.Class | AttributeTargets.Field | AttributeTargets.Property)]
    public class HideInInspector : Attribute
    {
    }

    /// <summary>
    /// 日后将要加入的功能,允许挂载这个属性的变量在编辑器中修改它的值
    /// </summary>
    public class CanChangeInInspector : Attribute
    {
    }

}
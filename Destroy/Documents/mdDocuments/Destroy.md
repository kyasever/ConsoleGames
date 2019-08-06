## `BoxDrawingCharacter`

用于制表符加法运算的一个辅助类
```csharp
public class Destroy.BoxDrawingCharacter

```

Methods

| Type | Name | Summary | 
| --- | --- | --- | 
| `Char` | ToChar() | 从一个对象转化为一个字符,进行输出 | 


Static Fields

| Type | Name | Summary | 
| --- | --- | --- | 
| `BoxDrawingCharacter` | BoxDownLeft |  | 
| `BoxDrawingCharacter` | BoxDownRight |  | 
| `BoxDrawingCharacter` | BoxHorizontal |  | 
| `BoxDrawingCharacter` | BoxHorizontalDown |  | 
| `BoxDrawingCharacter` | BoxHorizontalUp |  | 
| `BoxDrawingCharacter` | BoxUpLeft |  | 
| `BoxDrawingCharacter` | BoxUpRight |  | 
| `BoxDrawingCharacter` | BoxVertical |  | 
| `BoxDrawingCharacter` | BoxVerticalHorizontal |  | 
| `BoxDrawingCharacter` | BoxVerticalLeft |  | 
| `BoxDrawingCharacter` | BoxVerticalRight |  | 


Static Methods

| Type | Name | Summary | 
| --- | --- | --- | 
| `BoxDrawingCharacter` | Prase(`Char` c) | 从一个字符c解析一个对象,从而可以进行运算 | 


## `BoxDrawingSupply`

制表符绘制的辅助类
```csharp
public static class Destroy.BoxDrawingSupply

```

Static Fields

| Type | Name | Summary | 
| --- | --- | --- | 
| `Char` | boxDownLeft |  | 
| `Char` | boxDownRight |  | 
| `Char` | boxHorizontal |  | 
| `Char` | boxHorizontalDown |  | 
| `Char` | boxHorizontalUp |  | 
| `Char` | boxUpLeft |  | 
| `Char` | boxUpRight |  | 
| `Char` | boxVertical |  | 
| `Char` | boxVerticalHorizontal |  | 
| `Char` | boxVerticalLeft |  | 
| `Char` | boxVerticalRight |  | 


Static Methods

| Type | Name | Summary | 
| --- | --- | --- | 
| `String` | GetFirstLine(`Int32` width) | 获取一个方框第一行的字符串 | 
| `String` | GetLastLine(`Int32` width) | 获取一个方框最后一行的字符串 | 
| `String` | GetMiddleLine(`Int32` width, `String` str) | 获取一个方框中间行的字符串 | 
| `String` | GetMiddleLine(`Int32` width) | 获取一个方框中间行的字符串 | 


## `Button`

按钮组件 感觉这个组件好没有必要啊.... 回头删了吧
```csharp
public class Destroy.Button
    : Component

```

Fields

| Type | Name | Summary | 
| --- | --- | --- | 
| `Action` | OnClick | 点击回调 | 


Properties

| Type | Name | Summary | 
| --- | --- | --- | 
| `String` | Text | 设置文字 | 


Methods

| Type | Name | Summary | 
| --- | --- | --- | 
| `void` | Initialize() |  | 


## `Camera`

相机组件  暂时还没有别的功能,相当于正交视角摄像机
```csharp
public class Destroy.Camera
    : Component

```

Static Properties

| Type | Name | Summary | 
| --- | --- | --- | 
| `Camera` | Main | 单例的,只有一个相机 | 


## `CanChangeInInspector`

日后将要加入的功能,允许挂载这个属性的变量在编辑器中修改它的值
```csharp
public class Destroy.CanChangeInInspector
    : Attribute, _Attribute

```

## `CharacterController`

角色控制器 有比较完整的运动控制  可以通过Speed调整速度  可以通过CanMoveInCollider调整是否可以进入碰撞体
```csharp
public class Destroy.CharacterController
    : Script

```

Fields

| Type | Name | Summary | 
| --- | --- | --- | 
| `Boolean` | CanMoveInCollider | 是否可以穿过碰撞体. | 
| `Vector2Float` | FPosition | 当前处于的浮点数位置 | 
| `Int32` | Speed | 要控制的角色的速度 | 


Properties

| Type | Name | Summary | 
| --- | --- | --- | 
| `Vector2` | Direction | 当前人物的朝向 | 


Methods

| Type | Name | Summary | 
| --- | --- | --- | 
| `void` | OnCollision(`Collision` collision) | 碰撞检测 测试用 | 
| `void` | Update() | Update | 


## `CharUtils`

字符串工具类 <see langword="static" />
```csharp
public static class Destroy.CharUtils

```

Static Methods

| Type | Name | Summary | 
| --- | --- | --- | 
| `Char` | BoxDrawingAdd(`Char` c1, `Char` c2) | 将两个字符作为制表符混合相加,输出结果 | 
| `List<String>` | DivideString(`String` str) | 将string分割成一个个格子, 单宽字符占据半个格子, 双宽字符会占据一个格子, 如果单宽后面直接跟双宽, 会在单宽字符后面先补上一个空格 | 
| `Int32` | GetCharWidth(`Char` c) | 返回一个字符的宽度 | 
| `Int32` | GetStringWidth(`String` str) | 返回一个字符串的宽度 | 
| `Boolean` | IsTabChar(`Char` c) | 判断一个char是不是制表符 | 
| `String` | SubStr(`String` str, `Int32` width) | 按照标准长度截断字符串,不足用空格补上,超出截断 | 


## `CharWidthEnum`

字符宽度
```csharp
public enum Destroy.CharWidthEnum
    : Enum, IComparable, IFormattable, IConvertible

```

Enum

| Value | Name | Summary | 
| --- | --- | --- | 
| `1` | Single | 单宽字符(比如英文) | 
| `2` | Double | //双宽字符(比如中文) | 


## `Collider`

碰撞体组件,一般来说默认按着Mesh来
```csharp
public class Destroy.Collider
    : Component

```

Properties

| Type | Name | Summary | 
| --- | --- | --- | 
| `List<Vector2>` | ColliderList | 碰撞体包含的点的列表,通常情况下来说保持和Mesh的数据相同 | 


Methods

| Type | Name | Summary | 
| --- | --- | --- | 
| `void` | Initialize() |  | 
| `void` | OnAdd() |  | 
| `void` | OnRemove() |  | 


## `Collision`

发生碰撞的碰撞信息.
```csharp
public class Destroy.Collision

```

Properties

| Type | Name | Summary | 
| --- | --- | --- | 
| `List<Collider>` | ColliderList | 获取完整的产生碰撞的碰撞体列表 | 
| `Int32` | CollisionCount | 发生碰撞的碰撞体的数量 | 
| `Vector2` | HitPos | 发生碰撞的点的坐标 | 
| `Collider` | OtherCollier | 获取第一个产生碰撞的物体 | 


## `CollisionSystem`

轻量级碰撞检测系统,只会在移动的时候进行通知操作,不会管多余的事情.需要用Movement组件向其发送请求
```csharp
public class Destroy.CollisionSystem
    : DestroySystem

```

Properties

| Type | Name | Summary | 
| --- | --- | --- | 
| `Dictionary<Vector2, List<Collider>>` | Colliders | 每一个点都可以放很多碰撞体组件 | 


Methods

| Type | Name | Summary | 
| --- | --- | --- | 
| `void` | AddToSystem(`Collider` collider) | 将一个碰撞体加入系统 | 
| `void` | AddToSystem(`Collider` collider, `Vector2` position) | 将一个碰撞体加入系统 | 
| `Boolean` | CanMoveInPos(`Collider` collider, `Vector2` from, `Vector2` to) | 判断一个碰撞体是否可以在系统中移动,判断的时候会产生碰撞回调但是不会移动 | 
| `void` | MoveInSystem(`Collider` collider, `Vector2` from, `Vector2` to) | 让一个碰撞体在系统中移动 | 
| `void` | RemoveFromSystem(`Collider` collider) | 将一个碰撞体移除系统 | 
| `void` | RemoveFromSystem(`Collider` collider, `Vector2` position) | 将一个碰撞体移除系统 | 


## `Color`

颜色类,可以实现对ConsoleColor和Drawing.Color的兼容支持
```csharp
public struct Destroy.Color

```

Fields

| Type | Name | Summary | 
| --- | --- | --- | 
| `Nullable<ConsoleColor>` | consoleColor |  | 


Properties

| Type | Name | Summary | 
| --- | --- | --- | 
| `UInt32` | B | blue | 
| `UInt32` | G | green | 
| `UInt32` | R | red | 


Methods

| Type | Name | Summary | 
| --- | --- | --- | 
| `Boolean` | Equals(`Object` obj) | == | 
| `Int32` | GetHashCode() | Hash | 
| `ConsoleColor` | ToConsoleColor() | 将colour解析为ConsoleColor | 


Static Fields

| Type | Name | Summary | 
| --- | --- | --- | 
| `Color` | Black |  | 
| `Color` | Blue |  | 
| `Color` | Cyan |  | 
| `Color` | DarkBlue |  | 
| `Color` | DarkCyan |  | 
| `Color` | DarkGray |  | 
| `Color` | DarkGreen |  | 
| `Color` | DarkMagenta |  | 
| `Color` | DarkRed |  | 
| `Color` | DarkYellow |  | 
| `Color` | Gray |  | 
| `Color` | Green |  | 
| `Color` | Magenta |  | 
| `Color` | Red |  | 
| `Color` | White |  | 
| `Color` | Yellow |  | 


Static Methods

| Type | Name | Summary | 
| --- | --- | --- | 
| `ConsoleColor` | ColorToConsoleColor(`Byte` r, `Byte` g, `Byte` b) | 根据System.Drawing.Color的RGB值返回最接近的System.ConsoleColor | 
| `void` | ParseConsoleColor(`ConsoleColor` consoleColor, `UInt32&` r, `UInt32&` g, `UInt32&` b) | System.ConsoleColor转换为System.Drawing.Color | 


## `ColorStringBuilder`

使用这个工具类来生成一个组合颜色的字符串.  改变颜色 - 添加字符 - 改变颜色 - 添加字符 - 输出供Renderer使用
```csharp
public class Destroy.ColorStringBuilder

```

Fields

| Type | Name | Summary | 
| --- | --- | --- | 
| `Color` | BackColor | 背景色 | 
| `Color` | ForeColor | 前景色 | 


Methods

| Type | Name | Summary | 
| --- | --- | --- | 
| `void` | AppendChar(`Char` c) | 添加一个char | 
| `void` | AppendString(`String` str) | 添加一个字符串 | 
| `List<RenderPoint>` | ToRenderer() | 输出为List,供Renderer使用 | 


## `Component`

所有组件的基类,不要new Component对象,使用AddComponent
```csharp
public abstract class Destroy.Component

```

Properties

| Type | Name | Summary | 
| --- | --- | --- | 
| `Boolean` | Enable | 暂时的开关,不会产生移除或添加.如果系统不处理的话相当于没用 | 
| `GameObject` | GameObject | 游戏物体 | 
| `Vector2` | LocalPosition | 获取本地坐标 | 
| `Vector2` | Position | 获取世界坐标,重载这个东西,然后加塞 | 


Methods

| Type | Name | Summary | 
| --- | --- | --- | 
| `T` | AddComponent() | 添加指定组件 | 
| `T` | GetComponent() | 获取指定组件 | 
| `List<T>` | GetComponents() | 获取指定的类型及其子类的集合 | 
| `void` | Initialize() | 在该组件被添加到游戏物体时做初始化调用 (在该方法中可以使用GetComponent) | 
| `void` | OnAdd() | 组件被添加的时候要将自己的引用加入系统. 必须重载 | 
| `void` | OnRemove() | 组件被移除的时候要将自己的引用从系统中去掉 | 


## `Config`

引擎配置
```csharp
public class Destroy.Config

```

Static Fields

| Type | Name | Summary | 
| --- | --- | --- | 
| `Color` | DefaultBackColor | 系统默认背景色 | 
| `Color` | DefaultForeColor | 系统默认颜色 | 
| `Vector2` | RendererSize | 比如被初始化,表示一个RenderPoint在屏幕上占有的像素点  单宽推荐值8*16 双宽推荐值16*16 | 


Static Properties

| Type | Name | Summary | 
| --- | --- | --- | 
| `CharWidthEnum` | CharWidth | 引擎单个字符宽度 | 
| `NetConfig` | Net | 网络设置 | 
| `Int32` | ScreenHeight | 屏幕高度 | 
| `Int32` | ScreenWidth | 屏幕宽度 | 
| `Int32` | TickPerSecond | 引擎每秒Update的频率 | 


## `CoordinateType`

坐标系类型
```csharp
public enum Destroy.CoordinateType
    : Enum, IComparable, IFormattable, IConvertible

```

Enum

| Value | Name | Summary | 
| --- | --- | --- | 
| `0` | Window | right x, down Y | 
| `1` | Normal | right X, up Y | 


## `Debug`

调试类(用于调试程序) <see langword="static" />
```csharp
public static class Destroy.Debug

```

Static Methods

| Type | Name | Summary | 
| --- | --- | --- | 
| `void` | Error(`Object` msg) | 输出错误调试信息 | 
| `void` | Log(`Object` msg) | 输出普通调试信息 | 
| `void` | Warning(`Object` msg) | 输出警告调试信息 | 


## `DestroySystem`

所有系统的基类，继承这个类就可以AddSystem到RuntimeEngine上。
```csharp
public abstract class Destroy.DestroySystem

```

Fields

| Type | Name | Summary | 
| --- | --- | --- | 
| `Boolean` | needUpdate | 这个系统是否需要执行Update | 


Methods

| Type | Name | Summary | 
| --- | --- | --- | 
| `void` | Initialize() | 初步的初始化,在这个system被addsystem之后立即执行 | 
| `void` | Start() | 在外部初始化完毕之后按照顺序统一执行 | 
| `void` | Update() | 每帧执行.可以通过在初始化的时候改变needUpdate关闭. | 


## `EventHandlerSystem`

处理鼠标事件 通常只有UI和这个有关 游戏物体请用物理系统检测
```csharp
public class Destroy.EventHandlerSystem
    : DestroySystem

```

Properties

| Type | Name | Summary | 
| --- | --- | --- | 
| `Dictionary<Vector2, List<RayCastTarget>>` | UITargets | UI射线检测组件 | 


Methods

| Type | Name | Summary | 
| --- | --- | --- | 
| `void` | AddToSystem(`RayCastTarget` UITarget, `Vector2` position) | 加入系统 | 
| `void` | RemoveFromSystem(`RayCastTarget` UITarget, `Vector2` position) | 移除系统 | 
| `void` | Update() | Update | 


## `GameObject`

场景中所有实体的类型
```csharp
public class Destroy.GameObject

```

Fields

| Type | Name | Summary | 
| --- | --- | --- | 
| `Action<Collision>` | OnCollisionEvent | 碰撞回调事件. | 


Properties

| Type | Name | Summary | 
| --- | --- | --- | 
| `Boolean` | Active | 是否激活 | 
| `Int32` | ChildCount | 获取子物体个数 | 
| `List<GameObject>` | Childs | 该游戏物体的子物体集合 | 
| `Int32` | ComponentCount | 获取组件个数 | 
| `Dictionary<Type, Component>` | ComponentDict | 存成字典形式的组件 | 
| `List<Component>` | Components | 存成列表形式的组件 | 
| `Vector2` | LocalPosition | 本地坐标 | 
| `String` | Name | 名字 | 
| `GameObject` | Parent | 父物体 | 
| `Vector2` | Position | 世界坐标 | 
| `Scene` | Scene | 这个游戏物体属于哪个scene | 
| `String` | Tag | 标签 | 


Events

| Type | Name | Summary | 
| --- | --- | --- | 
| `Func<Vector2, Vector2, Boolean>` | ChangePositionEvnet | 当发生坐标改变时产生的回调事件,将自己的Position告诉组件,组件自行管理  参数一 发生改变之前所处的位置 参数二 发生改变之后所处的位置 | 


Methods

| Type | Name | Summary | 
| --- | --- | --- | 
| `T` | AddComponent() | 添加指定组件 | 
| `void` | AddToScene(`Scene` scene) |  | 
| `GameObject` | Find(`String` name) | 在当前场景中根据名字寻找游戏物体, 若有多个同名物体也只返回一个。 | 
| `List<GameObject>` | FindWithTag(`String` tag) | 在当前场景中根据标签寻找游戏物体, 若有多个则返回多个。 | 
| `T` | GetComponent() | 获取指定组件 | 
| `List<T>` | GetComponents() | 获取指定的类型及其子类的集合 | 
| `void` | SetActive(`Boolean` value) | 设置自己所有组件的active以及所有子物体组件的active | 


Static Methods

| Type | Name | Summary | 
| --- | --- | --- | 
| `T` | CreateWith(`String` name = GameObject, `String` tag = None, `GameObject` parent = null) | 3.20测试添加  提供一种新的更便捷的创建物体的思路.直接返回具体脚本而不是游戏物体 | 
| `void` | Destroy(`GameObject` gameObject) | 销毁一个游戏物体 | 
| `void` | Destroy(`GameObject` gameObject, `Single` delayTime) | 销毁一个游戏物体 | 
| `void` | DontDestroyOnLoad(`GameObject` gameObject) | 将一个游戏物体的引用加入DontDestroyOnLoad | 
| `GameObject` | Find(`String` sceneName, `String` name) | 在当前场景中根据名字寻找游戏物体, 若有多个同名物体也只返回一个。 | 
| `List<GameObject>` | FindWithTag(`String` sceneName, `String` tag) | 在当前场景中根据标签寻找游戏物体, 若有多个则返回多个。 | 


## `HideInInspector`

默认的组件和属性都是显示的,如果要隐藏或者组件属性,要加HideInInspector
```csharp
public class Destroy.HideInInspector
    : Attribute, _Attribute

```

## `ImageConvertor`

将字符串和Image对应起来,渲染的时候如果检测到有对应Image,则换成使用Image而非字符渲染  使用方式,渲染图片的前提是图片一定要被对应为一个ACSII字符. 想要显示为图片就要指定为对应字符  相应的,如果一个字符被指定为一个Image进行渲染,那么就不会再显示这个字符本身了
```csharp
public static class Destroy.ImageConvertor

```

Static Fields

| Type | Name | Summary | 
| --- | --- | --- | 
| `Dictionary<String, Image>` | ImageDic | 对应字典 | 


Static Methods

| Type | Name | Summary | 
| --- | --- | --- | 
| `void` | Init() | 将对应加入字典 | 


## `Input`

获取标准输入 对外接口 <see langword="static" />
```csharp
public static class Destroy.Input

```

Static Properties

| Type | Name | Summary | 
| --- | --- | --- | 
| `Vector2` | MousePosition | 获取鼠标当前所处的位置(场景中世界坐标的位置) | 
| `Vector2` | MousePositionInPixel | 获取鼠标在游戏中的像素坐标 | 


Static Methods

| Type | Name | Summary | 
| --- | --- | --- | 
| `Int32` | GetDirectInput(`ConsoleKey` negative, `ConsoleKey` positive) | 获取指定方向上的按键输入 | 
| `Boolean` | GetKey(`ConsoleKey` consoleKey) | 持续获取按键输入 | 
| `Boolean` | GetKeyDown(`ConsoleKey` consoleKey) | 获取按下的按键 | 
| `Boolean` | GetKeyUp(`ConsoleKey` consoleKey) | 获取弹起的按键 | 
| `Boolean` | GetMouseButton(`MouseButton` mouseButton) | 获取鼠标按键 | 
| `Boolean` | GetMouseButtonDown(`MouseButton` mouseButton) | 获取按下鼠标按键 | 
| `Boolean` | GetMouseButtonUp(`MouseButton` mouseButton) | 获取抬起鼠标按键 | 


## `InputSystem`

处理来自底层的输入
```csharp
public class Destroy.InputSystem
    : DestroySystem

```

Methods

| Type | Name | Summary | 
| --- | --- | --- | 
| `Boolean` | GetKeyDown(`ConsoleKey` consoleKey) |  | 
| `Boolean` | GetKeyUp(`ConsoleKey` consoleKey) |  | 
| `Boolean` | GetMouseButtonDown(`MouseButton` mouseButton) |  | 
| `Boolean` | GetMouseButtonUp(`MouseButton` mouseButton) |  | 
| `void` | Start() | 将getkey事件处理为getkeyDown事件并绑定到Input类 | 
| `void` | Update() | Update | 


## `KeyDownController`

按键控制器,按一下动一格
```csharp
public class Destroy.KeyDownController
    : Script

```

Methods

| Type | Name | Summary | 
| --- | --- | --- | 
| `void` | Update() | Update | 


## `ListBox`

列表组件  只有在视窗内的label才会active true,别的都是false.当移动的时候就是全体Label整体上/下平移.
```csharp
public class Destroy.ListBox
    : Script

```

Fields

| Type | Name | Summary | 
| --- | --- | --- | 
| `GameObject` | boxDrawing | 边框对象 | 
| `Int32` | EndIndex | 当前视图最下面的Item的序号 | 
| `Int32` | Height | 长款 | 
| `ListBoxItemList` | Items | 列表 | 
| `Int32` | TopIndex | 当前视图最上面的Item的序号 | 
| `Int32` | Width | 长款 | 


Properties

| Type | Name | Summary | 
| --- | --- | --- | 
| `ListBoxItem` | CurrentItem | 当前选择的标签对象 | 
| `Int32` | SelectedIndex | 当前被选中Item的序号 | 


Methods

| Type | Name | Summary | 
| --- | --- | --- | 
| `ListBoxItem` | CreateLabelItem(`String` text) | 创建一个新的列表对象.然后再进行进一步的处理 | 
| `void` | Init(`Int32` height, `Int32` width) | 初始化 | 
| `void` | OnClickAction(`Int32` index) | 当Item被点击的时候产生的事件 | 
| `Boolean` | RollDown() | 整体下滚 | 
| `Boolean` | RollUp() | 整体上滚 | 
| `void` | Update() | Update | 


## `ListBoxItem`

原Label组件被删除,现在用Renderer代替.  请通过ListBox组件中的CreateLabel方法进行创建
```csharp
public class Destroy.ListBoxItem
    : Component

```

Properties

| Type | Name | Summary | 
| --- | --- | --- | 
| `Color` | BackColor | 背景色 | 
| `Int32` | Index | 序号 | 
| `Boolean` | IsSelected | 表明这个子物体是否已经被选择 | 


Methods

| Type | Name | Summary | 
| --- | --- | --- | 
| `void` | Initialize() |  | 
| `void` | Rendering(`String` text) | 设置文字 | 
| `void` | SetSelected(`Boolean` selected) | 设置选中状态 | 


## `LoadSceneMode`

场景加载模式
```csharp
public enum Destroy.LoadSceneMode
    : Enum, IComparable, IFormattable, IConvertible

```

Enum

| Value | Name | Summary | 
| --- | --- | --- | 
| `0` | Single | 创建之后只保留这个Scene 并把DontDestroyOnLoad放入这个Scene | 
| `1` | Additive | 这个Scene作为额外的Scene创建. 依然以之前场景作为主场景 | 


## `Mathematics`

简易数学库 <see langword="static" />
```csharp
public static class Destroy.Mathematics

```

Static Methods

| Type | Name | Summary | 
| --- | --- | --- | 
| `Boolean` | Approximately(`Single` x, `Single` y, `Single` epsilon = 0.001) | 算两个浮点数是否相近 | 
| `Int32` | ClampInt(`Int32` i, `Int32` min, `Int32` max) | 限制Int取值范围 | 
| `T` | GetInArray(`T[,]` array, `Int32` x, `Int32` y, `CoordinateType` coordinate) | 在数组中取值 | 
| `T[,]` | RotateArray(`T[,]` array, `RotationAngle` angle) | 旋转数组 | 
| `void` | SetInArray(`T[,]` array, `T` item, `Int32` x, `Int32` y, `CoordinateType` coordinate) | 设置数组的值 | 


## `Matrix`

矩阵
```csharp
public class Destroy.Matrix

```

Properties

| Type | Name | Summary | 
| --- | --- | --- | 
| `Int32` | Column |  | 
| `Int32` | Item |  | 
| `Int32` | Row |  | 


Static Methods

| Type | Name | Summary | 
| --- | --- | --- | 
| `Matrix` | RotateR90() | 向量乘以该矩阵顺时针旋转90度 | 


## `MazeGeneration`

迷宫生成
```csharp
public class Destroy.MazeGeneration

```

Static Methods

| Type | Name | Summary | 
| --- | --- | --- | 
| `void` | <GetMazeMesh>g__LinkTo|1_0(`Vector2` currert, `<>c__DisplayClass1_0&`  = null) |  | 
| `GameObject` | GetMaze(`Int32` height, `Int32` width, `RenderPoint` wallRender) | 生成迷宫对象.带碰撞体 | 
| `List<Vector2>` | GetMazeMesh(`Int32` height, `Int32` width) | 生成迷宫Mesh信息 包含的信息为墙的位置 | 


## `Mesh`

Mesh组件 默认生成单点Mesh
```csharp
public class Destroy.Mesh
    : Component

```

Properties

| Type | Name | Summary | 
| --- | --- | --- | 
| `List<Vector2>` | PosList | Mesh对象的列表 | 


Methods

| Type | Name | Summary | 
| --- | --- | --- | 
| `void` | Init(`List<Vector2>` list) | 进行多点初始化 | 
| `Boolean` | Rotate() | TODO 这个方法需要重写或者删除  顺时针旋转90度,如果有碰撞的话会认为旋转失败 | 


## `MouseButton`

鼠标按键
```csharp
public enum Destroy.MouseButton
    : Enum, IComparable, IFormattable, IConvertible

```

Enum

| Value | Name | Summary | 
| --- | --- | --- | 
| `1` | Left | 左键 | 
| `2` | Right | 中键 | 
| `4` | Middle | 右键 | 


## `NaNException`

除0异常
```csharp
public class Destroy.NaNException
    : Exception, ISerializable, _Exception

```

Properties

| Type | Name | Summary | 
| --- | --- | --- | 
| `String` | Message |  | 


## `Navigation`

NavMesh工具类,提供算法支持 <see langword="static" />  应该还会有一个NavMeshAgent,但不会有System. 这个静态类提供算法Agent负责调用  在战棋游戏里有战棋的Agent,原理差不多
```csharp
public static class Destroy.Navigation

```

Static Methods

| Type | Name | Summary | 
| --- | --- | --- | 
| `Boolean` | <ExpandAera>g__AddNextRoute|6_0(`Vector2` ov, `<>c__DisplayClass6_0&`  = null) |  | 
| `Boolean` | <Search>g__AddNextRoute|5_0(`Vector2` ov, `<>c__DisplayClass5_0&`  = null) |  | 
| `Boolean` | CanMoveInAll(`Vector2` v) | 搜寻判定,所有的点均可以穿过 | 
| `Boolean` | CanMoveInPhysics(`Vector2` v) | 通常的一种搜寻判定,所有物理系统的物体均不可穿过 | 
| `List<Vector2>` | ExpandAera(`Vector2` startPos, `Int32` expandWidth, `Func<Vector2, Boolean>` canMoveFunc) | 返回distanse范围内所有可以通过的点 | 
| `SearchResult` | Search(`Vector2` start, `Vector2` end) | 默认搜索方法 | 
| `SearchResult` | Search(`Vector2` start, `Vector2` stop, `Func<Vector2, Boolean>` canMoveFunc) | 默认搜索方法 | 


## `NetConfig`

网络设置
```csharp
public class Destroy.NetConfig

```

Fields

| Type | Name | Summary | 
| --- | --- | --- | 
| `Int32` | ClientSyncRate | //客户端同步频率 | 
| `Int32` | GamePort | 游戏端口 | 
| `Int32` | ServerSyncRate | //服务器同步频率 | 


## `ObjectPool`

对象池
```csharp
public class Destroy.ObjectPool

```

Methods

| Type | Name | Summary | 
| --- | --- | --- | 
| `GameObject` | GetInstance() |  | 
| `void` | PreAllocate(`Int32` count) |  | 
| `void` | ReturnInstance(`GameObject` instance) |  | 


## `Physics`

物理检测 待补完
```csharp
public static class Destroy.Physics

```

Static Methods

| Type | Name | Summary | 
| --- | --- | --- | 
| `Boolean` | RayCast(`Collision` collision) | 射线检测,输入要的结构,和点集或者射线,返回是否发生了碰撞 | 


## `RayCastTarget`

通过挂载这个组件来接收UI点击事件.
```csharp
public class Destroy.RayCastTarget
    : Component

```

Fields

| Type | Name | Summary | 
| --- | --- | --- | 
| `List<Vector2>` | colliderList | 保存一个来自Mesh的引用.有点偷懒 | 
| `Action` | OnClickEvent | 当点击组件时产生的事件 | 
| `Action` | OnMoveInEvent | 当进入组件时产生的事件 | 
| `Action` | OnMoveOutEvent | 当离开组件时产生的事件 | 


Methods

| Type | Name | Summary | 
| --- | --- | --- | 
| `void` | Initialize() |  | 
| `void` | OnAdd() |  | 
| `void` | OnRemove() |  | 


## `Rectangle`

矩形类,用于返回标准矩形的点集.  TODO:需要重写
```csharp
public class Destroy.Rectangle

```

Fields

| Type | Name | Summary | 
| --- | --- | --- | 
| `Int32` | Height | 宽度高度 | 
| `List<Vector2>` | PosList | 点列表 | 
| `String` | Str | 字符 | 
| `Int32` | Width | 宽度高度 | 


Methods

| Type | Name | Summary | 
| --- | --- | --- | 
| `void` | Sort() | 去重复排序点集合 | 


## `RefHelper`

反射帮助
```csharp
public static class Destroy.RefHelper

```

Static Methods

| Type | Name | Summary | 
| --- | --- | --- | 
| `T` | GetPrivateProperty(this `Object` instance, `String` propertyname) | 获取private属性 | 


## `Renderer`

渲染组件,最基础的渲染组件只负责维护这点东西,理论上这个也是能用的.甚至不依赖Mesh组件,直接编辑渲染结果就好了
```csharp
public class Destroy.Renderer
    : Component

```

Properties

| Type | Name | Summary | 
| --- | --- | --- | 
| `Int32` | Depth | 渲染深度 越低的渲染优先级越高 | 
| `RendererMode` | Mode | 渲染模式 | 
| `Dictionary<Vector2, RenderPoint>` | RendererPoints | 渲染结果,允许直接操作它来进行渲染.没毛病 | 


Methods

| Type | Name | Summary | 
| --- | --- | --- | 
| `String` | GetString() | 获取渲染的字符串信息 | 
| `void` | Init(`RendererMode` mode = GameObject, `Int32` depth = 2147483647) | 初始化的时候指定mode和深度. | 
| `void` | Initialize() |  | 
| `void` | OnAdd() |  | 
| `void` | OnRemove() |  | 
| `void` | Rendering(`RenderPoint` renderPoint) | 修改这个API,变为使用这个点填充所有Mesh | 
| `void` | Rendering(`String` str) | 修改这个API,变为使用这个点填充所有Mesh | 
| `void` | Rendering(`String` str, `Color` foreColor, `Color` backColor) | 修改这个API,变为使用这个点填充所有Mesh | 
| `void` | Rendering(`List<RenderPoint>` list) | 修改这个API,变为使用这个点填充所有Mesh | 
| `void` | Rendering(`Dictionary<Vector2, RenderPoint>` dic) | 修改这个API,变为使用这个点填充所有Mesh | 
| `void` | SetBackColor(`Color` backColor) | 更改背景色 | 
| `void` | SetDepth(`Int32` depth) | 更改渲染深度 | 
| `void` | SetForeColor(`Color` foreColor) | 更改前景色 | 
| `void` | SetString(`String` str) | 更改渲染的字符 | 


## `RendererMode`

渲染模式
```csharp
public enum Destroy.RendererMode
    : Enum, IComparable, IFormattable, IConvertible

```

Enum

| Value | Name | Summary | 
| --- | --- | --- | 
| `0` | UI | 原点:屏幕左下角世界坐标(0,0)的坐标,不随摄像机运动 | 
| `1` | GameObject | 原点:屏幕左下角世界坐标(0,0)的坐标,随摄像机运动 | 


## `RendererSystem`

渲染系统
```csharp
public class Destroy.RendererSystem
    : DestroySystem

```

Properties

| Type | Name | Summary | 
| --- | --- | --- | 
| `List<Renderer>` | RendererCollection |  | 


Methods

| Type | Name | Summary | 
| --- | --- | --- | 
| `void` | Reset() | 重置系统,重新刷新屏幕.一般来说最小化窗口之后可能需要这种操作,和一些迷之显示bug需要 | 
| `void` | Start() |  | 
| `void` | Update() |  | 


## `RenderPoint`

标准输出点结构.所有的Renderer组件都会被处理为RenderPos的集合
```csharp
public struct Destroy.RenderPoint

```

Fields

| Type | Name | Summary | 
| --- | --- | --- | 
| `Color` | BackColor | 背景色 | 
| `Int32` | Depth | 渲染优先级(为0时脚本显示优先级最高(最后被渲染), 负数表示这个是UI(在游戏物体之前渲染)) | 
| `Color` | ForeColor | 前景色 | 
| `String` | Str | 这个点的信息,不长于Width | 


Methods

| Type | Name | Summary | 
| --- | --- | --- | 
| `Boolean` | Equals(`Object` obj) | 三项属性相同就视为相等,渲染不需要知道Depth | 
| `Int32` | GetHashCode() |  | 


Static Fields

| Type | Name | Summary | 
| --- | --- | --- | 
| `RenderPoint` | Block | 默认的空白渲染点,显示在最底层 | 
| `RenderPoint` | UIBlock | 默认的UI渲染点 显示在所有UI的最底层 | 


## `Resource`

资源读取模块 使用Resouce.Load&lt;Type&gt;加载资源文件 <see langword="static" />
```csharp
public static class Destroy.Resource

```

Static Fields

| Type | Name | Summary | 
| --- | --- | --- | 
| `Dictionary<String, String>` | Name_Path | 资源名字和完整路径 | 
| `String` | ResoucePath | 默认资源文件所处位置 | 


Static Methods

| Type | Name | Summary | 
| --- | --- | --- | 
| `void` | <FindALLInPath>g__FindInPath|4_0(`DirectoryInfo` dir) |  | 
| `void` | Init() | 初始化,将Resource下的文件检测一边,并保存其路径 | 
| `T` | Load(`String` name) | 加载资源文件,如果类型不支持或者没有对应文件,则返回null  不能区分重名文件,参数名字不包括扩展名 | 


## `RotationAngle`

旋转角度
```csharp
public enum Destroy.RotationAngle
    : Enum, IComparable, IFormattable, IConvertible

```

Enum

| Value | Name | Summary | 
| --- | --- | --- | 
| `0` | RotRight90 | 顺时针旋转90度 | 
| `1` | Rot180 | 旋转180度 | 
| `2` | RotLeft90 | 逆时针旋转90度 | 


## `RuntimeEngine`

运行时引擎, 接管整个游戏的生命周期 <see langword="static" />
```csharp
public static class Destroy.RuntimeEngine

```

Static Properties

| Type | Name | Summary | 
| --- | --- | --- | 
| `Boolean` | Enabled | 通过外部调用来终止引擎 | 
| `List<Action>` | StartActions | 初始化操作，系统按顺序执行Init操作 | 
| `List<Action>` | UpdateActions | 更新操作，系统按顺序执行Update操作 | 


Static Methods

| Type | Name | Summary | 
| --- | --- | --- | 
| `T` | AddSystem() | 将系统加入引擎总生命周期 完 全 一 致 | 
| `T` | GetSystem() | 获取系统,理论上来讲这个方法是不应该被滥用的 | 
| `void` | Init() | 引擎的初始化和引擎内部系统的添加 | 
| `void` | Run() | 开始运行游戏. 执行系统的Start循环执行系统的Update | 
| `void` | SetSystemUpdate() | 设置系统的执行顺序和优先级,这部分操作考虑交给中间层来做 | 


## `Scene`

场景基类, 用于保存游戏物体的引用
```csharp
public abstract class Destroy.Scene

```

Properties

| Type | Name | Summary | 
| --- | --- | --- | 
| `Int32` | GameObjectCount | 场景中的游戏对象的数量 | 
| `List<GameObject>` | GameObjects |  | 
| `Dictionary<String, List<GameObject>>` | GameObjectsWithTag |  | 
| `String` | Name | 场景的名字 | 


Methods

| Type | Name | Summary | 
| --- | --- | --- | 
| `void` | OnDestroy() | 在销毁这个Scene时调用 | 
| `void` | OnStart() | 在创建的时候会进行调用 | 


## `SceneManager`

场景管理器
```csharp
public static class Destroy.SceneManager

```

Static Properties

| Type | Name | Summary | 
| --- | --- | --- | 
| `Scene` | ActiveScene | 当前激活的场景,所有新创建的对象都自动属于当前激活的场景 | 
| `DefaultScene` | DefaultScene |  | 
| `DontDestroyOnLoad` | DontDestroyOnLoad |  | 
| `Int32` | SceneCount | 场景数量 | 
| `Dictionary<String, Scene>` | Scenes | 字符串索引,保存着所有场景 | 


Static Methods

| Type | Name | Summary | 
| --- | --- | --- | 
| `void` | Init() | 在初始化的时候创建两个默认的Scene. 这两个Scene永远不会被销毁 | 
| `void` | Load(`Scene` newScene, `LoadSceneMode` loadSceneMode = Single) | 加载场景 | 
| `void` | SetActiveScene(`Scene` scene) | 设置某个场景为活动场景 | 
| `void` | SetActiveScene(`String` str) | 设置某个场景为活动场景 | 


## `Script`

所有脚本的基类,通常开发者继承这个类来自定义组件
```csharp
public abstract class Destroy.Script
    : Component

```

Methods

| Type | Name | Summary | 
| --- | --- | --- | 
| `void` | Awake() | 相当于Initialize 在添加的时候进行调用 | 
| `void` | CancleInvoke(`String` methodName) | 取消一个延迟调用的方法 | 
| `void` | Initialize() |  | 
| `void` | Invoke(`String` methodName, `Single` delayTime) | 延迟调用一个方法(该方法必须为实例无参public方法) | 
| `Boolean` | IsInvoking(`String` methodName) | 该方法是否在延迟调用 | 
| `void` | OnAdd() |  | 
| `void` | OnCollision(`Collision` collision) | 重载来接收碰撞回调消息 | 
| `void` | OnRemove() |  | 
| `void` | Start() | 在生命周期的开始调用. 如果在Update中创建,那么就在下一次生命周期的时候调用 | 
| `void` | Update() | 每帧调用一次 | 


## `SearchResult`

寻路算法的返回值
```csharp
public class Destroy.SearchResult

```

Fields

| Type | Name | Summary | 
| --- | --- | --- | 
| `List<Vector2>` | ResultList | 结果路径 | 
| `Int32` | SearchAeraCount | 搜索一共搜了多少个点,用于评估算法的效率 | 
| `Boolean` | Success | 搜索是否成功 | 


## `ShowInInspector`

从原则上来说,所有字段都是默认隐藏的,可以加ShowInInspector来显示字段
```csharp
public class Destroy.ShowInInspector
    : Attribute, _Attribute

```

## `SimpleController`

简单控制器(每帧进行移动)
```csharp
public class Destroy.SimpleController
    : Script

```

Methods

| Type | Name | Summary | 
| --- | --- | --- | 
| `void` | Update() |  | 


## `StandardIO`

平台相关的系统只向这里注册,其他方法均为引擎内部实现  引擎入口务必要保证处理好里面的事件
```csharp
public static class Destroy.StandardIO

```

Static Fields

| Type | Name | Summary | 
| --- | --- | --- | 
| `Action<String>` | DebugLogEvent | 标准调试信息输出 | 
| `Func<ConsoleKey, Boolean>` | GetKeyEvent | 输入入口,键盘按键是否被按下  通过WindowsAPI实现 | 
| `Func<MouseButton, Boolean>` | GetMouseButtonEvent | 输入入口,该按键是否被按下  目前通过WindowsAPI实现 | 
| `Func<Vector2>` | GetMousePositionInPixelEvent | 鼠标位置  Winform实现,鼠标指针在游戏面板控件的位置  Windows实现,不怎么好用 | 
| `Action<List<RenderPoint>>` | RendererEvent | 渲染出口 | 


## `SystemUIFactroy`

系统的调试窗口组件,可以通过一行命令进行调出,使用时请注意注释提醒的窗口大小,给窗口留出足够的位置
```csharp
public static class Destroy.SystemUIFactroy

```

Static Methods

| Type | Name | Summary | 
| --- | --- | --- | 
| `GameObject` | GetMouseEventUI(`Vector2` pos) | 用于显示鼠标检测到的事件  范围5*10 | 
| `GameObject` | GetRendererTestAera(`Vector2` pos) | 一片用于测试渲染器压力测试的组件,每帧都会进行200个彩色格子的刷新,刷新率取决于帧率  范围20*20 | 
| `GameObject` | GetSystemInspector() | 系统监视器 默认位于屏幕右上角 | 
| `GameObject` | GetSystemInspector(`Vector2` pos) | 系统监视器 默认位于屏幕右上角 | 
| `GameObject` | GetTimerUI(`Vector2` pos, `Int32` interval = 10) | 显示系统中各个系统耗时  范围14*17 | 


## `TextBox`

文本框组件
```csharp
public class Destroy.TextBox
    : Component

```

Fields

| Type | Name | Summary | 
| --- | --- | --- | 
| `GameObject` | boxDrawing | 边框的对象 | 
| `List<Renderer>` | Labels | 最后返回的应该是TextBox组件,可以通过label更改每一条的信息, | 


Methods

| Type | Name | Summary | 
| --- | --- | --- | 
| `Boolean` | SetText(`String` str, `Int32` line) | 更改对应行上面的字符,也可以通过获取Labels自己找对应的Label组件 | 


## `Time`

时间类 <see langword="static" />
```csharp
public static class Destroy.Time

```

Static Properties

| Type | Name | Summary | 
| --- | --- | --- | 
| `Single` | AllSystemsSpendTime | 所有系统该帧运行的时间(秒) | 
| `Single` | CurFrameRate | 当前帧率 | 
| `Single` | DeltaTime | 这一帧距离上一帧的时间(秒) | 
| `Single` | MaxFrameRate | 最大帧率 | 
| `Dictionary<String, Single>` | SystemsSpendTimeDict | 单个系统该帧运行的时间(秒) | 
| `Single` | TotalTime | 游戏总运行时间(秒) | 


## `TypeRootConverter`

根类转换器 <see langword="static" />
```csharp
public static class Destroy.TypeRootConverter

```

Static Methods

| Type | Name | Summary | 
| --- | --- | --- | 
| `Type` | GetComponentRoot(`Type` type) |  | 
| `Type` | GetSystemRoot(`Type` type) |  | 
| `void` | Init() | 初始化 | 
| `void` | InitByOtherAssembly() | 将调用这个方法的程序集的类型加入索引. | 


## `UIFactroy`

DestroyUI 用这个创建常用UI组件
```csharp
public static class Destroy.UIFactroy

```

Static Methods

| Type | Name | Summary | 
| --- | --- | --- | 
| `GameObject` | CreateBoxDrawingRect(`Vector2` pos, `Int32` height, `Int32` width) | 创建一个制表符画出来的长方形 | 
| `Button` | CreateButton(`Vector2` pos, `String` text, `Action` onClick = null, `Int32` width = -1) | 创建一个按钮组件 | 
| `Renderer` | CreateLabel(`Vector2` pos, `String` text = , `Int32` width = -1) | 创建一个Lable组件,不带有默认文字 | 
| `ListBox` | CreateListBox(`Vector2` pos, `Int32` height, `Int32` width) | 文本列表,可以使用光标选择,并带有滚动条. | 
| `TextBox` | CreateTextBox(`Vector2` pos, `Int32` height, `Int32` width) | 文本框UI组件 | 


## `Vector2`

整数型二维向量  原Vector2Int更改为Vector2为了简化
```csharp
public struct Destroy.Vector2
    : IComparable

```

Fields

| Type | Name | Summary | 
| --- | --- | --- | 
| `Int32` | X |  | 
| `Int32` | Y |  | 


Properties

| Type | Name | Summary | 
| --- | --- | --- | 
| `Vector2` | Negative |  | 


Methods

| Type | Name | Summary | 
| --- | --- | --- | 
| `Int32` | CompareTo(`Object` obj) | 比较原则, 左上角的小于右下角的, 按照从上到下, 从左到右排序 | 
| `Int32` | Distance(`Vector2` otherV) | 向量之间的距离 | 
| `Boolean` | Equals(`Object` obj) |  | 
| `Int32` | GetHashCode() |  | 
| `String` | ToString() |  | 


Static Properties

| Type | Name | Summary | 
| --- | --- | --- | 
| `Vector2` | DefaultInput | 当检测到输入失灵的时候返回的默认值,不返回0,0为了加以区分 | 
| `Vector2` | Down |  | 
| `Vector2` | Left |  | 
| `Vector2` | Right |  | 
| `Vector2` | Up |  | 
| `Vector2` | Zero |  | 


Static Methods

| Type | Name | Summary | 
| --- | --- | --- | 
| `Int32` | Distance(`Vector2` a, `Vector2` b) | 向量之间的距离 | 


## `Vector2Float`

二维向量  原Vector2-&gt;Vector2Float
```csharp
public struct Destroy.Vector2Float

```

Fields

| Type | Name | Summary | 
| --- | --- | --- | 
| `Single` | X | X值 | 
| `Single` | Y | Y值 | 


Properties

| Type | Name | Summary | 
| --- | --- | --- | 
| `Single` | Magnitude | 向量长度 | 
| `Vector2Float` | Negative | 反向 | 
| `Vector2Float` | Normalized | 单位向量 | 


Methods

| Type | Name | Summary | 
| --- | --- | --- | 
| `Boolean` | Equals(`Object` obj) |  | 
| `Int32` | GetHashCode() |  | 
| `String` | ToString() |  | 


Static Properties

| Type | Name | Summary | 
| --- | --- | --- | 
| `Vector2Float` | Down |  | 
| `Vector2Float` | Left |  | 
| `Vector2Float` | Right |  | 
| `Vector2Float` | Up |  | 
| `Vector2Float` | Zero |  | 


Static Methods

| Type | Name | Summary | 
| --- | --- | --- | 
| `Single` | Distance(`Vector2Float` a, `Vector2Float` b) | 向量之间的距离float | 


## `WinformEngine`

Winform程序入口
```csharp
public class Destroy.WinformEngine

```

Static Methods

| Type | Name | Summary | 
| --- | --- | --- | 
| `void` | OpenWithEditor(`Action` action) | Winform的初始化. | 



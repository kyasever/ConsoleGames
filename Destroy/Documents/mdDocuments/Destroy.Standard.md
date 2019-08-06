## `Bullet`

一个会自动飞行的子弹对象
```csharp
public class Destroy.Standard.Bullet
    : Script

```

Fields

| Type | Name | Summary | 
| --- | --- | --- | 
| `Collider` | createrCollider |  | 
| `Vector2Float` | FPosition |  | 
| `Vector2Float` | Speed |  | 


Methods

| Type | Name | Summary | 
| --- | --- | --- | 
| `void` | Awake() |  | 
| `void` | OnCollision(`Collision` collision) |  | 
| `void` | Update() |  | 


## `CameraController`

相机跟随组件
```csharp
public class Destroy.Standard.CameraController
    : Script

```

Fields

| Type | Name | Summary | 
| --- | --- | --- | 
| `GameObject` | followTrans | 跟随的目标 | 


Methods

| Type | Name | Summary | 
| --- | --- | --- | 
| `void` | Update() |  | 


## `Cursor`

鼠标光标. 当鼠标位于屏幕边缘时会自动滚屏
```csharp
public class Destroy.Standard.Cursor
    : Script

```

Fields

| Type | Name | Summary | 
| --- | --- | --- | 
| `Single` | MoveSpeed | 鼠标滚屏速度 | 


Methods

| Type | Name | Summary | 
| --- | --- | --- | 
| `void` | Update() | 更新摄像机 | 


Static Properties

| Type | Name | Summary | 
| --- | --- | --- | 
| `Cursor` | Instance | 单例对象 | 


## `GameMode`

游戏管理器
```csharp
public class Destroy.Standard.GameMode
    : Singleton<GameMode>

```

Static Properties

| Type | Name | Summary | 
| --- | --- | --- | 
| `MoveIndicator` | MoveIndicator | 移动范围指示器 | 
| `Random` | Random | 随机数发生器 | 
| `RouteIndicator` | RouteIndicator | 移动路径指示器 | 


## `Gun`

枪 位于枪列表的一个底下的组件,代表这把武器的状态
```csharp
public class Destroy.Standard.Gun
    : Component

```

Fields

| Type | Name | Summary | 
| --- | --- | --- | 
| `Int32` | AM | 剩余弹药数量 | 


## `GunBox`

枪列表
```csharp
public class Destroy.Standard.GunBox
    : Component

```

Properties

| Type | Name | Summary | 
| --- | --- | --- | 
| `Int32` | CurrertAM |  | 


Methods

| Type | Name | Summary | 
| --- | --- | --- | 
| `void` | Initialize() |  | 
| `void` | OnAdd() |  | 


Static Fields

| Type | Name | Summary | 
| --- | --- | --- | 
| `GunBox` | Instance | 保存了一个单例,这个类主要就是为了这个了 | 


## `GunUI`

血量和剩余弹药指示UI
```csharp
public class Destroy.Standard.GunUI
    : Script

```

Methods

| Type | Name | Summary | 
| --- | --- | --- | 
| `void` | Awake() |  | 
| `void` | Update() |  | 


## `Layer`

Layer划分,越上面的显示的越靠前,数字可以随意改动,但是最好使用枚举来进行数字排列避免出错
```csharp
public enum Destroy.Standard.Layer
    : Enum, IComparable, IFormattable, IConvertible

```

Enum

| Value | Name | Summary | 
| --- | --- | --- | 
| `8` | Cursor |  | 
| `9` | Environment |  | 
| `10` | Agent |  | 
| `11` | MoveRoute |  | 
| `12` | MoveAera |  | 


## `MapLoader`

地图加载,从文件中加载地图
```csharp
public static class Destroy.Standard.MapLoader

```

Static Methods

| Type | Name | Summary | 
| --- | --- | --- | 
| `GameObject` | CreateWall(`List<Vector2>` list) | 创建地图对象 | 
| `void` | LoadMap(`String` name) | 加载指定资源名字作为地图 | 


## `MoveIndicator`

可移动范围指示器
```csharp
public class Destroy.Standard.MoveIndicator
    : Script

```

Methods

| Type | Name | Summary | 
| --- | --- | --- | 
| `void` | Awake() |  | 
| `Boolean` | Contains(`Vector2` v) | 展开的范围是否包括某一个点 | 
| `void` | ExpandAera(`Vector2` center, `Int32` expandWidth) | 沿中心点展开一个范围指示器 | 
| `void` | SetActive(`Boolean` active) | 设置是否激活/显示 | 


## `RouteIndicator`

路径指示器
```csharp
public class Destroy.Standard.RouteIndicator
    : Script

```

Methods

| Type | Name | Summary | 
| --- | --- | --- | 
| `void` | Awake() |  | 
| `void` | SearchRoute(`Vector2` beginPos, `Vector2` endPos) | 沿路径展开一个路径指示器 | 
| `void` | SetActive(`Boolean` active) | 设置是否显示 | 


## `Shooter`

枪手 可以发射子弹
```csharp
public class Destroy.Standard.Shooter
    : Script

```

Fields

| Type | Name | Summary | 
| --- | --- | --- | 
| `ShootMode` | shootMode |  | 


Methods

| Type | Name | Summary | 
| --- | --- | --- | 
| `void` | Awake() |  | 
| `void` | Shoot() |  | 
| `void` | Update() |  | 


## `Singleton<T>`

继承该类获得单例
```csharp
public class Destroy.Standard.Singleton<T>

```

Static Properties

| Type | Name | Summary | 
| --- | --- | --- | 
| `T` | Instance | 单例 | 


## `SRPGAgent`

战棋策略游戏的角色控制器
```csharp
public class Destroy.Standard.SRPGAgent
    : Script

```

Fields

| Type | Name | Summary | 
| --- | --- | --- | 
| `State` | state | 角色状态 | 


Methods

| Type | Name | Summary | 
| --- | --- | --- | 
| `void` | Awake() |  | 
| `void` | Update() |  | 


## `SRPGScene`

SRPG游戏范例
```csharp
public class Destroy.Standard.SRPGScene
    : Scene

```

Methods

| Type | Name | Summary | 
| --- | --- | --- | 
| `void` | OnDestroy() |  | 
| `void` | OnStart() |  | 


## `StartScene`

一个新的主场景,开始一个新的游戏
```csharp
public class Destroy.Standard.StartScene
    : Scene

```

Methods

| Type | Name | Summary | 
| --- | --- | --- | 
| `void` | OnStart() |  | 


## `TPSFactroy`

创建一个典型的组件的实例
```csharp
public static class Destroy.Standard.TPSFactroy

```

Static Methods

| Type | Name | Summary | 
| --- | --- | --- | 
| `GameObject` | CreateBox(`Vector2` pos) | 创建一个基于物理系统的标准箱子 | 
| `GameObject` | CreateStandardPlayer(`String` str, `Vector2` pos) | 创建一个标准单格主角 | 
| `Renderer` | CreateTestLable(`Vector2` pos) | 创建一个测试用字符串 | 
| `ListBox` | CreateTestListBox(`Vector2` pos) | 创建一个测试用ListBox | 
| `TextBox` | CreateTestTextBox(`Vector2` pos) | 创建一个测试用文本框 | 


## `TPSScene`

一个用于展示的场景
```csharp
public class Destroy.Standard.TPSScene
    : Scene

```

Methods

| Type | Name | Summary | 
| --- | --- | --- | 
| `void` | OnStart() | 在场景中创建对象 | 



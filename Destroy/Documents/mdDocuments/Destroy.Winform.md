## `EditorSystem`

将编辑器的生命周期加入整体生命周期来管理  所有编辑器操作使用异步的方式进行更新,不会影响主线程  同时负责将Winform的API对接到引擎里
```csharp
public class Destroy.Winform.EditorSystem
    : DestroySystem

```

Methods

| Type | Name | Summary | 
| --- | --- | --- | 
| `void` | Start() | 初始化 | 
| `void` | Update() | 更新 | 


Static Fields

| Type | Name | Summary | 
| --- | --- | --- | 
| `Vector2` | MousePosition | 用于指示当前鼠标所处位置 | 


Static Methods

| Type | Name | Summary | 
| --- | --- | --- | 
| `void` | DebugLog(`String` msg) | Debug输出 | 
| `Vector2` | GetMousePositionPixel() | 获得鼠标位置 | 
| `void` | Renderer(`List<RenderPoint>` list) | 使用异步的方式执行渲染,由于GDI的性能瓶颈限制大约只有40帧.  所以实际帧率可能不是很乐观 | 


## `ExtandColor`

```csharp
public static class Destroy.Winform.ExtandColor

```

## `ExtandVector2Int`

Vector2Int扩展,这个属于Destroy.Winform
```csharp
public static class Destroy.Winform.ExtandVector2Int

```

Static Methods

| Type | Name | Summary | 
| --- | --- | --- | 
| `Vector2` | Size(this `Size` _Size) | Size 转换为Vector2Int | 
| `Color` | ToColor(this `Color` color) | Colour转换为Drawing.Color | 
| `Point` | ToPoint(this `Vector2` _Vector2Int) | Vector2Int 转换为Point | 
| `Vector2` | ToVector2Int(this `Point` _Point) | Point转换为Vector2Int | 


## `MainForm`

Editor的主窗口
```csharp
public class Destroy.Winform.MainForm
    : Form, IComponent, IDisposable, IOleControl, IOleObject, IOleInPlaceObject, IOleInPlaceActiveObject, IOleWindow, IViewObject, IViewObject2, IPersist, IPersistStreamInit, IPersistPropertyBag, IPersistStorage, IQuickActivate, ISupportOleDropSource, IDropTarget, ISynchronizeInvoke, IWin32Window, IArrangedElement, IBindableComponent, IContainerControl

```

Fields

| Type | Name | Summary | 
| --- | --- | --- | 
| `GameObject` | CurrertGameObject | 当前被选中的游戏物体 | 


Methods

| Type | Name | Summary | 
| --- | --- | --- | 
| `void` | AddMessage(`String` msg) | 向listbos里添加MSG | 
| `void` | Dispose(`Boolean` disposing) | Clean up any resources being used. | 
| `void` | Draw(`List<RenderPoint>` drawList) | panel绘制方法 | 
| `void` | InitSize() | 开始的时调用一次,设置窗口大小 | 
| `void` | RefreshEditoerPosition() | 重新给Editor排版 | 
| `void` | RefrestGameObjects() | 刷新场景树的数据  随时间刷新,感觉不太好,但也没啥特别方便的办法了  把非active的折叠起来,把UI一层 感觉这样可行 | 
| `void` | SetRightTreeView(`GameObject` gameObject, `Boolean` firstOpen) | 刷新右侧组件信息界面 | 


Static Fields

| Type | Name | Summary | 
| --- | --- | --- | 
| `Dictionary<TreeNode, GameObject>` | tree_NodeDic | 用来保存节点和游戏对象的对应关系 | 


Static Properties

| Type | Name | Summary | 
| --- | --- | --- | 
| `MainForm` | Instanse | 单例 | 



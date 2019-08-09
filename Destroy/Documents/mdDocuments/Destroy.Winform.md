## `EditorRuntime`

将引擎传来的数据处理为图像,受独立线程管辖
```csharp
public class Destroy.Winform.EditorRuntime

```

Static Fields

| Type | Name | Summary | 
| --- | --- | --- | 
| `List<RenderPoint>` | buffer | 上次的渲染结果 | 
| `Bitmap` | bufferBitmap | 用于缓存的图片 | 
| `List<RenderPoint>` | bufferbuffer | 二级缓存 | 
| `List<Int32>` | diffIndex | 差异化,然后只渲染差异化的内容 | 
| `Boolean` | firstLoad | 第一次调用渲染的时候初始化buffer | 
| `List<RenderPoint>` | renderList | 当前渲染的数据来源 | 


Static Methods

| Type | Name | Summary | 
| --- | --- | --- | 
| `void` | PreDraw() | 预加载缓存 | 
| `void` | Run() | 运行编辑器生命周期 | 


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
| `Int32` | PreRenderCount | 预渲染计数 | 
| `Int32` | RenderCount | 渲染计数 | 
| `Int32` | TotalCount | 引擎计数 | 


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


## `FormEditor`

界面窗体交互部分
```csharp
public class Destroy.Winform.FormEditor
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
| `void` | Draw() | panel绘制方法 | 
| `void` | RefrestGameObjects() | 刷新场景树的数据  随时间刷新,感觉不太好,但也没啥特别方便的办法了 | 
| `void` | SetRightTreeView(`GameObject` gameObject, `Boolean` firstOpen) | 刷新右侧组件信息界面 | 
| `void` | UpdateForm() |  | 
| `void` | UpdateLabel2(`String` s) |  | 


Static Fields

| Type | Name | Summary | 
| --- | --- | --- | 
| `Boolean` | EditorMode | 是否打开编辑器,非编辑器模式关掉不需要的功能. | 
| `Dictionary<TreeNode, GameObject>` | tree_NodeDic | 用来保存节点和游戏对象的对应关系 | 


Static Properties

| Type | Name | Summary | 
| --- | --- | --- | 
| `FormEditor` | Instanse | 单例 | 



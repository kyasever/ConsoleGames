## `Windows`

暂时的API
```csharp
public static class Destroy.Windows.Windows

```

Static Methods

| Type | Name | Summary | 
| --- | --- | --- | 
| `Int16` | GetAsyncKeyState(`Int32` key) | 获取当前鼠标键盘的状态 | 
| `Boolean` | GetInput(`ConsoleKey` consoleKey) | 获取输入按键 | 
| `Boolean` | GetInputMouse(`MouseButton` mouseButton) | 获取鼠标按键 | 
| `Int16` | QueryPerformanceCounter(`Int64&` x) | 获取当前系统运行的时钟周期 | 
| `Int16` | QueryPerformanceFrequency(`Int64&` x) | 获取当前CPU的主频 | 


## `WindowsAPI`

留出接口,请帮忙实现一下
```csharp
public class Destroy.Windows.WindowsAPI

```

Static Methods

| Type | Name | Summary | 
| --- | --- | --- | 
| `void` | DebugLog(`String` s) | 输出调试信息 | 
| `Boolean` | GetInput(`ConsoleKey` consoleKey) | 获取键盘按键 | 
| `Boolean` | GetInputMouse(`MouseButton` mouseButton) | 获取鼠标按键 | 
| `Vector2` | GetMousePositionPixel() | 获得鼠标所在像素点的位置 | 
| `void` | Renderer(`List<RenderPoint>` list) | 渲染输出 | 


## `WindowsEngine`

windows初始化
```csharp
public class Destroy.Windows.WindowsEngine

```

Static Methods

| Type | Name | Summary | 
| --- | --- | --- | 
| `void` | OpenWithWindows(`Action` action) | WindorsAPI的初始化. | 



欢迎来到Destroy Wiki页面
-----

特别感谢: [MarkdownGenerator](https://github.com/neuecc/MarkdownGenerator)  本wiki使用了这个工具将xml注释转换为md格式.

命名空间:
### Destroy:

引擎核心,包含引擎核心功能。引擎核心功能是平台无关的,IO需要外部对接。

### Destroy.Stardard

包含一些例程和工具,尚在维护中

### Destroy.Winform

包含依赖GDI的启动器,使用GDI渲染引擎画面，使用winform的组件实现编辑器，和消息管理。

### Destroy.Windows

包含依赖WindowsAPI的启动器,使用Windows.h的cpp封装调用windows原生控制台和IO.没有编辑器功能.

### Destroy.OpenGL(待更新)

基于GLFW封装的OpenToolKit组件,调用OpenGL的IO进行引擎渲染.相关功能正在测试中.

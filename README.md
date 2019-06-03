DestroyClassic说明文档
===

一.开启引擎
----
>略

二.建立一个游戏物体
------

* 在5月更新中大幅改变了API调用方式,现在API更加单纯简单.更倾向于面向对象,但是依然保存了组件化的调用方式.创建一个游戏物体通常继承Script类,然后在Awake中完成这个对象的各种初始化和构建.
```cs
    public class MainUI : Script
    {
        public override void Awake()
        {
            //绑定UI的回调事件
            OnMoveInEvent += () => { Debug.Log("Move in"); };
            OnClickEvent += () => { Debug.Log("Clicked"); };
            //更改属性
            mainUI.Position = new Vector2(20, 20);
            mainUI.Draw(new Rectangle(10, 10));
            mainUI.RefreshCollider();
        }
    }
```

* 在建立好我们的脚本之后,脚本只是一个脚本,它本身不能作为一个游戏物体生成在游戏世界里.所以需要在游戏中创建一个游戏物体,并将自己的脚本挂在这个物体上使其生效.
```cs
    MainUI mainUI;
    //创建一个空的游戏物体,并把这个脚本挂在游戏物体上使其生效.
    UIObject uiObject = new UIObject("UI", "UI");
    mainUI = uiObject.AddComponent<MainUI>();
    //当然也可以使用一个便捷的API直接创造一个挂载这个脚本的游戏物体.
    mainUI = UIObject.CreateWith<MainUI>("UI","UI");
```

三.定制这个游戏物体
---
* 在新版本中,建议在通常情况下一个游戏物体上有一个主脚本,负责这个物体的初始化,构造和主逻辑. 

>继承树 : RawComponent ->Component -> Script  
>RawComponent : 引擎内置的隐藏组件.不需要关心  
>Component : 标准组件,继承这个类的组件拥有几乎全部Destroy引擎可以提供的API  
>Script : 除了标准组件API之外还提供生命周期管理功能.  

* 所以只要游戏脚本是继承自Component/Script 就可以在脚本内直接通过相应函数调用相应的功能.例如:

```cs
    public override void Start()
    {
        //改变自身的碰撞体积
        ColliderList = new List<Vector2>() { new Vector2(0, 0), new Vector2(1, 1) };
        //给自身增加碰撞回调
        OnCollisionEvent += (Collision c) => { Debug.Log(c.HitPos); };
        //给自身增加一个字符串显示
        DrawString("哇hahahaha");
        //改变自身的位置
        Position = new Vector2(10, 10);
        //改变自身的父物体
        Parent = Camera.Main.GameObject;
    }
```





这是一款控制台运行的战棋策略游戏. 
---
>本游戏正在进行Destroy控制台引擎的移植工作,并计划添加更多扩展和功能.
### 使用说明
编译后直接运行会缺失资源文件,将Resources文件夹中的文件复制到bin目录下即可  
EXE.rar是游戏的编译版本,可以直接解压这个文件点击游玩.(每大版本更新一次)  
推荐开始简单难度单人模式
### 当前版本

V2.1  
添加了网络联机功能(测试中,不可用)  
增加了两个新的角色

### 更新计划
增加更多的新手关卡  
缩小关卡的规模  
使用Destroy重构游戏  
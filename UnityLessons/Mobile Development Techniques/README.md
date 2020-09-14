# Trash Dash 代码演示

## 介绍

我们创建了Trash Dash游戏，作为在移动游戏中如何使用Unity Services的示例。它还包含一些用于解决常见游戏编程问题的编码体系结构和技术的有用示例。在本文中，我们将快速浏览Tash Dash的代码并了解这些功能。

![img](https://connect-prd-cdn.unity.com/20190130/7c4aeace-2ae5-499a-9825-7837ba4b65a8_trash1.jpg)

如果您尚未下载Trash Dash，可以在Unity Asset Store上下载：

![img](https://connect-prd-cdn.unity.com/20190308/p/images/d05859fa-f665-4301-a584-77565d93f5ff)

## Unity Services

`Trash Dash` 使用[Unity Services](https://unity3d.com/services) 来处理广告，分析和IAP。以及我们在完成的游戏中可以看到的服务，我们的团队在后台使用了Unity Collaborate和Unity Cloud Build来构建Trash Dash。

使用Unity Services可以轻松快捷地使这些重要功能正常运行：我们只需要在Services Dashboard中配置一些细节并编写少量代码即可。

要查看用于集成IAP的代码，请查看**IAPHandler.cs** 。要查看用于处理广告的代码，请参阅**AdsForMission.cs** 和**UnityAdsInitializer.cs** 。在整个项目中都可以找到Analytics（分析）代码，但在**GameManager.cs中**可以找到一些很好的示例。要了解有关设置Unity Services的更多信息，请参见 [Unity手册的这一部分](https://docs.unity3d.com/Manual/UnityServices.html) 。

## 架构

当我们谈论游戏的架构时，我们指的是代码的组织方式。好的架构可以帮助我们避免性能低下，加载时间长或难以维护的代码等问题。

任何游戏的最佳架构都取决于游戏本身。大型团队正在开发的在线多人游戏与一个人正在开发的单人离线游戏有着截然不同的需求。

让我们看一下Trash Dash体系结构中的几个关键点。我们将检查代码的组织方式以及解决的问题。

### Game Manager

**GameManager.cs** 是控制游戏总体流程的类。

GameManager使用一种称为[**Singleton**](https://en.wikipedia.org/wiki/Singleton_pattern) 的[**设计模式**](https://en.wikipedia.org/wiki/Software_design_pattern) 。Singleton模式是一种确保游戏中只有一个特定类的实例的方式。当类具有重要职责时，例如GameManager，这很有用。它还使其他任何类都可以使用公共静态引用轻松访问此实例。

GameManager也是[**有限状态机的**](https://en.wikipedia.org/wiki/Finite-state_machine) 一个示例。有限状态机可确保游戏在任何给定时间只能处于一种**状态**，并管理该状态改变时发生的情况。`Trash Dash` 可能处于三种状态：显示开始菜单，玩游戏和在屏幕上显示游戏。从游戏的整体流程到单个角色的行为，有限状态机是一种管理事物在游戏中行为方式的常见方法。

要查看Singleton和有限状态机的代码，请浏览**GameManager.cs** 并阅读注释。可以在以下文件中找到进入，执行和退出这三种状态的代码：**LoadoutState.cs** ，**GameState.cs** 和**GameOverState.cs** 。

### Object pooling

在`Trash Dash`中，单个游戏阶段可能会产生数百枚硬币。在运行时实例化和销毁大量对象可能会对性能造成压力：它可能涉及相对昂贵的代码，并且可能导致频繁且耗时的**[垃圾收集]（https://en.wikipedia.org/wiki/Garbage_collection** **（计算机****科学）**。

为了减少这种开销，Trash Dash使用了一种称为[**对象池**](https://en.wikipedia.org/wiki/Object_pool_pattern) 的技术。对象池是一种技术，其中对象被临时停用，然后根据需要进行回收，而不是被创建和销毁。

游戏开始时，会生成许多无效的硬币游戏对象并将其放置在“池”中。如果需要新硬币，则从池中请求一个新硬币并将其启用。当收集硬币或离开屏幕时，硬币将被禁用并返回到池中。

要查看`Trash Dash`的对象池代码，请查看**TrackManager.cs** ，**Coin.cs** 和**Pooler.cs** 。有关Unity中对象池的一般指南，请参见[Unity Learn网站上的本教程](https://unity3d.com/learn/tutorials/topics/scripting/object-pooling) 。

## 技巧

虽然架构决策通常会影响我们的游戏的许多类或部分，但技术的关注点较小。技术可能只影响单个函数或文件，但它们仍可以帮助我们解决问题。

让我们看一下`Trash Dash`中使用的几种技术，看看它们可以解决什么问题。

### 原点复位

在玩家旅行距离很长的任何游戏中（例如太空探索游戏或Trash Dash之类的“无限”游戏），开发人员必须决定如何处理玩家的位置。如果我们简单地移动玩家GameObject，则玩家的transform.position中的值将随着时间的流逝而越来越高。由于所谓的[**浮点不精确度**](https://docs.microsoft.com/zh-cn/cpp/build/why-floating-point-numbers-may-lose-precision?view=vs-2019) 这可能会导致问题。
https://msdn.microsoft.com/en-us/library/c151dt3s.aspx
浮点数不精确度意味着浮点数的值越大，精度越低。这是计算机存储数字数据方式的限制，并且不是Unity独有的。在具有较大或无限可玩区域的游戏中，用于存储位置的浮点数可能会变得足够大而引起问题。如果GameObjects的位置值不精确，它们可能会四处移动、闪烁或弹入和弹出。

解决此问题的方法有几种，哪种方法最好取决于游戏。在Trash Dash中，我们通过使用一种称为 **origin reset** 的技术来解决此问题。这意味着一旦玩家移动了超出世界*原点*的某个距离（即世界位置 0, 0, 0），我们就会将场景中的所有内容移回原点。这确保了用于位置的值始终保持较低，因此不容易出现不精确的情况。原点复位无缝进行，并且播放器不知道它。

要查看用于原点复位技术的代码，请查看**TrackManager.cs** 。

### 弯曲的世界着色器

在Trash Dash中，我们通过在播放器前面产生轨迹部分并将它们位于播放器后面时将其删除来创建无尽的轨迹。如果玩家可以看到很长的路要走，我们需要提前产生很多赛道部分。这可能会导致性能问题。

除此之外，世界上到处都是硬币，障碍物和人物。同样，如果玩家可以看到前方很远的距离，则Unity必须在它们足够接近以进行交互之前将所有这些对象绘制到屏幕上。这也可能导致性能问题，尤其是在移动设备上。

为了解决这个问题，世界从玩家的角度出发弯曲。这会产生一个无限世界的幻象，隐藏了轨迹碎片的生成，这意味着我们不必在玩家靠近它们之前生成硬币和障碍物。

如果检查场景，可以看到世界的实际几何形状是平坦的，而不是弯曲的。弯曲效果是由着色器创建的。着色器是告诉Unity如何在屏幕上绘制对象的代码。在这种情况下，着色器将计算水平线在弯曲时的外观，然后根据该计算告诉Unity在哪里将像素绘制到屏幕上。

要查看如何工作的文件是**WorldCurver.cs** 和**CurvedCode.cginc** 。乍一看，着色器代码可能有些棘手，但是[Unity手册的这一页是有关](https://docs.unity3d.com/Manual/SL-VertexFragmentShaderExamples.html)读写着色器代码的有用指南。

## 进一步阅读

Trash Dash中还有一些更有趣的方法和技术示例。看一下用于旋转鱼的着色器，使用AssetBundles加载角色和主题的方式以及保存玩家数据的方式。

要了解有关性能优化的更多信息，请阅读[Unity的Learn网站上的这些文章](https://unity3d.com/learn/tutorials/topics/performance-optimization) 。

要了解有关使用Unity制作手机游戏的更多信息，请查看[Unity Learn网站的这一部分](https://unity3d.com/learn/tutorials/topics/mobile-touch) 。
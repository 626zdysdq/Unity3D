#编程实践——Priests and Devils

-------------------------------------

* 阅读以下游戏脚本

> Priests and Devils is a puzzle game in which you will help the Priests and Devils to cross the river within the time limit. There are 3 priests and 3 devils at one side of the river. They all want to get to the other side of this river, but there is only one boat and this boat can only carry two persons each time. And there must be one person steering the boat from one side to the other side. In the flash game, you can click on them to move them and click the go button to move the boat to the other direction. If the priests are out numbered by the devils on either side of the river, they get killed and the game is over. You can try it in many > ways. Keep all priests alive! Good luck!

* 回答问题
<ul>
    <li>列出游戏中提及的事物（Objects）

     > Priests, devils, boat, coast, river

    <li>用表格列出玩家动作表（规则表），注意，动作越少越好

     | 对象      | 条件  |  结果  |
     | --------  | -----:  | :----:  |
     | 船        | 船不为空 |   移动到对岸    |
     | 人物（在岸上）  | 船未满且在人物所在河岸   |   上船   |
     | 人物（在船上）  |  无    |  上岸  |

</ul>

* 编程要求

>* 请将游戏中对象做成预制
>* 在 GenGameObjects 中创建 长方形、正方形、球 及其色彩代表游戏中的对象。
>* 使用 C# 集合类型 有效组织对象
>* 整个游戏仅主摄像机和一个Empty对象，其他对象必须代码动态生成！！！。整个游戏不许出现Find游戏对象SendMessage这类突破程序结构的 通讯耦合语句。违背本条准则,不给分
>* 请使用课件架构图编程，不接受非 MVC 结构程序
>* 注意细节，例如：船未靠岸，牧师与魔鬼上下船运动中，均不能接受用户事件！


-----------------------------------------
## 实现过程

1.建立MVC架构

* 初始时我建立的MVC模型为
   >* Model: Priests, Devils, Boat, Coast.
   >* View: UserGUI.
   >* Controller: BoatController, CoastController, PriestController, DevilController, FirstController, Director.

* 但参考了老师所给的优秀博客后发现：
  >* Priest和Devil虽然表示方式不同，但是所能发生的动作完全相同.
  >* 可以将UserGUI进行进一步的分离为对UI的动作的回应和对结果的反馈两个部分

所以对自己的MVC模型进行优化，如下图：

![MVC](https://m.qpic.cn/psb?/V12JKlAN0jHOQ9/oE8nHbypUCVlKoxWcJp0tlf.J8ECzZmt93k7kqTmEwY!/b/dPMAAAAAAAAA&bo=nALIAQAAAAADB3U!&rf=viewer_4)
<br>

2.构建model模型

* 对于一个Character来说，我们需要区分到底是牧师还是魔鬼，确定它所处的位置，这样我们才能确定它能够进行什么动作

```C#
public class MyCharacter {
		readonly GameObject character;
		readonly Moveable move;         //行动事件
		readonly int characterType;	    //确定类型
		public string Name              //确定名称
		bool _isOnBoat;                 //确定位置
}
```

* 对于一个Boat来说，我们则需要知道它所在的位置，它载Character的位置，和载Character的属性。

```C#
public class Boat {
		readonly GameObject boat;
		readonly Moveable move;              //确定行动事件
		readonly Vector3 fromPosition        //出发点
		readonly Vector3 toPosition          //到达点
		readonly Vector3[] from_positions;   //出发时两个对象的位置
		readonly Vector3[] to_positions;     //到达时两个对象的位置
		MyCharacterController[] passenger = new MyCharacterController[2];  //保存在船上的对象
		int  place;                          //记录船所在的位置
}
```

* 对于一个Coast来说我们需要确定它是哪一边的河岸，需要确定它的上面有多少个Character，以及它们的位置

```C#
public class Coast{
        readonly GameObject coast;             
        readonly Vector3[] positions;            //保存它加载Character的位置
        MyCharacterController[] person_number;   //保存它上面的Character
        int to_or_from;                          //确定是哪一条河岸
}
```

3.建立各Model的Controller

* 我在建立Model的Controller时遇到了一个问题，就是不知道应该把它对点击事件所做的回应放在Model中还是Controller。于是就又去找了网上的和老师给的代码去参考，发现大部分的代码都并没有将Model的Controller拆分出来，而是类似于C++中将对象的属性和操作方法都放在一个类中。所以我对自己实现的Model又进行了优化。

>* 最初遇到的一个巨大的问题是，我点击对象时，它并不做任何反应（明明已经添加了点击的事件）。最终只能像大佬求助，才发现我没有更新对象的状态......（简直智障）。
>* 刚开始时我并没有考虑物体的运动轨迹，而是直接设定了物体的起始点和终止点，导致对象出现了直接从coast中穿过去的现象。在同学的提示下我将上船和上岸的运动分为两段，终于解决了这个鬼畜的问题。
>* 我的第一个版本中对运动进行了起点上船，起点上岸，终点上船和终点上岸四种情况，在后期改进中发现，只需要两种情况的判断就可以解决移动的问题。
<br>

4.实现Director

```C#
public class Director : System.Object {
		private static Director _instance;
		public static Director getInstance() {
			if (_instance == null) {
				_instance = new Director ();
			}
			return _instance;
		}
		public SceneController currentSceneController { get; set; }
	}
```

5.进行实现最高层的View

>* 实现结果判定
>* 实现点击事件的分类

### 最终效果如图

![result](https://m.qpic.cn/psb?/V12JKlAN0jHOQ9/ucyRiQ*1NkqwfgeaEqaA1FbEMxTao8t35R3XeysTRy8!/b/dDABAAAAAAAA&bo=sAZMAwAAAAADB9s!&rf=viewer_4)









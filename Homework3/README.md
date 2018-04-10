
## 牧师与魔鬼优化——动作分离版

-----------------------------------

### 1. 改进的要求

* 在上一版本的的基础上实现游戏对象的移动和游戏对象分离

>* 首先建立一个动作的管理器，通过使用场景控制器，将需要移动的对象传入动作管理器。的把每个需要移动的游戏对象的移动方法提取出来，建立一个动作管理器来管理不同的移动方法。
>* 对应传入对象，实现传入对象相应的action
>* 找到在上一个版本中对于船和人物的移动函数，将移动函数改为调用自己对应的action方法。

### 2.改进的原因

>* 当有很多不同的对象有相同的动作时，可以减少代码量，提高代码的复用性。

### 3.改进的部分

>* SSAction——动作基类

SSAction为所有动作的基类，我们只需要其Transform属性来实现直线运动即可

```C#
using System.Collections.Generic;
using UnityEngine;

public class SSAction : ScriptableObject
{

	public bool enable = true;
	public bool destroy = false;

	public GameObject gameObject;    //创建动作对象
	public Transform transform;
	public ISSActionCallback callback;   //动作完成后的反馈

	public virtual void Start()
	{
		throw new System.NotImplementedException();
	}

	public virtual void Update()
	{
		throw new System.NotImplementedException();
	}
}
```
<br>

>* MoveToAction——基础动作类

构造我们需要的直线运动

```C#
public class MoveToAction : SSAction
{
	public Vector3 target;
	public float speed;

	private MoveToAction() { }
	public static MoveToAction getAction(Vector3 target, float speed)
	{
		MoveToAction action = ScriptableObject.CreateInstance<MoveToAction>();
		action.target = target;
		action.speed = speed;
		return action;
	}

	public override void Update()
	{
		this.transform.position = Vector3.MoveTowards(this.transform.position, target, speed * Time.deltaTime);
		if (this.transform.position == target)
		{
			this.destroy = true;
			this.callback.actionDone(this);
		}
	}

	public override void Start()
	{
		//
	}
}
```

<br>
>* SequenceAction——组合动作类

在这次的游戏中，我们只需要将上岸和上船两个动作看作是两个不同方向的直线运动的组合，按照顺序将其加入动作队列，然后执行，执行完毕后，清空队列。

```C#
public class SequenceAction : SSAction, ISSActionCallback
{
	public List<SSAction> sequence;
	public int repeat = 1;
	public int currentActionIndex = 0;

	public static SequenceAction getAction(int repeat, int currentActionIndex, List<SSAction> sequence)
	{
		SequenceAction action = ScriptableObject.CreateInstance<SequenceAction>();
		action.sequence = sequence;
		action.repeat = repeat;
		action.currentActionIndex = currentActionIndex;
		return action;
	}

	public override void Update()
	{
		if (sequence.Count == 0) return;
		if (currentActionIndex < sequence.Count)
		{
			sequence[currentActionIndex].Update();
		}
	}

	public void actionDone(SSAction source)
	{
		source.destroy = false;
		this.currentActionIndex++;
		if (this.currentActionIndex >= sequence.Count)
		{
			this.currentActionIndex = 0;
			if (repeat > 0) repeat--;
			if (repeat == 0)
			{
				this.destroy = true;
				this.callback.actionDone(this);
			}
		}
	}

	public override void Start()
	{
		foreach (SSAction action in sequence)
		{
			action.gameObject = this.gameObject;
			action.transform = this.transform;
			action.callback = this;
			action.Start();
		}
	}

	void OnDestroy()
	{
		foreach (SSAction action in sequence)
		{
			DestroyObject(action);
		}
	}
}
```

<br>


>* ISSActionCallback——动作管理的接口

实现动作和动作管理者之间的联系，使得动作管理类可以为动作的实现传递信息

```C#
public interface ISSActionCallback
{
	void actionDone(SSAction source);
}
```

<br>

>* SSActionManager——动作管理的基类

对动作进行管理，为其传递游戏对象，以实现我们所需要的动作以及动作之间的切换

```C#
public class SSActionManager : MonoBehaviour, ISSActionCallback
{
	private Dictionary<int, SSAction> actions = new Dictionary<int, SSAction>();
	private List<SSAction> waitingToAdd = new List<SSAction>();
	private List<int> watingToDelete = new List<int>();

	protected void Update()
	{
		foreach (SSAction ac in waitingToAdd)
		{
			actions[ac.GetInstanceID()] = ac;
		}
		waitingToAdd.Clear();

		foreach (KeyValuePair<int, SSAction> kv in actions)
		{
			SSAction ac = kv.Value;
			if (ac.destroy)
			{
				watingToDelete.Add(ac.GetInstanceID());
			}
			else if (ac.enable)
			{
				ac.Update();
			}
		}

		foreach (int key in watingToDelete)
		{
			SSAction ac = actions[key];
			actions.Remove(key);
			DestroyObject(ac);
		}
		watingToDelete.Clear();
	}

	public void addAction(GameObject gameObject, SSAction action, ISSActionCallback whoToNotify)
	{
		action.gameObject = gameObject;
		action.transform = gameObject.transform;
		action.callback = whoToNotify;
		waitingToAdd.Add(action);
		action.Start();
	}

	public void actionDone(SSAction source)
	{

	}
}
```
<br>

>* ActionManager——动作管理

管理动作，对动作的执行进行调度

```C#
public class ActionManager : SSActionManager
{
	public void moveBoat(BoatController boat)
	{
		MoveToAction action = MoveToAction.getAction(boat.getDestination(), boat.movingSpeed);
		this.addAction(boat.getGameobj(), action, this);
	}

	public void moveCharacter(MyCharacterController characterCtrl, Vector3 destination)
	{
		Vector3 currentPos = characterCtrl.getPosition();
		Vector3 middlePos = currentPos;
		if (destination.y > currentPos.y)
		{
			middlePos.y = destination.y;
		}
		else
		{
			middlePos.x = destination.x;
		}
		SSAction action1 = MoveToAction.getAction(middlePos, characterCtrl.movingSpeed);
		SSAction action2 = MoveToAction.getAction(destination, characterCtrl.movingSpeed);
		SSAction seqAction = SequenceAction.getAction(1, 0, new List<SSAction> { action1, action2 });
		this.addAction(characterCtrl.getGameobj(), seqAction, this);
	}
}
```
<br>

### 4.游戏截图

![a](https://m.qpic.cn/psb?/V12JKlAN0jHOQ9/6gN9wEy*curVoIFz*pZ.DJqs4P1nWR9MxVy9llQdicQ!/b/dJEAAAAAAAAA&bo=sgZoAwAAAAADB*0!&rf=viewer_4)

### 5.资源链接

源代码在这里哦
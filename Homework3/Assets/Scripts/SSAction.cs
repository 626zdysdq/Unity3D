using System.Collections;
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
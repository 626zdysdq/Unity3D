using UnityEngine;
using System.Collections;
using System.Collections.Generic;

//DiskFactory
public class Director : System.Object
{
	private static Director _instance;   //导演类的实例
	private static List<GameObject> diskList;   // 飞碟的列表
	public GameObject disk;      

	//实现单例模式
	public static Director getInstance()
	{
		if (_instance == null)
		{
			_instance = new Director();
			diskList = new List<GameObject>();
		}
		return _instance;
	}

	public int getDiskId()
	{
		for (int i = 0; i < diskList.Count; ++i)
		{
			if (!diskList[i].activeInHierarchy)
			{
				return i;
			}
		}
		diskList.Add(GameObject.Instantiate(disk) as GameObject);
		return diskList.Count - 1;
	}

	public GameObject getDiskObject(int id)
	{
		if (id > -1 && id < diskList.Count)
		{
			return diskList[id];
		}
		return null;
	}

	//实现飞碟的回收利用
	public void free(int id)
	{
		if (id > -1 && id < diskList.Count)
		{

			diskList[id].GetComponent<Rigidbody>().velocity = Vector3.zero;

			diskList[id].transform.localScale = disk.transform.localScale;
			diskList[id].SetActive(false);
		}
	}
}


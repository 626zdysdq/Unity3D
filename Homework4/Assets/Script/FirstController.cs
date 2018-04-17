using UnityEngine;
using System.Collections;
using System.Collections.Generic;

//主控制器
public class FirstController : MonoBehaviour
{
	private List<GameObject> disks = new List<GameObject>();   
	private List<int> diskIds = new List<int>();               
	private int diskScale;                               
	private Vector3 emissionPosition;          
	private Vector3 emissionDirection;         
	private float emissionSpeed;
	private int emissionNumber;                 
	private bool isEmissionEnable; 


	public float EmissionDelay = 1f;
	public float timeToNextEmission;
	private bool isCounting;
	private bool isShooting;
	public bool getIsCounting() { return isCounting; }
	public bool getIsShooting() { return isShooting; }
	//建立飞盘颜色的数组，用于后面随机选择颜色
	public Color[] TotalColor = {Color.black,Color.blue,Color.cyan,Color.green,Color.grey,Color.red,Color.yellow };

	private SceneController scene;
	private RoundController roundController;

	void Awake()
	{
		scene = SceneController.getInstance();
		scene.setFirstController(this);
	}

	public void setting(int scale, Color color, Vector3 emitPos, Vector3 emitDir, float speed, int num)
	{
		diskScale = scale;
		emissionPosition = emitPos;
		emissionDirection = emitDir;
		emissionSpeed = speed;
		emissionNumber = num;
	}


	public void MakeEmissionDiskable()
	{
		if (!isCounting && !isShooting)
		{
			timeToNextEmission = EmissionDelay;
			isEmissionEnable = true;
		}
	}

	//控制飞盘的发射函数
	void emissionDisks()
	{
		//每发射一次飞盘，回合数加一，到达10回合后进入下一关卡
		scene.setTrial(scene.getTrial()+1);

		//将创建的飞碟发射出去
		for (int i = 0; i < emissionNumber; ++i)
		{
			diskIds.Add(Director.getInstance().getDiskId());
			disks.Add(Director.getInstance().getDiskObject(diskIds[i]));
			diskScale = Random.Range(1,3);
			disks[i].transform.localScale *= diskScale;

			//随机选择飞碟的颜色
			int chooseColor = Random.Range(0, 7);
			disks[i].GetComponent<Renderer>().material.color = TotalColor[chooseColor];

			//将飞碟移入视线，重置发射时的速度方向，使飞碟在视线内发射
			disks[i].transform.position = new Vector3(Random.Range(-1.5f,1.5f), emissionPosition.y + i, emissionPosition.z);
			disks[i].SetActive(true);
			emissionDirection.x = emissionDirection.x * Random.Range(-1, 1);
			disks[i].GetComponent<Rigidbody>().AddForce(emissionDirection * Random.Range(emissionSpeed * 5, emissionSpeed * 10) / 10, ForceMode.Impulse);
		}
		//发射10次飞碟后进入下一关卡
		if(scene.getTrial() == 10)
		{
			scene.nextRound();
		}
		//当通过三轮关卡是，游戏结束获得胜利。或当生命值降为0时，同样游戏结束
		if ((scene.getRound() == 3 && scene.getTrial()==10) || scene.getChance()<=0)
		{
			scene.gameOver();
		}
	}

	//回收飞碟
	void freeADisk(int i)
	{
		Director.getInstance().free(diskIds[i]);
		disks.RemoveAt(i);
		diskIds.RemoveAt(i);
	}

	//每次发射之间要保持时间间隔，禁止无时间间隔的连续射击
	void FixedUpdate()
	{
		if (timeToNextEmission > 0)
		{
			isCounting = true;
			timeToNextEmission -= Time.deltaTime;
		}
		else
		{
			isCounting = false;
			if (isEmissionEnable)
			{
				emissionDisks();
				isEmissionEnable = false;
				isShooting = true;
			}
		}
	}

	//实现飞碟的回收
	void Update()
	{
		for (int i = 0; i < disks.Count; i++)
		{
			if (!disks[i].activeInHierarchy) //飞碟被击中后回收
			{  
				scene.getScoreController().hitDisk(); 
				freeADisk(i);
			}
			else if (disks[i].transform.position.y < 0)  //飞碟落到平台以下后回收
			{   
				scene.getScoreController().hitGround(); 
				freeADisk(i);
			}
		}
		if (disks.Count == 0)
		{
			isShooting = false;
		}
	}
}
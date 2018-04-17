using UnityEngine;
using UnityEngine.UI;
using System.Collections;

public class RoundController : MonoBehaviour
{
	private Color color;
	private Vector3 emissionPositon;
	private Vector3 emissionDiretion;
	private float speed;
	private SceneController scene;
	public int trial = 0;          //用于标记每一个关卡的发射飞碟的次数

	void Start()
	{
		scene.nextRound();
	}


	void Awake()
	{
		SceneController.getInstance().setRoundController(this);
		scene = SceneController.getInstance();
	}

	//加载资源，预先创建三个不同等级关卡飞碟发射位置，发射速度，发射角度的范围等
	public void loadRoundData(int round)
	{
		trial = 0;
		switch (round)
		{
		case 1:            //关卡1
			color = Color.gray;
			emissionPositon = new Vector3(3f, 0.2f, -5f);     //先将创建的飞碟放在我们不可见的位置
			emissionDiretion = new Vector3(10f, 35.0f, 67f);
			speed = 2;                                        //设置飞碟的速度
			//创建我们所需要的飞碟
			SceneController.getInstance().getFirstController().setting(1, color, emissionPositon, emissionDiretion.normalized, speed, 1); 
			break;
		case 2:           //关卡2
			color = Color.gray;
			emissionPositon = new Vector3(3f, 0.2f, -5f);
			emissionDiretion = new Vector3(10f, 35.0f, 67f);
			speed = 3;
			SceneController.getInstance().getFirstController().setting(1, color, emissionPositon, emissionDiretion.normalized, speed, 2);
			break;
		case 3:           //关卡3
			color = Color.gray;
			emissionPositon = new Vector3(3f, 0.2f, -5f);
			emissionDiretion = new Vector3(10f, 35.0f, 67f);
			speed = 5;
			SceneController.getInstance().getFirstController().setting(1, color, emissionPositon, emissionDiretion.normalized, speed, 3);
			break;
		}
	}
}
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
public class Particlehole : MonoBehaviour {

	public class particleClass  
	{  
		public float radius = 0f; //粒子的运动半径
		public float angle = 0f;  //粒子的角度
		public float time = 0f;   //粒子的时间
		public particleClass(float radius, float angle, float time)  
		{  
			this.radius = radius;    
			this.angle = angle;    
			this.time = time;       
		}  
	}  

	//创建粒子系统
	private ParticleSystem particleSystem; 
	//粒子数组
	private ParticleSystem.Particle[] particlesArray; 
	//粒子属性数组
	private particleClass[] particleAttr; 
	//速度差分层书
	private int tier = 10;  

	//粒子运动的最小半径
	public float minRadius = 5.0f;  
	//粒子运动的最大半径
	public float maxRadius = 8.0f; 

	//粒子数目
	public int count = 12000;
	//粒子的大小
	public float size = 0.1f; 
	//粒子的旋转方向
	public bool rotate = true; 
	//旋转速度
	public float speed = 0.8f; 
	//粒子的游离范围  
	public float pingPong = 0.02f; 
	public Gradient colorGradient;
	//标记光环当前的状态(粗或细）
	public int flag;
	//按钮
	public GameObject my_button;
	 

	// Use this for initialization
	void Start () {
		// 初始化粒子数组  
		particlesArray = new ParticleSystem.Particle[count];  
		particleAttr = new particleClass[count];  

		// 初始化粒子系统  
		particleSystem = this.GetComponent<ParticleSystem>();  
		particleSystem.startSpeed = 0;            // 粒子位置由程序控制  
		particleSystem.startSize = size;          // 设置粒子大小  
		particleSystem.loop = false;  
		particleSystem.maxParticles = count;      // 设置最大粒子量  
		particleSystem.Emit(count);               // 发射粒子  
		particleSystem.GetParticles(particlesArray);  

		// 初始化梯度颜色控制器  
		GradientAlphaKey[] alphaKeys = new GradientAlphaKey[5];  
		alphaKeys[0].time = 0.0f; alphaKeys[0].alpha = 1.0f;  
		alphaKeys[1].time = 0.4f; alphaKeys[1].alpha = 0.4f;  
		alphaKeys[2].time = 0.6f; alphaKeys[2].alpha = 1.0f;  
		alphaKeys[3].time = 0.9f; alphaKeys[3].alpha = 0.4f;  
		alphaKeys[4].time = 1.0f; alphaKeys[4].alpha = 0.9f;

		//设置色彩梯度，首位最好为同一种颜色，否则会出现断层
		//可以通过赠加数组的长度来增加颜色的种类
		GradientColorKey[] colorKeys = new GradientColorKey[2];  
		colorKeys[0].time = 0.0f; colorKeys[0].color = Color.yellow;
		colorKeys[1].time = 1.0f; colorKeys[1].color = Color.yellow;  
		colorGradient.SetKeys(colorKeys, alphaKeys);  

		initialization();   // 初始化各粒子位置   

		//建立监听器，改变通过改变粒子的最大值和最小值来使光环变粗或变细
		my_button.GetComponent<Button> ().onClick.AddListener (() => {
			flag = (flag == -1) ? 0 : 1 - flag;
			if (flag == 0) {
				//粗
				minRadius = 5.0f;   
				maxRadius = 8.0f;
			} else if (flag == 1) {
				//细
				minRadius = 6.0f;   
				maxRadius = 7.0f;
			}
			initialization ();
		});
	}


	// Update is called once per frame
	void Update () {
		for (int i = 0; i < count; i++)  
		{  
			if (rotate)  // 顺时针旋转  
				particleAttr[i].angle -= (i % tier + 1) * (speed / particleAttr[i].radius / tier);  
			else            // 逆时针旋转  
				particleAttr[i].angle += (i % tier + 1) * (speed / particleAttr[i].radius / tier);  

			// 保证angle在0~360度  
			particleAttr[i].angle = (360.0f + particleAttr[i].angle) % 360.0f;  
			float theta = particleAttr[i].angle / 180 * Mathf.PI; 

			/*if (flag == 0) {
				minRadius = 5.0f;   
				maxRadius = 8.0f;
			} else if (flag == 1) {
				minRadius = 6.0f;   
				maxRadius = 7.0f;
			}*/

			particlesArray[i].position = new Vector3(particleAttr[i].radius * Mathf.Cos(theta), 0f, particleAttr[i].radius * Mathf.Sin(theta));  
			// 粒子在半径方向上游离  
			particleAttr[i].time += Time.deltaTime;  
			particleAttr[i].radius += Mathf.PingPong(particleAttr[i].time / minRadius / maxRadius, pingPong) - pingPong / 2.0f;

			particlesArray[i].color = colorGradient.Evaluate(particleAttr[i].angle / 360.0f);
		}  
			 

		particleSystem.SetParticles(particlesArray, particlesArray.Length);  
	}


	//不可用，一个系统的OnGUI会被另一个系统的覆盖
	/*void OnGUI()
	{
		if (GUI.Button(new Rect(0, 10, 100, 30), "变粗/变细"))
		{
			//flag = (flag == -1)? 0: 1 - flag;
		}
	}*/



	void initialization()  
	{  
		for (int i = 0; i < count; ++i)  
		{   // 随机每个粒子距离中心的半径，同时希望粒子集中在平均半径附近  
			float midRadius = (maxRadius + minRadius) / 2;  
			float minRate = Random.Range(1.0f, midRadius / minRadius);  
			float maxRate = Random.Range(midRadius / maxRadius, 1.0f);  
			float radius = Random.Range(minRadius * minRate, maxRadius * maxRate);  

			// 随机每个粒子的角度  
			float angle = Random.Range(0.0f, 360.0f);  
			float theta = angle / 180 * Mathf.PI;  

			// 随机每个粒子的游离起始时间  
			float time = Random.Range(0.0f, 360.0f);  

			particleAttr[i] = new particleClass(radius, angle, time);  

			particlesArray[i].position = new Vector3(particleAttr[i].radius * Mathf.Cos(theta), 0f, particleAttr[i].radius * Mathf.Sin(theta));  
		}  

		particleSystem.SetParticles(particlesArray, particlesArray.Length);  
	}  
		

}

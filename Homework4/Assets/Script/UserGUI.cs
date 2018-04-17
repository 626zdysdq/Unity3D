using UnityEngine;
using UnityEngine.UI;
using System.Collections;


public class UserGUI : MonoBehaviour
{
	GUIStyle text_style = new GUIStyle();  //Label的样式

	private UIInterface gameInterface;     //实现用户界面和对象数据之间的接口
	private SceneController scene;         //引入场景控制器，实现数据传递

	public GameObject bullet;            //射击的子弹对象
	public ParticleSystem explosion;    //粒子效果
	public float fireRate = 1.25f;     //控制射击的时间间隔
	public float speed = 1000;        //发射出的射线的速度，设置为很大，可以忽略从发射到到达的时间
	public bool isGameOver = false;    //标记游戏是否结束
	private float nextFireTime;        //此次射击的时间


	public Text chanceText;     //剩余的生命值，当生命值变为零时游戏结束
	public Text scoreText;      //显示分数
	public Text roundText;      //显示关卡数
	private int round; 


	//初始化实例所需的游戏实例
	void Start()
	{
		bullet = GameObject.Instantiate(bullet) as GameObject;
		explosion = GameObject.Instantiate(explosion) as ParticleSystem;
		gameInterface = SceneController.getInstance() as UIInterface;
	}

	void OnGUI()
	{
		//实现显示结果的label样式
		text_style.fontSize = 30;
		text_style.normal.textColor = Color.black;

		//判断游戏是否结束，如果结束终止飞碟发射，显示得分，和重新开始的按钮
		if (isGameOver) {
			GUI.Label(new Rect(Screen.width / 2 - 150, Screen.width / 2 - 300, 400, 100), "恭喜你最后得分为：" + gameInterface.getScore ().ToString (), text_style);

			GUI.backgroundColor = Color.blue;
			GUI.skin.button.fontSize = 30;
			GUI.skin.button.fontStyle = FontStyle.Normal;
			if (GUI.Button (new Rect (Screen.width / 2 - 100, Screen.width / 2 - 200, 160, 80), "重新开始")) {
				Restart ();
			}
		}
		else
		{
			//如果没有结束进行射击事件的处理和射击结果的判定
			if (!gameInterface.isCounting ()) {

				gameInterface.MakeEmissionDiskable ();

				//控制每次射击之间的时间间隔，
				if (gameInterface.isShooting () && Input.GetMouseButtonDown (0) && Time.time > nextFireTime) {
					nextFireTime = Time.time + fireRate;

					//射击射线处理
					Ray ray = Camera.main.ScreenPointToRay (Input.mousePosition);
					bullet.GetComponent<Rigidbody> ().velocity = Vector3.zero;                   
					bullet.transform.position = transform.position;
					bullet.GetComponent<Rigidbody> ().AddForce (ray.direction * speed, ForceMode.Impulse);

					//击中时间的处理
					RaycastHit hit;
					if (Physics.Raycast (ray, out hit) && hit.collider.gameObject.tag == "Disk") {

						explosion.transform.position = hit.collider.gameObject.transform.position;
						explosion.GetComponent<Renderer> ().material.color = hit.collider.gameObject.GetComponent<Renderer> ().material.color;
						explosion.Play ();

						hit.collider.gameObject.SetActive (false);
					}
				}
			}

			//显示得分，关卡，生命值等信息
			roundText.text = "Round: " + gameInterface.getRound ().ToString ();
			scoreText.text = "Score: " + gameInterface.getScore ().ToString ();
			chanceText.text = "Chance: " + gameInterface.getChance ().ToString ();

			//判断是否到下一关
			if (round != gameInterface.getRound ()) {
				round = gameInterface.getRound ();
			}
		} 
	}

	//点击重新开始后重置信息
	void Restart(){
		scene.setScore (0);
		scene.setRound(0);
		scene.nextRound();
		scene.setChance (5);
		isGameOver = false;
	}

	public void Awake()
	{
		scene = SceneController.getInstance();
		scene.setUserInterface(this);
	}


	public void gameOver()
	{
		isGameOver = true;
	}
		
}
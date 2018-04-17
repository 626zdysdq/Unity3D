using UnityEngine;
using System.Collections;

//分数记录
public class ScoreRecorder : MonoBehaviour
{
	//每个飞碟代表的分数
	private int oneDiskScore = 5;

	private SceneController scene;

	void Awake()
	{
		scene = SceneController.getInstance();
		scene.setScoreRecorder(this);
	}

	//如果击中则加上相应的分数
	public void hitDisk()
	{
		scene.setScore(scene.getScore() + oneDiskScore);
	}

	//如果没有击中则扣掉一个生命值
	public void hitGround()
	{
		scene.setChance(scene.getChance() - 1);
	}
}
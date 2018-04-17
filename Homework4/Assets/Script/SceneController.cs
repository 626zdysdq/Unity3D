using UnityEngine;
using System.Collections;

//场景控制器（考虑在当前场景下的分数，生命值，关卡信息） 
public class SceneController : System.Object, UIInterface
{
	private static SceneController _instance;
	private RoundController _roundController;
	private FirstController _firstController;
	private ScoreRecorder recorder;
	private UserGUI _userInterface;

	private int _round;
	private int _score=0;
	private int _chance=5;
	private int _trial;
	public bool isGameOver;     //标记游戏是否结束


	//实现各参数设定的设定的获取函数
	//单例模式
	public static SceneController getInstance()
	{
		if (_instance == null)
		{
			_instance = new SceneController();
		}
		return _instance;
	}

	public void setFirstController(FirstController input)
	{
		_firstController = input;
	}
	internal FirstController getFirstController()
	{
		return _firstController;
	}

	public void setScoreRecorder(ScoreRecorder input)
	{
		recorder = input;
	}
	internal ScoreRecorder getScoreController()
	{
		return recorder;
	}
	internal UserGUI getUserInterface()
	{
		return _userInterface;
	}
	public void setUserInterface(UserGUI input)
	{
		_userInterface = input;
	}
	public void setRoundController(RoundController input)
	{
		_roundController = input;
	}
	internal RoundController getRoundController()
	{
		return _roundController;
	}


	public void MakeEmissionDiskable()
	{
		_firstController.MakeEmissionDiskable();
	}


	public bool isCounting()
	{
		return _firstController.getIsCounting();
	}
	public bool isShooting()
	{
		return _firstController.getIsShooting();
	}
	public int getRound()
	{
		return _round;
	}
	public void setRound(int input)
	{
		_round = input;
	}
	public int getScore()
	{
		return _score;
	}
	public int getTimeToNextEmissionTime()
	{
		return (int)_firstController.timeToNextEmission + 1;
	}
	public void setScore(int input)
	{
		_score = input;
	}
	public void nextRound()
	{
		_roundController.loadRoundData(++_round);
	}
	public int getTrial()
	{
		return _roundController.trial;
	}
	public void setTrial(int input)
	{
		_roundController.trial = input;
	}
	public int getChance()
	{
		return _chance;
	}
	public void setChance(int input)
	{
		_chance = input;
	}
	public void gameOver()
	{
		isGameOver = true;
		_userInterface.gameOver();
	}
}

//UI界面于数据之间的接口
public interface UIInterface
{
	void MakeEmissionDiskable();
	bool isCounting();
	bool isShooting();
	int getRound();
	int getScore();
	int getTimeToNextEmissionTime();
	void nextRound();
	void setScore(int point);
	void setRound(int input);
	int getTrial();
	void setTrial(int input);
	int getChance();
	void setChance(int input);
	void gameOver();
}
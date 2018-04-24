using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class UserGUI : MonoBehaviour
{
    private IUserAction action;
	int round = 1;                 //回合数
    public int life = 6;                   //血量
    //每个GUI的style
    GUIStyle score_style = new GUIStyle();
    GUIStyle text_style = new GUIStyle();
    private int high_score = 0;            //最高分
    private bool game_start = false;       //游戏开始


    void Start ()
    {
        action = SSDirector.GetInstance().CurrentScenceController as IUserAction;
		action.BeginGame();
    }
	
	void OnGUI ()
    {
        text_style.normal.textColor = new Color(0,0,0, 1);
        text_style.fontSize = 30;
        score_style.normal.textColor = new Color(1,0,1,1);
        score_style.fontSize = 30;

		if (action.GetScore () >= 25) {
			round = 3;
		} else if (action.GetScore () >= 10) {
			round = 2;
		} else
			round = 1;



        //用户射击
        if (Input.GetButtonDown("Fire1"))
        {
            Vector3 pos = Input.mousePosition;
            action.Hit(pos);
        }

	    GUI.Label(new Rect(10, 5, 200, 50), "Score: " + action.GetScore().ToString(), text_style);
		GUI.Label(new Rect(10, 40, 200, 50), "Round: " + round.ToString(), text_style);
		GUI.Label(new Rect(10, 75, 200, 50), "Chance: " + life.ToString(), text_style);
        //游戏结束
        if (life == 0)
        {
            high_score = high_score > action.GetScore() ? high_score : action.GetScore();
			GUI.Label(new Rect(Screen.width / 2 - 75, Screen.width / 2 - 325, 150, 50), "游戏结束", text_style);
			GUI.Label(new Rect(Screen.width / 2 - 75, Screen.width / 2 - 250, 50, 50), "最高分: " + high_score.ToString(), text_style);
            if (GUI.Button(new Rect(Screen.width / 2 - 75, Screen.width / 2 - 200, 100, 50), "重新开始"))
            {
                life = 6;
                action.ReStart();
                return;
            }
            action.GameOver();
        }        
    }

    public void ReduceBlood()
    {
        if(life > 0)
            life--;
    }
}

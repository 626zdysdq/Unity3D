using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using MyGame_object;

public class Game_result : MonoBehaviour {
	private UserAction action;
	public int status = 0;
	public int intro = 0;
	GUIStyle style;
	GUIStyle style1;
	GUIStyle buttonStyle;

	void Start() {
		action = Director.getInstance ().currentSceneController as UserAction;

		style = new GUIStyle();
		style1 = new GUIStyle ();
		style.fontSize = 40;
		style.alignment = TextAnchor.MiddleCenter;
		style1.fontSize = 20;
		style1.alignment = TextAnchor.MiddleCenter;

		buttonStyle = new GUIStyle("button");
		buttonStyle.fontSize = 30;
	}
	void OnGUI() {
		//点击后显示区分魔鬼与牧师，再点击后消失
		if(GUI.Button(new Rect(5,15,140, 50),"Introduce", buttonStyle)){
			intro = (intro+1) % 2;
		}
		if(intro==1)
			GUI.Label(new Rect(10,75,400, 70),"cube represent priest, sphere represent devil!",style1);

		//检查后显示游戏结果，判断胜负
		if (status == 1) {
			GUI.Label(new Rect(Screen.width/2-50, Screen.height/2-85, 100, 50), "Gameover!", style);
			if (GUI.Button(new Rect(Screen.width/2-70, Screen.height/2, 140, 70), "Restart", buttonStyle)) {
				status = 0;
				action.restart ();
			}
		} 
		else if(status == 2) {
			GUI.Label(new Rect(Screen.width/2-50, Screen.height/2-85, 100, 50), "You win!", style);
			if (GUI.Button(new Rect(Screen.width/2-70, Screen.height/2, 140, 70), "Restart", buttonStyle)) {
				status = 0;
				action.restart ();
			}
		}
	}
}

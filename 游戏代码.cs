using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class IMGUI : MonoBehaviour {

	private int player = 1;
	private int[,] state = new int[3, 3];
	private string reminder =  "";
	private string[] label = { "", "O", "X" }; 
	private int count=0;

	// Use this for initialization
	void Start () {
		reset ();
	}

	void reset() {
		player = 1;
		count = 0;
		for (int i = 0; i < 3; i++) {
			for (int j = 0; j < 3; j++) {
				state [i, j] = 0;
			}
		}
	}

	void OnGUI(){
		GUIStyle fontStyle = new GUIStyle ();
		GUIStyle fontStyle1 = new GUIStyle ();
		fontStyle.fontSize = 30;
		fontStyle1.fontSize = 20;
		GUI.Label (new Rect (300, 50, 100, 100), "Welcome to Game", fontStyle);
		if(GUI.Button(new Rect(300, 380, 100, 50),"start"))  {  
			reset();  
		} 

		//show result
		int result = winCheck();
		if (count == 0 && result == 0) {
			reminder = "";
		}
		else if (result != 0 && result != 3) {
			reminder = "player " + result + " win!";
		}
		else if(result == 3){
			reminder = "  Dogfall!   " ;
		}
		else{
			reminder = " Continuing! ";
		}
		GUI.Label (new Rect (350, 84, 100, 50), reminder , fontStyle);

		//The part of button
		for(int i = 0; i< 3; i++)
		{
			for(int j = 0; j < 3; j++)
			{
				if (state[i, j] == 1)  
				{  
					GUI.Button(new Rect(300+i * 80, 120+j * 80, 80, 80), label[1]);  
				}  
				if (state[i, j] == 2)  
				{  
					GUI.Button(new Rect(300+i * 80, 120+j * 80, 80, 80), label[2]);  
				}  
				if(GUI.Button(new Rect(300+i * 80, 120+j * 80, 80, 80), "")) {  
					if (result == 0)  
					{  
						if (player == 1) state[i, j] = 1;  
						else state[i, j] = 2;  
						count++;  
					    player = 3-player; 
					}  
				}  
			}
		}
	}

	private int winCheck()  {  
		for(int i = 0; i < 3; ++i)  {  
			if (state[i, 0] != 0 && state[i,0]==state[i, 1] && state[i, 1] == state[i, 2])  {  
				return state[i, 0];  
			}  
		}  
		for (int i = 0; i < 3; ++i)  {  
			if (state[0, i] != 0 && state[0, i] == state[1, i] && state[1, i] == state[2, i])  {  
				return state[0, i];  
			}  
		}  
		if((state[1,1]!=0 && state[0,0]==state[1,1] && state[1,1]==state[2,2])||  
		   (state[1,1]!=0 && state[0,2]==state[1,1]&& state[1,1]==state[2,0]) )  {  
				return state[1, 1];  
			}  
		if (count == 9) return 3;  
		return 0;  
	}  	

}

using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using MyGame_object;

public class Controller : MonoBehaviour, SceneController, UserAction {

	readonly Vector3 water_pos = new Vector3(0,0.5F,0);


    Game_result userGUI;

	public CoastController fromCoast;
	public CoastController toCoast;
	public BoatController boat;
	private MyCharacterController[] characters;

	//创建场景
	void Awake() {
		Director director = Director.getInstance ();
		director.currentSceneController = this;
		userGUI = gameObject.AddComponent <Game_result>() as Game_result;
		characters = new MyCharacterController[6];
		loadResources ();
	}

	//创建水和两边的河岸
	public void loadResources() {
		GameObject water = Instantiate (Resources.Load ("Perfabs/Water", typeof(GameObject)), water_pos, Quaternion.identity, null) as GameObject;
		water.name = "water";

		fromCoast = new CoastController ("from");
		toCoast = new CoastController ("to");
		boat = new BoatController ();

		loadCharacter ();
	}

	//创建牧师和恶魔的对象
	private void loadCharacter() {
		//创建恶魔
		for (int i = 0; i < 3; i++) {
			MyCharacterController cha = new MyCharacterController ("devil");
			cha.setName("priest" + i);
			cha.setPosition (fromCoast.getEmptyPosition ());
			cha.getOnCoast (fromCoast);
			fromCoast.getOnCoast (cha);

			characters [i] = cha;
		}
		//创建牧师
		for (int i = 0; i < 3; i++) {
			MyCharacterController cha = new MyCharacterController ("priest");
			cha.setName("devil" + i);
			cha.setPosition (fromCoast.getEmptyPosition ());
			cha.getOnCoast (fromCoast);
			fromCoast.getOnCoast (cha);

			characters [i+3] = cha;
		}
	}


	public void moveBoat() {   //如果船为空不移动，每次船移动后进行游戏结果的判断
		if (boat.isEmpty ())
			return;
		boat.Move ();
		userGUI.status = check_game_over ();
	}


	public void characterIsClicked(MyCharacterController characterCtrl) {
		if (characterCtrl.isOnBoat ()) {
			CoastController whichCoast;
			if (boat.get_to_or_from () == -1) { // to->-1; from->1
				whichCoast = toCoast;
			} else {
				whichCoast = fromCoast;
			}

			boat.GetOffBoat (characterCtrl.getName());
			characterCtrl.moveToPosition (whichCoast.getEmptyPosition ());
			characterCtrl.getOnCoast (whichCoast);
			whichCoast.getOnCoast (characterCtrl);

		} 
		else {									// character on coast
			CoastController whichCoast = characterCtrl.getCoastController ();

			if (boat.getEmptyIndex () == -1) {		// boat is full
				return;
			}

			if (whichCoast.get_to_or_from () != boat.get_to_or_from ())	// boat is not on the side of character
				return;

			whichCoast.find_object(characterCtrl.getName());
			characterCtrl.moveToPosition (boat.getEmptyPosition());
			characterCtrl.getOnBoat (boat);
			boat.GetOnBoat (characterCtrl);
		}
		userGUI.status = check_game_over ();
	}

	//判断游戏结果
	int check_game_over() {	// 0->not finish, 1->lose, 2->win
		int from_priest = 0;
		int from_devil = 0;
		int to_priest = 0;
		int to_devil = 0;

		int[] fromCount = fromCoast.getCharacterNum ();
		from_priest += fromCount[0];
		from_devil += fromCount[1];

		int[] toCount = toCoast.getCharacterNum ();
		to_priest += toCount[0];
		to_devil += toCount[1];

		//获胜
		if (to_priest + to_devil == 6)		
			return 2;

		//将船上的牧师或恶魔加入之前的计数器
		int[] boatCount = boat.getCharacterNum ();
		if (boat.get_to_or_from () == -1) {	// boat at toCoast
			to_priest += boatCount[0];
			to_devil += boatCount[1];
		} 
		else {	
			from_priest += boatCount[0];
			from_devil += boatCount[1];
		}

		//失败
		if (from_priest < from_devil && from_priest > 0) {		
			return 1;
		}
		if (to_priest < to_devil && to_priest > 0) {
			return 1;
		}

		//无影响
		return 0;			
	}

	public void restart() {
		boat.reset ();
		fromCoast.reset ();
		toCoast.reset ();
		for (int i = 0; i < characters.Length; i++) {
			characters [i].reset ();
		}
	}
}

using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using MyGame_object;

public class ClickGUI : MonoBehaviour {
	UserAction action;
	MyCharacterController characterController;

	public void setController(MyCharacterController characterCtrl) {
		characterController = characterCtrl;
	}

	void Start() {
		action = Director.getInstance ().currentSceneController as UserAction;
	}

	void OnMouseDown() {         //区分鼠标点击船时发生的事件和点击牧师等对象时发生的事件
		if (gameObject.name == "boat") {
			action.moveBoat ();
		} else {
			action.characterIsClicked (characterController);
		}
	}
}

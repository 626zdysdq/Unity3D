using UnityEngine;
using System.Collections;


public class UserGUI : MonoBehaviour
{
    GameSceneController scene;
    QueryGameStatus state;
    UserActions action;

    float width, height;
    float castw(float scale) { return (Screen.width - width) / scale; }
    float casth(float scale) { return (Screen.height - height) / scale; }

    void Start()
    {
        scene = GameSceneController.GetInstance();
        state = GameSceneController.GetInstance() as QueryGameStatus;
        action = GameSceneController.GetInstance() as UserActions;
    }

    void OnGUI()
    {
        GUI.skin.button.fontSize = GUI.skin.textArea.fontSize = 20;

        if (state.getMessage() != "")
        {
			if (GUI.Button(new Rect(350, 220, 100, 50), state.getMessage())) action.restart();
        }
        else
        {
            if (GUI.RepeatButton(new Rect(10, 10, 100, 40), "Tip")) GUI.TextArea(new Rect(10, 60, 330, 50), scene.gameRule);
			if (GUI.Button(new Rect(350, 220 , 100 , 50 ), "Next")) StartCoroutine(action.nextmove());
        }
    }
}
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using MyGame_object;

public class ActionManager : SSActionManager
{
	public void moveBoat(BoatController boat)
	{
		MoveToAction action = MoveToAction.getAction(boat.getDestination(), boat.movingSpeed);
		this.addAction(boat.getGameobj(), action, this);
	}

	public void moveCharacter(MyCharacterController characterCtrl, Vector3 destination)
	{
		Vector3 currentPos = characterCtrl.getPosition();
		Vector3 middlePos = currentPos;
		if (destination.y > currentPos.y)
		{
			middlePos.y = destination.y;
		}
		else
		{
			middlePos.x = destination.x;
		}
		SSAction action1 = MoveToAction.getAction(middlePos, characterCtrl.movingSpeed);
		SSAction action2 = MoveToAction.getAction(destination, characterCtrl.movingSpeed);
		SSAction seqAction = SequenceAction.getAction(1, 0, new List<SSAction> { action1, action2 });
		this.addAction(characterCtrl.getGameobj(), seqAction, this);
	}
}

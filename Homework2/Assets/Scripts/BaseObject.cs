using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using MyGame_object;

namespace MyGame_object {
	
	//
	//控制物体移动（包括物体上下船的移动和在船上的平行移动）
	public class Moveable: MonoBehaviour {

		int moving_status;	// 0->not moving, 1->moving to middle, 2->moving to dest
		Vector3 dest;
		Vector3 middle;
		readonly float move_speed = 20;


		//判断移动的方式
		public void setDestination(Vector3 _dest) {
			dest = _dest;    //移动的终点
			middle = _dest;  //移动过程中的中间位置
			if (_dest.y == transform.position.y) {	
				moving_status = 2;
			}
			else if (_dest.y < transform.position.y) {	//上船
				middle.y = transform.position.y;
			} 
			else {								       //上岸
				middle.x = transform.position.x;
			}
			moving_status = 1;
		}

        //根据我们所判断的状态进行物体位置的移动
        void Update()
        {
            if (moving_status == 1)  //移到中间位置
            {
                transform.position = Vector3.MoveTowards(transform.position, middle , move_speed * Time.deltaTime);
                if (transform.position == middle)
                {
                    moving_status = 2;
                }
            }
            else if (moving_status == 2)  //从中间位置移到终点
            {
                transform.position = Vector3.MoveTowards(transform.position, dest, move_speed * Time.deltaTime);
                if (transform.position == dest)
                {
                    moving_status = 0;
                }
            }
        }

        public void reset() {
			moving_status = 0;
		}
	}


	//
    //对河的两岸进行创建和管理
    public class CoastController
    {
        readonly GameObject coast;
        readonly Vector3 from_pos = new Vector3(8, 1, 0);
        readonly Vector3 to_pos = new Vector3(-8, 1, 0);
        readonly Vector3[] positions;
        readonly int to_or_from;    // to->-1, from->1

        // 储存天使于恶魔的对象
        MyCharacterController[] person_number;

        public CoastController(string move_way)  //创建河岸，包括分配河岸上object的位置
        {
			person_number = new MyCharacterController[6];
            positions = new Vector3[] {new Vector3(5.5F,2.25F,0), new Vector3(6.5F,2.25F,0), new Vector3(7.5F,2.25F,0),
                new Vector3(8.5F,2.25F,0), new Vector3(9.5F,2.25F,0), new Vector3(10.5F,2.25F,0)};

            if (move_way == "from")
            {
                coast = Object.Instantiate(Resources.Load("Perfabs/Stone", typeof(GameObject)), from_pos, Quaternion.identity, null) as GameObject;
                coast.name = "from";
                to_or_from = 1;
            }
            else
            {
                coast = Object.Instantiate(Resources.Load("Perfabs/Stone", typeof(GameObject)), to_pos, Quaternion.identity, null) as GameObject;
                coast.name = "to";
                to_or_from = -1;
            }
        }

        public int getEmptyIndex()   //获取某一边河岸上的空位位置
        {
			for (int i = 0; i < person_number.Length; i++)
            {
				if (person_number[i] == null)
                {
                    return i;
                }
            }
            return -1;
        }

        public Vector3 getEmptyPosition()  //根据所得到的空位的位置获得坐标
        {
            Vector3 pos = positions[getEmptyIndex()];
			if (to_or_from == 1)
				return pos;
			else {
				pos.x = -pos.x;           //根据对称性，区分两边河岸的坐标
				return pos;
			}
        }

        public void getOnCoast(MyCharacterController characterCtrl)  //在河岸上放置牧师或魔鬼
        {
            int index = getEmptyIndex();
			person_number[index] = characterCtrl;
        }

        public MyCharacterController find_object(string passenger_name) //在河岸上寻找是否有我们选中的对象，若没有则返回null。
        {   // 0->priest, 1->devil
			for (int i = 0; i < person_number.Length; i++)
            {
				if (person_number[i] != null && person_number[i].getName() == passenger_name)
                {
					MyCharacterController charactorCtrl = person_number[i];
					person_number[i] = null;
                    return charactorCtrl;
                }
            }
            Debug.Log("can‘t find passenger on coast: " + passenger_name);
            return null;
        }

        public int get_to_or_from() //返回河岸的属性
        {
            return to_or_from;
        }

		public int[] getCharacterNum() //确定河岸上牧师和魔鬼的个数，并返回一个数组（0->priest, 1->devil）
        {
            int[] count = { 0, 0 };
			for (int i = 0; i < person_number.Length; i++)
            {
				if (person_number[i] == null)
                    continue;
				if (person_number[i].getType() == 0)
                {   // 0->priest, 1->devil
                    count[0]++;
                }
                else
                {
                    count[1]++;
                }
            }
            return count;
        }

        public void reset()     
        {
			person_number = new MyCharacterController[6];
        }
    }

	//
    //对恶魔或者牧师对象的创建和管理
    public class MyCharacterController {
		readonly GameObject character;
		readonly Moveable move;
		readonly ClickGUI clickGUI;
		readonly int characterType;	    //（0->priest, 1->devil）

		bool _isOnBoat;
		CoastController coastController;


		public MyCharacterController(string which_character) {   //根据所需要的类型创建牧师或者魔鬼
			
			if (which_character == "priest") {
				character = Object.Instantiate (Resources.Load ("Perfabs/Priest", typeof(GameObject)), Vector3.zero, Quaternion.identity, null) as GameObject;
				characterType = 0;
			} else {
				character = Object.Instantiate (Resources.Load ("Perfabs/Devil", typeof(GameObject)), Vector3.zero, Quaternion.identity, null) as GameObject;
				characterType = 1;
			}
			move = character.AddComponent (typeof(Moveable)) as Moveable;

			clickGUI = character.AddComponent (typeof(ClickGUI)) as ClickGUI;
			clickGUI.setController (this);
		}

		 public void setName(string name) {
			character.name = name;
		}

		public void setPosition(Vector3 pos) {
			character.transform.position = pos;
		} 

		public void moveToPosition(Vector3 destination) {
			move.setDestination(destination);
		}

		public int getType() {	// 0->priest, 1->devil
			return characterType;
		}

		public string getName() {
			return character.name;
		}

		public void getOnBoat(BoatController boatCtrl) {            //变成船的子对象
			coastController = null;
			character.transform.parent = boatCtrl.getGameobj().transform;
			_isOnBoat = true;
		}

		public void getOnCoast(CoastController coastCtrl) {       //解除船和对象的父子关系
			coastController = coastCtrl;
			character.transform.parent = null;
			_isOnBoat = false;
		}

		public bool isOnBoat() {
			return _isOnBoat;
		}

		public CoastController getCoastController() {
			return coastController;
		}

		public void reset() {
			move.reset ();
			coastController = (Director.getInstance ().currentSceneController as Controller).fromCoast;
			getOnCoast (coastController);
			setPosition (coastController.getEmptyPosition ());
			coastController.getOnCoast (this);
		}
	}


	//
	//对船这个对象进行创建和管理
	public class BoatController {
		readonly GameObject boat;
		readonly Moveable moveableScript;
		readonly Vector3 fromPosition = new Vector3 (4, 1, 0);  //出发点
		readonly Vector3 toPosition = new Vector3 (-4, 1, 0);   //到达点
		readonly Vector3[] from_positions;
		readonly Vector3[] to_positions;


		int to_or_from; // to->-1; from->1
		MyCharacterController[] passenger = new MyCharacterController[2];

		public BoatController() {
			to_or_from = 1;

			from_positions = new Vector3[] { new Vector3 (3.5F, 1.5F, 0), new Vector3 (4.5F, 1.5F, 0) };
			to_positions = new Vector3[] { new Vector3 (-4.5F, 1.5F, 0), new Vector3 (-3.5F, 1.5F, 0) };

			boat = Object.Instantiate (Resources.Load ("Perfabs/Boat", typeof(GameObject)), fromPosition, Quaternion.identity, null) as GameObject;
			boat.name = "boat";

			moveableScript = boat.AddComponent (typeof(Moveable)) as Moveable;
			boat.AddComponent (typeof(ClickGUI));
		}


		public void Move() {
			if (to_or_from == -1) {
				moveableScript.setDestination(fromPosition);
				to_or_from = 1;
			} else {
				moveableScript.setDestination(toPosition);
				to_or_from = -1;
			}
		}

		public int getEmptyIndex() {
			for (int i = 0; i < passenger.Length; i++) {
				if (passenger [i] == null) {
					return i;
				}
			}
			return -1;
		}

		public bool isEmpty() {
			for (int i = 0; i < passenger.Length; i++) {
				if (passenger [i] != null) {
					return false;
				}
			}
			return true;
		}

		public Vector3 getEmptyPosition() {
			Vector3 pos;
			int emptyIndex = getEmptyIndex ();
			if (to_or_from == -1) {
				pos = to_positions[emptyIndex];
			} else {
				pos = from_positions[emptyIndex];
			}
			return pos;
		}

		public void GetOnBoat(MyCharacterController characterCtrl) {
			int index = getEmptyIndex ();
			passenger [index] = characterCtrl;
		}

		public MyCharacterController GetOffBoat(string passenger_name) {
			for (int i = 0; i < passenger.Length; i++) {
				if (passenger [i] != null && passenger [i].getName () == passenger_name) {
					MyCharacterController charactorCtrl = passenger [i];
					passenger [i] = null;
					return charactorCtrl;
				}
			}
			Debug.Log ("Cant find passenger in boat: " + passenger_name);
			return null;
		}

		public GameObject getGameobj() {
			return boat;
		}

		public int get_to_or_from() { // to->-1; from->1
			return to_or_from;
		}

		public int[] getCharacterNum() {
			int[] count = {0, 0};
			for (int i = 0; i < passenger.Length; i++) {
				if (passenger [i] == null)
					continue;
				if (passenger [i].getType () == 0) {	// 0->priest, 1->devil
					count[0]++;
				} else {
					count[1]++;
				}
			}
			return count;
		}

		public void reset() {
			moveableScript.reset ();
			if (to_or_from == -1) {
				Move ();
			}
			passenger = new MyCharacterController[2];
		}
	}


   //
   //单例模式的场景对象
	public class Director : System.Object {
		private static Director _instance;
		public SceneController currentSceneController { get; set; }

		public static Director getInstance() {
			if (_instance == null) {
				_instance = new Director ();
			}
			return _instance;
		}
	}

	public interface SceneController {
		void loadResources ();
	}

	public interface UserAction {
		void moveBoat();
		void characterIsClicked(MyCharacterController characterCtrl);
		void restart();
	}

}
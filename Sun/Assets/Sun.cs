using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Sun : MonoBehaviour {

	public Transform origin;  
	public float speed ;  
	float ry, rz;  
	// Use this for initialization  
	void Start () {  
		ry = Random.Range(10, 60);  
		rz = Random.Range(10, 60); 
		origin.position = new Vector3 (-7, 0, 0);
	}  

	// Update is called once per frame  
	void Update () {  
		Vector3 axis = new Vector3(0, ry, rz);  
		this.transform.RotateAround(origin.position, axis , speed * Time.deltaTime);  
		this.transform.Rotate (Vector3.up * 50 * Time.deltaTime);
	}  
}

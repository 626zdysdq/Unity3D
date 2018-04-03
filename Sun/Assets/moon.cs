using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class moon : MonoBehaviour {

	public Transform origin;  
	public float speed ;
	// Use this for initialization
	void Start () {
		
	}
	
	// Update is called once per frame
	void Update () {
		this.transform.RotateAround(origin.position, Vector3.left , speed * Time.deltaTime); 
		this.transform.Rotate (Vector3.up * 50 * Time.deltaTime);
	}
}

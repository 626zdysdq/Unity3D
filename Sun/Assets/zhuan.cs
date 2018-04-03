using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class zhuan : MonoBehaviour {

	// Use this for initialization
	void Start () {
		
	}
	
	// Update is called once per frame
	void Update () {
		this.transform.Rotate (Vector3.up * 50 * Time.deltaTime);
	}
}

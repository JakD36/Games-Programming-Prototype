using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SpaceBehaviour : MonoBehaviour {

	public GameObject Card;
	public GameObject cube;
	// Use this for initialization
	void Start () {
		
	}
	
	// Update is called once per frame
	void Update () {
		
		if(Card && cube==null){
			cube = GameObject.CreatePrimitive(PrimitiveType.Cube);
 			cube.transform.position = this.transform.position;
		}
	}

	bool setCard(GameObject Card){
		if(Card = null){
			this.Card = Card;
			return true;
		}
		else{
			return false;
		}
	}
}

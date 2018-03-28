using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DeckBehaviour : MonoBehaviour {
	public GameObject Card;
	private int numberOfCards = 40;
	private List<GameObject> cards;
	// Use this for initialization
	void Start () {
		for(int i = 0; i < numberOfCards; i++){
			GameObject myCard = (GameObject)Instantiate<GameObject>(Card);
			CardBehaviour script = (CardBehaviour)myCard.GetComponent("CardBehaviour");
			script.value = Random.Range(0,5);
//			cards.Add(myCard);
		}
		
	}
	
	// Update is called once per frame
	void Update () {
		
	}
}

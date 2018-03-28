using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DeckBehaviour : MonoBehaviour {
	public GameObject Card;
	private int numberOfCards = 40;
	public List<GameObject> cards = new List<GameObject> {};
	// Use this for initialization
	void Start () {
		
		for(int i = 0; i < numberOfCards; i++){
			GameObject myCard = (GameObject)Instantiate<GameObject>(Card);
			CardBehaviour script = (CardBehaviour)myCard.GetComponent("CardBehaviour");
			script.value = Random.Range(0,5);
			cards.Add(myCard);
		}
	}
	void shuffle(){
		
	}
}

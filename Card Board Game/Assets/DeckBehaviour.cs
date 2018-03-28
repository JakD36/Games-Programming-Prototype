using System.Collections;
using System.Collections.Generic;
using UnityEngine;


[System.Serializable] // This makes the struct visible in the Inspector.
   public struct cardDetails {
       public cardType type;
	   [Range(-20,20)]
	   public int value;

	   [Range(0,100)]
       public int count;
   }


public class DeckBehaviour : MonoBehaviour {

	const int maxOfCard = 10;
	
	public List<cardDetails> Set_Card_Details = new List<cardDetails>{};
	
	public GameObject Card;
	
	private Queue<GameObject> cards = new Queue<GameObject> {};
	public List<GameObject> grave = new List<GameObject> {};
	// Use this for initialization
	
	void Awake() {
		foreach (var item in Set_Card_Details)
		{
			addCard(item);
		}
		shuffle();
		Debug.Log("Number of cards! >> "+cards.Count);
	}

	void addCard(cardDetails details){
		for(int i = 0; i < details.count;i++){
			
			GameObject newCard = (GameObject)Instantiate<GameObject>(Card);
			CardBehaviour script = (CardBehaviour)newCard.GetComponent("CardBehaviour");
			if(script){
				script.value = details.value;
				script.type = details.type;
				grave.Add(newCard);
			}
			else{
				Debug.Log("Got a null return on getting a cards behaviour script\nNot adding a new card");
			}
		}
	}
	


	void shuffle(){
		foreach (var item in cards){
			grave.Add(cards.Dequeue());
		}
		while(grave.Count>0){
			int rand = Random.Range(0,grave.Count);
			cards.Enqueue( grave[rand] );
			grave.RemoveAt(rand);
		}
	}

	public GameObject drawCard(){
		if (cards.Count > 0 ){
			return cards.Dequeue();
		}
		else{
			shuffle();
			return cards.Dequeue();
		}
	}
}

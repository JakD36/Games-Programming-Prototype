using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.EventSystems;


public class PlayerBehaviour : MonoBehaviour {

	DeckBehaviour deckScript;
	public GameObject otherPlayer ;
	public GameObject Card;
	GameObject selectedCard;
	public string name;
	public Vector3 offset;
	public bool pickSpace = false;
	public bool myTurn = false;
	public int onSpace = 0;
	const int handSize = 4;
	public GameObject[] myHand = new GameObject[handSize];
	// Use this for initialization
	void Start () {
		// Get the decks script for the players use.
		GameObject deck = GameObject.Find("Deck");
		deckScript = (DeckBehaviour)deck.GetComponent("DeckBehaviour");
		
		// Get the position from the space the player is on.
		GameObject newSpace = GameObject.Find("Space ("+this.onSpace+")");
		this.transform.position = newSpace.transform.position+this.offset ;
		
		PlayerBehaviour playerScript = (PlayerBehaviour)this.GetComponentInParent(typeof(PlayerBehaviour));
		
		if(deckScript){
			for(int n = 0; n < handSize; n++){
				myHand[n] = deckScript.drawCard();
			}
		}
		else{
			Debug.Log("Error: Drawing card at start for" + playerScript.name);
		}
		setHand();
	}
	
	

   	public void OnClicked(int btnN)
 	{
		 if(myTurn){
			GameObject myCard = myHand[btnN];
			CardBehaviour cardScript = (CardBehaviour)myCard.GetComponent("CardBehaviour");
			if(cardScript.type == cardType.basic){
				// Move the player based on the value
				this.movePlayer(cardScript.value);
				
				// Copy card in hand back into the deck
				deckScript.grave.Add(copyCard(myCard));
				Destroy(myCard);
				myHand[btnN] = deckScript.drawCard();
			}else if(cardScript.type == cardType.trap){
				pickSpace = true;
				selectedCard = copyCard(myCard);
				Destroy(myCard);
				myHand[btnN] = deckScript.drawCard();
			}
				
			
			for(int n = 0; n<4; n++){
				Button myButton = GameObject.Find("Button ("+n+")").GetComponent<Button>();		
				myButton.interactable = false;
				myButton.GetComponentInChildren<Text>().text = "No peeking!";
			}
			Button nextTurnButton = GameObject.Find("NextTurnButton").GetComponent<Button>();		
			nextTurnButton.interactable = true;
		}
 	}

	// Update is called once per frame
	public void setHand () {
		if(!myTurn){ // if it is not this players turn, make it
			myTurn = true;
			for(int n = 0; n<4; n++){
				// for each card in players hand, get its script
				GameObject myCard = myHand[n];
				CardBehaviour cardScript = (CardBehaviour)myCard.GetComponent("CardBehaviour");

				// Get the button relating to that card
				Button myButton = GameObject.Find("Button ("+n+")").GetComponent<Button>();
				// We need to build the string for the card 
				string myCardType;
				if(cardScript){
					if(cardScript.type == cardType.basic){
						myCardType = "Basic";
					}else if(cardScript.type == cardType.trap && cardScript.value<0){
						myCardType = "Trap";
					}
					else{
						myCardType = "Boost";
					}
					myButton.GetComponentInChildren<Text>().text = myCardType+" "+cardScript.value.ToString();
				}
				else{
					Debug.Log("cardScript is null");
				}
				myButton.interactable = true; // make sure we can press this button
			}
			// 
			Button nextTurnButton = GameObject.Find("NextTurnButton").GetComponent<Button>();
			nextTurnButton.GetComponentInChildren<Text>().text = "Next Turn";
			nextTurnButton.interactable = false;
		}else{
			myTurn = false;
		}
	}

	void Update(){
		// If we click on a space, and its this players turn and that we need to pick a space to place a card on
		if(Input.GetMouseButtonDown(0) && myTurn && pickSpace){
			RaycastHit hit; // Use a raycast to select a space
			SpaceBehaviour spaceScript; 
			Ray ray = Camera.main.ScreenPointToRay(Input.mousePosition);
			if (Physics.Raycast(ray, out hit,100))
				if(hit.transform!=null){
					// get the script for the space so we can place the card on
					if(spaceScript = (SpaceBehaviour)hit.transform.gameObject.GetComponent("SpaceBehaviour")){
						GameObject copiedCard = copyCard(selectedCard);
						spaceScript.Card = copiedCard;
						Destroy(selectedCard);
						pickSpace = false;
					}
				}
			}
	}
	public void movePlayer(int value){
		this.onSpace+=value;
		
		if(this.onSpace>35){
			//Game over!
			this.onSpace = 0;
			PlayerBehaviour otherScript = (PlayerBehaviour)otherPlayer.GetComponent("PlayerBehaviour");
			otherScript.onSpace = 0;
		}
		if(this.onSpace<0){
			this.onSpace = 0;
		}

		GameObject newSpace = GameObject.Find("Space ("+this.onSpace+")");
		this.transform.position = newSpace.transform.position+this.offset ;
		
		SpaceBehaviour spaceScript = (SpaceBehaviour)newSpace.GetComponent("SpaceBehaviour");
		
		if(spaceScript.Card){
			
			CardBehaviour cardScript = (CardBehaviour)spaceScript.Card.GetComponent("CardBehaviour");
			
			movePlayer(cardScript.value);
			
			GameObject copiedCard = copyCard(spaceScript.Card);
			
			deckScript.grave.Add(copiedCard);
			
			Destroy(spaceScript.Card);
			Destroy(spaceScript.cube);
		}
	}

	GameObject copyCard(GameObject original){
		GameObject copy = (GameObject)Instantiate<GameObject>(Card);
		CardBehaviour copiedScript = (CardBehaviour)copy.GetComponent("CardBehaviour");
		CardBehaviour originalScript = (CardBehaviour)original.GetComponent("CardBehaviour");
		copiedScript.value = originalScript.value;
		copiedScript.type = originalScript.type;
		return copy;
	}
}



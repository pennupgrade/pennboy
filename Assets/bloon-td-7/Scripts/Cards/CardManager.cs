using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;
using System.Diagnostics.Tracing;
using System.Linq;
using UnityEngine.UI;

public class CardManager : MonoBehaviour
{
    public PlacementSystem placementSystem;

     //unity can not even serialize int here, but it can serialize GameObject
    public int test;

    [SerializeField] private int HandSize;
    [SerializeField] private GameObject TowerCard;
    private Card[] hand;
    [SerializeField] private List<int> deck;
    [SerializeField] private Card[] CardTypes;
    [SerializeField] private GameObject[] towerObjects;

    private void Start()
    {
        hand = new Card[5];
        // generate hand
        generateHand();

        deck = new List<int>();

        generateDeck();
    }

    private void Awake()
    {
        test = 5;
        Debug.Log(test);
    }

    private void generateHand()
    {
        int counter = 0;
        for (int i = -750; i <= 750; i += 1500/(HandSize-1))
        {
            GameObject newcard = Instantiate(TowerCard, this.transform);
            newcard.name = "Card" + (counter + 1);
            RectTransform trans = newcard.GetComponent<RectTransform>();
            trans.anchoredPosition = new Vector3(i, 0, 0);
            Card c = newcard.GetComponent<TowerCard>();
            hand[counter] = c;
            int savedcounter = counter;
            counter++;

            Button button = newcard.GetComponent<Button>();
            button.onClick.AddListener(delegate { UseCard(savedcounter); });
        }
    }

    private void generateDeck()
    {
        // actually generate deck
        List<DeckPair> deckCopy = new List<DeckPair>(deckBreakupByIndex);
        while (deckCopy.Count > 0)
        {
            int index = UnityEngine.Random.Range(0, deckCopy.Count);
            deck.Add(deckCopy[index].cardId);
            Debug.Log("deck_list " + deck.Count);
            deckCopy[index].count--;
            if (deckCopy[index].count <= 0)
                deckCopy.RemoveAt(index);
        }

        // Generate hand
        for (int i = 0; i < hand.Length; i++)
        {
            int cardType = deck[0];
            deck.RemoveAt(0);
            hand[i].ResetCard(towerObjects[cardType],cardType);
        }
    }

    /// <summary>
    /// Removes the given card from the hand and generates a new card in its place.  
    /// </summary>
    /// <param name="card"></param>
    public void removeCardFromHand(Card card)
    {
        for (int i = 0; i < HandSize; i++)
        {
            if (hand[i] == card)
            {
                int newId = deck[0];
                deck.RemoveAt(0);
                deck.Add(card.id);
                hand[i].ResetCard(towerObjects[newId],newId);
                return;
            }
        }
    }

    /// <summary>
    /// Uses the card with number cardNumber from the current hand. If already in use, does nothing.
    /// </summary>
    /// <param name="cardNumber"></param>
    public void UseCard(int cardNumber)
    {
        hand[cardNumber].isUsing = hand[cardNumber].Use();
    }

    private class DeckPair
    {
        public int count;
        public int cardId;
        public DeckPair(int count, int cardId)
        {
            this.count = count;
            this.cardId = cardId;
        }

        public override string ToString()
        {
            return "CardId: " + cardId + "  Count: " + count;
        }
    }
    // Breakup of deck by index, where (3,0) represents 3 cards of id 0
    private static DeckPair[] deckBreakupByIndex =
    {
        new DeckPair (1, 0),
        new DeckPair (3, 1),
        new DeckPair (6, 2),
    };
}

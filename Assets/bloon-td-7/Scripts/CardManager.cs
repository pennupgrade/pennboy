using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CardManager : MonoBehaviour
{
    public List<Card> deck;

    private void Start()
    {
        deck = new List<Card>();
    }

    /// <summary>
    /// Uses the card with number cardNumber from the deck and puts it back at the bottom of the deck.
    /// </summary>
    /// <param name="cardNumber"></param>
    public void UseCard(int cardNumber)
    {
        Card card = deck[cardNumber];
        deck.Remove(card);
        deck.Add(card);
        
    }
}

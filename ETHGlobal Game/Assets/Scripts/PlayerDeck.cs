using System.Collections;
using System.Collections.Generic;
using UnityEngine;


public class PlayerDeck : MonoBehaviour
{
    public List<Card> DisabledCards = new List<Card>();

    public List<Card> ActiveCards = new List<Card>();

    public int maxDeckSize = 20;

    public PlayerDeck(){
        maxDeckSize = 20;
    }

    public void SpawnAllCards()
    {
        List<Card> HolderList = new List<Card>();
        foreach (Card card in DisabledCards)
        {
           GameObject go = Instantiate(card.gameObject);
            HolderList.Add(go.GetComponent<Card>());
        }

        DisabledCards = HolderList;
    }

    public Card GrabRandomCard()
    {
        if (DisabledCards.Count <= 0)
            return new Card();

        int index = Random.Range(0, DisabledCards.Count);
        Card PulledCard = DisabledCards[index];

        DisabledCards.RemoveAt(index);
        ActiveCards.Add(PulledCard);

        PulledCard.gameObject.SetActive(true);

        return PulledCard;
    }

    public int DeckCount()
    {
        return DisabledCards.Count;
    }

    public void DisableAll()
    {
        foreach (Card card in DisabledCards)
        {
            card.gameObject.SetActive(false);
        }
    }

    public void PutCardBack(Card _card)
    {
        ActiveCards.Remove(_card);
        DisabledCards.Add(_card);


    }
}

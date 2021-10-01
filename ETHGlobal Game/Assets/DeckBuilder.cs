using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DeckBuilder : MonoBehaviour
{
    public static DeckBuilder deckBuilder;

    public GameObject DeckBuilderUI;

    // free cards everyone can use
    public CardDataBase BaseSetCards;

    //nft  cards 
    //public CardDataBase NFTSetCards;

    public GameObject UI;


    public GameObject CardSlotParent;
    public GameObject CardSlotParentCurrentDeck;

    public List<DeckBuilderCardSlot> AvaibleSlots;
    public List<DeckBuilderCardSlot> CurrentDeckSlots;


    // Start is called before the first frame update
    void Start()
    {
        deckBuilder = this;
        SetSlots();
        LoadCards(BaseSetCards);
        ToggleDeckBuilderUI(false);
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    public void SetSlots()
    {
        foreach (Transform slot in CardSlotParent.transform)
        {
            DeckBuilderCardSlot Slot = slot.GetComponent<DeckBuilderCardSlot>();
            Slot.SetSlotType(SlotType.InventorySlot);
            Slot.SetAmountText();

            AvaibleSlots.Add(Slot);
        }

        foreach (Transform slot in CardSlotParentCurrentDeck.transform)
        {
            DeckBuilderCardSlot Slot = slot.GetComponent<DeckBuilderCardSlot>();
            Slot.SetSlotType(SlotType.DeckSlot);
            Slot.SetAmountText();

            CurrentDeckSlots.Add(Slot);
         
        }
    }

    public void LoadCards(CardDataBase data)
    {
      
        foreach (var item in data.cards)
        {
            GameObject card = Instantiate(item.gameObject);

            FindSlotForCardToInventory(card.GetComponent<Card>());
        }
    }

    public void ToggleDeckBuilderUI(bool toggle)
    {
        UI.SetActive(toggle);
    }

    // card koomt vanuit deck naar inventory toe
    public void FindSlotForCardToInventory(Card card)
    {
        // find the next empty spot
        for (int i = 0; i < AvaibleSlots.Count; i++)
        {
            if (AvaibleSlots[i].isSlotEmpty())
            {

                AvaibleSlots[i].SetSlot(card);
                return;
            }
        }

    }

    // card komt vanuit inventory naar deck toe
    public void FindSlotForCardToDeck(Card card)
    {
        for (int i = 0; i < CurrentDeckSlots.Count; i++)
        {
            if (CurrentDeckSlots[i].isSlotEmpty())
            {
                CurrentDeckSlots[i].SetSlot(card);
                return;
            }
        }
    }

    // players choose the cards before every dungeon run
    // saves the new created deck for this dungeon run
    public void SaveChanges()
    {
        // maak een check als we minimaal 5 cards hebben in de deck
        List<Card> AllCardsInDeck = new List<Card>();
        for (int i = 0; i < CurrentDeckSlots.Count; i++)
        {
            if (!CurrentDeckSlots[i].isSlotEmpty())
            {
                AllCardsInDeck.Add(CurrentDeckSlots[i].slottedCard);                 
            }
        }

        PlayerDeck newDeck = new PlayerDeck();
        newDeck.DisabledCards = AllCardsInDeck;

        CardCastManager.cardManager.LoadDeck(newDeck);

        ToggleDeckBuilderUI(false);

        DungeonManager.dungeonManager.StartGenerating();
    }
}

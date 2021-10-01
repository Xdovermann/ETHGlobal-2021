using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

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

    public TextMeshProUGUI deckCounterText;
    public int CurrentCardAmountInDeck;
    // Start is called before the first frame update
    void Start()
    {
        deckBuilder = this;
        SetSlots();
        LoadCards(BaseSetCards);
        ToggleDeckBuilderUI(false);
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
        
            for (int i = 0; i < item.AmountToSpawnInStack; i++)
            {
                GameObject card = Instantiate(item.gameObject);

                FindSlotForCardToInventory(card.GetComponent<Card>());
            }
         
        }

        CurrentCardAmountInDeck = 0;
        UpdateDeckCounter();
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
            if (AvaibleSlots[i].isSlotEmpty(card))
            {
                CurrentCardAmountInDeck--;
                UpdateDeckCounter();
                AvaibleSlots[i].SetSlot(card);
                return;
            }
        }

    }

    // card komt vanuit inventory naar deck toe
    public void FindSlotForCardToDeck(Card card)
    {
        if (CurrentCardAmountInDeck == CardCastManager.cardManager.currentDeck.maxDeckSize)
        {
            Debug.Log("Card deck is maxed out");
            return;
        }
       
        for (int i = 0; i < CurrentDeckSlots.Count; i++)
        {
            if (CurrentDeckSlots[i].isSlotEmpty(card))
            {
                CurrentCardAmountInDeck++;
                UpdateDeckCounter();
                CurrentDeckSlots[i].SetSlot(card);
                return;
            }
        }
    }

    // players choose the cards before every dungeon run
    // saves the new created deck for this dungeon run
    public void SaveChanges()
    {
        if(CurrentCardAmountInDeck < 5)
        {
            Debug.Log("NOT ENOUGH CARDS IN DECK ADD MORE TO CONTINUE !");
            return;
        }

        // maak een check als we minimaal 5 cards hebben in de deck
        List<Card> AllCardsInDeck = new List<Card>();
        for (int i = 0; i < CurrentDeckSlots.Count; i++)
        {
            if(CurrentDeckSlots[i].slottedCards.Count != 0)
            {
                    for (int x = 0; x < CurrentDeckSlots[i].slottedCards.Count; x++)
                    {
                        AllCardsInDeck.Add(CurrentDeckSlots[i].slottedCards[x]);
                    }
            }
          
        }

        PlayerDeck newDeck = CardCastManager.cardManager.currentDeck;
        newDeck.DisabledCards = AllCardsInDeck;

        CardCastManager.cardManager.LoadDeck(newDeck);

        ToggleDeckBuilderUI(false);

        DungeonManager.dungeonManager.StartGenerating();
    }

    private void UpdateDeckCounter()
    {
        deckCounterText.SetText(CurrentCardAmountInDeck.ToString()+"/"+CardCastManager.cardManager.currentDeck.maxDeckSize);
    }
}

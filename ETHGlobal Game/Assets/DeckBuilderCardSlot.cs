using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;
public enum SlotType
{
    // komt de slot vanuit de inventory
    InventorySlot,

    // komt de card vanuit de current deck
    DeckSlot,
}

public class DeckBuilderCardSlot : MonoBehaviour
{
   
    public SlotType slotType;
    public List<Card> slottedCards = new List<Card>();
    public TextMeshProUGUI NumberInSlotText;
    public int AmountInSlot;

    public Transform CardParent;

    public bool isSlotEmpty(Card card)
    {
        if (slottedCards.Count == 0)
            return true;

        if(slottedCards[0] == null)
        {
            return true;
        }
        else
        {
            if(card.isNFT)
            {
                if (slottedCards[0].Cardindex == card.Cardindex && slottedCards[0].isNFT)
                {
                    return true;
                }
                else
                {
                    return false;
                }
            }
            else
            {
                if (slottedCards[0].Cardindex == card.Cardindex && !slottedCards[0].isNFT)
                {
                    return true;
                }
                else
                {
                    return false;
                }
            }
           
            
        }
    }

    public void SetSlot(Card card)
    {
        slottedCards.Add(card);
        card.transform.SetParent(CardParent);
        card.transform.localPosition = Vector3.zero;
        AmountInSlot++;

        SetAmountText();
    }

    public void SetSlotType(SlotType type)
    {
        slotType = type;
    }

    public void ClickedSlot()
    {
        if (slottedCards.Count == 0)
            return;

        if(slotType == SlotType.DeckSlot)
        {
            DeckBuilder.deckBuilder.FindSlotForCardToInventory(slottedCards[0]);
        }
        else
        {
            DeckBuilder.deckBuilder.FindSlotForCardToDeck(slottedCards[0]);
        }

        AmountInSlot--;
        slottedCards.Remove(slottedCards[0]);

        SetAmountText();
    }

    public void SetAmountText()
    {
        if(AmountInSlot == 0)
        {
            NumberInSlotText.SetText("");
        }
        else
        {
            NumberInSlotText.SetText(AmountInSlot.ToString());
        }
       
    }

    public void ClearSlot()
    {
        foreach (var card in slottedCards)
        {
            Destroy(card.gameObject);
        }

        slottedCards.Clear();
        AmountInSlot = 0;
        SetAmountText();
    }
}

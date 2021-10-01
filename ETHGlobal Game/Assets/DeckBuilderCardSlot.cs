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
    public Card slottedCard;
    public TextMeshProUGUI NumberInSlotText;
    public int AmountInSlot;

    public Transform CardParent;

    public bool isSlotEmpty()
    {
        if(slottedCard == null)
        {
            return true;
        }
        else
        {
            return false;
        }
    }

    public void SetSlot(Card card)
    {
        slottedCard = card;
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
        if (slottedCard == null)
            return;

        if(slotType == SlotType.DeckSlot)
        {
            DeckBuilder.deckBuilder.FindSlotForCardToInventory(slottedCard);
        }
        else
        {
            DeckBuilder.deckBuilder.FindSlotForCardToDeck(slottedCard);
        }

        AmountInSlot--;
        slottedCard = null;

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
}

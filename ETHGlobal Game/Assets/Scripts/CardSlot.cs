using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using DG.Tweening;
using UnityEngine.UI;
public class CardSlot : MonoBehaviour
{
    public int SlotIndex;
    public Card SlottedCard;
    private bool canBeUsed = true;

    public void SetCard(Card card)
    {
        canBeUsed = false;
        SlottedCard = card;

        card.transform.SetParent(this.transform);

      
        SlottedCard.transform.localPosition = new Vector3(0, -250, 0);

        SetInSlotAnim();
       
    }

    private void Update()
    {
        if (Input.GetKeyDown(KeyCode.Q) && SlotIndex == 0)
        {
            UseCard();
        }
        else
        if (Input.GetKeyDown(KeyCode.W) && SlotIndex == 1)
        {
            UseCard();
        }
        else
        if (Input.GetKeyDown(KeyCode.E) && SlotIndex == 2)
        {
            UseCard();
        }else
        if (Input.GetKeyDown(KeyCode.R) && SlotIndex == 3)
        {
            UseCard();
        }

       
    }

    public void UseCard()
    {
        if (SlottedCard == null)
            return;

        if (!canBeUsed)
            return;
       
        if (CardManager.cardManager.CanUseCard(SlottedCard.ManaUse))
        {

            SlottedCard.CardEffect();

            UseAnim();

        }
       

      
    }

    public void SetInSlotAnim()
    {
        // slide the card in from the bottom
        SlottedCard.transform.DOLocalMoveY(0, 0.25f).OnComplete(CanBeUsed);

        void CanBeUsed()
        {
            canBeUsed = true;
        }
    }


    public void UseAnim()
    {
        // slide the card out from the top // wait for anim to finish when drawing a new card
        canBeUsed = false;

        CardManager.cardManager.UIJuice();

        SlottedCard.transform.DOLocalMoveY(200, 0.1f).OnComplete(PutCardBack);
   //     SlottedCard.CardBackGround.DOFade(0, 0.2f).OnComplete(DrawNewCard);



        void PutCardBack()
        {
            CardManager.cardManager.currentDeck.PutCardBack(SlottedCard);
            DrawNewCard();
        }

        void DrawNewCard()
        {
            SlottedCard.transform.SetParent(CardManager.cardManager.transform);
            SlottedCard.transform.position = new Vector3(0, 0, 0);
            SlottedCard.gameObject.SetActive(false);


            Card card = CardManager.cardManager.currentDeck.GrabRandomCard();
            SetCard(card);

           
        }
    }


}

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
        
        SlottedCard.CardBackGround.DOFade(255, 0f);
        SlottedCard.transform.localPosition = new Vector3(0, -250, 0);

        SetInSlotAnim();
       
    }

    private void Update()
    {
        switch (SlotIndex)
        {
            case 0:
                if (Input.GetKeyDown(KeyCode.Q))
                {
                    UseCard();
                }
            break;
            case 1:
                if (Input.GetKeyDown(KeyCode.W))
                {
                    UseCard();
                }
                break;
            case 2:
                if (Input.GetKeyDown(KeyCode.E))
                {
                    UseCard();
                }
                break;
            case 3:
                if (Input.GetKeyDown(KeyCode.R))
                {
                    UseCard();
                }

                break;
            default:
                break;
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
            SlottedCard.Use();

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

        SlottedCard.transform.DOLocalMoveY(200, 0.1f).OnComplete(PutCardBack);

     
        SlottedCard.CardBackGround.DOFade(50, 0.1f).OnComplete(DrawNewCard);

        void PutCardBack()
        {
            CardManager.cardManager.currentDeck.PutCardBack(SlottedCard);
        }

        void DrawNewCard()
        {

            SlottedCard.transform.SetParent(transform);
            SlottedCard.transform.position = new Vector3(0, 0, 0);
            SlottedCard.gameObject.SetActive(false);


            Card card = CardManager.cardManager.currentDeck.GrabRandomCard();
            SetCard(card);

           
        }
    }


}

using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;
using UnityEngine.UI;
using DG.Tweening;

public class CardManager : MonoBehaviour
{
    public static CardManager cardManager;

    public PlayerDeck currentDeck;

    public float MaxMana;
    public float CurrentMana;

    public TextMeshProUGUI DeckCount;
    public TextMeshProUGUI ManaCount;
    public Image ManaBarFiller;

    public CardSlot[] CardSlots;

    public float ManaRechargeTime = 2f;
    private float ManaRechargeTimeHolder;
    // Start is called before the first frame update
    void Start()
    {
        cardManager = this;

        ManaRechargeTimeHolder = ManaRechargeTime;

        LoadDeck();
        UpdateManaUI();
    }

    // Update is called once per frame
    void Update()
    {
        RechargeMana();
    }

    private void LoadDeck()
    {
        currentDeck.DisableAll();
        CurrentMana = MaxMana;
        DeckCount.SetText("DECK : "+currentDeck.DeckCount().ToString()+"/"+currentDeck.maxDeckSize.ToString());

        for (int i = 0; i < CardSlots.Length; i++)
        {
            Card card = currentDeck.GrabRandomCard();
            CardSlots[i].SetCard(card);
            CardSlots[i].SlotIndex = i;

        }



    }

    public bool CanUseCard(float manaUse)
    {
        float holder = CurrentMana;
        holder -= manaUse;

        if(holder >= 0)
        {
            CurrentMana -= manaUse;
            UpdateManaUI();
            return true;
        }
        else
        {
            return false;
        }

    }

   

    private void UpdateManaUI()
    {
        int curMana = (int)CurrentMana;
        ManaCount.SetText(curMana + "/" + MaxMana.ToString());
  
        float curM = CurrentMana / 100f;
        float maxM = MaxMana / 100f;

        float holder = curM / maxM;

        ManaBarFiller.fillAmount = holder;
        //ManaBarFiller.DOFillAmount(holder, 0.1f);
    }

    private void RechargeMana()
    {
        if(CurrentMana < MaxMana)
        {
            CurrentMana += Time.deltaTime;
            UpdateManaUI();
        }
    }

   
}

using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;
using UnityEngine.UI;
using DG.Tweening;

public enum CardType
{
    Bowman,
    Warrior,
    Rogue,
    AllClasses
}

public enum CardRarity
{
    Common,
    Uncommon,
    Rare,
    Epic,
    Legendary
}

public class Card : MonoBehaviour
{
    // this is the amount that will spawn when spawning the card in the deck builder
    public int AmountToSpawnInStack = 2;

    public CardRarity cardRarity;
    public CardType cardType;

    public int Cardindex;
    public int ManaUse;

    public TextMeshProUGUI ManaText;
    [HideInInspector]
    public Image CardBackGround;
    // Start is called before the first frame update
    void Start()
    {
        StartFunction();
    }

    public void StartFunction()
    {
        CardBackGround = GetComponent<Image>();
        ManaText.SetText(ManaUse.ToString());
    }

   
    public void FadeCard()
    {
        Debug.LogError("FADE CARD ALPHA");
        CardBackGround.DOFade(0, 1);
    }

    public void ResetAlpha()
    {

       // CardBackGround.DOFade(255, 0);
    }

    public virtual void CardEffect()
    {
        Debug.Log("base card effect");
    }
}

using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;
using UnityEngine.UI;
using DG.Tweening;

public class Card : MonoBehaviour
{
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

        CardBackGround.DOFade(255, 0);
    }

    public virtual void CardEffect()
    {
        Debug.Log("base card effect");
    }
}

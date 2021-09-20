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

    public Image CardBackGround;
    // Start is called before the first frame update
    void Start()
    {
        ManaText.SetText(ManaUse.ToString());
    }

    public void Use()
    {
        
        CardEffect();

    }

   

    public virtual void CardEffect()
    {

    }
}

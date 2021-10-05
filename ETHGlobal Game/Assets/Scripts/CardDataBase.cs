using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEditor;


[CreateAssetMenu(fileName = "New Card Database", menuName = "Databases/Card-Database")]
public class CardDataBase : ScriptableObject
{
    public bool nftDatabase = false;
    public Card[] cards;
  
    // automatticly sets the card ids of the card in the databas
#if (UNITY_EDITOR)
    [ContextMenu("Update ID's")]
    public void UpdateID()
    {

        for (int i = 0; i < cards.Length; i++)
        {


            if (cards[i].Cardindex != i)
                cards[i].Cardindex = i;
            if (nftDatabase)
            {
                cards[i].isNFT = true;
            }
            PrefabUtility.SavePrefabAsset(cards[i].gameObject);
        }

    }

#endif

    public Card ReturnCard(int id)
    {
        return cards[id];
    }
}

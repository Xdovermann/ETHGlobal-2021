using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEditor;

public enum CardDataType
{
    FreeCards,
    NftCards
}

[CreateAssetMenu(fileName = "New Card Database", menuName = "Databases/Card-Database")]
public class CardDataBase : ScriptableObject
{
    public Card[] cards;
    public CardDataType type;
    // automatticly sets the card ids of the card in the databas
#if (UNITY_EDITOR)
    [ContextMenu("Update ID's")]
    public void UpdateID()
    {

        for (int i = 0; i < cards.Length; i++)
        {


            if (cards[i].Cardindex != i)
                cards[i].Cardindex = i;

            PrefabUtility.SavePrefabAsset(cards[i].gameObject);
        }

    }

#endif
}

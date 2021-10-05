using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;
using UnityEngine.UI;
using System.Runtime.InteropServices;
using DG.Tweening;


public class GameManager : MonoBehaviour
{
    // Import functions from moralis.jslib
    [DllImport("__Internal")] private static extern string LoginMoralis();
    [DllImport("__Internal")] private static extern string BuyBoosterPack();

    [DllImport("__Internal")] private static extern string GetNftCards();

    private string CurrentUserAdress;


    public TextMeshProUGUI ButtonText;
    public TextMeshProUGUI idsText;
    public GameObject LoginScreen;
    public GameObject BuyBoosterPackButton;

    public GameObject GetNftLoadingScreen;

    public string ContractAdress = "card contract";

    public int GoldAmount;
    public int PackPriceInGold = 5;

    public Transform[] BoosterPackSlots;
    public GameObject BoosterPackLoadingScreen;
    public CardDataBase NftCards;
    private void Awake()
    {
        EnableObject(LoginScreen);
        DisableObject(BuyBoosterPackButton);
        DisableObject(BoosterPackLoadingScreen);
        GetNftLoadingScreen.SetActive(true);
    }



    public void EnableObject(GameObject gameObject)
    {
        gameObject.SetActive(true);
    }

    public void DisableObject(GameObject gameObject)
    {
        gameObject.SetActive(false);
    }

    private void BoosterPackLoadinScreen()
    {
        BoosterPackLoadingScreen.SetActive(true);
    }

    private void PackOpenAnim(List<int> cards)
    {
       
        BoosterPackLoadingScreen.SetActive(false);
        // spawn de bijbehorende cards en stop ze in de card slots
        for (int i = 0; i < cards.Count; i++)
        {
            Card card = NftCards.ReturnCard(cards[i]);
            GameObject go = Instantiate(card.gameObject, BoosterPackSlots[i]);
            go.transform.DOLocalMove(Vector3.zero, 0);
        }

#if (!UNITY_EDITOR)
        GetNftCards();
#endif
#if (UNITY_EDITOR)
        GetNftLoadingScreen.SetActive(false);
#endif

    }

    public void ExitStore(GameObject gameObject)
    {
        DisableObject(gameObject);
        ClearSlots();
    }

    private void ClearSlots()
    {
        foreach (Transform child in BoosterPackSlots)
        {
            if (child.childCount <= 0)
                return;

         
            Destroy(child.GetChild(0).gameObject);
        }
    }
    /*BUTTON FUNCTIONS THAT CALL TO THE MORALIS.JSLIB-*/
    public void LoginUser()
    {
    // handles the web3stuff
#if (!UNITY_EDITOR)
         LoginMoralis();
#endif
    // in editor no web 3 stuff avaible 
#if (UNITY_EDITOR)
    GetUserAdress("IN EDITOR MODE");
#endif


    }
    public void BuyPack()
    {
        GetNftLoadingScreen.SetActive(true);
        ClearSlots();
        BoosterPackLoadinScreen();
#if (!UNITY_EDITOR)
            BuyBoosterPack();
      
#endif
#if (UNITY_EDITOR)
            List<int> ParsedIds = new List<int>();
            for (int i = 0; i < 4; i++)
            {
                int rand = Random.Range(0, NftCards.cards.Length);
                ParsedIds.Add(rand);
            }
            PackOpenAnim(ParsedIds);
#endif

       
    }
   
    /*PUT DOWN HERE ALL THE CALLBACK METHODS THAT GET CALLED FROM JS*/

    // gets called from ConnectMoralis.js and returns the user adress and continues the player to the game
    public void GetUserAdress(string adress)
    {
        CurrentUserAdress = adress;
        ButtonText.SetText(adress);

        DisableObject(LoginScreen);
#if (!UNITY_EDITOR)
        GetNftCards();
#endif
#if (UNITY_EDITOR)
  GetNftLoadingScreen.SetActive(false);
#endif
        DeckBuilder.deckBuilder.ToggleDeckBuilderUI(true);
      
  

    }

    public void OpenPack(string ids)
    {
        string AllIDS = ids;
        List<int> ParsedIds = new List<int>();
        foreach (var letter in AllIDS)
        {
            // need to create a string to try and parse cant parse a char
            string LETTER =""+ letter;
            int value;
            // try and parse the char if its a number it will succeed and it gets added into the int array 
            if (int.TryParse(LETTER, out value))
            {
                ParsedIds.Add(value);
            }
        }

      //  idsText.SetText("MINTED CARDS IDS ARE "+ ParsedIds[0] +","+ ParsedIds[1] + "," + ParsedIds[2] + "," + ParsedIds[3]);

        PackOpenAnim(ParsedIds);
    }

    public void ReturnNftCards(string ids)
    {
        DeckBuilder.deckBuilder.ClearNFTSlots();

        // convert from json string array to object
        string newJson = "{\"CardList\": " + ids + "}";
        CardDataList cardDataList = JsonUtility.FromJson<CardDataList>(newJson);  
        for (int i = 0; i < cardDataList.CardList.Length; i++)
        {
            Debug.Log("NEED TO SPAWN NFT CARD : "+cardDataList.CardList[i].Cardindex + " , Amount to spawn : " + cardDataList.CardList[i].CardAmount);
            Card card = NftCards.ReturnCard(cardDataList.CardList[i].Cardindex);
            for (int x = 0; x < cardDataList.CardList[i].CardAmount; x++)
            {
                GameObject go = Instantiate(card.gameObject, transform);
                DeckBuilder.deckBuilder.FindSlotForNFT(go.GetComponent<Card>());
            }
        }

        GetNftLoadingScreen.SetActive(false);
        // loop door cardlist 
        // spawn voor elke index een card
        // x aantal 


    }
}

public class CardDataList
{
   public CardDataHolder[] CardList;
}

[System.Serializable]
public class CardDataHolder
{
    public int Cardindex;
    public int CardAmount;

    CardDataHolder(int _index,int _amount)
    {
        Cardindex = _index;
        CardAmount = _amount;
    }
}
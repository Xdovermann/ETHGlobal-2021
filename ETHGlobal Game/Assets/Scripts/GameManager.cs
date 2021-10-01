using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;
using UnityEngine.UI;
using System.Runtime.InteropServices;

public class GameManager : MonoBehaviour
{
    // Import functions from moralis.jslib
    [DllImport("__Internal")] private static extern string LoginMoralis();
    [DllImport("__Internal")] private static extern string BuyBoosterPack();

    private string CurrentUserAdress;


    public TextMeshProUGUI ButtonText;
    public TextMeshProUGUI idsText;
    public GameObject LoginScreen;
    public GameObject BuyBoosterPackButton;

    public string ContractAdress = "card contract";

    public int GoldAmount;
    public int PackPriceInGold = 5;

    private void Awake()
    {
        EnableObject(LoginScreen);
        DisableObject(BuyBoosterPackButton);
    }

    public void EnableObject(GameObject gameObject)
    {
        gameObject.SetActive(true);
    }

    public void DisableObject(GameObject gameObject)
    {
        gameObject.SetActive(false);
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
        BuyBoosterPack();
    }

    /*PUT DOWN HERE ALL THE CALLBACK METHODS THAT GET CALLED FROM JS*/

    // gets called from ConnectMoralis.js and returns the user adress and continues the player to the game
    public void GetUserAdress(string adress)
    {
        CurrentUserAdress = adress;
        ButtonText.SetText(adress);

        DisableObject(LoginScreen);
        DeckBuilder.deckBuilder.ToggleDeckBuilderUI(true);
        EnableObject(BuyBoosterPackButton);
  

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

        idsText.SetText("MINTED CARDS IDS ARE "+ ParsedIds[0] +","+ ParsedIds[1] + "," + ParsedIds[2] + "," + ParsedIds[3]);
    }
}

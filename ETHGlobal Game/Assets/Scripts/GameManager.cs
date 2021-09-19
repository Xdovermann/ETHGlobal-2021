using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;
using UnityEngine.UI;
using System.Runtime.InteropServices;

public class GameManager : MonoBehaviour
{
    // Import function from moralis.jslib
    [DllImport("__Internal")] private static extern string LoginMoralis();

    private string CurrentUserAdress;


    public TextMeshProUGUI ButtonText;
    public GameObject LoginScreen;
    public GameObject EmbarkMissionScreen;

    public void LoginUser()
    {
        LoginMoralis();
    }

    public void GetUserAdress(string adress)
    {
        CurrentUserAdress = adress;
        ButtonText.SetText(adress);

        DisableObject(LoginScreen);
        EnableObject(EmbarkMissionScreen);

    }

    public void EnableObject(GameObject gameObject)
    {
        gameObject.SetActive(true);
    }

    public void DisableObject(GameObject gameObject)
    {
        gameObject.SetActive(false);
    }
}

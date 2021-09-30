using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DeadZone : MonoBehaviour
{

    private void OnTriggerEnter(Collider other)
    {
        other.gameObject.SetActive(false);
        Debug.Log("Object fell off map disable it");
    }
}

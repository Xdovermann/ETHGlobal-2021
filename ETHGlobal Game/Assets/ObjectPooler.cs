using System.Collections.Generic;
using UnityEngine;




public enum ObjectPoolerType
{
    DamageNumber,

}

public class ObjectPooler : MonoBehaviour
{

    public static ObjectPooler DamageNumber;

    [SerializeField]
    private int amountToSpawnOnStart = 25;

    [SerializeField]
    private GameObject PooledObject;
    private bool notEnoughInPool = true;
    public List<GameObject> PoolObjects;



    public ObjectPoolerType Pooler_Type;



    private void Awake()
    {
        switch (Pooler_Type)
        {
            case ObjectPoolerType.DamageNumber:
                DamageNumber = this;
                break;

        }

    }

    void Start()
    {


        PoolObjects = new List<GameObject>();

        for (int i = 0; i < amountToSpawnOnStart; i++)
        {

            GameObject bul = Instantiate(PooledObject, transform);

            bul.SetActive(false);
            PoolObjects.Add(bul);

        }

    }


    public GameObject GetObject()
    {
        if (PoolObjects.Count > 0)
        {
            for (int i = 0; i < PoolObjects.Count; i++)
            {

                if (PoolObjects[i] != null)
                {
                    if (!PoolObjects[i].activeInHierarchy)
                    {
                        PoolObjects[i].SetActive(true);
                        return PoolObjects[i];
                    }
                }

            }
        }

        if (notEnoughInPool)
        {
            GameObject bul = Instantiate(PooledObject, transform);
            PoolObjects.Add(bul);
            bul.SetActive(true);
            return bul;
        }

        return null;
    }

    public GameObject SetObject(Vector3 pos)
    {
        GameObject go = GetObject();
        go.transform.position = pos;
        go.SetActive(true);

        return go;
    }

    public void DisableAll()
    {
        for (int i = 0; i < PoolObjects.Count; i++)
        {
            if (PoolObjects[i] != null)
            {
                if (PoolObjects[i].activeInHierarchy)
                {
                    PoolObjects[i].SetActive(false);
                }
            }

        }
    }


}



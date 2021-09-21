using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MouseCursor : MonoBehaviour
{
    public Transform Cursor;
    public Transform targetSelector;
    AttackHandler attackHandler;
    // Start is called before the first frame update
    void Start()
    {
        attackHandler = AttackHandler.attackHandler;


    }

    // Update is called once per frame
    void Update()
    {
        UpdatePositions();

        targetSelector.Rotate(Vector3.forward * 150 * Time.deltaTime);
    }

   private void UpdatePositions()
    {
        Cursor.transform.position = Input.mousePosition;

        if (attackHandler.CurrentTarget == null)
        {

            targetSelector.transform.position = Input.mousePosition;
         
        }
        else
        {
            Vector3 pos = Camera.main.WorldToScreenPoint(attackHandler.CurrentTarget.transform.position);
         
            targetSelector.transform.position = pos;
        }
    }

    
}

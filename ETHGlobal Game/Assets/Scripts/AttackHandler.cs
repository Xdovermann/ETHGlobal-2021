using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AttackHandler : MonoBehaviour
{

    public static AttackHandler attackHandler;

    public Transform CurrentTarget;
    private Target target;

    public LayerMask TargetMask;

    public MouseCursor cursor;


    private void Awake()
    {
        attackHandler = this;
    }

    // Update is called once per frame
    void Update()
    {   

        CheckForTarget();
    }

    private void CheckForTarget()
    {
        if (Input.GetMouseButtonDown(0))
        {
            RaycastHit hit;
            Ray ray = Camera.main.ScreenPointToRay(Input.mousePosition);
            if (Physics.Raycast(ray, out hit, Mathf.Infinity, TargetMask))
            {
                // check if we clicked the same target again untarget that enemy then
                if(CurrentTarget != null)
                {
                    if(CurrentTarget == hit.transform)
                    {                     
                        Debug.Log("Removed target");
                       
                        CurrentTarget = null;
                        target = null;

                    }
                }
                else
                {
                    if (hit.transform != null)
                    {
                        Debug.Log("Setted new target : " + hit.transform.gameObject.name);
                        CurrentTarget = hit.transform;
                        target = CurrentTarget.GetComponent<Target>();
                        
                    }
                }

               
              
            }
        }
    }


    public void ClearTarget(Target _target)
    {
        if(_target == target)
        {
            CurrentTarget = null;
            target = null;

        }
    }
}

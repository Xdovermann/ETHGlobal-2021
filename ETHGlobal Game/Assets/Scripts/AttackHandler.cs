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

    public Transform WeaponRotator;
    public Transform WeaponAttackPoint;
    private float angle;

    private void Awake()
    {
        attackHandler = this;
    }

    // Update is called once per frame
    void Update()
    {   

        CheckForTarget();
        RotateAttackPoint();
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
                    if (hit.transform != null)
                    {
                        SetNewTarget(hit.transform);
                    }
                

               
              
            }
        }
    }

    private void SetNewTarget(Transform hit)
    {

        if (target != null)
        {       

            if(hit.GetComponent<Target>() == target)
            {
                Debug.Log("Removed target");
                target.RemoveTarget();
                CurrentTarget = null;
                target = null;
            }
            else
            {
                target.RemoveTarget();

                Debug.Log("Setted new target : " + hit.transform.gameObject.name);
                CurrentTarget = hit.transform;
                target = CurrentTarget.GetComponent<Target>();
                target.SettedTarget();
            }

                  
        }
        else 
        {
         

            Debug.Log("Setted new target : " + hit.transform.gameObject.name);
            CurrentTarget = hit.transform;
            target = CurrentTarget.GetComponent<Target>();
            target.SettedTarget();



        }

       


    }

    public bool hasTarget()
    {
        if(target == null)
        {
            return false;
        }
        else
        {
            return true;
        }
    }

    // rotates the weapon point 
    private void RotateAttackPoint()
    {
        // if we got target we wanna lock onto that with aiming
        if (hasTarget())
        {
            Vector3 Pos = GetWorldPosition();
            angle = -1 * PlayerController.AngleBetweenTwoPoints(transform.position, target.transform.position);
         
        }
        else // this is for free aiming
        {
            Vector3 Pos = GetWorldPosition();
            angle = -1 * PlayerController.AngleBetweenTwoPoints(transform.position, Pos);

         
        }

        WeaponRotator.rotation = Quaternion.AngleAxis(angle, new Vector3(0, 1, 0));
    }

    private Vector3 GetWorldPosition()
    {

        Plane groundPlane = new Plane(Vector3.up, new Vector3(0, 0, 0));
        Ray ray = Camera.main.ScreenPointToRay(Input.mousePosition);
        Debug.DrawLine(ray.origin, ray.origin + ray.direction * 100, Color.red);
        float distance;
        Vector3 point = Vector3.zero;
        if (groundPlane.Raycast(ray, out distance))
        {

            point = ray.GetPoint(distance);

        }

        return point;
    }

    public Vector3 GetTargetDirection()
    {
        Vector3 pos = new Vector3(0, 0, 0);
        if (hasTarget())
        {
            // grab dir between player and target
            pos = (target.transform.position - PlayerController.playerController.transform.position).normalized;
            return pos;
        }
        else
        {
            Vector3 mousePos = PlayerController.GetMousePosition();
            pos = (mousePos - PlayerController.playerController.transform.position).normalized;
            return pos;
            // between player and mouse position
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

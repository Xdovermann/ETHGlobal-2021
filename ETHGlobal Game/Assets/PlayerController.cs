using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Pathfinding;

public class PlayerController : MonoBehaviour
{
    private IAstarAI agent;
    public SpriteRenderer playerRenderer;
    public float aimAngle;
    private Animator anim;
    // Start is called before the first frame update
    void Awake()
    {
        agent = GetComponent<IAstarAI>();
        agent.isStopped = true;
        anim = GetComponent<Animator>();
    }

    // Update is called once per frame
    void Update()
    {
        MoveToPosition();
        FlipHandler();
        AnimHandler();
    }


    private void MoveToPosition()
    {
        if (Input.GetMouseButton(1))
        {
            agent.isStopped = false;
            // grab mouse position
            NNInfo info = AstarPath.active.GetNearest(GetMousePosition());
     
            agent.destination = info.position;
            agent.SearchPath();
        }
    }

    private Vector3 GetMousePosition()
    {
        RaycastHit hit;
        Ray ray = Camera.main.ScreenPointToRay(Input.mousePosition);
        if (Physics.Raycast(ray, out hit, Mathf.Infinity))
        {
            return hit.point;
        }

        return new Vector3(0,0,0);
    }



    private void FlipHandler()
    {
           Vector3 POS = GetMousePosition();
           aimAngle = -1 * AngleBetweenTwoPoints(transform.position, POS);

        // cam angle is 135 so when adding 45 you get 180 and at that point you wanna flip
        if(aimAngle > 45)
        {
            playerRenderer.flipX = false;
        }
        else if(aimAngle >= -135)
        {
            playerRenderer.flipX = true;
        }
        

        //if (GetMousePosition().z > transform.position.z)
        //{
        //    playerRenderer.flipX = true;
        //}
        //else
        //{
        //    playerRenderer.flipX = false;
        //}
    }

    private void AnimHandler()
    {
        if (anim == null)
            return;

        if (agent.isStopped)
        {
            anim.SetBool("isRunning", false);
        }
        else
        if (agent.remainingDistance <= 0.1f)
        {
            anim.SetBool("isRunning", false);
        }
        else
        {
            anim.SetBool("isRunning", true);
        }
    }

    public static float AngleBetweenTwoPoints(Vector3 a, Vector3 b)
    {
        return Mathf.Atan2(a.z - b.z, a.x - b.x) * Mathf.Rad2Deg;
    }
}

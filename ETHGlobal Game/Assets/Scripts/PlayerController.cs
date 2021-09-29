using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Pathfinding;

public enum Stats
{
    Strength, // melee / health points
    Dexterity, // ranged damage / chance to dodge attacks
    Intellect, // spell damage / mana increase 
    Luck, // Critical hit chance 
}

[System.Serializable]
public class Attribute
{
    public Stats stat;
    public float statValue;
}

public class PlayerController : MonoBehaviour
{
    public static PlayerController playerController;

    public Attribute[] PlayerAttributes;

    private IAstarAI agent;
    public Transform playerRendererParent;
    public float aimAngle;
    private Animator anim;
    private Rigidbody rb;
    public SpriteRenderer WeaponRenderer;
    // Start is called before the first frame update
    void Awake()
    {
        playerController = this;
        agent = GetComponent<IAstarAI>();
        agent.isStopped = true;
        anim = GetComponent<Animator>();
        rb = GetComponent<Rigidbody>();
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
            CancelMovement();

             // grab mouse position
             NNInfo info = AstarPath.active.GetNearest(GetMousePosition());
          

          
                agent.isStopped = false;
                agent.destination = info.position;
                agent.SearchPath();
            
         
        }
    }

    public static Vector3 GetMousePosition()
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
        //    playerRenderer.flipX = false;
            playerRendererParent.transform.localScale = new Vector3(-1, 1, 1);
        }
        else if(aimAngle >= -135)
        {
           // playerRenderer.flipX = true;
            playerRendererParent.transform.localScale =new Vector3(1,1,1);
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

        if (agent.remainingDistance <= 0.01f)
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

    public void CancelMovement()
    {
        agent.isStopped = true;
        rb.velocity = new Vector3(0, 0, 0);
    }
}

using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using DG.Tweening;
using Pathfinding;

public class SlimeAI : MonoBehaviour
{
    public enum SlimeState
    {
        Idle,
        Roaming,
        AttackPlayer,
    }

    public SlimeState slimeState;

    public float IdleTime = 1f;

    public float MoveTimer = 2.5f;
    private float MovetimerHolder;
    private IAstarAI agent;
    private Rigidbody rb;

    public Rigidbody SlimeJumpRB;
    // Start is called before the first frame update
    void Start()
    {
        agent = GetComponent<IAstarAI>();
        rb = GetComponent<Rigidbody>();

        MovetimerHolder = MoveTimer;

        slimeState = SlimeState.Roaming;
    }

    // Update is called once per frame
    void Update()
    {
        switch (slimeState)
        {
            case SlimeState.Idle:
                // do nothing
                break;
            case SlimeState.Roaming:

                if(MoveTimer <= 0)
                {
                    // create new move
                    int rand = Random.Range(0, 3);
                    if(rand == 1)
                    {
                        StartCoroutine(Idle());
                    }
                    else
                    {
                        RandomMovement();
                    }

                   
                }
                else
                {
                    MoveTimer -= Time.deltaTime;
                }

                break;
            case SlimeState.AttackPlayer:
                break;
            default:
                break;
        }
    }

    private void RandomMovement()
    {
       

        MoveTimer = MovetimerHolder;

        Vector3 randDirection = Random.insideUnitSphere * 3;
        randDirection += transform.position;

      

        NNInfo info = AstarPath.active.GetNearest(new Vector3(randDirection.x, agent.position.y, randDirection.z));
        Vector3 pos = info.position;

        agent.destination = pos;
        agent.SearchPath();


    
    }

    private IEnumerator Idle()
    {
        slimeState = SlimeState.Idle;

        float Idle = Random.Range(0, 1);
        Idle += IdleTime;
        yield return new WaitForSeconds(Idle);

        slimeState = SlimeState.Roaming;
    }
}

using System.Collections;
using UnityEngine;
using UnityEngine.AI;

public class BehaviorController : MonoBehaviour
{
    [Header("stats")]
    public float attackDistance;
    public float attackRate;
    private float nextAttack;
    public int minNodes;
    public int maxNodes;
    public float minWait;
    public float maxWait;

    [Header("Debug")]
    public Transform debugDestination;
    public int debugNodesUntilWait;
    public float debugAgentSpeed;
    public bool nearPlayer;

    [Header("Anim")]
    public GameObject enemySprite;
    Animator animator;
    private bool moving;

    public Transform[] points;
    public GameObject lastPosition;
    private NavMeshAgent agent;
    private int nodesUntilWait;
    public GameObject player;
    private bool isVisible = false;
    private int layerMask = 1 << 8;
    Vector3 playerDirection;

    private int RNG = 1;

    void Awake()
    {
        animator = enemySprite.GetComponent<Animator>();
    }

    void Start()
    {
        agent = GetComponent<NavMeshAgent>();
        RNG = RandomNumber();
        layerMask = ~layerMask;
    }

    void Update()
    {
        animator.SetBool("isMoving", moving);

        if (agent.velocity.sqrMagnitude > 0.16f)
        {
            moving = true;
        }
        else
        {
            moving = false;
        }

        int layerMask = 1 << 8;
        layerMask = ~layerMask;

        debugNodesUntilWait = nodesUntilWait + 1;
        debugAgentSpeed = agent.speed;
        Debug.DrawRay(transform.position, playerDirection, Color.green);
        Debug.DrawRay(lastPosition.transform.position, new Vector3(0, 10, 0.5f), Color.yellow);

        playerDirection = player.transform.position - transform.position;
        if (agent.destination != points[RNG].position && playerDirection.sqrMagnitude < 2)
        {
            nearPlayer = true;
        }
        else
        {
            nearPlayer = false;
        }

        if (!nearPlayer)
        {
            if (nodesUntilWait < 1)
            {
                StartCoroutine(Wait());
            }
            else
            {
                VisibleCheck();

                if (isVisible)
                {
                    CheckIfStillVisible();
                }
            }

            /*Choose the next destination point when the agent gets
          close to the current one.*/
            if (!agent.pathPending && agent.remainingDistance < 0.5f)
            {
                GoToNextPoint("node");
            }
        }
        else
        {
            agent.speed = 0;
        }

    void VisibleCheck()
    {
        if (playerDirection.sqrMagnitude < 100f && !Physics.Linecast(transform.position, player.transform.position, layerMask))
        {
            lastPosition.transform.position = player.transform.position;
            Debug.DrawRay(transform.position, Vector3.up * 10, Color.red);
            agent.speed = 5f;
            GoToNextPoint("player");
            isVisible = true;
        }
    }

    void CheckIfStillVisible()
    {
        if (playerDirection.sqrMagnitude > 100f || Physics.Linecast(transform.position, player.transform.position, layerMask))
        {
            agent.speed = 3.5f;
            isVisible = false;
            GoToNextPoint("lostPlayer");
        }
    }

    void GoToNextPoint(string target)
    {
        switch(target)
        {
            case "node":
                //If no nodes, AI doesn't move.
                if (points.Length == 0)
                {
                    return;
                }

                /*Checks if it's time to stop, then randomly selects next destination.*/
                if (nodesUntilWait < 1)
                {
                    Wait();
                }
                RNG = RandomNumber();
                NextNode();

                return;
            case "lostPlayer":
                agent.destination = lastPosition.transform.position;

                if(!agent.pathPending && agent.remainingDistance < 0.5f)
                {
                    StartCoroutine(Wait());
                }
                return;

            case "player":
                agent.destination = player.transform.position;
                return;

            default:
                return;
        }

        }
        
    }

    int RandomNumber()
    {
        return Random.Range(0, points.Length);
    }

    void NextNode()
    {
        agent.destination = points[RNG].position;
        debugDestination = points[RNG];
        nodesUntilWait--;
    }

    //Stops AI for a random amount, then sets the amount of nodes to pass until next wait.
    IEnumerator Wait()
    {
        agent.speed = 0f;
        yield return new WaitForSecondsRealtime(Random.Range(minWait, maxWait));
        agent.speed = 3.5f;
        nodesUntilWait = Random.Range(minNodes, maxNodes);
    }
}

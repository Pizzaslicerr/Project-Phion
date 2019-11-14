using UnityEngine;
using UnityEngine.AI;
using System.Collections;

public class BehaviorController : MonoBehaviour
{
    [Header("stats")]
    public float attackDistance;
    public float attackRate;
    private float nextAttack;

    public Transform[] points;
    private int destPoint = 0;
    private int prevDest = 0;
    private NavMeshAgent agent;

    void Start()
    {
        agent = GetComponent<NavMeshAgent>();
    }

    void Update()
    {
            /*Choose the next destination point when the agent gets
              close to the current one.*/
        if (!agent.pathPending && agent.remainingDistance < 0.5f)
        {
            GoToNextPoint();
        }
    }

    void GoToNextPoint()
    {

        if (points.Length == 0)
        {
            return;
        }

        //Set the agent to go to the currently selected destination.
        StartCoroutine(wait());
        agent.destination = points[RandomNode()].position;
    }

    int RandomNode()
    {
        return Random.Range(0, points.Length);
    }

    IEnumerator wait()
    {
        yield return new WaitForSeconds(10);
    }
}

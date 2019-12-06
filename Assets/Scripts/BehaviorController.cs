﻿using UnityEngine;
using UnityEngine.AI;
using System.Collections;

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

    public Transform[] points;
    public GameObject lastPosition;
    private NavMeshAgent agent;
    private int nodesUntilWait;
    public GameObject player;
    private bool isVisible = false;

    private int RNG = 1;

    void Start()
    {
        agent = GetComponent<NavMeshAgent>();
        RNG = RandomNumber();
    }

    void Update()
    {
        int layerMask = 1 << 8;
        layerMask = ~layerMask;

        debugNodesUntilWait = nodesUntilWait + 1;
        debugAgentSpeed = agent.speed;

        Vector3 playerDirection = player.transform.position - transform.position;
        Debug.DrawRay(transform.position, playerDirection, Color.green);
        Debug.DrawRay(lastPosition.transform.position, new Vector3(0, 10, 0.5f), Color.yellow);
        
        if (playerDirection.sqrMagnitude < 100f && !Physics.Linecast(transform.position, player.transform.position, layerMask))
        {
            lastPosition.transform.position = player.transform.position;
            Debug.DrawRay(transform.position, Vector3.up * 10, Color.red);
            agent.speed = 5f;
            GoToNextPoint("player");
            isVisible = true;
        }

        if (isVisible)
        {
            if (playerDirection.sqrMagnitude > 100f || Physics.Linecast(transform.position, player.transform.position, layerMask))
            {
                agent.speed = 3.5f;
                isVisible = false;
                GoToNextPoint("lostPlayer");
            }
        }
        

        /*Choose the next destination point when the agent gets
          close to the current one.*/
        if (!agent.pathPending && agent.remainingDistance < 0.5f)
        {
            GoToNextPoint("node");
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
                if (nodesUntilWait == 0)
                {
                    
                }
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

    int RandomNumber()
    {
        return Random.Range(0, points.Length);
    }

    void NextNode()
    {
        RNG = RandomNumber();
        agent.destination = points[RNG].position;
        debugDestination = points[RNG];
        nodesUntilWait--;

        return;
    }

    //Stops AI for a random amount, then sets the amount of nodes to pass until next wait.
    IEnumerator Wait()
    {
        agent.speed = 0.0f;
        yield return new WaitForSecondsRealtime(Random.Range(minWait, maxWait));
        agent.speed = 3.5f;
        nodesUntilWait = Random.Range(minNodes, maxNodes);
    }
}

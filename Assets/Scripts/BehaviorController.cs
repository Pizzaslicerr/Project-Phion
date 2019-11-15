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

    public Transform[] points;
    private NavMeshAgent agent;
    private int nodesUntilWait;


    private int RNG = 1;
    public Transform debugDestination;
    public int debugNodesUntilWait;

    void Start()
    {
        agent = GetComponent<NavMeshAgent>();
        RNG = RandomNumber();
    }

    void Update()
    {
        debugNodesUntilWait = nodesUntilWait + 1;

        /*Choose the next destination point when the agent gets
          close to the current one.*/
        if (!agent.pathPending && agent.remainingDistance < 0.5f)
        {
            GoToNextPoint();
        }
    }

    void GoToNextPoint()
    {
        //If no nodes, AI doesn't move.
        if (points.Length == 0)
        {
            return;
        }

        /*Checks if it's time to stop, then randomly selects next destination.*/
        if (nodesUntilWait == 0)
        {
            StartCoroutine(Wait());
        }

        NextNode();
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

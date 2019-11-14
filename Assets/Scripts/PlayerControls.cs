using UnityEngine;
using UnityEngine.AI;

public class PlayerControls : MonoBehaviour
{
    [Header("stats")]
    public float attackDistance;
    public float attackRate;
    private float nextAttack;

    private NavMeshAgent navMeshAgent;
    

    private Transform targetedEnemy;
    private bool enemyClicked;
    private bool walking;
    // Start is called before the first frame update
    void Awake()
    {
        
        navMeshAgent = GetComponent<NavMeshAgent>();
    }

    // Update is called once per frame
    void Update()
    {
        Ray ray = Camera.main.ScreenPointToRay(Input.mousePosition);
        RaycastHit hit;

        if (Input.GetButton("Fire1"))
        {
            if(Physics.Raycast(ray, out hit, 1000))
            {
                if(hit.collider.tag == "Enemy")
                {
                    targetedEnemy = hit.transform;
                    enemyClicked = true;
                }
                else
                {
                    walking = true;
                    enemyClicked = false;
                    navMeshAgent.isStopped = false;
                    navMeshAgent.destination = hit.point;
                }
            }
        }
        if(enemyClicked)
        {
            MoveAndAttack();
        }
        if(navMeshAgent.remainingDistance <= navMeshAgent.stoppingDistance)
        {
            walking = false;
        }
        else
        {
            walking = true;
        }
       
    }

    void MoveAndAttack()
    {
        if(targetedEnemy == null)
        {
            return;
        }
        navMeshAgent.destination = targetedEnemy.position;
        if(navMeshAgent.remainingDistance > attackDistance)
        {
            navMeshAgent.isStopped = false;
            walking = true;
        }
        else
        {
            transform.LookAt(targetedEnemy);
            Vector3 dirToAttack = targetedEnemy.transform.position - transform.position;

            if(Time.time > nextAttack)
            {
                nextAttack = Time.time + attackRate;
            }
            navMeshAgent.isStopped = true;
            walking = false;
        }
    }
}

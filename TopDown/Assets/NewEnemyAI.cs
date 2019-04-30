using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;
public class NewEnemyAI : MonoBehaviour
{

    //timer
    public float waitTime;
    private float waitTimeStart;
    //anim
    public Animator anim;
    //bools
    public bool isChasing;
    public bool isRoaming;
    public bool foundTarget;
    public Transform[] points;
    //agent
    public NavMeshAgent agent;
    //dest point
    [SerializeField]
    private int destPoint = 0;
    //enemies
    public GameObject[] enemies;
    //tag
    public string enemyTag;
    //radius
    public float targetRadius;
    public float AILookRadius;
    
    //target
    public Transform closestTarget;
    //visible targets;
    public List<Transform> visibleTargets = new List<Transform>();
    //finding visible targets
    public float viewRadius;
    [Range(0, 360)]
    public float viewAngle;
    //mask
    public LayerMask targetMask;
    public LayerMask obstacleMask;
    public float pointRoundOff;


    void Start()
    {
        waitTimeStart = waitTime;
        InvokeRepeating("FindTarget", 0f, 1f);
        GotoNextPoint();
       
    }


    void FindTarget()
    {

        enemies = GameObject.FindGameObjectsWithTag(enemyTag);
        float shortestDistance = Mathf.Infinity;
        GameObject nearestEnemy = null;
        foreach (GameObject enemy in enemies)
        {
            float distanceToEnemy = Vector3.Distance(transform.position, enemy.transform.position);
            if (distanceToEnemy < shortestDistance)
            {

                shortestDistance = distanceToEnemy;
                nearestEnemy = enemy;

            }
        }

        if (nearestEnemy != null && shortestDistance <= targetRadius)
        {

            closestTarget = nearestEnemy.transform;
            foundTarget = true;
        }


        if (shortestDistance >= targetRadius)
        {

            closestTarget = null;

        }

    }



    // Update is called once per frame
    void Update()
    {
        if (!agent.pathPending && agent.remainingDistance < pointRoundOff)
            GotoNextPoint();


        if (isRoaming)
            GotoNextPoint();


        if (closestTarget != null) {
            Vector3 direction = (closestTarget.transform.position - transform.position).normalized;
            Quaternion lookRotation = Quaternion.LookRotation(new Vector3(direction.x, 0, direction.z));
            transform.rotation = Quaternion.Slerp(transform.rotation, lookRotation, Time.deltaTime * 5f);
            float _distance = Vector3.Distance(closestTarget.transform.position, transform.position);
            if (_distance <= AILookRadius)
            {

                StartCoroutine("FindTargetsWithDelay", .25f);

            }

        }
       
    }

    IEnumerator FindTargetsWithDelay(float delay)
    {
        while (true)
        {
            yield return new WaitForSeconds(delay);
            FindVisibleTargets();
        }
    }

    void FindVisibleTargets()
    {
        visibleTargets.Clear();
        Collider[] targetsInViewRadius = Physics.OverlapSphere(transform.position, viewRadius, targetMask);

        for (int i = 0; i < targetsInViewRadius.Length; i++)
        {
            //Transform target = targetsInViewRadius[i].transform;
            Vector3 dirToTarget = (closestTarget.position - transform.position).normalized;
            if (Vector3.Angle(transform.forward, dirToTarget) < viewAngle / 2)
            {
                float dstToTarget = Vector3.Distance(transform.position, closestTarget.position);

                if (!Physics.Raycast(transform.position, dirToTarget, dstToTarget, obstacleMask))
                {
                    visibleTargets.Add(closestTarget);
                    OnInspect();
                    isRoaming = false;
                    isChasing = true;
                }
                else
                {
                    isRoaming = true;
                    isChasing = false;

                }
            }
        }
    }

    private void OnInspect()
    {
        if (isChasing)
            agent.SetDestination(closestTarget.position);
        else {
            isRoaming = true;
            isChasing = false;
            GotoNextPoint();
        }
           
    }

    void GotoNextPoint()
    {

        waitTime -= Time.deltaTime;
        isChasing = false;
        isRoaming = true;
        if (waitTime <= 0)
        {


            
            // Returns if no points have been set up
            if (points.Length == 0)
                return;


            // Set the agent to go to the currently selected destination.
            anim.SetBool("isRoaming", true);
            anim.SetBool("isChasing", false);
            agent.destination = points[destPoint].transform.position;
            
            // Choose the next point in the array as the destination,
            // cycling to the start if necessary.
            destPoint = (destPoint + 1) % points.Length;

            waitTime = waitTimeStart;
           
            float _dist = Vector3.Distance(points[destPoint].transform.position,transform.position);
            Debug.Log(_dist);
            if (_dist <= pointRoundOff)
            {

                

                    anim.SetBool("isRoaming", false);
                    anim.SetBool("isChasing", false);
                
            }
                
        }
        



    }

    public Vector3 DirFromAngle(float angleInDegrees, bool angleIsGlobal)
    {
        if (!angleIsGlobal)
        {
            angleInDegrees += transform.eulerAngles.y;
        }
        return new Vector3(Mathf.Sin(angleInDegrees * Mathf.Deg2Rad), 0, Mathf.Cos(angleInDegrees * Mathf.Deg2Rad));
    }
}

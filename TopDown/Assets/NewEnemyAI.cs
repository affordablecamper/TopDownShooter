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
    //misc
    public float pointRoundOff;
    private Vector3 direction;
    private float _distance;
    //shooting
    public float fireCountdown = 0f;
    public float fireRate = 1f;
    public float muzzleflashtime = 0.1f;
    private float muzzleFlashTimerStart;
    public AudioSource Audio;
    public AudioClip fire;
    public bool muzzleFlashEnable;
    public GameObject muzzleFlash;
    public bool canShoot;
    public int damage;
    public Transform shootPos;
    public bool playerisDead;
    public float botAimSpeed;
    public float shootDistance;
    void Start()
    {
        waitTimeStart = waitTime;
        InvokeRepeating("FindTarget", 0f, 1f);
        GotoNextPoint();
        waitTime = 0;
        muzzleFlashTimerStart = muzzleflashtime;
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
        


        //accuracy of bot
        if (closestTarget != null) {
            Vector3 dir = closestTarget.position - shootPos.position;
            Quaternion lookRotatation = Quaternion.LookRotation(dir);
            Vector3 rotation = Quaternion.Lerp(shootPos.rotation, lookRotatation, Time.deltaTime * botAimSpeed).eulerAngles;
            shootPos.rotation = Quaternion.Euler(rotation.x, rotation.y, rotation.z);
        }




        //checks if player is alive
        if (playerisDead) {
            closestTarget = null;
            this.enabled = false;
        }
            

        //shooting
        if (muzzleFlashEnable == true)
        {

            muzzleFlash.SetActive(true);
            muzzleflashtime -= Time.deltaTime;


        }


        if (muzzleflashtime <= 0)
        {
            muzzleFlash.SetActive(false);
            muzzleFlashEnable = false;
            muzzleflashtime = muzzleFlashTimerStart;



        }



        if (fireCountdown <= 0f)
        {


            OnShoot();


            fireCountdown = 1f / fireRate;


        }
        

        fireCountdown -= Time.deltaTime;






        if (isRoaming) {

           
                GotoNextPoint();
        }


       
        if (closestTarget != null) {
            direction = (closestTarget.transform.position - transform.position).normalized;
            
            _distance = Vector3.Distance(closestTarget.transform.position, transform.position);
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
                   
                    agent.speed = 4.5f;
                    visibleTargets.Add(closestTarget);
                    OnInspect();
                    isRoaming = false;
                    isChasing = true;
                    anim.SetBool("isChasing", true);
                    anim.SetBool("isRoaming", false);
                }
                else
                {
                    //agent.speed = 3.5f;
                    //waitTime = 0;
                    //isRoaming = true;
                    //isChasing = false;
                    //anim.SetBool("isChasing", false);
                    //anim.SetBool("isRoaming", true);
                    
                }
            }
            else {

                agent.speed = 3.5f;
                waitTime = 0;
                isRoaming = true;
                isChasing = false;
                anim.SetBool("isChasing", false);
                anim.SetBool("isRoaming", true);
            }
        }
    }

    private void OnInspect()
    {
        if (isChasing)
        {
            agent.SetDestination(closestTarget.position);
            //if (_distance <= agent.stoppingDistance + shootDistance)
            //{
                    //agent.isStopped = true;
                    //canShoot = true;
            //}
            

        
        }
        else {
            //isRoaming = true;
            //isChasing = false;
            //canShoot = false;
        }
           
    }

    private void OnShoot()
    {

        if (canShoot == true)
        {
            //shootPos.LookAt(closestTarget);
            Vector3 fwd = shootPos.transform.TransformDirection(Vector3.forward);
            Debug.DrawRay(shootPos.transform.position, fwd * 50, Color.red);

            RaycastHit hitInfo;
            muzzleFlashEnable = true;
            Audio.PlayOneShot(fire);


            if (Physics.Raycast(shootPos.transform.position, shootPos.transform.forward, out hitInfo, 1000000))
            {
                if (hitInfo.collider.tag == enemyTag)
                {





                    PlayerHealth _Player = hitInfo.collider.GetComponent<PlayerHealth>();
                    _Player.takeDamage(damage);


                    if (_Player.health <= 0)
                        playerisDead = true;


                }


            }
        }

    }

    void GotoNextPoint()
    {


        if (agent.remainingDistance <= 0)
        {

            waitTime -= Time.deltaTime;
            anim.SetBool("isRoaming", false);
            anim.SetBool("isChasing", false);
        }
        else {
            anim.SetBool("isRoaming", true);
            anim.SetBool("isChasing", false);

        }



        isChasing = false;
        isRoaming = true;
        if (waitTime <= 0)
        {


            
            // Returns if no points have been set up
            if (points.Length == 0)
                return;


            // Set the agent to go to the currently selected destination.
            
            agent.destination = points[destPoint].position;
            
            // Choose the next point in the array as the destination,
            // cycling to the start if necessary.
            destPoint = (destPoint + 1) % points.Length;

            waitTime = waitTimeStart;
           
            
            
                
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

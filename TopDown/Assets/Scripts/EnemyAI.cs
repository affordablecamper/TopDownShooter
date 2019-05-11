// Patrol.cs
using UnityEngine;
using UnityEngine.AI;
using System.Collections;
using System;

public class EnemyAI : MonoBehaviour
{
    [Header("AnimationStuff")]

    
    [Space]
    [Header("GameOjectInfo")]
    public Transform shootPos;
    
    //public GameObject Enemy;
    public GameObject muzzleFlash;
    // public GameObject closestPlayer;
    [Space]


    [Header("Bools")]

    [Space]
    [SerializeField]
    private bool findPlayer;
    
    public bool muzzleFlashEnable;
    
    [Header("Floats")]

    [Space]
    public float radius;
    private float fireCountdown = 0f;
    public float fireRate = 1f;
    public float muzzleflashtime = 0.1f;
    private float muzzleFlashTimerStart;
    public float minshootdistance;
    public AudioSource Audio;
    public AudioClip fire;
    [SerializeField]

    public float lookingTimer = 5f;
    private float lookingTimerStart;
    public float stuckTimer = 7.5f;
    private float stuckTimerStart;
    //public float detectDistance;
    
    // private float distance ;




    [Header("Integer")]

    [Space]
    public int Damage;

    [Header("AI Info")]

    [Space]
    [SerializeField]
    Transform target;
    private NavMeshAgent agent;
    public GameObject[] enemies;
    public string enemyTag;
    public GameObject[] points;
    public int destPoint = 0;
    //public bool smoothStop;
    public Animator anim;
    public float pointRoundOff;
    public float waitTime;
    private float waitTimeStart;
    public bool isRoaming;
    public bool isChasing;
    public float targetRadius;
    public float detectRadius;
    public bool canShoot;
    public PlayerHealth _Player;
    public bool playerisDead;
    public float viewDistance;
    void Start()
    {
        isRoaming = true;
        waitTimeStart = waitTime;
        agent = GetComponent<NavMeshAgent>();
        //points = GameObject.FindGameObjectsWithTag("Waypoint");
        Audio = GetComponent<AudioSource>();
        agent = GetComponent<NavMeshAgent>();
        
        muzzleFlashTimerStart = muzzleflashtime;
        lookingTimerStart = lookingTimer;
        stuckTimerStart = stuckTimer;
        GotoNextPoint();
        InvokeRepeating("UpdateTarget", 0f, 0.5f);
    }


    void UpdateTarget()
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

            target = nearestEnemy.transform;
            findPlayer = true;
        }


        if (shortestDistance >= targetRadius)
        {
       
            target = null;

        }

    }


    void GotoNextPoint()
    {
        
        waitTime -= Time.deltaTime;
        isChasing = false;
        
        if (waitTime <= 0)
        {
            
            // Returns if no points have been set up
            if (points.Length == 0)
                return;

           
            // Set the agent to go to the currently selected destination.
            anim.SetBool("isRoaming", true);
            agent.destination = points[destPoint].transform.position;
            
            // Choose the next point in the array as the destination,
            // cycling to the start if necessary.
            destPoint = (destPoint + 1) % points.Length;

            waitTime = waitTimeStart;
        }
        else {
            
            anim.SetBool("isRoaming", false);
            anim.SetBool("isChasing", false);
        }
        
    }


    void Update()
    {

        if (playerisDead == true)
            target = null;
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


            canShoot = true;


            fireCountdown = 1f / fireRate;


        }
        else canShoot = false;

        fireCountdown -= Time.deltaTime;





        RaycastHit hitInfo;
        
        shootPos.LookAt(target);

        if (Physics.SphereCast(transform.position, detectRadius,transform.forward, out hitInfo, viewDistance))
        {
            if (hitInfo.collider.tag == "Player")
            {

                transform.LookAt(target);
                Debug.Log("ai can hit target");
                isRoaming = false;
                isChasing = true;





            }
            else if (hitInfo.collider.tag != "Player")
            {

                isRoaming = true;
                isChasing = false;
                anim.SetBool("IsRoaming", true);
                
               // Debug.Log(this.transform + " Cant hit the target and is repositioning");

            }


        }




        
            Vector3 direction = (target.transform.position - transform.position).normalized;
            Quaternion lookRotation = Quaternion.LookRotation(new Vector3(direction.x, 0, direction.z));
            transform.rotation = Quaternion.Slerp(transform.rotation, lookRotation, Time.deltaTime * 5f);
            float _distance = Vector3.Distance(target.transform.position, transform.position);

            if (_distance <= agent.stoppingDistance + minshootdistance &&!isRoaming &&!canShoot)
           {
                Debug.Log("Agent might be stuck checking with timer");
                stuckTimer -= Time.deltaTime;
                if (stuckTimer <= 0)
                {

                   Debug.Log(this.transform + " Died due to not reaching target in time");
                   Destroy(this.gameObject, 2f);
                }


            }
            else
            {

                stuckTimer = stuckTimerStart;
            }

            if (isChasing)
                if (_distance <= radius)
                {

                    //CmdOnMove();
                    agent.SetDestination(target.position);
                    anim.SetBool("isChasing", true);
                    agent.isStopped = false;

                }


            if (_distance >= radius) {
                agent.isStopped = true;
                anim.SetBool("isChasing", false);
                lookingTimer -= Time.deltaTime;
                if (lookingTimer <= 0)
                {
                    waitTime = 0;
                    anim.SetBool("isRoaming", true);
                    agent.isStopped = false;
                    GotoNextPoint();
                    isRoaming = true;
                    isChasing = false;
                }


            }
            else
            {

                lookingTimer = lookingTimerStart;
            }




            if(isChasing)
            if (_distance <= agent.stoppingDistance + minshootdistance)
            {
                
                agent.isStopped = true;
                anim.SetBool("isChasing", false);
                anim.SetBool("isRoaming", false);

                OnShoot();



            }
            else {
                
            }


        

        if (isRoaming) {
            //anim.SetBool("isRoaming", true);
            if (!agent.pathPending && agent.remainingDistance < pointRoundOff)
                GotoNextPoint();
            
        }
           
    }

    private void OnShoot()
    {
        if (canShoot == true)
        {
            shootPos.LookAt(target);
            Vector3 fwd = shootPos.transform.TransformDirection(Vector3.forward);
            Debug.DrawRay(shootPos.transform.position, fwd * 50, Color.red);

            RaycastHit hitInfo;
            muzzleFlashEnable = true;
            Audio.PlayOneShot(fire);
            
                
            if (Physics.Raycast(shootPos.transform.position, shootPos.transform.forward, out hitInfo, 1000000))
            {
                if (hitInfo.collider.tag == enemyTag)
                {


                 


                    _Player = hitInfo.collider.GetComponent<PlayerHealth>();
                    _Player.takeDamage(Damage, transform.forward);


                    if (_Player.health <= 0)
                        playerisDead = true;


                }


            }


        }
    }

    private void OnDrawGizmosSelected()
    {


        Gizmos.color = Color.green;
        Gizmos.DrawWireSphere(transform.position, radius);
        Gizmos.color = Color.red;
        Gizmos.DrawWireSphere(transform.position, minshootdistance);
        Gizmos.color = Color.yellow;
        Gizmos.DrawWireSphere(transform.position, targetRadius);
        
    }

    //private void OnDrawGizmos()
    //{
        //Gizmos.color = Color.green;
        //Gizmos.DrawSphere(transform.position, detectRadius);
        //if (isChasing) {
            //Gizmos.color = Color.red;
        //}
        
    //}

}
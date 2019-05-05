﻿using System;
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
    public GameObject[] weapons;
    //tag
    public string enemyTag;
    //radius
    
    
    //target
    public Transform target;
    public Transform weaponTarget;
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
    public string weaponTag;
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
    public bool canSee;
    public float inspectTimer;
    private float inspectTimerStart;
    public bool droppedWeapon;
    public GameObject weapon;
    void Start()
    {
        waitTimeStart = waitTime;
        
        GotoNextPoint();
        waitTime = 0;
        muzzleFlashTimerStart = muzzleflashtime;
        inspectTimerStart = inspectTimer;
        target = GameObject.FindGameObjectWithTag("Player").transform;
    }


    


    // Update is called once per frame
    void Update()
    {

        if (droppedWeapon) {
            isRoaming = false;
            isChasing = false;
            canShoot = false;
            LookForWeapon();

        }
        if (!droppedWeapon)
        {

            if (isRoaming)
            {
                isChasing = false;
                anim.SetBool("isChasing", false);


            }
            if (canShoot)
            {
                anim.SetBool("isRoaming", false);
                anim.SetBool("isChasing", false);
            }


            if (isChasing)
            {
                isRoaming = false;
                anim.SetBool("isChasing", true);
            }



            if (visibleTargets.Count <= 0)
            {

                canSee = false;
                canShoot = false;
                if (!canSee && !isRoaming)
                {
                    inspectTimer -= Time.deltaTime;
                    if (inspectTimer <= 0)
                    {

                        isRoaming = true;
                    }
                    else
                    {
                        isChasing = true;
                        agent.isStopped = false;
                        agent.SetDestination(target.position);

                    }

                }
            }



            //checks if player is alive
            if (playerisDead)
            {
                target = null;
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

                if (canShoot && !droppedWeapon)
                    OnShoot();




                fireCountdown = 1f / fireRate;


            }


            fireCountdown -= Time.deltaTime;






            if (isRoaming && !droppedWeapon)
            {


                GotoNextPoint();
            }



            if (target != null && !droppedWeapon)
            {
                direction = (target.transform.position - transform.position).normalized;
                StartCoroutine("FindTargetsWithDelay", .125f);
                _distance = Vector3.Distance(target.transform.position, transform.position);
                if (_distance <= viewRadius)
                {



                }
                else
                {
                    
                    canShoot = false;
                    canSee = false;
                }

            }




        }


    }

    void LookForWeapon()
    {

        weapons = GameObject.FindGameObjectsWithTag(weaponTag);
        float shortestDistance = Mathf.Infinity;
        GameObject nearestEnemy = null;
        foreach (GameObject gun in weapons)
        {
            float distanceToEnemy = Vector3.Distance(transform.position, gun.transform.position);
            if (distanceToEnemy < shortestDistance)
            {

                shortestDistance = distanceToEnemy;
                nearestEnemy = gun;

            }
        }

        if (nearestEnemy != null && shortestDistance <= viewRadius)
        {

            weaponTarget = nearestEnemy.transform;
            
        }


        if (shortestDistance >= viewRadius)
        {

            weaponTarget = null;

        }

        GoToWeaponTarget();
    }

    private void GoToWeaponTarget()
    {
        agent.isStopped = false;
        agent.SetDestination(weaponTarget.position);
        anim.SetBool("isRoaming", true);
        if (agent.remainingDistance <= 0) {
            Destroy(weaponTarget.gameObject);
            weapon.SetActive(true);
            droppedWeapon = false;
            isRoaming = true;
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
        RaycastHit hitInfo;
        for (int i = 0; i < targetsInViewRadius.Length; i++)
        {
            //Transform target = targetsInViewRadius[i].transform;
            Vector3 dirToTarget = (target.position - transform.position).normalized;
            if (Vector3.Angle(transform.forward, dirToTarget) < viewAngle / 2)
            {
                float dstToTarget = Vector3.Distance(transform.position, target.position);


                if (!Physics.Raycast(transform.position, dirToTarget, dstToTarget, obstacleMask))
                {
                    shootPos.LookAt(target);
                    if (Physics.Raycast(shootPos.transform.position, shootPos.transform.forward, out hitInfo))
                    {

                        if (hitInfo.collider.tag == "Player")
                        {

                            visibleTargets.Add(hitInfo.transform);
                            isRoaming = false;
                            canSee = true;
                            canShoot = true;
                            agent.isStopped = true;
                            anim.SetBool("isRoaming", false);
                            inspectTimer = inspectTimerStart;

                        }


                    }

            }   }  
        }
    }

    

    private void FixedUpdate()
    {
        if (canShoot) {

            Vector3 dir = target.position - transform.position;
            Quaternion lookRotatation = Quaternion.LookRotation(dir);
            Vector3 rotation = Quaternion.Lerp(transform.rotation, lookRotatation, Time.deltaTime * botAimSpeed).eulerAngles;
            transform.rotation = Quaternion.Euler(rotation.x, rotation.y, rotation.z);
        }
        
    }

    private void OnShoot()
    {

        
            //shootPos.LookAt(closestTarget);
            
            

            
            RaycastHit hitInfo;
            muzzleFlashEnable = true;
            Audio.PlayOneShot(fire);


            if (Physics.Raycast(transform.position, transform.forward, out hitInfo, 1000000))
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

    void GotoNextPoint()
    {

        agent.isStopped = false;
        if (agent.remainingDistance <= 0)
        {

            waitTime -= Time.deltaTime;
            anim.SetBool("isRoaming", false);
            
        }
        else {
            anim.SetBool("isRoaming", true);
            

        }



        
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
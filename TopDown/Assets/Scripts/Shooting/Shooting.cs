﻿using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using EZCameraShake;
using UnityEngine.UI;
public class Shooting : MonoBehaviour
{

    public Behaviour thisScript;
    public CameraShake cameraShake;
    [Header("UI")]
    public Text magAmmoText;
    public Text maxMagazineSizeText;

    [Space]
    [Header("Anim")]
    public Animator anim;

    [Space]
    [Header("Audio")]
    public AudioClip gunshotClip;
    public AudioSource source;
    public AudioClip outOfAmmo;
    [Space]
    [Header("Ammo")]
    public int magAmmo = 30;            //Total mag ammo.
    public int reserveAmmo = 90;            //Ammo that is left or reserved.
    public int maxMagazineSize = 30; //The total Size of the mag.
    [Space]
    [Header("CameraShake")]
    public float Magnitude = 2f;
    public float Roughness = 10f;
    public float FadeOutTime = 5f;
    [Space]
    [Header("Floats/Values")]
    public float weaponRange = 100f;    //Range of the weapon.
    public float tracerPower;               //The thrust of the tracer.
    public float throwPower;
    public float fireRate = 1f;
    private float fireCountdown = 0f;
    private float muzzleFlashTimerStart;
    public float muzzleflashtime = 0.1f;
    public int damage = 1;
    [Header("GameObjects")]
    [Space]
    public GameObject muzzleFlashObject; //muzzle flash
    public GameObject bulletCasing; //bullet casing prefab
    public GameObject tracer;
    public GameObject throwGun;
    public GameObject mp5;
    [Space]
    [Header("Transforms")]
    public Transform bulletCasingSpawn; //The location of the "bulletcasingspawn"
    public Transform fwd;             //The forward working direction that the "tracer" gameobject comes out from
    public Transform shootPos;

    
   
    [Space]
    [Header("Booleans")]
    public bool canShoot;              //If the weapon can shoot or not.
    public bool muzzleFlashEnabled = false;//If the muzzle flash is enabled.
    public bool magEmpty;              //If the mag is empty.
    //public bool enabled;


    
    private void Start()
    {
        muzzleFlashTimerStart = muzzleflashtime;
        canShoot = true;
        source = GetComponent<AudioSource>();
        maxMagazineSizeText.text = maxMagazineSize.ToString();
    }

    // Update is called once per frame
    void Update()
    {
        magAmmoText.text = magAmmo.ToString();


        if (Input.GetButtonDown("Fire2"))
        {

            GameObject gunProjectile = Instantiate(throwGun, shootPos.transform.position, shootPos.transform.rotation) as GameObject;
            gunProjectile.GetComponent<Rigidbody>().AddForce(fwd.transform.forward.normalized * throwPower);
            mp5.SetActive(false);
            thisScript.enabled = false;

        }


            if (Input.GetButtonDown("Fire1") && magEmpty == true)
            source.PlayOneShot(outOfAmmo);

        if (magAmmo <= 0)
            {

                magEmpty = true;


            }
            else magEmpty = false;
            


        if (Input.GetButtonDown("Fire1")&&magEmpty == false)
            anim.SetBool("Shoot", true);
        if (Input.GetButtonUp("Fire1"))
            anim.SetBool("Shoot", false);


        Debug.DrawRay(shootPos.transform.position, shootPos.forward * weaponRange, Color.green);
        if (muzzleFlashEnabled == true)
        {

            muzzleFlashObject.SetActive(true);
            muzzleflashtime -= Time.deltaTime;
           
        }

        if (muzzleflashtime <= 0)
        {
            muzzleFlashObject.SetActive(false);
            muzzleFlashEnabled = false;
            muzzleflashtime = muzzleFlashTimerStart;
            
        }

        if (fireCountdown <= 0f)
        {


            if (canShoot == true)
            {
                
                
                Shoot();
                fireCountdown = 1f / fireRate;
            }

        }

        fireCountdown -= Time.deltaTime;



    }

    private void Shoot()
    {



            if (Input.GetButton("Fire1") && magEmpty == false)
            {
                GameCamera.ToggleShake(Magnitude);
                GameObject newProjectile = Instantiate(tracer, fwd.transform.position, fwd.transform.rotation) as GameObject;
                newProjectile.GetComponent<Rigidbody>().AddForce(fwd.transform.forward.normalized * tracerPower);
                Instantiate(bulletCasing, bulletCasingSpawn.position, bulletCasingSpawn.transform.rotation);
                magAmmo -= 1;
                muzzleFlashEnabled = true;
                //CameraShaker.Instance.ShakeOnce(Magnitude, Roughness, 0, FadeOutTime);
                source.PlayOneShot(gunshotClip);
                RaycastHit hitInfo;
                //StartCoroutine(cameraShake.Shake(FadeOutTime, Magnitude));
            if (Physics.Raycast(shootPos.transform.position, shootPos.transform.forward, out hitInfo, weaponRange))
                {
                
                    if (hitInfo.collider.tag == "Enemy") {

                    EnemyHealth enem = hitInfo.collider.GetComponent<EnemyHealth>();
                    enem.takeDamage(damage);

                }
                }
            }

    }
}

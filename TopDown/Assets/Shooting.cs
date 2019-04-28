using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Shooting : MonoBehaviour
{
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
    public float fireRate = 1f;
    private float fireCountdown = 0f;
    private float muzzleFlashTimerStart;
    public float muzzleflashtime = 0.1f;
    [Header("GameObjects")]
    [Space]
    public GameObject muzzleFlashObject; //muzzle flash
    public GameObject bulletCasing; //bullet casing prefab
    [Space]
    [Header("Transforms")]
    public Transform bulletCasingSpawn; //The location of the "bulletcasingspawn"
    public Transform fwd;             //The forward working direction that the "tracer" gameobject comes out from
    [Space]
    [Header("Audio")]
    private AudioSource Audio;         //The audio source
    public AudioClip gunshotClip;      //Audio stuff
    [Space]
    [Header("Booleans")]
    public bool canShoot;              //If the weapon can shoot or not.
    public bool muzzleFlashEnabled = false;//If the muzzle flash is enabled.
    public bool magEmpty;              //If the mag is empty.
    
    
    
    // Update is called once per frame
    void Update()
    {
        if (Input.GetButton("Fire1")) {

            
        }
    }
}

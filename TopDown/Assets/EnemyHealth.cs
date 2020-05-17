using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.AI;
using TMPro;

public class EnemyHealth : MonoBehaviour
{
    private bool _isDead = false;
    [SerializeField]
    public float health;
    public bool isDead;
    public Rigidbody[] bodyParts;
    public Killstreak streak;
    public AudioSource source;
    public AudioClip deathNoise;
    public float throwPower;
    public Transform shootPos;
    public GameObject throwGun;
    private float fixedDeltaTime;
    public Score score;
    public GameObject addText;
    public GameObject canvas;
    public TextMeshPro mText;
    public GameObject weapon;
    public CountEnemies countEnem;
    public float force = 500f;
    public Behaviour[] toDisable;
    [SerializeField]
    private Transform hitBodyPart;
    
    void Update ()
	{
        Time.timeScale += (1.0f / 1f) * Time.unscaledDeltaTime;
        Time.timeScale = Mathf.Clamp(Time.timeScale, 0f, 1f);
        if (Time.timeScale == 1.0f)
        {
            Time.fixedDeltaTime = Time.deltaTime;
        }
    }


    public void takeDamage(float __amount, Transform bodypart)
    {
        hitBodyPart = bodypart;
        if (isDead)
            return;


        health -= __amount;
        
        if (health <= 0)
        {

           
            die();
            

        }

    }

    
    
    
    public void die()
    {
        weapon.SetActive(false);
        countEnem.count--;
        foreach (Behaviour b in toDisable)
        {
            b.enabled = false;
        }


        source.PlayOneShot(deathNoise);
        isDead = true;
        
        foreach (Rigidbody rb in bodyParts)
        {
            rb.isKinematic = false;
            
        }
        GameObject textAdd = Instantiate(addText, transform.position, addText.transform.rotation) as GameObject;
        mText = textAdd.GetComponent<TextMeshPro>();

        textAdd.transform.SetParent(canvas.transform);
        
        score.AddPoints(150f);

        mText.text = "+ " + score.multipliedScore.ToString();

        Rigidbody _rb = hitBodyPart.GetComponent<Rigidbody>();
        //_rb.AddExplosionForce(9000f, hitBodyPart.position, 2.25f);
        Transform player = GameObject.FindGameObjectWithTag("Player").transform;
        _rb.AddForce(player.forward * 5000f);
        Time.timeScale = .1f;
        Time.fixedDeltaTime = Time.timeScale * .02f;
        GameObject gunProjectile = Instantiate(throwGun, shootPos.position, shootPos.transform.rotation) as GameObject;
        gunProjectile.GetComponent<Rigidbody>().AddForce(shootPos.forward.normalized * throwPower);
        streak.addKill();


    }

   

}

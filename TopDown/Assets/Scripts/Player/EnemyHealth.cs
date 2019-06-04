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
    public GameObject ragdoll;
    public float knockOutTime;
    private GameObject rgk;
    public GameObject gfx;
    public NewEnemyAI aiScript;
    public GameObject weapon;
    public GameObject throwGun;
    public GameObject shootPos;
    public float throwPower;
    public bool knockedOut;
    public CountEnemies countEnem;
    public NavMeshAgent agent;
    public TimeManager timeManage;
    public Score score;
    public GameObject addText;
    public GameObject canvas;
    public TextMeshPro mText;
    public Killstreak streak;
    public Rigidbody[] bodyParts;
    public Animator anim;
    private void Start()
    {
        agent = GetComponent<NavMeshAgent>();
        aiScript = GetComponent<NewEnemyAI>();
    }
    public void takeDamage(float __amount)
    {

        if (isDead)
            return;


        health -= __amount;
        
        if (health == 0)
        {


            die();
            

        }

    }

    
    public void knockOut() {

        GameObject gunProjectile = Instantiate(throwGun, shootPos.transform.position, shootPos.transform.rotation) as GameObject;
        gunProjectile.GetComponent<Rigidbody>().AddForce(shootPos.transform.forward.normalized * throwPower);
        

        foreach (Rigidbody rb in bodyParts)
        {
            rb.isKinematic = false;
        }
        anim.enabled = false;
        //rgk = (GameObject)Instantiate(ragdoll, transform.position, Quaternion.identity);
        StartCoroutine(BackUp());
        aiScript.enabled = false;

        agent.isStopped = true;
        weapon.SetActive(false);
        knockedOut = true;
        
        aiScript.droppedWeapon = true;
    }

    IEnumerator BackUp()
    {

        
        yield return new WaitForSeconds(knockOutTime);
        foreach (Rigidbody rb in bodyParts)
        {
            rb.isKinematic = true;
        }
        Destroy(rgk);
        aiScript.enabled = true;
        knockedOut = false;
        weapon.SetActive(true);
        anim.enabled = true;
        agent.isStopped = false;
    }

    public void die()
    {
        countEnem.count--;
        aiScript.enabled = false;
        anim.enabled = false;
        weapon.SetActive(false);
        foreach (Rigidbody rb in bodyParts)
        {
            rb.isKinematic = false;
        }
        GameObject textAdd = Instantiate(addText, transform.position, addText.transform.rotation) as GameObject;
        mText = textAdd.GetComponent<TextMeshPro>();
        
        textAdd.transform.SetParent(canvas.transform);
        streak.addKill();
        score.AddPoints(150f);

        mText.text = "+ "  + score.multipliedScore.ToString();
        
        
        GameObject gunProjectile = Instantiate(throwGun, shootPos.transform.position, shootPos.transform.rotation) as GameObject;
        gunProjectile.GetComponent<Rigidbody>().AddForce(shootPos.transform.forward.normalized * throwPower);
        //timeManage.slowdownLength = .55f;
        //timeManage.DoSlowmotion();
        //Destroy(rg, 5f);
        //gameObject.SetActive(false);
        
    }
}

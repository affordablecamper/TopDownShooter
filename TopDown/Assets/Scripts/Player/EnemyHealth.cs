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
    public Collider col;
    public NavMeshAgent agent;
    public TimeManager timeManage;
    public Score score;
    public GameObject addText;
    public GameObject canvas;
    public TextMeshPro mText;
    public Killstreak streak;
    
    private void Start()
    {
        agent = GetComponent<NavMeshAgent>();
        aiScript = GetComponent<NewEnemyAI>();
    }
    public void takeDamage(float __amount, Vector3 fwd)
    {

        if (isDead)
            return;


        health -= __amount;
        if (health == 0)
        {


            this.enabled = false;
            die(fwd);

        }

    }

    
    public void knockOut() {

        if (aiScript.droppedWeapon == false) {
            GameObject gunProjectile = Instantiate(throwGun, shootPos.transform.position, shootPos.transform.rotation) as GameObject;
            gunProjectile.GetComponent<Rigidbody>().AddForce(shootPos.transform.forward.normalized * throwPower);

        }
        rgk = (GameObject)Instantiate(ragdoll, transform.position, Quaternion.identity);
        StartCoroutine(BackUp());
        aiScript.enabled = false;
        col.enabled = false;
        agent.isStopped = true;
        weapon.SetActive(false);
        knockedOut = true;
        gfx.SetActive(false);
        aiScript.droppedWeapon = true;
    }

    IEnumerator BackUp()
    {

        
        yield return new WaitForSeconds(knockOutTime);
        Destroy(rgk);
        aiScript.enabled = true;
        knockedOut = false;
        col.enabled = true;
        gfx.SetActive(true);
        agent.isStopped = false;
    }

    public void die(Vector3 fwd)
    {

        GameObject rg = (GameObject)Instantiate(ragdoll, transform.position, Quaternion.identity);
        GameObject textAdd = Instantiate(addText, transform.position, addText.transform.rotation) as GameObject;
        mText = textAdd.GetComponent<TextMeshPro>();       
        
        textAdd.transform.SetParent(canvas.transform);
        streak.addKill();
        score.AddPoints(150f);

        mText.text = "+ "  + score.multipliedScore.ToString();
        rg.transform.Find("spine").GetComponent<Rigidbody>().AddForce(fwd.normalized * 1500f);
        
        GameObject gunProjectile = Instantiate(throwGun, shootPos.transform.position, shootPos.transform.rotation) as GameObject;
        gunProjectile.GetComponent<Rigidbody>().AddForce(shootPos.transform.forward.normalized * throwPower);
        //timeManage.slowdownLength = .55f;
        //timeManage.DoSlowmotion();
        //Destroy(rg, 5f);
        gameObject.SetActive(false);

    }
}

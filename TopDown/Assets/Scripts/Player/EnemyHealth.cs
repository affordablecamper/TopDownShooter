using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
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
    private void Start()
    {

        aiScript = GetComponent<NewEnemyAI>();
    }
    public void takeDamage(int __amount)
    {

        if (isDead)
            return;


        health -= __amount;
        if (health <= 0)
        {


            this.enabled = false;
            die();

        }

    }

    
    public void knockOut() {

        rgk = (GameObject)Instantiate(ragdoll, transform.position, Quaternion.identity);
        StartCoroutine(BackUp());
        aiScript.enabled = false;
        col.enabled = false;
        GameObject gunProjectile = Instantiate(throwGun, shootPos.transform.position, shootPos.transform.rotation) as GameObject;
        gunProjectile.GetComponent<Rigidbody>().AddForce(shootPos.transform.forward.normalized * throwPower);
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
    }

    public void die()
    {

        GameObject rg = (GameObject)Instantiate(ragdoll, transform.position, Quaternion.identity);
        GameObject gunProjectile = Instantiate(throwGun, shootPos.transform.position, shootPos.transform.rotation) as GameObject;
        gunProjectile.GetComponent<Rigidbody>().AddForce(shootPos.transform.forward.normalized * throwPower);
        //Destroy(rg, 5f);
        Destroy(gameObject);

    }
}

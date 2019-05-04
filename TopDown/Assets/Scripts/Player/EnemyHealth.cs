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
    
    private void Start()
    {


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





    public void die()
    {

        GameObject rg = (GameObject)Instantiate(ragdoll, transform.position, Quaternion.identity);
        Destroy(rg, 5f);
        Destroy(gameObject);

    }
}

using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;
public class PlayerHealth : MonoBehaviour
{
    private bool _isDead = false;
    [SerializeField]
    public float health;
    public bool isDead;
    public GameObject ragdoll;
    public GameObject GFX;
    private GameObject text;
    public Behaviour[] toDisable;
    public CapsuleCollider collider;
  
    public GameObject gameOver;
    private void Start()
    {
       
        
    }
    public void takeDamage(int __amount)
    {
       
        if (isDead)
            return;
       
        
        health -= __amount;
        if (health == 0)
        {



            die();

        }

    }


    public void Update()
    {
        if (health <= 0) {

            if (Input.GetKeyDown(KeyCode.R))
                SceneManager.LoadSceneAsync(SceneManager.GetActiveScene().buildIndex);

        }
    }


    public void die()
    {
        
       GameObject rg = (GameObject)Instantiate(ragdoll, transform.position, Quaternion.identity);
       //Destroy(rg, 5f);
       GFX.SetActive(false);
       collider.enabled = false;
        foreach (Behaviour b in toDisable)
        {
            b.enabled = false;
        }
        this.enabled = false;
        
        gameOver.SetActive(true);
    }
}

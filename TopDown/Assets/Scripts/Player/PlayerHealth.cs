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
    public TimeManager timeManage;
    public GameObject gameOver;
    private void Start()
    {
       
        
    }
    public void takeDamage(int __amount, Vector3 fwd)
    {
       
        if (isDead)
            return;
       
        
        health -= __amount;
        if (health == 0)
        {



            die(fwd);

        }

    }


    public void Update()
    {
        if (health <= 0) {

            if (Input.GetKeyDown(KeyCode.R))
                SceneManager.LoadSceneAsync(SceneManager.GetActiveScene().buildIndex);

        }
    }


    public void die(Vector3 fwd)
    {
       //timeManage.slowdownLength = 2f;
       //timeManage.DoSlowmotion();
       GameObject rg = (GameObject)Instantiate(ragdoll, transform.position, Quaternion.identity);
       rg.transform.Find("spine").GetComponent<Rigidbody>().AddForce(fwd.normalized * 1500f);
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

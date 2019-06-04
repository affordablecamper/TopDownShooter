using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DamageInfo : MonoBehaviour
{
    public EnemyHealth health;
    public float force = 250f;
    public bool hips;
    public GameObject bloodPool;
    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    public void knockOut()
    {
        health.knockOut();

    }

    public void takeDamage(float __amount, Vector3 fwd)
    {
        if (!hips) {
            GameObject blood = Instantiate(bloodPool, transform.position, transform.rotation) as GameObject;
            Destroy(blood, 5f);
            transform.localScale = new Vector3(0, 0, 0);
            //Debug.Log("Damage info working");
            Rigidbody rb = GetComponent<Rigidbody>();
            rb.AddExplosionForce(force, transform.position, 5f);
        }
        
        health.takeDamage(__amount);
        
    }
}

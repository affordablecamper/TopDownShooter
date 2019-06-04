using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ThrowableObject : MonoBehaviour
{
    public AudioSource source;
    public AudioClip clip;
    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    

    void OnCollisionEnter(Collision col)
    {

        if (col.gameObject.tag == "EnemyBody")
        {
            if (col.relativeVelocity.magnitude > 1) {
                //DamageInfo enem = col.collider.GetComponent<DamageInfo>();
                //enem.knockOut();
                source.PlayOneShot(clip);
            }
                
        }
    }



}

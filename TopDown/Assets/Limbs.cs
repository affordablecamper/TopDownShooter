using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Limbs : MonoBehaviour
{
    public EnemyHealth health;
    public float force = 500f;
    public bool hips;
    public void SendDamage(int damage)
    {

        if (!hips)
        {
            
            transform.localScale = new Vector3(0, 0, 0);
            health.takeDamage(damage,transform);
            
        }
        else {
            
            
            health.takeDamage(damage,transform);
            
        }
       


        
    }


}

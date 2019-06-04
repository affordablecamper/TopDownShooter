using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ExplosiveBarrel : MonoBehaviour
{
    public float range;
    public float audioRange;
    public GameObject Explosion;
    public GameObject fire;
    public float explosionForce;
    public void Explode() {

        Instantiate(Explosion, transform.position, transform.rotation);
        Instantiate(fire, transform.position, transform.rotation);



        Collider[] colliders = Physics.OverlapSphere(transform.position, range);
        foreach (Collider nearbyObject in colliders)
        {


            Rigidbody rb = nearbyObject.GetComponent<Rigidbody>();
            if (rb != null)
                rb.AddExplosionForce(explosionForce, transform.position, range);


            if (nearbyObject != null && nearbyObject.gameObject.tag == "EnemyBody")
            {

                DamageInfo enem = nearbyObject.GetComponent<DamageInfo>();
                enem.takeDamage(1, transform.forward);
            }

                if (nearbyObject != null && nearbyObject.gameObject.tag == "Player")
            {

                nearbyObject.GetComponent<PlayerHealth>().takeDamage(1, transform.forward);
            }

            

        }



        AlertEnemies();
        Destroy(gameObject);
    }

    private void AlertEnemies()
    {
        RaycastHit[] hits = Physics.SphereCastAll(transform.position, audioRange, transform.up);
        foreach (RaycastHit hit in hits)
        {

            if (hit.collider != null && hit.collider.tag == "Enemy")
            {
                Debug.Log(hit);
                hit.collider.GetComponent<NewEnemyAI>().SetAlertPos(transform.position);
            }
        }
    }

}

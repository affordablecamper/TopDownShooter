using UnityEngine;
using System.Collections;
using UnityEngine.SceneManagement;
public class Laser : MonoBehaviour
{
    public Transform muzzle;
    private LineRenderer lr;
    public Transform target;
    // Use this for initialization
    void Start()
    {
        lr = GetComponent<LineRenderer>();
    }

    // Update is called once per frame
    void Update()
    {
        
        


        RaycastHit hit;
        Debug.DrawRay(muzzle.position, muzzle.forward * 100,Color.red);
        if (Physics.Raycast(muzzle.position, muzzle.forward, out hit))
        {
            if (hit.collider)
                lr.SetPosition(0, muzzle.position);
                lr.SetPosition(1, hit.point);

        }
        

    }
}
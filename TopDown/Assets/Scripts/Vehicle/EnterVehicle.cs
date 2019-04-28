using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnterVehicle : MonoBehaviour
{

    public bool inRange;
    public Behaviour[] playerBehaviours;
    public GameObject[] playerObjectsToDisable;
    public Behaviour[] vehicleBehavioursToDisable;
    public CapsuleCollider _collider;
    public Behaviour followTarget;
    public GameObject vehicleCam;
    public GameObject playerCam;
    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        if (inRange) {
            if (Input.GetKey(KeyCode.F))
            {
                followTarget.enabled = true;
                vehicleCam.SetActive(true);
                _collider.enabled = false;
                playerCam.SetActive(false);
                foreach (Behaviour be in vehicleBehavioursToDisable)
                {
                    be.enabled = true;
                }

                foreach (GameObject go in playerObjectsToDisable)
                {
                    go.SetActive(false);
                }

                foreach (Behaviour be in playerBehaviours)
                {
                    be.enabled = false;
                }

            }
            else if(Input.GetKeyDown(KeyCode.Space)){
                followTarget.enabled = false;
                _collider.enabled = true;
                vehicleCam.SetActive(false);
                playerCam.SetActive(true);
                foreach (GameObject go in playerObjectsToDisable)
                {
                    go.SetActive(true);
                }
                foreach (Behaviour be in playerBehaviours)
                {
                    be.enabled = true;
                }

              

                foreach (Behaviour be in vehicleBehavioursToDisable)
                {
                    be.enabled = false;
                }

            }
        }

    }

    private void OnTriggerStay(Collider other)
    {
        inRange = true;
    }

    private void OnTriggerExit(Collider other)
    {
        inRange = false;
    }
}

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
    public bool inCar;
    // Start is called before the first frame update
    void Start()
    {
        inCar = false;
    }

    // Update is called once per frame
    void Update()
    {
        
            
           



            if(Input.GetKeyDown(KeyCode.F)){
                if (!inCar)
                {
                if (inRange)
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
                        inCar = true;
                    }
                }
                else
                {
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
                    inCar = false;
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

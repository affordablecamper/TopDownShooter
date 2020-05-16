using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TopDownCamera : MonoBehaviour
{

    public Transform target;
    public Camera cam;
    public float maxDistance;

    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void FixedUpdate()
    {
        
        Vector3 cursorPos = Camera.main.ScreenToViewportPoint(Input.mousePosition);
        Vector3 diff = target.position - cursorPos;




    }
}

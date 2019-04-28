using UnityEngine;
using System.Collections;
using System;

[RequireComponent(typeof(PlayerController))]
public class Player : MonoBehaviour
{

    public float moveSpeed = 5;
    public GameObject mp5;
    public Behaviour shootingScript;
    public Camera viewCamera;
    PlayerController controller;
    private Vector3 relativePoint;
    public Animator anim;
    Transform camTransform;
    Vector3 camForward;
    Vector3 move;
    Vector3 moveInput;
    public Transform shootPos;
    public Transform fwd;
    float forwardAmount;
    float turnAmount;
    public bool inTrig;
    void Start()
    {
        controller = GetComponent<PlayerController>();
        camTransform = viewCamera.transform;
        

        //viewCamera = Camera.main;
    }
    private void OnTriggerStay(Collider other)
    {
        if(other.tag == "Weapon")
            inTrig = true;
    }

    private void OnTriggerExit(Collider other)
    {
        if (other.tag == "Weapon")
            inTrig = false;
    }
    void Update()
    {



        float horizontal = Input.GetAxis("Horizontal");
        float vert = Input.GetAxis("Vertical");

        if (camTransform != null)
        {
            camForward = Vector3.Scale(camTransform.up, new Vector3(1, 0, 1)).normalized;
            move = vert * camForward + horizontal * camTransform.right;

        }
        else {
            Debug.LogError("Camera not found");
           
        }

        if (move.magnitude > 1) {

            move.Normalize();
        }

        OnMove(move);

        Vector3 moveInput = new Vector3(Input.GetAxisRaw("Horizontal"), 0, Input.GetAxisRaw("Vertical"));
        Vector3 moveVelocity = moveInput.normalized * moveSpeed;
        controller.Move(moveVelocity);

        Ray ray = viewCamera.ScreenPointToRay(Input.mousePosition);
        
        RaycastHit hit;

        if (Physics.Raycast(ray, out hit))
        {
            if (hit.collider.tag == "Weapon") {
                
                if (Input.GetButtonDown("Fire2")&&inTrig)
                {
                    hit.collider.gameObject.SetActive(false);
                    shootingScript.enabled = true;
                    mp5.SetActive(true);

                }
            }
            
            Debug.DrawLine(ray.origin, hit.point, Color.red);
            //Debug.DrawRay(ray.origin,ray.direction * 100,Color.red);
            controller.LookAt(hit.point);
            shootPos.LookAt(hit.point);
            //fwd.LookAt(hit.point);
        }
        

    }

    private void OnMove(Vector3 move)
    {
        if(move.magnitude > 1)
        {

            move.Normalize();

        }
        this.moveInput = move;

        ConvertMoveInput();
        UpdateAnimator();
    }

    private void UpdateAnimator()
    {
        anim.SetFloat("Forward", forwardAmount, 0.1f, Time.deltaTime);
        anim.SetFloat("Turn", turnAmount, 0.1f, Time.deltaTime);
    }

    private void ConvertMoveInput()
    {
        Vector3 localMove = transform.InverseTransformDirection(moveInput);
        turnAmount = localMove.x;
        forwardAmount = localMove.z;
    }
}
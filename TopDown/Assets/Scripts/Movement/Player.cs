using UnityEngine;
using System.Collections;
using System;
using UnityEngine.UI;
[RequireComponent(typeof(PlayerController))]
public class Player : MonoBehaviour
{

    public float moveSpeed = 5;
    public GameObject[] guns;
    
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
    public LayerMask gunMask;
    public AudioSource source;
    public AudioClip pickUp;
    public Text text;
    public Text pump;
    public Text ak47;
    public Text mp5sd;
    public Text mp5;
    public Text mp40;
    public GameObject weaponSelect;
    void Start()
    {
        controller = GetComponent<PlayerController>();
        camTransform = viewCamera.transform;
        

        //viewCamera = Camera.main;
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

        if (Physics.Raycast(ray, out hit,gunMask))
        {

                
                if (Input.GetButtonDown("Fire2"))
                {
                
                

                if (hit.collider.tag == "AK47") {
                    WeaponData data = hit.collider.GetComponent<WeaponData>();
                    Gun weapon = guns[0].GetComponent<Gun>();
                    weapon.magAmmo = data.magAmmo;
                    Destroy(hit.collider.gameObject);
                    guns[0].SetActive(true);
                    source.PlayOneShot(pickUp);
                    text.enabled = true;
                    ak47.enabled = true;
                    mp5.enabled = false;
                    mp5sd.enabled = false;
                    mp40.enabled = false;
                    pump.enabled = false;
                }

                    

                if (hit.collider.tag == "MP5") {
                    WeaponData data = hit.collider.GetComponent<WeaponData>();
                    Gun weapon = guns[1].GetComponent<Gun>();
                    weapon.magAmmo = data.magAmmo;
                    Destroy(hit.collider.gameObject);
                    guns[1].SetActive(true);
                    source.PlayOneShot(pickUp);
                    text.enabled = true;
                    mp5.enabled = true;
                    ak47.enabled = false;
                    mp5sd.enabled = false;
                    mp40.enabled = false;
                    pump.enabled = false;
                }

                    


                if (hit.collider.tag == "MP5SD") {
                    WeaponData data = hit.collider.GetComponent<WeaponData>();
                    Gun weapon = guns[2].GetComponent<Gun>();
                    weapon.magAmmo = data.magAmmo;
                    Destroy(hit.collider.gameObject);
                    guns[2].SetActive(true);
                    source.PlayOneShot(pickUp);
                    text.enabled = true;
                    mp5sd.enabled = true;
                    ak47.enabled = false;
                    mp40.enabled = false;
                    pump.enabled = false;
                }


                if (hit.collider.tag == "MP40") {
                    WeaponData data = hit.collider.GetComponent<WeaponData>();
                    Gun weapon = guns[3].GetComponent<Gun>();
                    weapon.magAmmo = data.magAmmo;
                    source.PlayOneShot(pickUp);
                    Destroy(hit.collider.gameObject);
                    guns[3].SetActive(true);
                    text.enabled = true;
                    mp40.enabled = true;
                    mp5sd.enabled = false;
                    ak47.enabled = false;
                    mp5.enabled = false;
                    pump.enabled = false;
                }


                if (hit.collider.tag == "Pump")
                {
                    WeaponData data = hit.collider.GetComponent<WeaponData>();
                    Shotgun weapon = guns[4].GetComponent<Shotgun>();
                    weapon.magAmmo = data.magAmmo;
                    source.PlayOneShot(pickUp);
                    Destroy(hit.collider.gameObject);
                    guns[4].SetActive(true);
                    text.enabled = true;
                    pump.enabled = true;
                    mp5sd.enabled = false;
                    ak47.enabled = false;
                    mp40.enabled = false;
                    mp5.enabled = false;
                }

            }

              

        }

            

                

        

    
    
            
            Debug.DrawLine(ray.origin, hit.point, Color.red);
            //Debug.DrawRay(ray.origin,ray.direction * 100,Color.red);
            controller.LookAt(hit.point);
            shootPos.LookAt(hit.point);
            //fwd.LookAt(hit.point);
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